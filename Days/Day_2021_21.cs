using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2021_21 : DayScript2021
{
    protected override string part_1()
    {
        string[] lines = _input.Split('\n');

        int pos1 = int.Parse(lines[0].Substring(lines[0].IndexOf(':') + 1).Trim());
        int pos2 = int.Parse(lines[1].Substring(lines[1].IndexOf(':') + 1).Trim());

        int score1 = 0, score2 = 0;
        int diceMovement = 6;
        int diceCounter = 0;
        while (score1 < 1000 && score2 < 1000)
        {
            if (diceMovement % 2 == 0)
            {
                
                pos1 += diceMovement;
                if (pos1 > 10)
                    pos1 -= 10;

                score1 += pos1;
            }
            else
            {
                pos2 += diceMovement;
                if (pos2 > 10)
                    pos2 -= 10;

                score2 += pos2;
            }

            diceMovement--;
            if (diceMovement < 0)
                diceMovement = 9;

            diceCounter += 3;
        }

        return (diceCounter * (score1 < 1000 ? score1 : score2)).ToString();

    }

    protected override string part_2()
    {
        string[] lines = _input.Split('\n');
        int pos1 = 4, pos2 = 8;
        if (!IsTestInput)
        {
            pos1 = int.Parse(lines[0].Substring(lines[0].IndexOf(':') + 1).Trim());
            pos2 = int.Parse(lines[1].Substring(lines[1].IndexOf(':') + 1).Trim());
        }

        long universeWin1 = 0, universeWin2 = 0;
        Dictionary<int, int> combinations = new Dictionary<int, int>()
        {
            {3,1 }, {4,3 }, {5,6 }, {6, 7 }, {7, 6 }, {8,3 }, {9, 1 }
        };

        Dictionary<State, long> universesLeft = new Dictionary<State, long>();
        // Do first turn, 1 universe with +3 and + 9, 3 universes with +4 and + 8, 6 universes with +5 and +7, 7 universes with +6
        // tg Amora, oui ca pourrait se faire dynamiquement dans la boucle d'en dessous mais il aurait fallu gérer les constructeurs différemment
        universesLeft.Add(new State(pos1 + 3, 0, pos2, 0, true), 1);
        universesLeft.Add(new State(pos1 + 4, 0, pos2, 0, true), 3);
        universesLeft.Add(new State(pos1 + 5, 0, pos2, 0, true), 6);
        universesLeft.Add(new State(pos1 + 6, 0, pos2, 0, true), 7);
        universesLeft.Add(new State(pos1 + 7, 0, pos2, 0, true), 6);
        universesLeft.Add(new State(pos1 + 8, 0, pos2, 0, true), 3);
        universesLeft.Add(new State(pos1 + 9, 0, pos2, 0, true), 1);

        bool isP1Turn = false;
        int safetyCount = 40;
        while (universesLeft.Count > 0 && safetyCount > 0)
        {
            //string log = " TURN " + (41 - safetyCount).ToString() + " : " + universesLeft.Count + " more universes in progress" + System.Environment.NewLine + System.Environment.NewLine;
            Dictionary<State, long> tmp = new Dictionary<State, long>();
            if (isP1Turn)
            {
                foreach (var pair in universesLeft)
                {
                    foreach (var combination in combinations)
                    {
                        State tmpState = new State(pair.Key, isP1Turn, combination.Key);
                        if (tmpState.score1 >= 21)
                            universeWin1 += pair.Value * combination.Value;
                        else
                        {
                            if (!tmp.ContainsKey(tmpState))
                                tmp.Add(tmpState, 0);

                            tmp[tmpState] += pair.Value * combination.Value;
                        }
                    }
                }
            }
            else
            {
                foreach (var pair in universesLeft)
                {
                    foreach (var combination in combinations)
                    {
                        State tmpState = new State(pair.Key, isP1Turn, combination.Key);
                        if (tmpState.score2 >= 21)
                            universeWin2 += pair.Value * combination.Value;
                        else
                        {
                            if (!tmp.ContainsKey(tmpState))
                                tmp.Add(tmpState, 0);

                            tmp[tmpState] += pair.Value * combination.Value;
                        }
                    }
                }
            }

            universesLeft = new Dictionary<State, long>(tmp);

            //if (safetyCount > 35 || universesLeft.Count < 20)
            //{
            //    long totaluniverses = 0;
            //    foreach (var s in universesLeft)
            //    {
            //        log += "State " + s.Key.key + " \t in " + s.Value + " universes" + System.Environment.NewLine;
            //        totaluniverses += s.Value;
            //    }
            //    Debug.Log(totaluniverses + " totaluniverses after v   (score are " + universeWin1 + " // " + universeWin2+ ")");
            //}
            //Debug.Log(log);

            isP1Turn = !isP1Turn;
            safetyCount--;
        }

        //Debug.Log(universeWin1 + " // " + universeWin2 + " - " + safetyCount + " // " + universesLeft.Count);
        return Mathf.Max(universeWin1, universeWin2).ToString();
    }

    struct State
    {
        public int pos1, score1;
        public int pos2, score2;

        public State(int p1, int oldScore1, int p2, int oldScore2, bool isP1Turn, int offset = 0)
        {
            this.pos1 = p1;
            this.score1 = oldScore1;
            this.pos2 = p2;
            this.score2 = oldScore2;

            setState(isP1Turn, offset);
        }

        void setState(bool isP1Turn, int offset)
        { 
            if (isP1Turn)
            {
                pos1 += offset;
                if (pos1 > 10)
                    pos1 -= 10;

                score1 += pos1;
            }
            else
            {
                pos2 += offset;
                if (pos2 > 10)
                    pos2 -= 10;

                score2 += pos2;
            }
        }

        public State(State copy, bool isP1Turn, int offset = 0) 
        {
            this.pos1 = copy.pos1;
            this.score1 = copy.score1;
            this.pos2 = copy.pos2;
            this.score2 = copy.score2;

            setState(isP1Turn, offset);
        }

        public string key { get { return pos1 + "-" + score1 + "-" + pos2 + "-" + score2; } }
    }
}
