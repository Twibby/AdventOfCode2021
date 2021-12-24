using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Day_2021_23 : DayScript2021
{
    protected override string part_1()
    {
        return base.part_1();
    }

    protected override string part_2()
    {
        int score = 0;

        State startState = new State("00000000000", new List<Room>() { new Room("CDDC", 'A'), new Room("BCBD", 'B'), new Room("ABAA", 'C'), new Room("DACB", 'D') }, new List<string>());
        State goalState = new State("00000000000", new List<Room>() { new Room("AAAA", 'A'), new Room("BBBB", 'B'), new Room("CCCC", 'C'), new Room("DDDD", 'D') }, new List<string>());

        //State startState = new State("00000000000", new List<Room>() { new Room("CC", 'A', 2), new Room("BD", 'B', 2), new Room("AA", 'C', 2), new Room("DB", 'D', 2) });
        //State goalState = new State("00000000000", new List<Room>() { new Room("AA", 'A', 2), new Room("BB", 'B', 2), new Room("CC", 'C', 2), new Room("DD", 'D', 2) });

        HashSet<State> statesDone = new HashSet<State>();
        Dictionary<State, int> statesWithCost = new Dictionary<State, int>();
        statesWithCost.Add(startState, 0);

        List<KeyValuePair<State, int>> statesToDo = new List<KeyValuePair<State, int>>();
        statesToDo.Add(new KeyValuePair<State, int>(startState, 0));

        int safetyCount = 0;
        while (statesToDo.Count > 0 && safetyCount < 1000000)
        {
            // deal with first state
            State currentState = statesToDo[0].Key;
            Debug.LogWarning("Current state is " + currentState.ToString());

            if (currentState.Equals(goalState))
                break;

            foreach (KeyValuePair<State, int> nextState in currentState.GetNextPossiblesStates())
            {
                if (!statesWithCost.ContainsKey(nextState.Key)
                    || statesToDo[0].Value + nextState.Value < statesWithCost[nextState.Key])
                {
                    int cost = statesToDo[0].Value + nextState.Value;
                    statesWithCost[nextState.Key] = cost;
                    statesToDo.Add(new KeyValuePair<State, int>(nextState.Key, cost));
                }
            }

            // sort to have minimum priority first
            statesToDo.RemoveAt(0);
            statesToDo.Sort(delegate (KeyValuePair<State, int> a, KeyValuePair<State, int> b) { return a.Value.CompareTo(b.Value); });

            safetyCount++;
        }

        if (safetyCount <= 0)
            Debug.LogError("safety exit");
        else if (statesToDo.Count == 0)
            Debug.LogError("no more possible states, weird");
        else
            Debug.Log(statesToDo[0].Value.ToString());


        Debug.Log("MEMORY IS : " + System.Environment.NewLine + System.String.Join(System.Environment.NewLine, statesToDo[0].Key.memory));

        return score.ToString();
    }

    public class Room
    {
        public string roomContent;
        public char type;
        public int index { get { return (int)(this.type - 'A'); } }

        public int size;

        public Room(string pRoomContent, char pType, int pSize = 4)
        {
            this.roomContent = pRoomContent;
            this.type = pType;
            this.size = pSize;
        }
        public Room(Room copy)
        {
            this.roomContent = copy.roomContent;
            this.type = copy.type;
        }

        public bool IsEmpty() { return this.roomContent.Trim(new char[] { '0' }).Length == 0; }
        public bool IsFull() { return !this.roomContent.Contains("0"); }

        public bool IsCleared()
        {
            foreach (char c in this.roomContent) { if (c != '0' && c != type) { return false; } }
            return true;
        }

        public bool IsComplete() { return IsFull() && IsCleared(); }

        public override string ToString() { return roomContent; }

        /// <summary>
        /// returns room with 1 correct char entering in it (in lower place)
        /// </summary>
        public (Room, int) Push()
        {
            int index = this.roomContent.LastIndexOf('0');
            if (index < 0)
                throw new System.Exception("meh " + this.roomContent + " / " + this.type);

            this.roomContent = this.roomContent.Substring(0, index) + type + (index < roomContent.Length - 1 ? roomContent.Substring(index + 1) : "");

            //Debug.Log("Pushing in room " + this.type + " => " + this.roomContent);

            return (this, index + 1);
        }

        public (Room, char, int) Pop()
        {

            if (this.IsEmpty() || this.IsCleared())
                return (this, '0', 0);

            int index = this.roomContent.LastIndexOf('0');
            if (index >= roomContent.Length - 1)
                throw new System.Exception("bluh " + this.roomContent + " / " + this.type);

            char c = roomContent[index + 1];
            this.roomContent = (index < 0 ? "" : this.roomContent.Substring(0, index + 1)) + '0' + (index < roomContent.Length - 2 ? roomContent.Substring(index + 2) : "");

            //Debug.Log("poping out " + c + " of room " + this.type + " => " + this.roomContent);

            return (this, c, index + 2);
        }
    }

    public class State
    {
        public string hallway;
        public List<Room> rooms;
        public string roomsHash { get { return System.String.Join("", rooms); } }

        private List<int> roomPosition = new List<int>() { 2, 4, 6, 8 };
        public List<string> memory;

        #region constructor & overrides
        public State(string pHallway, List<Room> pRooms, List<string> pMemory)
        {
            this.hallway = pHallway;
            this.rooms = new List<Room>();
            foreach (Room room in pRooms) { this.rooms.Add(new Room(room)); }
            this.memory = pMemory;
            this.memory.Add(this.ToString());
        }

        public override string ToString()
        {
            return hallway + "  |  " + System.String.Join("/", rooms);
        }
        public override int GetHashCode()
        {
            return (hallway + "-" + roomsHash).GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(State))
            {
                State stt = (State)obj;
                return this.roomsHash.Equals(stt.roomsHash) && this.hallway.Equals(stt.hallway);
            }
            return false;
        }
        #endregion

        public List<KeyValuePair<State, int>> GetNextPossiblesStates()
        {
            List<KeyValuePair<State, int>> result = new List<KeyValuePair<State, int>>();

            // see if we can move a char that is currently in hallway to his room
            for (int pos = 0; pos < hallway.Length; pos++)
            {
                char c = hallway[pos];
                if (c != '0' && canEnterHisRoom(c, pos))
                {
                    int cost = 0;

                    string tmpHallway = hallway.Substring(0, pos) + '0' + (pos < hallway.Length - 1 ? hallway.Substring(pos + 1) : "");
                    List<Room> tmpRooms = new List<Room>();
                    for (int i = 0; i < rooms.Count; i++)
                    {
                        if (rooms[i].type != c)
                            tmpRooms.Add(new Room(rooms[i]));
                        else
                        {
                            (Room, int) r = new Room(rooms[i]).Push();
                            tmpRooms.Add(r.Item1);

                            int movingCount = r.Item2 + System.Math.Abs(pos - this.roomPosition[r.Item1.index]);
                            cost = (int)System.Math.Pow(10, costValue(c)) * movingCount;
                        }
                    }

                    result.Add(new KeyValuePair<State, int>(new State(tmpHallway, tmpRooms, new List<string>(this.memory)), cost));
                }
            }

            if (result.Count > 0)       // always priorize entering char
                return result;

            // see if we can move out first char of a room
            foreach (Room r in rooms)
            {
                if (r.IsEmpty() || r.IsCleared())
                    continue;

                foreach (int availableExitPos in getAvailablePos(this.roomPosition[r.index]))
                {
                    int cost = 0;
                    (Room, char, int) val = new Room(r).Pop();
                    if (val.Item2 == '0')
                        continue;

                    string tmpHallway = hallway.Substring(0, availableExitPos) + val.Item2 + (availableExitPos < hallway.Length - 1 ? hallway.Substring(availableExitPos + 1) : "");
                    List<Room> tmpRooms = new List<Room>();
                    for (int i = 0; i < rooms.Count; i++)
                    {
                        if (rooms[i].type != val.Item1.type)
                            tmpRooms.Add(new Room(rooms[i]));
                        else
                            tmpRooms.Add(new Room(val.Item1));

                        int movingCount = val.Item3 + System.Math.Abs(availableExitPos - this.roomPosition[val.Item1.index]);
                        cost = (int)System.Math.Pow(10, costValue(val.Item2)) * movingCount;
                    }

                    result.Add(new KeyValuePair<State, int>(new State(tmpHallway, tmpRooms, new List<string>(this.memory)), cost));
                }
            }

            return result;
        }

        bool canEnterHisRoom(char c, int hallwayPos)
        {
            int indexRoom = this.rooms.FindIndex(x => x.type == c);

            if (!this.rooms[indexRoom].IsCleared())
                return false;

            int incrementor = hallwayPos < this.roomPosition[indexRoom] ? 1 : -1;
            for (int i = hallwayPos + incrementor; i != this.roomPosition[indexRoom]; i += incrementor)
            {
                if (hallway[i] != '0')
                    return false;
            }

            return true;
        }

        List<int> getAvailablePos(int index)
        {
            List<int> result = new List<int>();
            for (int i = index + 1; i < hallway.Length; i++)  // to the right;
            {
                if (hallway[i] != '0')
                    break;

                if (roomPosition.Contains(i))
                    continue;

                result.Add(i);
            }
            for (int i = index - 1; i >= 0; i--)  // to the right;
            {
                if (hallway[i] != '0')
                    break;

                if (roomPosition.Contains(i))
                    continue;

                result.Add(i);
            }

            return result;
        }

        int costValue(char c)
        {
            switch (c)
            {
                case 'A': return 0;
                case 'B': return 1;
                case 'C': return 2;
                case 'D': return 3;
                default: Debug.LogError("Cost value wrong : " + c); return 0;
            }
        }
    }
}
