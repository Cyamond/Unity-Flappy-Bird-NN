using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PipeSpawner : MonoBehaviour
{
    public float maxTime = 1;
    private float timer = 0;
    public GameObject pipePair;
    public float height;
    private float groundHeight;

    private int pipeCounter = 0;

    void Awake()
    {
        groundHeight = GameObject.Find("Ground").GetComponent<BoxCollider2D>().bounds.size.y;
        SpawnPipe();
    }
    
    void Start()
    {
        
    }
    
    void Update()
    {
        if (timer > maxTime)
        {
            SpawnPipe();
            timer = 0;
        }

        timer += Time.deltaTime;
    }

    private void SpawnPipe()
    {
        GameObject newPipe = Instantiate(pipePair);
        pipeCounter++;
        newPipe.transform.position = transform.position + new Vector3(0, Random.Range(-height, height) + 0.5f*groundHeight, 0);
        newPipe.gameObject.name = $"Pipe {pipeCounter}";
    }
}
