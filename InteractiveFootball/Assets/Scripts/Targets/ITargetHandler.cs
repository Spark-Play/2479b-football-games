using UnityEngine;

public interface ITargetHandler
{

    public int pointValue { get; set; }

    public abstract void OnHit(Vector3 hitPoint);

}
