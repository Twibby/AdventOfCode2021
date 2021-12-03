using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2021_01 : DayScript2021
{
    protected override string part_1()
    {
        int previousDepth = int.MaxValue, currentDepth = 0;
        int counter = 0;
        foreach (string depth in _input.Split('\n'))
        {
            if (int.TryParse(depth, out currentDepth))
            {
                if (currentDepth > previousDepth)
                    counter++;

                previousDepth = currentDepth;
            }
            else
                Debug.LogError("[part_1] Couldn't parse int of '" + depth + "'");
        }

        return counter.ToString();            
    }


    protected override string part_2()
    {
        int previousDepth1 = int.MaxValue, previousDepth2 = int.MaxValue, previousDepth3 = int.MaxValue, currentDepth = 0;
        int counter = 0;
        foreach (string depth in _input.Split('\n'))
        {
            if (int.TryParse(depth, out currentDepth))
            {
                if (currentDepth > previousDepth1)
                    counter++;

                previousDepth1 = previousDepth2;
                previousDepth2 = previousDepth3;
                previousDepth3 = currentDepth;
            }
            else
                Debug.LogError("[part_2] Couldn't parse int of '" + depth + "'");
        }

        return counter.ToString();
    }
}
