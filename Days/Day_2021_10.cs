using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2021_10 : DayScript2021
{
    protected override string part_1()
    {
        int score = 0;
        foreach (string line in _input.Split('\n'))
        {
            bool goOn = true;
            Stack<char> pile = new Stack<char>();
            foreach (char c in line)
            {
                switch (c)
                {
                    case '(':
                    case '<':
                    case '[':
                    case '{':
                        pile.Push(c); break;

                    case ')':
                        if (pile.Pop() != '(')
                        {
                            score += 3;
                            goOn = false;
                        }
                        break;

                    case ']':
                        if (pile.Pop() != '[')
                        {
                            score += 57;
                            goOn = false;
                        }
                        break;

                    case '}':
                        if (pile.Pop() != '{')
                        {
                            score += 1197;
                            goOn = false;
                        }
                        break;


                    case '>':
                        if (pile.Pop() != '<')
                        {
                            score += 25137;
                            goOn = false;
                        }
                        break;
                }

                if (!goOn)
                    break;
            }
        }

        return score.ToString();
    }

    protected override string part_2()
    {
        List<long> scores = new List<long>();
        foreach (string line in _input.Split('\n'))
        {
            bool goOn = true;
            Stack<char> pile = new Stack<char>();
            foreach (char c in line)
            {
                switch (c)
                {
                    case '(':
                    case '<':
                    case '[':
                    case '{':
                        pile.Push(c); break;

                    case ')':
                        if (pile.Pop() != '(')
                            goOn = false;
                        break;
                    case ']':
                        if (pile.Pop() != '[')
                            goOn = false;
                        break;
                    case '}':
                        if (pile.Pop() != '{')
                            goOn = false;
                        break;
                    case '>':
                        if (pile.Pop() != '<')
                            goOn = false;
                        break;
                }

                if (!goOn)
                    break;
            }

            if (!goOn)  // corrupted line
                continue;

            long lineScore = 0;
            while (pile.Count > 0)
            {
                lineScore *= 5;
                switch (pile.Pop())
                {
                    case '(': lineScore += 1; break;
                    case '[': lineScore += 2; break;
                    case '{': lineScore += 3; break;
                    case '<': lineScore += 4; break;
                    default: Debug.LogError("weird ?");break;
                }
            }
            scores.Add(lineScore);
        }

        scores.Sort();

        //Debug.Log(System.String.Join("; ", scores));

        return scores[scores.Count/2].ToString();
    }
}
