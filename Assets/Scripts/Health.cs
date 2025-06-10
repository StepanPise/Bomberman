using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 10;
    private Animator animator;
    public bool isDead = false;
    private PlayerController playerController;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerController= GetComponent<PlayerController>();
    }

    public void Death()
    {
        if (isDead == true) return;

        isDead = true;
        Debug.Log("You Died: " + gameObject.name);
        animator.SetTrigger("TrDeath");
        GetComponent<AudioSource>().Play();
        playerController.enabled = false;
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if(health <= 0)
        {
            Death();
        }
    }

    public void TakeHeal(int heal)
    {
        health += heal;
    }

}
