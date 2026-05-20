using UnityEngine;

public interface ITargetHandler
{
    public GameObject scorePopup { get; set; }

    public int pointValue { get; set; }

    public abstract void OnHit(Vector3 hitPoint);

}
