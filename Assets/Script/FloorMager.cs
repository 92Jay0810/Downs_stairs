using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMager : MonoBehaviour
{
    [SerializeField] GameObject[] FloorPrefabs;
    public void swapFloor()
    {
        int random_floor = Random.Range(0, FloorPrefabs.Length);
        //crate floor and assign as child to FloorMager 
        GameObject temp_floor= Instantiate(FloorPrefabs[random_floor], transform);
        temp_floor.transform.position = new Vector3(Random.Range(-2.5f,2.5f), Random.Range(-6.0f, -5.0f), 0);
    }
}
