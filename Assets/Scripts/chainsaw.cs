using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chainsaw : MonoBehaviour
{

    private Health healthThisPlayer;

    private void Start()
    {
        healthThisPlayer = GetComponentInParent<Health>();
    }

    private void Update()
    {
        if(healthThisPlayer.isDead == true)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(100);
            }

        }

        if (other.CompareTag("Breakable"))
        {
            breakableWall wall = other.GetComponent<breakableWall>();

            if (wall != null)
            {
                wall.breakWall();
            }
        }
    }


}
