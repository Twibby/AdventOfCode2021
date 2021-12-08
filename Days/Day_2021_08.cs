using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Day_2021_08 : DayScript2021
{
    protected override string part_1()
    {
        int count = 0;
        int debugIndex = 3;
        foreach (string line in _input.Split('\n'))
        {
            string log = "";
            foreach (string digit in line.Substring(line.IndexOf('|') + 2).Split(' '))
            {
                log += "'" + digit + "', ";
                if (isEasyDigit(digit))
                    count++;
            }

            if (debugIndex > 0)
                Debug.Log(line + " | " + log);


            debugIndex--;
        }
        return count.ToString();
    }

    bool isEasyDigit(string digit)
    {
        return digit.Length < 5 || digit.Length > 6;
    }

    protected override string part_2()
    {
        int count = 0;
        int debugIndex = 3;
        foreach (string line in _input.Split('\n'))
        {
            if (debugIndex > 0)
                Debug.LogWarning("Starting line : " + line);

            Dictionary<char, char> associations = GetPermutations(line.Substring(0, line.IndexOf('|') - 1).Split(' ').ToList(), debugIndex >0 );

            List<string> encryptedCodeSegments = line.Substring(line.IndexOf('|') + 2).Split(' ').ToList();

            count += GetCodeFrom4Segments(encryptedCodeSegments, associations, debugIndex > 0);

            debugIndex--;
        }
        return count.ToString();
    }

    /// <summary>
    /// /!\ Assume input is correct : there are 9 segments which all refers to a unique other segment (bijection)
    /// </summary>
    /// <param name="input"></param>
    /// <param name="debug"></param>
    /// <returns></returns>
    public Dictionary<char, char> GetPermutations(List<string> input, bool debug = false)
    {
        if (debug)
            Debug.Log("Uncrypting line : " + System.String.Join(" ", input));
        
        // Disclaimer : reading below, you might think my dictionary is strange it's reverted but it's easier that way for uncrypting phase. 
        Dictionary<char, char> associations = new Dictionary<char, char>() { { 'a', '0' }, { 'b', '0' }, { 'c', '0' }, { 'd', '0' }, { 'e', '0' }, { 'f', '0' }, { 'g', '0' } };

        // First get easy digit 1 4 and 7
        string digit1 = input.Find(x => x.Length == 2);
        string digit7 = input.Find(x => x.Length == 3);
        string digit4 = input.Find(x => x.Length == 4);

        if (debug)
            Debug.Log(" 1 is " + digit1 + ", 4 is " + digit4 + " and 7 is " + digit7);

        // 1. find 'a' thanks to 7-1 segments (acf - cf => a)
        associations[digit7.Replace(digit1[0].ToString(), "").Replace(digit1[1].ToString(), "")[0]] = 'a';

        // 2. find 'b', 'e' and 'f' thatnks to their occurrences
        char charB = '0', charF = '0', charE = '0';
        for (char c = 'a'; c <='g'; c++)
        {
            int occurences = input.Count(x => x.Contains(c));
            if (occurences == 6)
            {
                charB = c;
                associations[c] = 'b';
            }
            else if (occurences == 4)
            {
                charE = c;
                associations[c] = 'e';
            }
            else if (occurences == 9)
            {
                charF = c;
                associations[c] = 'f';
            }
        }

        if (debug)
            Debug.Log("char B is " + charB + ", charE is " + charE + " and charF is " + charF);

        // 3. find 'c' with 1 and we already know f
        char charC = digit1.Replace(charF.ToString(), "")[0];
        associations[charC] = 'c';
        if (debug)
            Debug.Log("char C is " + charC);

        // 4. find 'd' with 4 and we already know b,c,f
        associations[digit4.Replace(charF.ToString(), "").Replace(charB.ToString(), "").Replace(charC.ToString(),"")[0]] = 'd';

        // 5. find last char 'g' by elimination
        char charG = '0';
        foreach (var pair in associations) {  if (pair.Value == '0') { charG = pair.Key; break; } }
        if (charG != '0')
            associations[charG] = 'g';


        if (debug)
            Debug.Log("Final associations are : " + System.String.Join(", ", associations));

        // Even if we assume input is correct, and i know my devs are perfect, just put a verification if :p
        if (associations.ContainsValue('0'))
        {
            Debug.LogError("GetPermutations doesn't work properly");
            string log = "";
            foreach (var pair in associations) { log += pair.Value != '0' ? pair.Key + "->" + pair.Value + ", " : pair.Key + " has no associated value !!, "; }
            Debug.Log(log);
            return null;
        }


        return associations;
    }

    /// <summary>
    /// Give 4 segments, uncrypt them and get digit associated to them. Then return parsed code of 4 digits
    /// </summary>
    /// <param name="segments"></param>
    /// <param name="associations"></param>
    /// <param name="debug"></param>
    /// <returns></returns>
    public int GetCodeFrom4Segments(List<string> segments, Dictionary<char, char> associations, bool debug = false)
    {
        if (debug)
            Debug.Log("Now uncrypting and gettint output displayed");

        if (segments.Count != 4)
        {
            Debug.LogError("Segments not correctly formated");
            return -1;
        }

        List<string> uncryptedSegments = new List<string>();
        foreach (string seg in segments)
        {
            string res = "";
            foreach (char c in seg) { res += associations[c]; }
            uncryptedSegments.Add(res);

            if (debug)
                Debug.Log(" * " + seg + " -> " + res);
        }



        string code = "";
        uncryptedSegments.ForEach(x => code += GetDigitFromSegments(x));

        if (debug)
            Debug.Log("Code of '" + System.String.Join(", ", segments) + "' ==> " + code);

        return int.Parse(code);
    }

    public string GetDigitFromSegments(string digitSegments)
    {
        string sortedSegments = System.String.Concat(digitSegments.OrderBy(c => c));    // sort alphabetically
         
        switch (sortedSegments)
        {
            case "abcefg":  return "0";
            case "cf":      return "1";
            case "acdeg":   return "2";
            case "acdfg":   return "3";
            case "bcdf":    return "4";
            case "abdfg":   return "5";
            case "abdefg":  return "6";
            case "acf":     return "7";
            case "abcdefg": return "8";
            case "abcdfg":  return "9";
            default:
                Debug.LogError("NO digit associated to segment : " + digitSegments + " -> " + sortedSegments);
                return "0";
        }
    }
}
