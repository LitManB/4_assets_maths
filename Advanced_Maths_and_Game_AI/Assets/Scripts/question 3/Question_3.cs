using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question_3 : MonoBehaviour
{
    
    private Vector3 r1;
    private Vector3 r2;

    private float t = 0;


    void Start()
    {
        run();
    }

    public void run()
    {


        for (float i = 0.0f; i < 20; i += 0.5f)
        {
            t = i;

            r1 = new Vector3(t, t * t, t * t * t);
            r2 = new Vector3(1 +(2 * t), 1 + (6 * t), 1 + (14 * t));

            Debug.Log(r1 + " || " + r2);

        }
    }
 


}
