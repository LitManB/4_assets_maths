using System;
using UnityEngine;

public class Doolittle
{
    // C# Program to decompose a matrix into 
    // lower and upper traingular matrix
    public static void LUDecomposition(float[,] mat, int n)
    {
        float[,] lower = new float[n, n];
        float[,] upper = new float[n, n];

        // Decomposing matrix into Upper and Lower 
        // triangular matrix 
        for (int i = 0; i < n; i++)
        {

            // Upper Triangular 
            for (int k = i; k < n; k++)
            {

                // Summation of L(i, j) * U(j, k) 
                float sum = 0;
                for (int j = 0; j < i; j++)
                    sum += (lower[i, j] * upper[j, k]);

                // Evaluating U(i, k) 
                upper[i, k] = mat[i, k] - sum;
            }

            // Lower Triangular 
            for (int k = i; k < n; k++)
            {
                if (i == k)
                    lower[i, i] = 1; // Diagonal as 1 
                else
                {

                    // Summation of L(k, j) * U(j, i) 
                    float sum = 0;
                    for (int j = 0; j < i; j++)
                        sum += (lower[k, j] * upper[j, i]);

                    // Evaluating L(k, i) 
                    lower[k, i] = (mat[k, i] - sum) / upper[i, i];
                }
            }
        }

        PrintLU(n, lower, upper);
    }

    private static void PrintLU(int n, float[,] lower, float[,] upper)
    {
        // SetWhiteSpace is for displaying nicely 
        Debug.Log("Doolittle for LU decomposition\n*************************************\n");

        var output = "";
        // Displaying the result : 
        for (int i = 0; i < n; i++)
        {
            // Lower 
            for (int j = 0; j < n; j++)
            {
                output += SetWhiteSpace(4) + lower[i, j] + "\t";
            }
            output += "\t";

            // Upper 
            for (int j = 0; j < n; j++)
            {
                output += SetWhiteSpace(4) + upper[i, j] + "\t";
            }
            output += "\n";
        }
        Debug.Log(output);
    }

    private static string SetWhiteSpace(int noOfSpace)
    {
        var s = "";
        for (int i = 0; i < noOfSpace; i++)
            s += " ";
        return s;
    }
}
