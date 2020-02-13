using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class NNGameManager : MonoBehaviour
{
    public int populationSize = 10;
    public float mutateRate = 0.2f;
    public int winners = 4;

    public GameObject nnBirdPrefab;

    public static NNGameManager instance;
    public Population population;

    private GameObject[] fittestBirds;
    private NeuralNetwork[] nnForNextGen;

    private bool doingEvolution = false;

    public float timeScale = 1;

    private int allTimeBestScore = 0;
    private int allTimeBestScoreGeneration = 1;
    

    void Awake()
    {
        Time.timeScale = timeScale;
        
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoad;
            population = new Population(populationSize, nnBirdPrefab);
            population.Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        //Check if current generation is dead
        if (GameObject.FindGameObjectsWithTag("Agent").Length == 0 && doingEvolution == false)
        {
            doingEvolution = true;
            //DO EVOLUTION
            
            population.IncreaseGeneration();
            
            fittestBirds = new GameObject[populationSize];
            nnForNextGen = new NeuralNetwork[populationSize];
            Debug.Log($"Generation {population.GetGeneration() - 1} dead!");
            
            Selection();

            
            for (int i = winners; i < populationSize; i++)
            {
                //Fill the rest of the next generation's population with crossover and mutation units
                NeuralNetwork offspring;
                
                if (i == winners)
                {
                    //Make offspring by crossover of two best birds
                    GameObject a = fittestBirds[0];
                    GameObject b = fittestBirds[1];
                    offspring = Crossover(a, b);
                }
                else if (i < populationSize - 2)
                {
                    //Make offspring by crossover of two random fittest birds
                    GameObject a = fittestBirds[Random.Range(0, winners - 1)];
                    GameObject b = fittestBirds[Random.Range(0, winners - 1)];
                    offspring = Crossover(a, b);
                }
                else
                {
                    //offspring is a random winner
                    offspring = fittestBirds[Random.Range(0, winners - 1)].GetComponent<NNBird>().GetNeuralNetwork();
                }

                offspring = Mutation(offspring);
                
                nnForNextGen[i] = offspring;
            }
            

            
            population.SetNNForNextGen(nnForNextGen);

            SceneManager.LoadScene(0);
        }
    }

    void Selection()
    {
        //Sort population by fitness (ascending order)
        GameObject[] curPop = population.GetPopulation();

        for (int i = 1; i < populationSize; i++)
        {
            for (int j = 0; j < populationSize - i; j++)
            {
                if (curPop[j].GetComponent<NNBird>().fitness > curPop[j + 1].GetComponent<NNBird>().fitness)
                {
                    GameObject temp = curPop[j];
                    curPop[j] = curPop[j + 1];
                    curPop[j + 1] = temp;
                }
            }
        }

        for (int i = 0; i < winners; i++)
        {
            //fittestBirds now has #winners fittest birds in DESCENDING order
            fittestBirds[i] = curPop[populationSize - i - 1];

            //Copy fittest birds' neural networks to next generation
            nnForNextGen[i] = fittestBirds[i].GetComponent<NNBird>().GetNeuralNetwork();
        }

        int curBestScore = fittestBirds[0].GetComponent<NNBird>().score;
        
        if (curBestScore > allTimeBestScore)
        {
            allTimeBestScore = curBestScore;
            allTimeBestScoreGeneration = population.GetGeneration() - 1;
        }
        
        Debug.Log($"Best Score: {curBestScore} | All-Time Best Score: {allTimeBestScore} - Generation {allTimeBestScoreGeneration}");
    }

    private NeuralNetwork Crossover(GameObject a, GameObject b)
    {
        float[] genesA = a.GetComponent<NNBird>().GetNeuralNetwork().GetGenes();
        float[] genesB = b.GetComponent<NNBird>().GetNeuralNetwork().GetGenes();
        
        int cutPoint = Random.Range(0, 17);
        
        float[] newA = new float[18];
        float[] newB = new float[18];
        
        Array.Copy(genesA, 0, newA, 0, cutPoint+1);
        Array.Copy(genesB, cutPoint+1, newA, cutPoint, 17 - cutPoint);
        
        Array.Copy(genesB, 0, newB, 0, cutPoint+1);
        Array.Copy(genesA, cutPoint+1, newB, cutPoint, 17 - cutPoint);
        
        NeuralNetwork newNN = new NeuralNetwork();
        newNN.SetGenes(Random.Range(0, 1) == 0 ? newA : newB);
        
        return newNN;
    }

    private NeuralNetwork Mutation(NeuralNetwork unit)
    {
        float[] newGenes = unit.GetGenes();
        for (int i = 0; i < 17; i++)
        {
            if (Random.Range(0f, 1f) > mutateRate)
            {
                float mutateFactor = Random.Range(-1f, 1f);
                newGenes[i] *= mutateFactor;
            }
        }
        NeuralNetwork newNN = new NeuralNetwork();
        newNN.SetGenes(newGenes);
        return newNN;
    }


    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        doingEvolution = false;
        if (population.GetGeneration() != 1)
            population.StartNewGeneration();
    }
}
