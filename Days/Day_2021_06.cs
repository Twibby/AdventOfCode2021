using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Day_2021_06 : DayScript2021
{
    protected override string part_1()
    {
        int breed_cycle = 6;
        int first_breed_offset = 2;
        int daysCount = 80;

        return computeFishExpansion(breed_cycle, first_breed_offset, daysCount);
    }

    protected override string part_2()
    {
        int breed_cycle = 6;
        int first_breed_offset = 2;
        int daysCount = 256;

        return computeFishExpansion(breed_cycle, first_breed_offset, daysCount);
    }

    string computeFishExpansion(int breedCycle, int firstBreedOffset, int daysCount)
    {
        List<int> fishes = _input.Split(',').Select(int.Parse).ToList();

        List<long> groupedFishes = new List<long>();
        for (int i = 0; i <= breedCycle+firstBreedOffset; i++) { groupedFishes.Add(0); }
        foreach (int fish in fishes) { groupedFishes[fish] += 1; }

        for (int day = 0; day < daysCount; day++)
        {
            long tmp = groupedFishes[0];

            for (int i = 1; i < groupedFishes.Count; i ++)
            {
                groupedFishes[i - 1] = groupedFishes[i];
            }

            groupedFishes[breedCycle] += tmp;
            groupedFishes[breedCycle + firstBreedOffset] = tmp;
        }

        long sum = 0;
        groupedFishes.ForEach(x => sum += x);
        return sum.ToString();
    }
}
