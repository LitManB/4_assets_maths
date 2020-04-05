using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decomposition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MatrixClass();
        DoolittleClass();
    }

    private void DoolittleClass()
    {
        //Coefficient matrix A
        float[,] coefficientMatrix = {
                                    { 5f, 6f, 2.3f, 6f },
                                    { 9f, 2f, 3.5f, 7f },
                                    { 3.5f, 6f, 2f, 3f },
                                    { 1.5f, 2f, 1.5f, 6f }
                                  };

        Doolittle.LUDecomposition(coefficientMatrix, 4);
    }

    private void MatrixClass()
    {
        //A*X=B
        //Values for Matrix A
        var coefficientMatrixValues = "5 6 2.3 6\r\n" +
                                      "9 2 3.5 7\r\n" +
                                      "3.5 6 2 3\r\n" +
                                      "1.5 2 1.5 6";

        //Values for Matrix B
        var resultMatrixValues = "4\r\n" +
                                 "5\r\n" +
                                 "6.7\r\n" +
                                 "7.8";

        //Converts sting to Matrix
        Matrix coefficientMatrix = Matrix.Parse(coefficientMatrixValues);
        Debug.Log("A Matrix:\n" + coefficientMatrix.ToString());

        //Uncomment if you want to use random values
        //Matrix coeficientMatrix = Matrix.RandomMatrix(3, 3, 5);
        //Debug.Log(coeficientMatrix.ToString());

        //Makes L and U matrices and stores the values to L and U property
        coefficientMatrix.MakeLU();
        Debug.Log("L Matrix:\n" + coefficientMatrix.L.ToString());
        Debug.Log("U Matrix:\n" + coefficientMatrix.U.ToString());


        //Result matrix B
        Matrix resultMatrix = Matrix.Parse(resultMatrixValues);
        Debug.Log("resultMatrix:\n" + resultMatrix.ToString());

        //Solves the system of linear equations, stores the result to X and prints the result
        var xMatrix = coefficientMatrix.SolveWith(resultMatrix);
        Debug.Log("xMatrix:\n" + xMatrix.ToString());
    }
}
