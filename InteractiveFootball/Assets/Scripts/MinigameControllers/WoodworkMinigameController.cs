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
        base.Start();

        Invoke("TurnOffRive", 0.5f);
    }

    void TurnOffRive()
    {

        crossbarRive.StateMachine.ViewModelInstance.GetTriggerProperty("outro").Trigger();
    }

    void OnEnable()
    {
        IMinigameController.OnMinigameStart += GameplayStart;
    }

    void OnDisable()
    {
        IMinigameController.OnMinigameStart -= GameplayStart;
    }

    public void GameplayStart()
    {
        crossbarRive.StateMachine.ViewModelInstance.GetTriggerProperty("intro").Trigger();
        StartCoroutine(LoopCrossbarAnim());
    }

    IEnumerator LoopCrossbarAnim()
    {
        yield return new WaitForSeconds(8);

        crossbarRive.StateMachine.ViewModelInstance.GetTriggerProperty("outro").Trigger();
        yield return new WaitForSeconds(0.8f);
        crossbarRive.StateMachine.ViewModelInstance.GetTriggerProperty("intro").Trigger();

        yield return new WaitForSeconds(8);

        StartCoroutine(LoopCrossbarAnim());
    }


}
