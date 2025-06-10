using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosionParticle : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Health>() != null)
        {
            Health health = other.GetComponent<Health>();
            health.TakeDamage(100);
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
