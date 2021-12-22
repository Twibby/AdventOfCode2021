using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2021_11 : DayScript2021
{
    protected override string part_1()
    {
        int[,] dumbos = new int[10, 10];
        int row = 0;
        foreach (string line in _input.Split('\n'))
        {
            for (int j=0; j < line.Length;j++)
            {
                dumbos[row, j] = int.Parse(line[j].ToString());
            }
            row++;
        }

        int counter = 0;
        
        for (int step = 1; step <= 100; step++)
        {
            List<(int, int)> flashingPoints = new List<(int, int)>();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    dumbos[i, j] += 1;
                    if (dumbos[i, j] > 9)
                        shine(i, j, ref dumbos, ref flashingPoints);
                }
            }

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (dumbos[i, j] > 9)
                    {
                        counter++;
                        dumbos[i, j] = 0;
                    }
                }
            }
            Debug.Log("After step " + step + ", number of flash : " + counter);
        }

        return counter.ToString();
    }

    void shine(int x, int y, ref int[,] grid, ref List<(int, int)> flashingPoints)
    {
        if (flashingPoints.Contains((x, y)))
            return;

        flashingPoints.Add((x, y));
        List<KeyValuePair<int,int>> neighbours = new List<KeyValuePair<int,int>>();
        if (x > 0 && y > 0)
            neighbours.Add(new KeyValuePair<int,int>(x - 1, y - 1));
        if (y > 0)
            neighbours.Add(new KeyValuePair<int,int>(x, y - 1));
        if (x < 9 && y > 0)
            neighbours.Add(new KeyValuePair<int,int>(x + 1, y - 1));
        if (x > 0)
            neighbours.Add(new KeyValuePair<int,int>(x - 1, y));
        if (x < 9)
            neighbours.Add(new KeyValuePair<int,int>(x + 1, y));
        if (x > 0 && y < 9)
            neighbours.Add(new KeyValuePair<int,int>(x - 1, y + 1));
        if (y < 9)
            neighbours.Add(new KeyValuePair<int,int>(x, y + 1));
        if (x < 9 && y < 9)
            neighbours.Add(new KeyValuePair<int,int>(x + 1, y + 1));

        foreach (var neighbour in neighbours)
        {
            //Debug.Log(neighbour);
            grid[neighbour.Key, neighbour.Value] += 1;
            if (grid[neighbour.Key, neighbour.Value] > 9)
                shine(neighbour.Key, neighbour.Value, ref grid, ref flashingPoints);
        }

    }

    protected override string part_2()
    {
        int[,] dumbos = new int[10, 10];
        int row = 0;
        foreach (string line in _input.Split('\n'))
        {
            for (int j = 0; j < line.Length; j++)
            {
                dumbos[row, j] = int.Parse(line[j].ToString());
            }
            row++;
        }

        int counter = 0;

        for (int step = 1; step <= 1000; step++)    // 1000 is safety count
        {
            List<(int, int)> flashingPoints = new List<(int, int)>();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    dumbos[i, j] += 1;
                    if (dumbos[i, j] > 9)
                        shine(i, j, ref dumbos, ref flashingPoints);
                }
            }

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (dumbos[i, j] > 9)
                    {
                        counter++;
                        dumbos[i, j] = 0;
                    }
                }
            }
            Debug.Log("After step " + step + ", number of flash : " + counter);
            if (flashingPoints.Count == 100)
                break;
        }

        return counter.ToString();
    }
}
