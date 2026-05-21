using Rive;
using Rive.Components;
using Rive.EditorTools;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WoodworkMinigameController : IMinigameController
{


    [SerializeField]
    RiveWidget crossbarRive;


    private void Start()
    {
        StartCoroutine(LoopCrossbarAnim());
    }

    IEnumerator LoopCrossbarAnim()
    {

        yield return new WaitForSeconds(8);

        crossbarRive.StateMachine.ViewModelInstance.GetTriggerProperty("outro").Trigger();
        yield return new WaitForSeconds(0.8f);
        crossbarRive.StateMachine.ViewModelInstance.GetTriggerProperty("intro").Trigger();

        StartCoroutine(LoopCrossbarAnim());
    }


}
