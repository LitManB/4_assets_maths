using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    private const int STRAIGHT_COST = 10;
    private const int DIAGONAL_COST = 14;

    private GridSection[,] grid;
    private List<GridSection> openList;
    private List<GridSection> closedList;

    public AStar(GridSection[,] g)
    {
        grid = g;
    }

    public List<GridSection> FindPath(int startX, int startY, int endX, int endY)
    {
        GridSection startGridSection = grid[startX, startY];
        GridSection endGridSection = grid[endX, endY];

        openList = new List<GridSection> { startGridSection };
        closedList = new List<GridSection>();

        for(int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                GridSection GridSection = grid[x, y];
                GridSection.gCost = int.MaxValue;
                GridSection.CalculateFCost();
                GridSection.lastSection = null;
            }
        }

        startGridSection.gCost = 0; 
        startGridSection.hCost = CalculateDistance(startGridSection, endGridSection); 
        startGridSection.CalculateFCost(); 

        //-------------------------------------------------------------------//

        while(openList.Count > 0)
        {
            GridSection currentGridSection = GetLowestFCostGridSection(openList);

            if(currentGridSection == endGridSection) //
            {
                return CalculatePath(endGridSection); //
            }

            openList.Remove(currentGridSection);
            closedList.Add(currentGridSection);

            foreach(GridSection neighbourGridSection in GetNeighbourGridSections(currentGridSection)) //
            {
                if (closedList.Contains(neighbourGridSection)) continue;

                if(neighbourGridSection.wall)
                {
                    closedList.Add(neighbourGridSection);
                    continue;
                }

                int tempGCost = currentGridSection.gCost + CalculateDistance(currentGridSection, neighbourGridSection); //

                if(tempGCost < neighbourGridSection.gCost) //
                {
                    neighbourGridSection.lastSection = currentGridSection; //
                    neighbourGridSection.gCost = tempGCost;    //
                    neighbourGridSection.hCost = CalculateDistance(neighbourGridSection, endGridSection); //
                    neighbourGridSection.CalculateFCost(); //

                    if(!openList.Contains(neighbourGridSection)) //
                    {
                        openList.Add(neighbourGridSection);
                    }
                }
            }
        }
        //
        return null;
    }

    private List<GridSection> GetNeighbourGridSections(GridSection currentGridSection)
    {
        List<GridSection> neighbourList = new List<GridSection>();

        if((int)currentGridSection.pos.x - 1 > 0) //Gets X Minus Neigbours
        {
            neighbourList.Add(grid[(int)currentGridSection.pos.x - 1, (int)currentGridSection.pos.y]); //Left

            if((int)currentGridSection.pos.y - 1 > 0)
                neighbourList.Add(grid[(int)currentGridSection.pos.x - 1, (int)currentGridSection.pos.y - 1]); //Left Down

            if ((int)currentGridSection.pos.y + 1 > 0)
                neighbourList.Add(grid[(int)currentGridSection.pos.x - 1, (int)currentGridSection.pos.y + 1]); //Left Up
        }
        if ((int)currentGridSection.pos.x + 1 < grid.GetLength(0)) //Gets X Plus Neighbours
        {
            neighbourList.Add(grid[(int)currentGridSection.pos.x + 1, (int)currentGridSection.pos.y]); //Right

            if ((int)currentGridSection.pos.y - 1 > 0)
                neighbourList.Add(grid[(int)currentGridSection.pos.x + 1, (int)currentGridSection.pos.y - 1]); //Right Down

            if ((int)currentGridSection.pos.y + 1 > 0)
                neighbourList.Add(grid[(int)currentGridSection.pos.x + 1, (int)currentGridSection.pos.y + 1]); //Right Up
        }

        if((int)currentGridSection.pos.y - 1 > 0) //Gets Y Above
            neighbourList.Add(grid[(int)currentGridSection.pos.x, (int)currentGridSection.pos.y - 1]); //Down

        if ((int)currentGridSection.pos.y + 1 < grid.GetLength(1)) //Gets Y Below
            neighbourList.Add(grid[(int)currentGridSection.pos.x, (int)currentGridSection.pos.y + 1]); //UP

        return neighbourList;

    }

    private List<GridSection> CalculatePath(GridSection endGridSection)
    {
        List<GridSection> path = new List<GridSection>();
        path.Add(endGridSection); 
        GridSection currentGridSection = endGridSection; 

        while(currentGridSection.lastSection != null)
        {
            path.Add(currentGridSection.lastSection); 
            currentGridSection = currentGridSection.lastSection; 
        }

        path.Reverse(); 
        return path; 
    }

    private int CalculateDistance(GridSection start, GridSection end)
    {
        int disX = Mathf.Abs((int)start.pos.x - (int)end.pos.x);
        int disY = Mathf.Abs((int)start.pos.y - (int)end.pos.y);
        int dis = Mathf.Abs(disX - disY);
        return DIAGONAL_COST * Mathf.Min(disX, disY) + STRAIGHT_COST * dis;
    }

    private GridSection GetLowestFCostGridSection(List<GridSection> GridSectionList)
    {
        GridSection lowestCostGridSection = GridSectionList[0];

        for(int i = 1; i < GridSectionList.Count; i++)
        {
            if (GridSectionList[i].fCost < lowestCostGridSection.fCost)
                lowestCostGridSection = GridSectionList[i];
        }

        return lowestCostGridSection;
    }
}
