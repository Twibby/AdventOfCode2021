using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2021_20 : DayScript2021
{
    private string _algorithm;

    private int _rawCount, _colCount;
    private int[,] _grid;

    private int _offset = 5;

    protected override string part_1()
    {
        _offset = 5;
        init();
        return auxFunction(2);
    }

    protected override string part_2()
    {
        _offset = 55; 
        init(false);        
        StartCoroutine(coPart2(50));
        return "";
        //return auxFunction(50);
    }

    void init(bool debug = false)
    {
        string[] inputs = _input.Split(new string[] { "\n\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        _algorithm = inputs[0];

        _rawCount = inputs[1].Split('\n').Length + 2 * _offset;
        _colCount = inputs[1].Split('\n')[0].Length + 2 * _offset;

        _grid = new int[_rawCount, _colCount];
        for (int i = 0; i < _rawCount; i++)
        {
            for (int j = 0; j < _colCount; j++)
            {
                _grid[i, j] = 0;
            }
        }


        int rawIndex = 0;
        foreach (string raw in inputs[1].Split('\n'))
        {
            for (int colIndex = 0; colIndex < raw.Length; colIndex++)
            {
                _grid[rawIndex + _offset, colIndex + _offset] = (raw[colIndex] == '#' ? 1 : 0);
            }
            rawIndex++;
        }

        if (debug)
        {
            Debug.Log("After Init : ");
            Debug.Log("raw count is " + _rawCount + " and col Count is " + _colCount);
            printGrid();
        }
    }

    IEnumerator coPart2(int stepCount)
    {
        string result = "";
        for (int cnt = 0; cnt < stepCount; cnt++)
        {
            Debug.Log("Start step " + (cnt + 1).ToString());
            result = auxFunction(1, true);
            Debug.Log("End step " + (cnt + 1).ToString() + " with result " + result);

            yield return new WaitForSeconds(0.2f);
        }

        printGrid();
    }

    string auxFunction(int stepCount, bool print = false)
    {
        for (int step = 1; step <= stepCount; step++)
        {
            applyEnhancement(print);
            
            if (print)
            {
                Debug.Log("After step " + step);
                printGrid();
            }
        }

        long result = 0;
        for (int i = 0; i < _rawCount; i++)
        {
            for (int j = 0; j < _colCount; j++)
            {
                if (_grid[i, j] == 1)
                    result++;
            }
        }
        
        return result.ToString();
    }



    void printGrid()
    {
        string log = "";
        for (int i = 0; i < _rawCount; i++)
        {
            for (int j = 0; j < _colCount; j++)
            {
                log += _grid[i, j].ToString();
            }
            log += System.Environment.NewLine;
        }

        Debug.Log(log);
    }

    void applyEnhancement(bool debug = false)
    {
        int[,] newGrid = new int[_rawCount, _colCount];

        for (int i=0; i<_rawCount; i++)
        {
            for (int j=0;j<_colCount; j++)
            {
                // Compute i,j pos
                string number = getNumberNeighbours(i, j);
                int index = System.Convert.ToInt32(number, 2);
                newGrid[i, j] = _algorithm[index] == '#' ? 1 : 0;
                
                if (debug && i== _offset+2 && j==_offset+2)
                {
                    Debug.Log("For pos : " + i + ", " + j + " : number is '" + number + "' and worths " + index + " -> " + _algorithm[index] + " -> " + newGrid[i,j]);
                }
            }
        }

        //reinject in grid
        for (int i = 0; i < _rawCount; i++)
        {
            for (int j = 0; j < _colCount; j++)
            {
                _grid[i, j] = newGrid[i, j];
            }
        }
         
    }

    string getNumberNeighbours(int i, int j)
    {
        string result = "";

        result += _grid[Mathf.Max(i - 1, 0), Mathf.Max(j - 1, 0)];
        result += _grid[Mathf.Max(i - 1, 0), j ];
        result += _grid[Mathf.Max(i - 1, 0), Mathf.Min(j + 1, _colCount-1)];

        result += _grid[ i , Mathf.Max(j - 1, 0)];
        result += _grid[ i , j];
        result += _grid[ i , Mathf.Min(j + 1, _colCount - 1)];

        result += _grid[Mathf.Min(i + 1, _rawCount-1), Mathf.Max(j - 1, 0)];
        result += _grid[Mathf.Min(i + 1, _rawCount - 1), j];
        result += _grid[Mathf.Min(i + 1, _rawCount - 1), Mathf.Min(j + 1, _colCount - 1)];

        return result;
    }

}
