using UnityEngine;

public class SplittingTarget : ITargetHandler
{
    public override void OnHit()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
