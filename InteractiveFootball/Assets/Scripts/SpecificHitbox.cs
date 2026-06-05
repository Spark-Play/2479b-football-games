using Rive.Components;
using System.Collections;
using UnityEngine;

public class SpecificHitbox : MonoBehaviour, ITargetHandler
{

    public int triggerIndex = 1;

    [field: SerializeField]
    public int pointValue { get; set; } = 10;

    [field: SerializeField]
    public GameObject scorePopup { get; set; }

    public bool canHit = true;

    [SerializeField] RiveWidget riveWidget;


    public void OnHit(Vector3 hitPoint)
    {
        if (!canHit) return;

        riveWidget.StateMachine.ViewModelInstance.GetTriggerProperty("hitbox" + triggerIndex + "Click").Trigger();



    }


}
