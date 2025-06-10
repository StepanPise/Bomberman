using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class bombScript : MonoBehaviour
{
    private Collider collider;
    private int explosionRange;
    public GameObject explosionBlock;
    private PlayerController playerController;
    private int playersInsideCollider = 0;

    void Start()
    {
        collider = GetComponent<Collider>();
    }

    public void SetPlayerController(PlayerController playerController)
    {
        this.playerController = playerController;
        StartCoroutine(TimedExplosion());

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInsideCollider--;

            if (playersInsideCollider <= 0)
            {
                collider.isTrigger = false;
                playersInsideCollider = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInsideCollider++;
        }
    }

    private IEnumerator TimedExplosion()
    {
        yield return new WaitForSeconds(Random.Range(2.3f,2.8f));
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        collider.enabled = false;
        explosionRange = playerController.bombRange;

        playerController.RemoveBomb(transform.position);

        CrossExplosion();
    }

    private void CrossExplosion()
    {
        SpawnExplosion(transform.position);

        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

        foreach (Vector3 dir in directions)
        {
            Vector3 explosionPos = transform.position;

            for (int i = 1; i <= explosionRange; i++)
            {
                explosionPos += dir;
                Vector3 rayStart = explosionPos - dir;

                if (Physics.Raycast(rayStart, dir, out RaycastHit hit, 1f))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        SpawnExplosion(explosionPos);
                        continue;
                    }else if (hit.collider.CompareTag("Breakable"))
                    {
                        SpawnExplosion(explosionPos);
                        break;
                    }else{
                        break;
                    }
                }

                SpawnExplosion(explosionPos);
            }
        }
        Destroy(gameObject, 1f);
    }


    private void SpawnExplosion(Vector3 position)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.pitch = Random.Range(0.9f, 1.1f);
        audio.volume = Random.Range(0.8f, 1.0f);
        audio.Play();
        
        GameObject explosionEffect = Instantiate(explosionBlock, position, Quaternion.identity);
        Destroy(explosionEffect, 1f);
    }


}
