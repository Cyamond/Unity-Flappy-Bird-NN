using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NNBird : MonoBehaviour
{
    public NNGameManager gm;
    private float velocity = 1.7f;
    public int score;
    public float fitness;
    private int nextPipeCount = 0;
    private Text scoreText;
    private Text fitnessText;

    private NeuralNetwork nn;

    private PipeSpawner pipeSpawner;

    private GameObject nextPipe;
    
    void Awake()
    {
        gm = GameObject.Find("NNGameManager").GetComponent<NNGameManager>();
        pipeSpawner = GameObject.Find("PipeSpawner").GetComponent<PipeSpawner>();

        score = 0;
        fitness = 0;

        nn = new NeuralNetwork();

        scoreText = GameObject.Find("DummyScore").GetComponent<Text>();
        fitnessText = GameObject.Find("DummyFitness").GetComponent<Text>();
    }

    public void SetNeuralNetwork(NeuralNetwork neuralNet)
    {
        nn = neuralNet;
    }
    
    void Update()
    {
        nextPipe = GameObject.Find($"Pipe {nextPipeCount + 1}");
        fitness += 1f;
        fitness -= Vector3.Magnitude(nextPipe.transform.position - transform.position);
        Debug.DrawLine(transform.position, new Vector3(nextPipe.transform.position.x + 0.13f, nextPipe.transform.position.y, nextPipe.transform.position.z));
        scoreText.text = score.ToString();
        fitnessText.text = fitness.ToString();
        UpdateNeuralNet();
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -0.69f, 1.22f), transform.position.z);
    }

    private void UpdateNeuralNet()
    {
        //Input 1: Distance to end of next pipe
        float x_1 = nextPipe.transform.position.x + 0.13f;
        
        //Input 2: Height difference to center of next pipe
        float x_2 = nextPipe.transform.position.y - transform.position.y;
        
        if (nn.Recalculate(x_1, x_2))
            Jump();
    }

    private void Jump()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.up * velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        score++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        nextPipeCount++;
    }

    public NeuralNetwork GetNeuralNetwork()
    {
        return nn;
    }

    public void SetScoreText(Text text)
    {
        scoreText = text;
    }

    public void SetFitnessText(Text text)
    {
        fitnessText = text;
    }
}
