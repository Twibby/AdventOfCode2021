using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2021_09 : DayScript2021
{
    protected override string part_1()
    {
        int rawLength = _input.Substring(0, _input.IndexOf('\n')).Length;
        int colLength = _input.Split('\n').Length;

        int sum = 0;
        string[] heights = _input.Split('\n');
        for (int rawIndex = 0; rawIndex < heights.Length; rawIndex++)
        {
            for (int colIndex = 0; colIndex < rawLength; colIndex++)
            {
                if (colIndex > 0)
                {
                    if (heights[rawIndex][colIndex] >= heights[rawIndex][colIndex - 1])
                        continue;
                }

                if (colIndex < rawLength -1)
                {
                    if (heights[rawIndex][colIndex] >= heights[rawIndex][colIndex + 1])
                        continue;
                }

                if (rawIndex > 0)
                {
                    if (heights[rawIndex][colIndex] >= heights[rawIndex - 1][colIndex])
                        continue;
                }

                if (rawIndex < heights.Length -1)
                {
                    if (heights[rawIndex][colIndex] >= heights[rawIndex + 1][colIndex])
                        continue;
                }

                // if we reach this reach, this is a low point
                sum += 1+int.Parse(heights[rawIndex][colIndex].ToString());

                if (rawIndex < 6 && colIndex < 10)
                {
                    Debug.Log("Found low point for raw " + rawIndex + " and column " + colIndex + " : " + heights[rawIndex][colIndex]);
                }

            }
        }

        return sum.ToString();
    }

    private int _rawLength, _colLength;
    private KeyValuePair<char,bool>[,] _heights;
    protected override string part_2()
    {
        _rawLength = _input.Substring(0, _input.IndexOf('\n')).Length;
        _colLength = _input.Split('\n').Length;
        _heights = new KeyValuePair<char, bool>[_colLength, _rawLength];

        string[] tmpHeights = _input.Split('\n');
        for (int rawIndex = 0; rawIndex < _colLength; rawIndex++)
        {
            for (int colIndex = 0; colIndex < _rawLength; colIndex++)
            {
                _heights[rawIndex, colIndex] = new KeyValuePair<char, bool>(tmpHeights[rawIndex][colIndex], false);
            }
        }

        List<int> basinsSizeList = new List<int>();

        for (int rawIndex = 0; rawIndex < _colLength; rawIndex++)
        {
            for (int colIndex = 0; colIndex < _rawLength; colIndex++)
            {
                if (_heights[rawIndex, colIndex].Value)  // point is already part of a basin, then continue
                    continue;

                if (colIndex > 0)
                {
                    if (_heights[rawIndex,colIndex].Key >= _heights[rawIndex, colIndex -1].Key)
                        continue;
                }

                if (colIndex < _rawLength - 1)
                {
                    if (_heights[rawIndex, colIndex].Key >= _heights[rawIndex, colIndex + 1].Key)
                        continue;
                }

                if (rawIndex > 0)
                {
                    if (_heights[rawIndex, colIndex].Key >= _heights[rawIndex-1, colIndex].Key)
                        continue;
                }

                if (rawIndex < _colLength - 1)
                {
                    if (_heights[rawIndex, colIndex].Key >= _heights[rawIndex+1, colIndex].Key)
                        continue;
                }

                Debug.Log("low point is at " + rawIndex + ", " + colIndex + " : " + _heights[rawIndex, colIndex].Key);

                // if we reach this reach, this is a low point
                // Start computing recursively basin size
                int basinSize = getSumOfNeighbourBasinSize(rawIndex, colIndex);
                Debug.Log("basin size is : " + basinSize);
                basinsSizeList.Add(basinSize);
            }
        }

        basinsSizeList.Sort(delegate(int a, int b) { return b.CompareTo(a); }) ;
        Debug.Log(System.String.Join(", ", basinsSizeList));
        return (basinsSizeList[0] * basinsSizeList[1] * basinsSizeList[2]).ToString();
    }

    int getSumOfNeighbourBasinSize(int x, int y, int offset = 0)
    {
        //Debug.Log(Tools.writeOffset(offset) + " Computing point " + x + ", " + y + "  -> " + _heights[x, y].ToString());
        if (_heights[x, y].Value)
        {
            //Debug.Log(Tools.writeOffset(offset) + " Already calculated, return 0");
            return 0;
        }

        _heights[x, y] = new KeyValuePair<char, bool>(_heights[x,y].Key, true);

        if (_heights[x, y].Key == '9')
        {
            //Debug.Log(Tools.writeOffset(offset) + " Is a 9, so returning 0");
            return 0;
        }

        int sum = 1;

        // top neighbour
        if (x > 0 && !_heights[x - 1, y].Value              // has neighbour + that has not been calculated
            && _heights[x - 1, y].Key > _heights[x, y].Key)  //  + which is higher
        {
            //Debug.Log(Tools.writeOffset(offset) + " Computing top neighbour");
            sum += getSumOfNeighbourBasinSize(x - 1, y, offset+1);
        }

        // bot neighbour
        if (x < _colLength - 1 && !_heights[x + 1, y].Value   // has neighbour + that has not been calculated
            && _heights[x + 1, y].Key > _heights[x, y].Key) // + which is higher
        {
            //Debug.Log(Tools.writeOffset(offset) + " Computing bot neighbour");
            sum += getSumOfNeighbourBasinSize(x + 1, y, offset+1);
        }

        // left neighbour
        if (y > 0 && !_heights[x, y - 1].Value   // has neighbour + that has not been calculated
            && _heights[x, y - 1].Key > _heights[x, y].Key) // + which is higher
        {
            //Debug.Log(Tools.writeOffset(offset) + " Computing left neighbour");
            sum += getSumOfNeighbourBasinSize(x, y - 1, offset+1);
        }


        // right neighbour
        if (y < _rawLength - 1 && !_heights[x, y + 1].Value   // has neighbour + that has not been calculated
            && _heights[x, y + 1].Key > _heights[x, y].Key) // + which is higher
        {
            //Debug.Log(Tools.writeOffset(offset) + " Computing right neighbour");
            sum += getSumOfNeighbourBasinSize(x, y + 1, offset+1);
        }

        //Debug.Log(Tools.writeOffset(offset) + " sum of basin in point " + x + ", " + y + " is : " + sum);

        return sum;
    }
}
