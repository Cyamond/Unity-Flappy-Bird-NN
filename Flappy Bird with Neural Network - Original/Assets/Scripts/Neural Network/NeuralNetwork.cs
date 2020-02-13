using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class NeuralNetwork
{
    private float[,] hiddenLayerWeights;
    private float[] outputWeights;

    public NeuralNetwork()
    {
        InitRandomWeights();
    }

    //TODO: Add distance to beginning of next pipe to Input
    public Boolean Recalculate(float x_1, float x_2)
    {
        float output = 0;
        
        float[] hiddenLayer = new float[6];
        
        for (int i = 0; i < 6; i++)
        {
            hiddenLayer[i] = hiddenLayerWeights[i, 0] * x_1 + hiddenLayerWeights[i, 1] * x_2;
        }

        for (int i = 0; i < 6; i++)
        {
            output += outputWeights[i] * hiddenLayer[i];
        }
        
        return (output > 0.5);
    }

    public void InitRandomWeights()
    {
        hiddenLayerWeights = new float[6, 2];
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                hiddenLayerWeights[i, j] = Random.Range(-10f, 10f);
            }
        }
        
        outputWeights = new float[6];
        for (int i = 0; i < 6; i++)
        {
            outputWeights[i] = Random.Range(-10f, 10f);
        }
    }
    
    public void SetGenes(float[] genes)
    {
        for (int i = 0; i < 6; i++)
        {
            hiddenLayerWeights[i, 0] = genes[i * 2];
            hiddenLayerWeights[i, 1] = genes[i * 2 + 1];
        }

        for (int i = 0; i < 6; i++)
        {
            outputWeights[i] = genes[i + 12];
        }
    }

    public float[] GetGenes()
    {
        float[] genes = new float[18];
        
        for (int i = 0; i < 6; i++)
        {
            genes[i * 2] = hiddenLayerWeights[i, 0];
            genes[i * 2 + 1] = hiddenLayerWeights[i, 1];
        }

        for (int i = 0; i < 6; i++)
        {
            genes[i + 12] = outputWeights[i];
        }
        
        return genes;
    }
}
