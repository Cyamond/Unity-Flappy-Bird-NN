using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Population
{
    private int populationSize;
    private GameObject[] population;
    private GameObject prefab;

    private int generation;

    private NeuralNetwork[] nnForNextGen;

    public Population(int populationSize, GameObject nnBirdPrefab)
    {
        this.populationSize = populationSize;
        this.prefab = nnBirdPrefab;
        population = new GameObject[populationSize];
        this.generation = 1;
    }

    public void Init()
    {
        Debug.Log("Population Init");
        for (int i = 0; i < populationSize; i++)
        {
            GameObject newBird = GameObject.Instantiate(prefab);
            newBird.GetComponent<NNBird>().SetScoreText(GameObject.Find($"Score {i + 1}").GetComponent<Text>());
            newBird.GetComponent<NNBird>().SetFitnessText(GameObject.Find($"Fitness {i + 1}").GetComponent<Text>());
            newBird.name = $"Bird {i}";
            population[i] = newBird;
        }
    }

    public void StartNewGeneration()
    {
        //generation++;
        Debug.Log($"Spawning Generation {generation}");
        
        for (int i = 0; i < populationSize; i++)
        {
            GameObject newBird = GameObject.Instantiate(prefab);
            newBird.GetComponent<NNBird>().SetScoreText(GameObject.Find($"Score {i + 1}").GetComponent<Text>());
            newBird.GetComponent<NNBird>().SetFitnessText(GameObject.Find($"Fitness {i + 1}").GetComponent<Text>());
            newBird.name = $"Bird {i}";
            newBird.GetComponent<NNBird>().SetNeuralNetwork(nnForNextGen[i]);
            population[i] = newBird;
        }
    }

    public void SetNNForNextGen(NeuralNetwork[] nn)
    {
        nnForNextGen = nn;
    }

    public int GetGeneration()
    {
        return generation;
    }

    public void IncreaseGeneration()
    {
        generation++;
    }

    public GameObject[] GetPopulation()
    {
        return population;
    }
}
