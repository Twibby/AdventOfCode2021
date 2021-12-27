using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2021_17 : DayScript2021
{
    protected override string part_1()
    {
        string xbounds = _input.Substring(_input.IndexOf("x=") + 2, _input.IndexOf(",") - _input.IndexOf("x=") - 2);
        string ybounds = _input.Substring(_input.IndexOf("y=") + 2);
        Debug.Log(xbounds + " // " + ybounds);

        int yMin =1+ int.Parse(ybounds.Substring(0, ybounds.IndexOf(".")));

        Debug.Log(yMin);
        return (yMin * (yMin - 1) / 2).ToString();

    }

    protected override string part_2()
    {
        string xbounds = _input.Substring(_input.IndexOf("x=") + 2, _input.IndexOf(",") - _input.IndexOf("x=") - 2);
        int xMin = int.Parse(xbounds.Substring(0, xbounds.IndexOf(".")));
        int xMax = int.Parse(xbounds.Substring(xbounds.IndexOf("..") + 2));

        string ybounds = _input.Substring(_input.IndexOf("y=") + 2);
        int yMin = int.Parse(ybounds.Substring(0, ybounds.IndexOf(".")));
        int yMax = int.Parse(ybounds.Substring(ybounds.IndexOf("..")+2));

        int score = 0;

        score += (xMax - xMin +1) * (yMax - yMin +1);
        Debug.Log("range in 1 step : " + score);

        List<KeyValuePair<int, int>> correctLaunch = new List<KeyValuePair<int, int>>();
        // y must be between yMin/2 and -ymin -1   (or yMin and yMax)
        // x must be between between 1 and xMax/2  (or xMin and xMax)
        for (int y= yMax+1; y < Mathf.Abs(yMin); y++)
        {
            List<int> stepsCorrect = new List<int>();
            //get number of steps after which y is within the bounds
            int steps = 0;
            int tmpY = 0 - Mathf.Abs(y);
            if (y >= 0)
            {
                steps += (y * 2) + 1;
                tmpY--;
            }

            int sum = 0;
            while (sum >= yMin)
            {
                steps++;
                sum += tmpY;
                tmpY--;
                if (sum >= yMin && sum <= yMax)
                {
                    stepsCorrect.Add(steps);
                }
            }
            if (stepsCorrect.Count > 0)
            {
                //Debug.Log("For y=" + y + ", steps available are : " + System.String.Join(",", stepsCorrect));
            }
            else
            {
                //Debug.Log("For y=" + y + ", No steps are valid");
                continue;
            }

            for (int x = 1; x < xMin; x++)
            {
                if (x * (x + 1) / 2 < xMin)
                    continue;

                foreach (int stepCounter in stepsCorrect)
                {
                    int sumX = 0;
                    int tmpX = x;
                    for (int i=0; i <stepCounter;i++)
                    {
                        sumX += tmpX;
                        tmpX--;
                        if (tmpX <= 0 || sumX > xMax)
                            break;
                    }

                    if (sumX >= xMin && sumX <= xMax)
                    {
                        if (!correctLaunch.Contains(new KeyValuePair<int, int>(x,y)))
                        {
                            correctLaunch.Add(new KeyValuePair<int, int>(x, y));
                            //Debug.Log("Combination correct for x=" + x + " & y=" + y + " after " + stepCounter + " steps (" + sumX + ")");
                        }
                        else
                        {
                            //Debug.LogWarning("Already in existing combinations for x=" + x + " & y=" + y + " after " + stepCounter + " steps (" + sumX + ")");
                        }
                    }
                }
            }
        }
        score += correctLaunch.Count;


        return score.ToString();
    }
}
