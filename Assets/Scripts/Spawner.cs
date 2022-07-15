using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool isPool;
    public bool isSmart;
    public int numToPool;
    public int maxPoolPerFrame;
    public GameObject objectToSpawn;
    private GameObject[] spawnables;


    // Start is called before the first frame update
    private void Start()
    {
        if (isPool)
        {
            Pool();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Spawn();
        }
    }

    private void Pool()
    {
        spawnables = new GameObject[numToPool];

        if (isSmart)
        {
            StartCoroutine(SmartPool());
            return;
        }
        for (int i = 0; i < numToPool; i++)
        {
            spawnables[i] = Instantiate(objectToSpawn, this.transform);
            spawnables[i].name = "Spawnable" + i;
            spawnables[i].SetActive(false);
        }
    }

    IEnumerator SmartPool()
    {
        var pooledThisFrame = 0;
        for (int i = 0; i < numToPool; i++)
        {
            spawnables[i] = Instantiate(objectToSpawn, this.transform);
            spawnables[i].name = "Spawnable" + i;
            spawnables[i].SetActive(false);

            pooledThisFrame++;

            if (pooledThisFrame >= maxPoolPerFrame)
            {
                yield return null;
                pooledThisFrame = 0;
            }
        }
    }
    private void Spawn()
    {
        if (spawnables == null)
        {
            Instantiate(objectToSpawn, transform.position, transform.rotation);
        }
        //find the first inactive Object and activate it
        for (int i = 0; i < spawnables.Length; i++)
        {
            if (spawnables[i].activeInHierarchy == false)
            {
                spawnables[i].SetActive(true);
                spawnables[i].transform.position = transform.position;
                return;
            }
        }
    }
}
