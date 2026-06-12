using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Drives the fiery STREAK frame effect from the current streak/score.
///
/// As the streak climbs, <see cref="_Fieriness"/> (0..1) rises and the shader
/// makes the flames taller, faster, hotter (orange -> white) and brighter, and
/// the ember emitter spits more particles. Each increment also fires a "punch":
/// a quick scale bounce, a brief fieriness overshoot (emission spike), and a
/// tiny positional wiggle.
///
/// Setup:
///   1. Put this on the "Banner" RectTransform (the parent that holds
///      Plate / FrameFire / Text / Embers).
///   2. Drag the FrameFire Image into <see cref="frameImage"/>.
///   3. (Optional) Drag the ember ParticleSystem into <see cref="emberSystem"/>.
///   4. Call AddStreak() on a successful hit, ResetStreak() on a miss, or
///      SetStreak(n) to jump to a value.
///
/// The shader (StreakFireFrame, Canvas type) must expose a Float "_Fieriness".
/// </summary>
[DisallowMultipleComponent]
public class LightningStreak : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The middle Image that uses the StreakFireFrame material (the frame that burns).")]
    [SerializeField] private Image frameImage;

    [Tooltip("Optional ember ParticleSystem (e.g. Coffee UIParticle child). Its emission rate scales with fieriness.")]
    [SerializeField] private ParticleSystem emberSystem;

    [Tooltip("Optional transform to bounce/wiggle on each increment. Defaults to this object's RectTransform.")]
    [SerializeField] private RectTransform punchTarget;

    [Header("Escalation")]
    [Tooltip("Streak value at which the fire reaches full blaze (_Fieriness = 1).")]
    [Min(1)][SerializeField] private int maxStreakForFullBlaze = 10;

    [Tooltip("How fast _Fieriness eases toward its target, in units/second. Higher = snappier.")]
    [SerializeField] private float rampSpeed = 1.5f;

    [Tooltip("Max embers per second at full blaze. Scales linearly with fieriness.")]
    [SerializeField] private float maxEmberRate = 20f;

    [Header("Punch (per increment)")]
    [SerializeField] private float punchScale = 1.15f;
    [SerializeField] private float punchDuration = 0.30f;
    [Tooltip("Extra fieriness added on top of the steady value during the punch (clamped to 1). Drives the emission/bloom spike.")]
    [SerializeField] private float punchEmissionSpike = 0.35f;
    [Tooltip("How long the emission spike lasts before decaying back.")]
    [SerializeField] private float punchSpikeDuration = 0.15f;
    [SerializeField] private float wiggleAmplitude = 6f;   // pixels
    [SerializeField] private float wiggleDuration = 0.15f;

    private static readonly int FierinessId = Shader.PropertyToID("_Fieriness");

    private Material _mat;                 // private instance so we don't edit the shared asset
    private RectTransform _rt;
    private Vector3 _baseScale;
    private Vector2 _baseAnchoredPos;

    private int _streak;
    private float _targetFieriness;        // steady target from streak (0..1)
    private float _current;                // eased steady value
    private float _spike;                  // transient additive spike from punch (decays to 0)
    private Coroutine _punchRoutine;

    private void Awake()
    {
        _rt = (RectTransform)transform;
        if (punchTarget == null) punchTarget = _rt;

        _baseScale = punchTarget.localScale;
        _baseAnchoredPos = punchTarget.anchoredPosition;

        if (frameImage != null)
        {
            // Instance the material so SetFloat affects only this banner, not the shared asset.
            _mat = Instantiate(frameImage.material);
            frameImage.material = _mat;
        }
        else
        {
            Debug.LogWarning("[StreakController] frameImage is not assigned; fieriness will not drive a shader.", this);
        }

        ApplyFieriness(0f);
    }

    // ---- Public API -----------------------------------------------------

    /// <summary>Increase the streak by one (call on a successful hit).</summary>
    public void AddStreak() => SetStreak(_streak + 1);

    /// <summary>Reset the streak to zero (call on a miss / streak break).</summary>
    public void ResetStreak() => SetStreak(0);

    /// <summary>Jump the streak to an explicit value.</summary>
    public void SetStreak(int streak)
    {
        bool increased = streak > _streak;
        _streak = Mathf.Max(0, streak);
        _targetFieriness = Mathf.Clamp01((float)_streak / maxStreakForFullBlaze);

        if (increased) Punch();
    }

    /// <summary>Current streak value.</summary>
    public int Streak => _streak;

    // ---- Per-frame drive ------------------------------------------------

    private void Update()
    {
        // Ease the steady value toward its target for a smooth ramp-up.
        _current = Mathf.MoveTowards(_current, _targetFieriness, rampSpeed * Time.deltaTime);

        // Decay any transient punch spike back to zero.
        if (_spike > 0f)
            _spike = Mathf.Max(0f, _spike - (punchEmissionSpike / Mathf.Max(0.0001f, punchSpikeDuration)) * Time.deltaTime);

        ApplyFieriness(Mathf.Clamp01(_current + _spike));
    }

    private void ApplyFieriness(float value)
    {
        if (_mat != null) _mat.SetFloat(FierinessId, value);

        if (emberSystem != null)
        {
            var emission = emberSystem.emission;
            emission.rateOverTime = value * maxEmberRate;
        }
    }

    // ---- Juice ----------------------------------------------------------
    [ContextMenu("Punch")]
    private void Punch()
    {
        _spike = Mathf.Min(_spike + punchEmissionSpike, 1f);

        if (!isActiveAndEnabled) return; // can't run coroutines while disabled
        if (_punchRoutine != null) StopCoroutine(_punchRoutine);
        _punchRoutine = StartCoroutine(PunchRoutine());
    }

    private IEnumerator PunchRoutine()
    {
        float duration = Mathf.Max(punchDuration, wiggleDuration);
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;

            // Scale: 1 -> punchScale -> 1, eased with a single sine hump.
            if (t <= punchDuration)
            {
                float n = t / punchDuration;                 // 0..1
                float hump = Mathf.Sin(n * Mathf.PI);        // 0 -> 1 -> 0
                punchTarget.localScale = _baseScale * (1f + (punchScale - 1f) * hump);
            }
            else
            {
                punchTarget.localScale = _baseScale;
            }

            // Wiggle: decaying Perlin shake on anchored position.
            if (t <= wiggleDuration)
            {
                float decay = 1f - (t / wiggleDuration);
                float x = (Mathf.PerlinNoise(Time.time * 25f, 0.0f) - 0.5f) * 2f;
                float y = (Mathf.PerlinNoise(0.0f, Time.time * 25f) - 0.5f) * 2f;
                punchTarget.anchoredPosition = _baseAnchoredPos + new Vector2(x, y) * (wiggleAmplitude * decay);
            }
            else
            {
                punchTarget.anchoredPosition = _baseAnchoredPos;
            }

            yield return null;
        }

        punchTarget.localScale = _baseScale;
        punchTarget.anchoredPosition = _baseAnchoredPos;
        _punchRoutine = null;
    }

    private void OnDisable()
    {
        // Disabling the *component* (enabled = false) does NOT auto-stop coroutines —
        // only deactivating the GameObject does. So stop it explicitly, otherwise the
        // running punch keeps overwriting the transform we restore just below.
        if (_punchRoutine != null) StopCoroutine(_punchRoutine);
        _punchRoutine = null;

        // Restore transform if disabled mid-punch.
        if (punchTarget != null)
        {
            punchTarget.localScale = _baseScale;
            punchTarget.anchoredPosition = _baseAnchoredPos;
        }
    }

    private void OnDestroy()
    {
        if (_mat != null) Destroy(_mat); // clean up the instanced material
    }
}