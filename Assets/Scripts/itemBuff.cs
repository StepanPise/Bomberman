using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemBuff : MonoBehaviour
{
    public bool isSpeedBuff;
    public bool isRangeBuff;
    public bool isBombAmountBuff;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isSpeedBuff)
            {
                other.GetComponent<PlayerController>().playerSpeed += 1;
            }
            if (isRangeBuff)
            {
                other.GetComponent<PlayerController>().bombRange += 2;
            }
            if (isBombAmountBuff)
            {
                other.GetComponent<PlayerController>().bombAmount += 1;
            }

            Destroy(gameObject);
        }
    }

}
