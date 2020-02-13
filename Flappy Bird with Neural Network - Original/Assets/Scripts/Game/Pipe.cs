using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public float speed;
    private GameObject pipeSpawner;

    private float timer = 0;
    
    void Start()
    {
        pipeSpawner = GameObject.Find("PipeSpawner");
    }
    
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (timer > 5)
        {
            Destroy(gameObject);
        }

        timer += Time.deltaTime;
    }
}
