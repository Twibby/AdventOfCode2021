using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2021_14 : DayScript2021
{
    protected override string part_1()
    {
        return getDifferenceCharOccurences(10).ToString();
    }

    protected override string part_2()
    {
        return getDifferenceCharOccurences(40).ToString();
    }

    public long getDifferenceCharOccurences(int counter)
    {
        string[] inputs = _input.Split(new string[] { "\n\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        Dictionary<string, long> pairOccurences = new Dictionary<string, long>();
        for (int i = 0; i < inputs[0].Length - 1; i++)
        {
            if (!pairOccurences.ContainsKey(inputs[0].Substring(i, 2)))
                pairOccurences.Add(inputs[0].Substring(i, 2), 0);

            pairOccurences[inputs[0].Substring(i, 2)] += 1;
        }

        char firstChar = inputs[0][0];
        char lastChar = inputs[0][inputs[0].Length - 1];

        Dictionary<string, char> rules = new Dictionary<string, char>();
        foreach (string rule in inputs[1].Split('\n'))
        {
            rules.Add(rule.Substring(0, 2), rule[rule.Length - 1]);
        }

        for (int step = 0; step < counter; step++)
        {
            Dictionary<string, long> newPairOccurence = new Dictionary<string, long>();
            foreach (var pair in pairOccurences)
            {
                if (!rules.ContainsKey(pair.Key))
                {
                    if (!newPairOccurence.ContainsKey(pair.Key))
                        newPairOccurence.Add(pair.Key, 0);

                    newPairOccurence[pair.Key] += pair.Value;
                }
                else
                {
                    char a = pair.Key[0];
                    char c = pair.Key[1];
                    char b = rules[pair.Key];

                    string firstPair = a.ToString() + b.ToString();
                    if (!newPairOccurence.ContainsKey(firstPair))
                        newPairOccurence.Add(firstPair, 0);

                    newPairOccurence[firstPair] += pair.Value;

                    string secondPair = b.ToString() + c.ToString();
                    if (!newPairOccurence.ContainsKey(secondPair))
                        newPairOccurence.Add(secondPair, 0);

                    newPairOccurence[secondPair] += pair.Value;
                }
            }

            pairOccurences = new Dictionary<string, long>(newPairOccurence);
        }

        // Count each char
        Dictionary<char, long> counts = new Dictionary<char, long>();
        foreach (var pair in pairOccurences)
        {
            char a = pair.Key[0];
            char b = pair.Key[1];

            if (!counts.ContainsKey(a))
                counts.Add(a, 0);

            counts[a] += pair.Value;

            if (!counts.ContainsKey(b))
                counts.Add(b, 0);

            counts[b] += pair.Value;
        }

        if (counts.ContainsKey(firstChar))
            counts[firstChar] += 1;
        if (counts.ContainsKey(lastChar))
            counts[lastChar] += 1;

        Dictionary<char, long> finalCounts = new Dictionary<char, long>();
        long max = 0, min = long.MaxValue;
        foreach (var count in counts)
        {
            if (count.Value % 2 != 0)
            {
                Debug.LogWarning("weird ! " + count.Key + " -> " + count.Value);
            }

            finalCounts[count.Key] = count.Value / 2;

            if (finalCounts[count.Key] > max)
                max = finalCounts[count.Key];
            if (finalCounts[count.Key] < min)
                min = finalCounts[count.Key];
        }

        return (max - min);
    }
}
