using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2021_15 : DayScript2021
{
    protected override string part_1()
    {
        int colCount = _input.IndexOf('\n');
        int rawCount = _input.Split('\n').Length;

        int[,] map = new int[5*rawCount, 5*colCount];
        int[,] distance = new int[5*rawCount, 5*colCount];

        int index = 0;
        foreach (string line in _input.Split('\n'))
        {
            for (int cnt=0; cnt < colCount; cnt++)
            {
                int number = int.Parse(line[cnt].ToString());

                for (int tiledX = 0; tiledX < 5; tiledX++)
                {
                    for (int tiledY = 0; tiledY < 5; tiledY++)
                    {
                        int tmp = number + tiledX + tiledY;
                        map[index + rawCount*tiledX, cnt + colCount*tiledY] = ( tmp > 9 ? tmp-9 : tmp);
                        distance[index + rawCount * tiledX, cnt + colCount * tiledY] = int.MaxValue;
                    }
                }

            }
            index++;
        }
        distance[0, 0] = 0;

        List<IntVector2> neighboursToVisit = new List<IntVector2>();
        neighboursToVisit.Add(IntVector2.zero);

        List<IntVector2> pointsDone = new List<IntVector2>();

        int safetyCount = 500000;
        while (neighboursToVisit.Count > 0 && safetyCount > 0)
        {
            neighboursToVisit.Sort(
                delegate (IntVector2 itemA, IntVector2 itemB)
                { return distance[itemA.x, itemA.y].CompareTo(distance[itemB.x, itemB.y]); }
             );

            IntVector2 currentPos = neighboursToVisit[0];
            //Debug.Log("Current pos : " + currentPos);
            while (pointsDone.Exists(p => p.x == currentPos.x && p.y == currentPos.y))
            {
                neighboursToVisit.RemoveAt(0);
                currentPos = neighboursToVisit[0];
                //Debug.Log("-- Already done -- Current pos : " + currentPos);
            }

            safetyCount--;
            if (currentPos.x == (5*rawCount) - 1 && currentPos.y == (5*colCount) - 1)
                break;

            if (currentPos.x > 0 && !pointsDone.Exists(v => v.x + 1 == currentPos.x && v.y == currentPos.y))   // left neighbour
            {
                int x = currentPos.x -1, y = currentPos.y;
                IntVector2 neighbour = new IntVector2(x, y);
                distance[x, y] = Mathf.Min(distance[x, y], distance[currentPos.x, currentPos.y] + map[x, y]);

                if (!neighboursToVisit.Contains(neighbour))
                    neighboursToVisit.Add(neighbour);
            }

            if (currentPos.y > 0 && !pointsDone.Exists(v => v.x == currentPos.x && v.y + 1 == currentPos.y))   // top neighbour
            {
                int x = currentPos.x, y = currentPos.y - 1;
                IntVector2 neighbour = new IntVector2(x,y);
                distance[x, y] = Mathf.Min(distance[x, y], distance[currentPos.x, currentPos.y] + map[x, y]);

                if (!neighboursToVisit.Contains(neighbour))
                    neighboursToVisit.Add(neighbour);
            }

            if (currentPos.x < (5 * rawCount) - 1 && !pointsDone.Exists(v => v.x - 1 == currentPos.x && v.y == currentPos.y))   // right neighbour
            {
                int x = currentPos.x + 1 , y = currentPos.y;
                IntVector2 neighbour = new IntVector2(x, y);
                distance[x, y] = Mathf.Min(distance[x, y], distance[currentPos.x, currentPos.y] + map[x, y]);

                if (!neighboursToVisit.Contains(neighbour))
                    neighboursToVisit.Add(neighbour);
            }

            if (currentPos.y < (5 * colCount) - 1 && !pointsDone.Exists(v => v.x == currentPos.x && v.y -1 == currentPos.y))   // bot neighbour
            {
                int x = currentPos.x, y = currentPos.y + 1;
                IntVector2 neighbour = new IntVector2(x, y);
                distance[x, y] = Mathf.Min(distance[x, y], distance[currentPos.x, currentPos.y] + map[x, y]);

                if (!neighboursToVisit.Contains(neighbour))
                    neighboursToVisit.Add(neighbour);
            }

            neighboursToVisit.RemoveAt(0);
            pointsDone.Add(currentPos);

            //string log = "Distance map : " + System.Environment.NewLine;
            //for (int i = 0; i < 20; i++)
            //{
            //    for (int j = 0; j < 20; j++)
            //    {
            //        log += (distance[i, j] < int.MaxValue ? distance[i, j].ToString() : "N/A") + "\t";
            //    }
            //    log += System.Environment.NewLine;
            //}

            //if (safetyCount > 9990)
            //    Debug.Log(log);
        }


        Debug.LogWarning("sc : " + safetyCount);

        string log = "Distance map : " + System.Environment.NewLine;
        for (int i = 5*rawCount-20; i < 5*rawCount; i++)
        {
            for (int j = 5*colCount-20; j < 5*colCount; j++)
            {
                log += (distance[i, j] < int.MaxValue ? distance[i, j].ToString() : "N/A") + "\t";
            }
            log += System.Environment.NewLine;
        }

        return distance[5*rawCount -1, 5*colCount -1].ToString();
    }

    protected override string part_2()
    {
        return "";
    }
}
