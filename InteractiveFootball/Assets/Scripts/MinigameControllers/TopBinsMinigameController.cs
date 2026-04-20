using Rive;
using Rive.Components;
using Rive.EditorTools;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TopBinsMinigameController : IMinigameController
{


    [SerializeField]
    GameObject[] targetRounds;

    [SerializeField]
    Image[] shotsLeftDisplay;

    public int shotsLeft = 3;

    public int round = 0;

    public override void ShotTaken()
    {

        print("caught");

        shotsLeft--;

        for (int i = 0; i < shotsLeftDisplay.Length; i++)
        {
            shotsLeftDisplay[i].enabled = (i < shotsLeft);
        }

        if (shotsLeft <= 0)
        {
            StartCoroutine(NextRound());
        }
    }

    IEnumerator NextRound()
    {
        canShoot = false;

        yield return new WaitForSeconds(1f);


        round++;

        if (round < 3)
        {
            shotsLeft = 3;

            for (int i = 0; i < shotsLeftDisplay.Length; i++)
            {
                shotsLeftDisplay[i].enabled = true;
            }

            for (int i = 0; i < targetRounds.Length; i++)
            {
                targetRounds[i].SetActive(i == round);
            }


            canShoot = true;

        }
        else
        {
            //End Minigame

            for (int i = 0; i < targetRounds.Length; i++)
            {
                targetRounds[i].SetActive(false);
            }

            secondSpeed = 0.05f;
        }


    }

}
