using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakableWall : MonoBehaviour
{
    public List<GameObject> items;
    public void breakWall()
    {
        if (Random.Range(1, 3) == 1 && items != null)
        {
            GameObject randomItem = items[Random.Range(0, items.Count)];

            Instantiate(randomItem, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}