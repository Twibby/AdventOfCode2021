using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Day_2021_04 : DayScript2021
{
    public TMPro.TMP_Text debugText, pickedText;

    protected override string part_1()
    {
        Queue<int> pickedNumbers = new Queue<int>(_input.Substring(0, _input.IndexOf('\n')).Split(',').Select(int.Parse).ToList());

        List<BingoBoard> boards = new List<BingoBoard>();
        int index = 0;  // to debug boards
        foreach (string boardInput in _input.Substring(_input.IndexOf('\n') + 2).Split(new string[] { "\n\n" }, System.StringSplitOptions.RemoveEmptyEntries))
        {
            if (index == 7)
                debugText.text = " " + boardInput + " ";

            boards.Add(new BingoBoard(boardInput, index, (index == 7)));
            index++;
        }
        
        pickedText.text = "";
        int bingoScore = -1;
        while (pickedNumbers.Count > 0 && bingoScore < 0)
        {
            int currentNumber = pickedNumbers.Dequeue();
            Debug.Log("Current picked number is : " + currentNumber);

            foreach (BingoBoard board in boards)
            {
                int score = board.BingoStep(currentNumber, board.index == 7);
                if (score > 0) 
                {
                    bingoScore = score * currentNumber;
                    break;
                }
            }
        }

        return bingoScore.ToString();
    }

    protected override string part_2()
    {
        Queue<int> pickedNumbers = new Queue<int>(_input.Substring(0, _input.IndexOf('\n')).Split(',').Select(int.Parse).ToList());

        List<BingoBoard> boards = new List<BingoBoard>();
        foreach (string boardInput in _input.Substring(_input.IndexOf('\n') + 2).Split(new string[] { "\n\n" }, System.StringSplitOptions.RemoveEmptyEntries))
        {
            boards.Add(new BingoBoard(boardInput));
        }

        pickedText.text = "";
        int bingoScore = -1;
        while (pickedNumbers.Count > 0 && bingoScore < 0)
        {
            int currentNumber = pickedNumbers.Dequeue();
            Debug.Log("Current picked number is : " + currentNumber);

            boards.ForEach(x => x.checkNumber(currentNumber));
            if (boards.Count > 1)
                boards.RemoveAll(x => x.isBingo());
            else
            {
                if (boards.Count == 0)
                    Debug.LogWarning("Houston, we got a problem");
                else 
                {   // count is 1 bc not > 1 and not 0
                    if (boards[0].isBingo())
                        bingoScore = boards[0].getBoardScore() * currentNumber;
                }
            }
        }

        return bingoScore.ToString();
    }


    class BingoBoard
    {
        public int index;   // for debug purpose
        List<List<int>> rowsAndColumns;

        public BingoBoard(string pInput, int pIndex = -1, bool debug = false)
        {
            this.index = pIndex;

            if (debug)
                Debug.LogWarning(pInput);

            rowsAndColumns = new List<List<int>>();
            if (pInput.Split('\n').Length != pInput.Split('\n')[0].Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries).Length)
            {
                Debug.LogError("Board not correctly formated !" + System.Environment.NewLine + pInput);
                return;
            }

            List<List<int>> rows = new List<List<int>>();
            foreach (string line in pInput.Split('\n'))
            {
                rows.Add(new List<int>(line.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()));
            }

            // from this moment, assume input is correctly formated
            for (int i = 0; i < rows.Count; i++)
            {
                for (int column = 0; column < rows[i].Count; column++)
                {
                    if (i == 0)   //on first line, create list
                        rowsAndColumns.Add(new List<int>());

                    rowsAndColumns[column].Add(rows[i][column]);
                }
            }

            rowsAndColumns.AddRange(rows);

            if (debug)
                printLines();

        }

        void printLines()
        {
            string log = "";
            foreach (var list in rowsAndColumns)
            {
                list.ForEach(x => log += x + ", ");
                log += System.Environment.NewLine;
            }
            Debug.Log(log);
        }

        /// <summary>
        /// Check number picked 
        /// Then look if there is a bingo (a line or a column is fully marked)
        /// If so, return board score
        /// </summary>
        /// <param name="pickedNumber"></param>
        /// <returns></returns>
        public int BingoStep(int pickedNumber, bool debug = false)
        {
            checkNumber(pickedNumber, debug);
            if (isBingo())
                return getBoardScore();

            return -1;
        }

        public void checkNumber(int pickedNumber, bool debug = false)
        {
            foreach (List<int> line in rowsAndColumns)
            {
                if (line.Remove(pickedNumber) && debug)
                    Debug.LogWarning("Number in my card !");
            }
            if (debug) { printLines(); }

        }

        public bool isBingo() { return (rowsAndColumns.Exists(x => x.Count == 0)); }

        public int getBoardScore()
        {
            int score = 0;
            foreach (List<int> line in rowsAndColumns)
            {
                line.ForEach(x => score += x);
            }

            if (score % 2 != 0)
                Debug.LogError("score should be even as all number ar in double");

            return score / 2;
        }
    }
}
