using Rive;
using Rive.Components;
using Rive.EditorTools;
using UnityEngine;

public class SmashTheWallMinigameController : IMinigameController
{
    public RiveWidget riveWidget;

    private ViewModelInstanceNumberProperty healthProperty;




    public void OnHitbox()
    {
        riveWidget.StateMachine.ViewModelInstance.GetTriggerProperty("hitbox1Click").Trigger();
        riveWidget.StateMachine.ViewModelInstance.GetTriggerProperty("hitbox2Click").Trigger();
        riveWidget.StateMachine.ViewModelInstance.GetTriggerProperty("hitbox3Click").Trigger();
        riveWidget.StateMachine.ViewModelInstance.GetTriggerProperty("hitbox4Click").Trigger();
        riveWidget.StateMachine.ViewModelInstance.GetTriggerProperty("hitbox5Click").Trigger();
    }

}
