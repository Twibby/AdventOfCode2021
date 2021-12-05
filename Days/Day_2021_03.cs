using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2021_03 : DayScript2021
{
    protected override string part_1()
    {
        List<int> counts = new List<int>();
        for (int i = 0; i < _input.IndexOf('\n') ; i++) { counts.Add(0); }

        int lineCount = 0;
        foreach (string line in _input.Split('\n'))
        {
            //Debug.Log("*" + line.Length + " : " + line);
            for (int i = 0; i < line.Length; i++)
            {
                //Debug.Log(" * * " + i + " -> " + line[i]);
                counts[i] += (line[i] == '1') ? 1 : 0;
            }
            lineCount++;
        }

        string gammaBin = ""; //, epsilonBin = "";
        foreach (int cnt in counts)
        {
            gammaBin += (2 * cnt > lineCount) ? "1" : "0";
            //epsilonBin += (2 * cnt > lineCount) ? "0" : "1";
        }
        int gammaDec = System.Convert.ToInt32(gammaBin, 2);
        int smartEpsilon = (int)Mathf.Pow(2, counts.Count) - gammaDec -1;
        //Debug.Log("gamma is " + System.Convert.ToInt32(gammaBin, 2) + " | epsilon is " + System.Convert.ToInt32(epsilonBin, 2) + " | smart epsilon is : " + smartEpsilon);
        return (gammaDec * smartEpsilon).ToString();   //System.Convert.ToInt32(gammaBin, 2) * System.Convert.ToInt32(epsilonBin, 2)).ToString();

    }

    protected override string part_2()
    {
        // O2 capacity
        List<string> inputs = new List<string>(_input.Split('\n'));

        int bitPosition = 0;
        do
        {
            char c = isMostCommonBit1(inputs, bitPosition) ? '0' : '1';
            inputs.RemoveAll(x => x[bitPosition] == c);

            bitPosition++;
        } while (inputs.Count > 1 && bitPosition < inputs[0].Length);

        if (inputs.Count >1 )   // means bitPosition is too high, shouldn't be here)
        {
            string dlog = "Inputs remaining : ";
            inputs.ForEach(x => dlog += x + " / ");
            Debug.LogWarning(dlog);
            return "0";
        }

        int O2capacity = System.Convert.ToInt32(inputs[0], 2);

        // CO2 scrubber rating
        inputs = new List<string>(_input.Split('\n'));
        bitPosition = 0;
        do
        {
            char c = isMostCommonBit1(inputs, bitPosition) ? '1' : '0';
            inputs.RemoveAll(x => x[bitPosition] == c);

            bitPosition++;
        } while (inputs.Count > 1 && bitPosition < inputs[0].Length);

        if (inputs.Count > 1)   // means bitPosition is too high, shouldn't be here)
        {
            string dlog = "Inputs remaining : ";
            inputs.ForEach(x => dlog += x + " / ");
            Debug.LogWarning(dlog);
            return "0";
        }

        int CO2rating = System.Convert.ToInt32(inputs[0], 2);

        return (O2capacity * CO2rating).ToString();
    }

    /// <summary>
    /// Takes a list of binary numbers (string format) eg "01101011", all same size, and a bit position
    /// Then returns if there is more 1 or not at that position
    /// eg {"101", "011", "111"}, position 0 -> there are 2 1 and 1 0, so returns true
    /// Returns true if it's equal
    /// </summary>
    /// <param name="numbers">List of binary numbers (in string format), they should all have same length</param>
    /// <param name="position">int that refers to bit position we're looking for. must be less than all numbers length</param>
    /// <returns>True if more 1 than 0 at position in numbers</returns>
    private bool isMostCommonBit1(List<string> numbers, int position)
    {
        int count1 = 0;
        foreach (string line in numbers)
        {
            if (position > line.Length)
            {
                Debug.LogWarning("weird " + position + " | " + line);
                continue;
            }

            if (line[position] == '1')
                count1++;
        }

        return 2 * count1 >= numbers.Count;
    }
}
