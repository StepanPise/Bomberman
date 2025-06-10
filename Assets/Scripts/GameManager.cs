using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    private bool someoneWon;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI restartText;
    public GameObject confetti;
    private List<ParticleSystem> confettiList = new List<ParticleSystem>();

    private void Start()
    {
        foreach (Transform child in confetti.transform)
        {
            confettiList.Add(child.gameObject.GetComponent<ParticleSystem>());
        }

        resultText.text = "";
        restartText.text = "";
        GridInstance.Instance.GenerateWalls();

    }

    void Update()
    {
        if ((player1.GetComponent<Health>().isDead == true || player2.GetComponent<Health>().isDead == true) && someoneWon == false)
        {
            someoneWon = true;
            StartCoroutine(WhoWon());
        }

        if (Input.GetKeyDown(KeyCode.R)) ResetGame();

    }

    private IEnumerator WhoWon()
    {
        yield return new WaitForSeconds(2f);

        if(player1.GetComponent<Health>().isDead == true && player2.GetComponent<Health>().isDead == true)
        {
            resultText.text = "TIE";
        }
        if (player1.GetComponent<Health>().isDead == false && player2.GetComponent<Health>().isDead == true)
        {
            resultText.text = "Player 1 WON";
            setParticlesactive(true);
        }
        if (player1.GetComponent<Health>().isDead == true && player2.GetComponent<Health>().isDead == false)
        {
            resultText.text = "Player 2 WON";
            setParticlesactive(true);
        }

        restartText.text = "Press <b><color=#FFD700>R</color></b> to restart";
    }

    private void ResetGame()
    {
        someoneWon = false;
        resultText.text = "";
        restartText.text = "";
        setParticlesactive(false);

        player1.GetComponent<Health>().health = 10;
        player2.GetComponent<Health>().health = 10;

        player1.GetComponent<Health>().isDead = false;
        player2.GetComponent<Health>().isDead = false;

        player1.transform.position = player1.GetComponent<PlayerController>().initialPos;
        player2.transform.position = player2.GetComponent<PlayerController>().initialPos;

        Animator anim1 = player1.GetComponentInChildren<Animator>();
        Animator anim2 = player2.GetComponentInChildren<Animator>();

        anim1.Rebind();
        anim1.Update(0f);
        anim2.Rebind();
        anim2.Update(0f);

        player1.GetComponent<PlayerController>().ResetAbilities();
        player2.GetComponent<PlayerController>().ResetAbilities();

        player1.GetComponent<PlayerController>().enabled = true;
        player2.GetComponent<PlayerController>().enabled = true;

        player1.GetComponent<PlayerController>().chainsaw.SetActive(false);
        player2.GetComponent<PlayerController>().chainsaw.SetActive(false);

        GridInstance.Instance.GenerateWalls();
    }


    private void setParticlesactive(bool isActive)
    {
        if(isActive)
        {
            foreach (ParticleSystem confetti in confettiList)
            {
                confetti.Play();
            }
        }
        else
        {
            foreach (ParticleSystem confetti in confettiList)
            {
                confetti.Stop();
                confetti.Clear();

            }
        }
    }
}
