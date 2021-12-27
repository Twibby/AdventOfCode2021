using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2021_25 : DayScript2021
{
    protected override string part_1()
    {
        int colCount = _input.IndexOf('\n');
        int rawCount = _input.Split('\n').Length;

        Debug.LogError(rawCount + " / " + colCount);
        char[,] map = new char[rawCount, colCount];
        int index = 0;
        foreach (string line in _input.Split('\n'))
        {
            Debug.Log(line);
            for (int cnt = 0; cnt <colCount; cnt++)
            {
                //Debug.Log(line[cnt]);
                map[index, cnt] = line[cnt];
            }
            index++;
        }
        string prelog = "STEP 0 : " + System.Environment.NewLine;
        for (int i = 0; i < rawCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {
                prelog += map[i, j];
            }
            prelog += System.Environment.NewLine;
        }
        Debug.Log(prelog);

        int stepCounter = 0;
        bool hasSomethingMoved = true;
        int safetyCount = 10000;
        while (hasSomethingMoved && safetyCount > 0)
        {
            hasSomethingMoved = false;
            stepCounter++;

            char[,] interState = new char[rawCount, colCount];
            //copy 
            for (int i=0; i < rawCount;i++)
            {
                for (int j = 0; j<colCount; j++)
                {
                    interState[i, j] = map[i, j];
                }
            }

            // move est-side cucumber
            for (int i = 0; i < rawCount; i++)
            {
                if (map[i, 0] == '.' && map[i, colCount - 1] == '>')
                {
                    interState[i, 0] = '>';
                    interState[i, colCount - 1] = '.';
                    hasSomethingMoved = true;
                }

                for (int j = 0; j < colCount - 1; j++)
                {
                    if (map[i, j] == '>' && map[i, j + 1] == '.')
                    {
                        interState[i, j] = '.';
                        interState[i, j + 1] = '>';
                        hasSomethingMoved = true;
                        j++;
                    }
                }
            }


            //copy 
            for (int i = 0; i < rawCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    map[i, j] = interState[i, j];
                }
            }

            // move south-side cucumber
            for (int j = 0; j < colCount; j++)
            {
                for (int i = 0; i < rawCount - 1; i++)
                {
                    if (interState[i, j] == 'v' && interState[i + 1, j] == '.')
                    {
                        map[i, j] = '.';
                        map[i + 1, j] = 'v';
                        hasSomethingMoved = true;
                        i++;
                    }
                }

                if (interState[0, j] == '.' && interState[rawCount-1,j] == 'v')
                {
                    map[rawCount-1, j] = '.';
                    map[0, j] = 'v';
                    hasSomethingMoved = true;
                }
            }

            safetyCount--;
        }

        Debug.Log("stop because of safety count ? " + (safetyCount <= 0).ToString());

        return stepCounter.ToString();

    }

    protected override string part_2()
    {
        return base.part_2();
    }
}
