using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2021_13 : DayScript2021
{
    protected override string part_1()
    {
        string[] inputs = _input.Split(new string[] { "\n\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        List<IntVector2> gridPoints = new List<IntVector2>();
        foreach (string point in inputs[0].Split('\n'))
        {
            string[] coords = point.Split(',');
            gridPoints.Add(new IntVector2(int.Parse(coords[0]), int.Parse(coords[1])));
        }

        long result = 0;

        //read first fold isntruction
        string instruction = inputs[1].Split('\n')[0];
        int foldingLine = int.Parse(instruction.Substring(instruction.IndexOf('=') + 1));
        if (instruction[instruction.IndexOf('=') - 1] == 'x')
        {
            List<IntVector2> newPoints = new List<IntVector2>();
            foreach (IntVector2 point in gridPoints)
            {
                if (point.x < foldingLine)
                    continue;

                if (point.x == foldingLine)
                    Debug.LogWarning("having a point on line : " + point.ToString());

                if (!gridPoints.Exists(p => p.x == point.x - 2 * (point.x - foldingLine) && p.y == point.y))
                    newPoints.Add(new IntVector2(point.x - 2 * (point.x - foldingLine), point.y));
            }
            gridPoints.AddRange(newPoints);
            foreach (IntVector2 point in gridPoints)
            {
                if (point.x < foldingLine)
                    result++;
            }
        }
        else
        {
            List<IntVector2> newPoints = new List<IntVector2>();
            foreach (IntVector2 point in gridPoints)
            {
                if (point.y < foldingLine)
                    continue;

                if (point.y == foldingLine)
                    Debug.LogWarning("having a point on col : " + point.ToString());

                if (!gridPoints.Exists(p => p.y == point.y - 2 * (point.y - foldingLine) && p.x == point.x))
                    newPoints.Add(new IntVector2(point.x, point.y - 2 * (point.y - foldingLine)));
            }
            gridPoints.AddRange(newPoints);
            foreach (IntVector2 point in gridPoints)
            {
                if (point.y < foldingLine)
                    result++;
            }
        }

        return result.ToString();
    }

    protected override string part_2()
    {
        string[] inputs = _input.Split(new string[] { "\n\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        List<IntVector2> gridPoints = new List<IntVector2>();
        foreach (string point in inputs[0].Split('\n'))
        {
            string[] coords = point.Split(',');
            gridPoints.Add(new IntVector2(int.Parse(coords[0]), int.Parse(coords[1])));
        }

        //read first fold isntruction
        foreach (string instruction in inputs[1].Split('\n'))
        {
            int foldingLine = int.Parse(instruction.Substring(instruction.IndexOf('=') + 1));
            if (instruction[instruction.IndexOf('=') - 1] == 'x')
            {
                List<IntVector2> newPoints = new List<IntVector2>();
                foreach (IntVector2 point in gridPoints)
                {
                    if (point.x < foldingLine)
                        continue;

                    if (point.x == foldingLine)
                        Debug.LogWarning("having a point on line : " + point.ToString());

                    if (!gridPoints.Exists(p => p.x == point.x - 2 * (point.x - foldingLine) && p.y == point.y))
                        newPoints.Add(new IntVector2(point.x - 2 * (point.x - foldingLine), point.y));
                }
                gridPoints.RemoveAll(point => point.x > foldingLine);
                gridPoints.AddRange(newPoints);

            }
            else
            {
                List<IntVector2> newPoints = new List<IntVector2>();
                foreach (IntVector2 point in gridPoints)
                {
                    if (point.y < foldingLine)
                        continue;

                    if (point.y == foldingLine)
                        Debug.LogWarning("having a point on col : " + point.ToString());

                    if (!gridPoints.Exists(p => p.y == point.y - 2 * (point.y - foldingLine) && p.x == point.x))
                        newPoints.Add(new IntVector2(point.x, point.y - 2 * (point.y - foldingLine)));
                }
                gridPoints.RemoveAll(point => point.y > foldingLine);
                gridPoints.AddRange(newPoints);
            }
        }

        int xMax = 0, yMax = 0;
        foreach (IntVector2 point in gridPoints)
        {
            xMax = Mathf.Max(xMax, point.x);
            yMax = Mathf.Max(yMax, point.y);
        }

        xMax++;
        yMax++;

        char[,] grid = new char[xMax, yMax];
        for (int i = 0; i < xMax; i++)
        {
            for (int j = 0; j < yMax; j++)
            {
                grid[i, j] = '.';
            }
        }

        foreach (IntVector2 point in gridPoints)
        {
            grid[point.x, point.y] = '#';
        }

        string log = "";
        for (int i = 0; i < yMax; i++)
        {
            for (int j = 0; j < xMax; j++)
            {
                log += grid[j, i];
            }
            log += System.Environment.NewLine;
        }
        Debug.Log(log);

        return "";
    }
}