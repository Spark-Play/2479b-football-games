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

    public override void StartMinigame()
    {
        base.StartMinigame();
        shotsLeftDisplay[0].transform.parent.parent.gameObject.SetActive(true);
    }

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
            //Loop Minigame

            round = 0;

            shotsLeft = 3;

            for (int i = 0; i < shotsLeftDisplay.Length; i++)
            {
                shotsLeftDisplay[i].enabled = true;
            }

            for (int i = 0; i < targetRounds.Length; i++)
            {
                targetRounds[i].SetActive(i == round);

                foreach (Transform target in targetRounds[i].transform)
                {
                    target.gameObject.SetActive(true);
                }

            }


            canShoot = true;

            //secondSpeed = 0.05f;
        }


    }

}
