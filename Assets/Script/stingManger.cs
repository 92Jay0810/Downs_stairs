using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class stingManger : MonoBehaviour
{
    [SerializeField] GameObject[]  propPrefab;
    [SerializeField] int minCount;
    [SerializeField] int maxCount;
    [SerializeField] float generateInterval;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        minCount = 3;
        maxCount = 5;
        generateInterval = 30f;
        GenerateProps();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= generateInterval)
        {
            GenerateProps();
            timer = 0f; //重製
        }
    }
    void GenerateProps()
    {
        // 摧毀之前的道具
        DestroyOldProps();
        // 隨機生成3~5個道具
        int stingCount = Random.Range(minCount, maxCount + 1);
        for (int i = 0; i < stingCount; i++)
        {
            int loveORsting = Random.Range(0, propPrefab.Length);
            GameObject sting= Instantiate(propPrefab[loveORsting], transform);
            sting.transform.position = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-5.0f, 4.0f), 0); ;
        }
    }
    void DestroyOldProps()
    {
        GameObject[] stings = GameObject.FindGameObjectsWithTag("sting");
        foreach (GameObject sting in stings)
        {
            Destroy(sting);
        }
        GameObject[] loves = GameObject.FindGameObjectsWithTag("love");
        foreach (GameObject love in loves)
        {
            Destroy(love);
        }
    }
}
