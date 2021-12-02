using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2021_02 : DayScript
{

    protected override string part_1()
    {
        int depth = 0, pos = 0;
        foreach (string command in _input.Split('\n'))
        {
            string[] cmds = command.Split(' ');
            if (cmds.Length != 2)
            {
                Debug.LogError("line not correctly formated : '" + command + "'");
                continue;
            }
            else
            {
                int val = 0;
                if (int.TryParse(cmds[1], out val))
                {
                    switch (cmds[0])
                    {
                        case "up": depth -= val; break;     // up decreases 'depth' bc we're underwater
                        case "down": depth += val; break;   // down increases 'depth'  bc we're underwater
                        case "forward": pos += val; break;
                        default:
                            Debug.LogError("Command is not correct " + cmds[0]);
                            break;
                    }
                }
                else
                {
                    Debug.LogError("Value not correct : " + cmds[1]);
                    continue;
                }
            }
        }
        Debug.LogWarning("Final pos are " + pos + ", depth " + depth);

        return (depth * pos).ToString();
    }

    protected override string part_2()
    {
        int depth = 0, pos = 0, aim = 0;
        foreach (string command in _input.Split('\n'))
        {
            string[] cmds = command.Split(' ');
            if (cmds.Length != 2)
            {
                Debug.LogError("line not correctly formated : '" + command + "'");
                continue;
            }
            else
            {
                int val = 0;
                if (int.TryParse(cmds[1], out val))
                {
                    switch (cmds[0])
                    {
                        case "up": aim -= val; break;     // up decreases 'depth' bc we're underwater
                        case "down": aim += val; break;   // down increases 'depth'  bc we're underwater
                        case "forward": 
                            pos += val; 
                            depth += aim * val;
                            break;
                        default:
                            Debug.LogError("Command is not correct " + cmds[0]);
                            break;
                    }
                }
                else
                {
                    Debug.LogError("Value not correct : " + cmds[1]);
                    continue;
                }
            }
        }

        return (depth * pos).ToString();
    }
}
