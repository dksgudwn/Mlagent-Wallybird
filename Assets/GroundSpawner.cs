using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject Ground;

    public List<GameObject> Spawns;

    private void Start()
    {
        Spawn();
    }
    //스폰하고 지우는거만들기

    public void Spawn()
    {
        print("스폰");

        GameObject obj;

        obj = Instantiate(Ground, new Vector3(transform.localPosition.x, -3.85f, 0), Quaternion.identity);
        Spawns.Add(obj);
        for (int i = 0; i < 1000; i++)
        {
            float randX = Random.Range(-5f, 5f);

            obj = Instantiate(Ground, new Vector3(transform.localPosition.x + randX, (float)(i * 4 + 0.25), 0), Quaternion.identity);
            //obj.transform.position = transform.position;
            Spawns.Add(obj);
        }
    }

    public void Destory()
    {
        foreach (GameObject obj in Spawns)
        {
            Destroy(obj);
        }
    }
}
