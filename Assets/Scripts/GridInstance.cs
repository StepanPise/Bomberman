using System.Collections.Generic;
using UnityEngine;

public class GridInstance : MonoBehaviour
{
    public static GridInstance Instance { get; private set; }
    public GameObject wall;
    public HashSet<Vector3> placedBombs = new HashSet<Vector3>();
    private List<GameObject> spawnedWalls = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GenerateWalls(){

        placedBombs.Clear();

        Collider[] cleanupColliders = Physics.OverlapBox(
            center: new Vector3(0, 0.5f, 1),
            halfExtents: new Vector3(5, 0.5f, 5)
        );

        foreach (var col in cleanupColliders)
        {
            if (col.CompareTag("toReset"))
            {
                Destroy(col.gameObject);
            }
        }

        foreach (GameObject go in spawnedWalls)
        {
            if (go != null)
                Destroy(go);
        }
        spawnedWalls.Clear();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int x = -5; x <= 5; x++)
        {
            for (int z = -4; z <= 6; z++)
            {
                Vector3 position = new Vector3(x, 0.5f, z);

                bool nearPlayer = false;
                foreach (var player in players)
                {
                    if (Vector3.Distance(player.transform.position, position) <= 1f)
                    {
                        nearPlayer = true;
                        break;
                    }
                }
                if (nearPlayer)
                    continue;

                if (Random.value < 0.7f)
                {
                    if (placedBombs.Contains(position))
                        continue;

                    GameObject newWall = Instantiate(wall, position, Quaternion.identity);
                    spawnedWalls.Add(newWall);

                }
            }
        }
    }
}
