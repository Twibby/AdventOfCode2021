using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Day_2021_16 : DayScript2021
{
    protected override string part_1()
    {
        string binaryString = System.String.Join("",
          _input.Select(
            c => System.Convert.ToString(System.Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
          )
        );

        Debug.Log(binaryString);

        (List<Packet>, string) mainPack = parsePacket(binaryString);
        Debug.Log(Tools.writeOffset(0) + " End queue is : " + mainPack.Item2);
        if (mainPack.Item1 != null && mainPack.Item1.Count > 0)
            return mainPack.Item1[0].GetVersionSum().ToString();
        else
            return "error";
    }

    protected override string part_2()
    {
        string binaryString = System.String.Join("",
                 _input.Select(
                   c => System.Convert.ToString(System.Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                 )
               );

        Debug.Log(binaryString);

        (List<Packet>, string) mainPack = parsePacket(binaryString);
        Debug.Log(Tools.writeOffset(0) + " End queue is : " + mainPack.Item2);
        if (mainPack.Item1 != null && mainPack.Item1.Count > 0)
            return mainPack.Item1[0].Evaluate().ToString();
        else
            return "error";
    }

    class Packet
    {
        public int version;
        public int type;
        public long numericVal;
        public int operatorType;
        public int operatorParam;
        public List<Packet> subpackets;

        public Packet() { subpackets = new List<Packet>(); }

        public long GetVersionSum()
        {
            long result = version;
            foreach (var p in subpackets) { result += p.GetVersionSum(); }
            return result;
        }

        public long Evaluate(int offset = 0)
        {
            long result = 0;
            switch (this.type)
            {
                case 4: return this.numericVal;
                case 0:
                    subpackets.ForEach(x => result += x.Evaluate(offset + 1));
                    //Debug.Log(Tools.writeOffset(offset) + "result for sum : " + result);
                    break;
                case 1:
                    result = 1;
                    subpackets.ForEach(x => result *= x.Evaluate(offset + 1));
                    break;
                case 2:
                    result = long.MaxValue;
                    foreach (var sub in subpackets)
                    {
                        long subEval = sub.Evaluate(offset + 1);
                        if (subEval < result)
                            result = subEval;
                    }
                    break;
                case 3:
                    result = 0;
                    foreach (var sub in subpackets)
                    {
                        long subEval = sub.Evaluate(offset + 1);
                        if (subEval > result)
                            result = subEval;
                    }
                    break;
                case 5:
                    result = (subpackets[0].Evaluate(offset + 1) > subpackets[1].Evaluate(offset + 1) ? 1 : 0);
                    break;
                case 6:
                    result = (subpackets[0].Evaluate(offset + 1) < subpackets[1].Evaluate(offset + 1) ? 1 : 0);
                    break;
                case 7:
                    result = (subpackets[0].Evaluate(offset + 1) == subpackets[1].Evaluate(offset+1) ? 1 : 0);
                    break;
                default:
                    Debug.LogError("[Packet/Evaluate] Non-exiting type : " + this.type);
                    break;
            }

            //Debug.Log(Tools.writeOffset(offset) + "Evaluate for type " + this.type + " : " + result);
            return result;
        }
    }

    (List<Packet>,string) parsePacket(string input, int count = -1, int offset = 0)
    {
        if (input.Length < 6)
        {
            Debug.LogError("input is too short : " + input);
            return (new List<Packet>(), "");
        }

        List<Packet> result = new List<Packet>();
        while (input.Length > 6 && (count < 0 || result.Count < count))
        {
            //Debug.Log(Tools.writeOffset(offset) + "Starting parse for input : " + input);
            Packet packet = new Packet();

            packet.version = System.Convert.ToInt32(input.Substring(0, 3), 2);
            packet.type = System.Convert.ToInt32(input.Substring(3, 3), 2);

            //Debug.Log(Tools.writeOffset(offset) + " has version " + packet.version + " and type " + packet.type);
            string queue = "";
            if (packet.type == 4)
            {
                bool continueParse = true;
                int segment = 0;
                string numberBit = "";
                while (continueParse)
                {
                    continueParse = (input[6 + segment * 5] == '1');
                    numberBit += input.Substring(7 + segment * 5, 4);
                    segment++;
                }

                packet.numericVal = System.Convert.ToInt64(numberBit, 2);
                //Debug.Log(Tools.writeOffset(offset) + " Is numeral and worths " + packet.numericVal);
                queue = input.Substring(6 + segment * 5);
            }
            else
            {
                packet.operatorType = int.Parse(input[6].ToString());
                //Debug.Log(Tools.writeOffset(offset) + " Is operator with type " + packet.operatorType);
                if (packet.operatorType == 0)
                {
                    packet.operatorParam = System.Convert.ToInt32(input.Substring(7, 15), 2);
                    //Debug.Log(Tools.writeOffset(offset) + " . . and operator param " + packet.operatorParam);
                    string subpacketsString = input.Substring(22, packet.operatorParam);
                    packet.subpackets = new List<Packet>(parsePacket(subpacketsString, -1, offset+1).Item1);

                    queue = input.Substring(22 + packet.operatorParam);
                }
                else
                {
                    packet.operatorParam = System.Convert.ToInt32(input.Substring(7, 11), 2);
                    //Debug.Log(Tools.writeOffset(offset) + " . . and operator param " + packet.operatorParam);

                    (List<Packet>, string) res = parsePacket(input.Substring(18), packet.operatorParam, offset+1);
                    if (res.Item1.Count != packet.operatorParam)
                        Debug.LogError(Tools.writeOffset(offset) + "getSubpacketsQueue not working properly, operator param is " + packet.operatorParam + " and res count is " + res.Item1.Count);
                    else
                        packet.subpackets = new List<Packet>(res.Item1);

                    queue = res.Item2;
                    //Debug.Log(Tools.writeOffset(offset) + " queue is : " + queue);
                }
            }

            result.Add(packet);
            input = queue;
        }

        return (result, input);
    }
}
