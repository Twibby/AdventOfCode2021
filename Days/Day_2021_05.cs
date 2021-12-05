using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Day_2021_05 : DayScript2021
{
    protected override string part_1()
    {
        return day5Function(false).ToString();
    }

    protected override string part_2()
    {
        return day5Function(true).ToString();
    }

    public class StraightVent
    {
        public int from;
        public int to;
        public int constante;

        public StraightVent (int pFrom, int pTo, int pConstante)
        {
            this.from = pFrom;
            this.to = pTo;
            this.constante = pConstante;
        }
    }

    /// <summary>
    /// assume that to.x is always greater than from.y by construction
    /// </summary>
    public class DiagonalVent
    {
        public (int,int) from;
        public (int,int) to;
        public bool yIncreasing;

        public DiagonalVent(int pX1, int pY1, int pX2, int pY2)
        {
            if (pX1 < pX2)
            {
                this.from = (pX1, pY1);
                this.to = (pX2, pY2);
                this.yIncreasing = pY1 < pY2;
            }
            else
            {
                this.from = (pX2, pY2);
                this.to = (pX1, pY1);
                this.yIncreasing = pY2 < pY1;
            }

            if (Mathf.Abs(to.Item1 - from.Item1) != Mathf.Abs(to.Item2 - from.Item2))
                Debug.LogWarning("Diagonal vent is not 45° angle ! " + pX1 + ", " + pY1 + " -> " + pX2 + ", " + pY2);
        }

        public List<(int,int)> GetPoints(bool debug = false)
        {
            List<(int, int)> result = new List<(int, int)>();

            for (int i = 0; i <= to.Item1 - from.Item1; i++)
            {
                result.Add((from.Item1 + i, from.Item2 + (yIncreasing ? i : 0 - i)));
            }

            return result;
        }

        //public (int,int) getMaximums() { return (to.Item1, yIncreasing ? to.Item2 : from.Item2); }
    }

    public int day5Function(bool countDiagonals)
    {
        List<StraightVent> horizontalsVent = new List<StraightVent>();
        List<StraightVent> verticalsVent = new List<StraightVent>();
        List<DiagonalVent> diagonalVents = new List<DiagonalVent>();

        // Parsing inputs, creating straight vents
        int maxX = 0, maxY = 0;
        foreach (string vent in _input.Split('\n'))
        {
            List<int> values = vent.Split(new string[] { " -> ", "," }, System.StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            // must have 4 values to have correct segment
            if (values.Count != 4) { Debug.LogError("Not 4 values in this line : '" + vent + "'"); continue; }

            // if this is not a straight vent
            if (values[0] != values[2] && values[1] != values[3]) 
            {
                if (countDiagonals) //count it in part2
                {
                    DiagonalVent diagVent = new DiagonalVent(values[0], values[1], values[2], values[3]);
                    maxX = Mathf.Max(maxX, diagVent.to.Item1);
                    maxY = Mathf.Max(maxY, diagVent.yIncreasing ? diagVent.to.Item2 : diagVent.from.Item2);
                    diagonalVents.Add(diagVent);
                }
                else
                {
                    // ignore line if not straight (means x or y doesn't change)
                    Debug.Log(vent + " is not a straight vent, ignore it for part1"); 
                }
                continue;
            }                

            int constante, from, to;
            if (values[0] == values[2])
            {
                constante = values[0];
                if (values[1] <= values[3])
                {
                    from = values[1];
                    to = values[3];
                }
                else
                {
                    from = values[3];
                    to = values[1];
                }

                maxY = Mathf.Max(maxY, to);
                verticalsVent.Add(new StraightVent(from, to, constante));
            }
            else
            {
                constante = values[1];
                if (values[0] <= values[2])
                {
                    from = values[0];
                    to = values[2];
                }
                else
                {
                    from = values[2];
                    to = values[0];
                }

                maxX = Mathf.Max(maxX, to);
                horizontalsVent.Add(new StraightVent(from, to, constante));
            }
        }

        Debug.Log("Max ranges are " + maxX + ", " + maxY);

        // Creating grid and filling it horizontal vents and vertical vents
        int[,] grid = new int[maxX + 1, maxY + 1];
        foreach (StraightVent vent in horizontalsVent)
        {
            for (int i = vent.from; i <= vent.to; i++)
            {
                grid[i, vent.constante] += 1;
            }
        }

        foreach (StraightVent vent in verticalsVent)
        {
            for (int i = vent.from; i <= vent.to; i++)
            {
                grid[vent.constante, i] += 1;
            }
        }

        if (countDiagonals)
        {
            foreach (DiagonalVent vent in diagonalVents)
            {
                foreach (var point in vent.GetPoints())
                {
                    grid[point.Item1, point.Item2] += 1;
                }
            }
        }

        // Going through grid to count overlaps
        int count = 0;
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j] > 1)
                    count++;
            }
        }

        return count;
    }
}
