using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Day_2021_07 : DayScript2021
{
    protected override string part_1()
    {
        List<long> crabs = _input.Split(',').Select(long.Parse).ToList();

        long minSum = crabs.Sum();
        for (int i = 0; i <= crabs.Max(); i++)
        {
            List<long> tmp = new List<long>(crabs);
            for (int crab = 0; crab < tmp.Count; crab++)
            {
                tmp[crab] = (long)Mathf.Abs(tmp[crab] - i);
            }

            if (tmp.Sum() < minSum)
                minSum = tmp.Sum();
        }

        return minSum.ToString();
    }

    protected override string part_2()
    {
        List<long> crabs = _input.Split(',').Select(long.Parse).ToList();

        long minSum = long.MaxValue;
        for (int i = 0; i <= crabs.Max(); i++)
        {
            List<long> tmp = new List<long>(crabs);
            for (int crab = 0; crab < tmp.Count; crab++)
            {
                long diff = (long)Mathf.Abs(tmp[crab] - i);
                tmp[crab] = diff * (diff + 1) / 2;
            }

            if (tmp.Sum() < minSum)
                minSum = tmp.Sum();
        }

        return minSum.ToString();
    }
}
