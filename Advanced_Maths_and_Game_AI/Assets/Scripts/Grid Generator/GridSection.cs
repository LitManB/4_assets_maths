using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSection
{

    public GameObject segment;
    public Vector2 pos;
    public bool wall = false;

    public int gCost, hCost, fCost;
    public GridSection lastSection;

    public GridSection(Vector2 xy)
    {
        pos = xy;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    
}
