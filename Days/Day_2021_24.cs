using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2021_24 : DayScript2021
{
    protected override string part_1()
    {

        List<string> constraints = new List<string>();
        
        string w = "0", x = "0", y = "0", z = "0";
        List<string> commands = new List<string>() { w, x, y, z };
        List<bool> isCommandInt = new List<bool>() { true, true, true, true };
        int index = 0;
        int counter = 1;
        foreach (string inst in _input.Split('\n'))
        {
            Debug.LogWarning("Command " + counter + " is : \t" + inst);
            string[] vars = inst.Split(' ');
            switch(vars[0])
            {
                case "inp":
                    Debug.Log("new input for " + vars[1]);

                    // First assumption i[2] = 1 & i[3] = 9 for further equality and see if we get a result
                    if (index == 2) { commands[getPos(vars[1])] = "1"; isCommandInt[getPos(vars[1])] = true; }
                    else if (index == 3) { commands[getPos(vars[1])] = "9"; isCommandInt[getPos(vars[1])] = true; }
                    else
                    {
                        commands[getPos(vars[1])] = "i[" + index + "]";
                        isCommandInt[getPos(vars[1])] = false;
                    }
                    index++;
                    break;

                case "add":
                    int val = -1;
                    if (int.TryParse(vars[2], out val))
                    {
                        if (val != 0)
                        {
                            if (isCommandInt[getPos(vars[1])])
                                commands[getPos(vars[1])] = (int.Parse(commands[getPos(vars[1])]) + val).ToString();
                            else
                                commands[getPos(vars[1])] = commands[getPos(vars[1])] + " + " + val;
                        }
                    }
                    else
                    {
                        if(commands[getPos(vars[1])] == "0")
                        {
                            commands[getPos(vars[1])] = commands[getPos(vars[2])];
                            isCommandInt[getPos(vars[1])] = isCommandInt[getPos(vars[2])];
                        }
                        else if (commands[getPos(vars[2])] == "0")
                        {
                            // Nothing to do
                        }
                        else if (isCommandInt[getPos(vars[1])] && isCommandInt[getPos(vars[2])])
                        {
                            commands[getPos(vars[1])] = (int.Parse(commands[getPos(vars[1])]) + int.Parse(commands[getPos(vars[2])])).ToString();
                        }
                        else
                        {
                            Debug.Log("cant parse " + vars[2]);

                            commands[getPos(vars[1])] = commands[getPos(vars[1])] + " + " + commands[getPos(vars[2])];
                            isCommandInt[getPos(vars[1])] = false;
                        }
                    }
                    break;

                case "mul":
                    int val2 = -1;
                    if (int.TryParse(vars[2], out val2))
                    {
                        if (val2 == 0)
                        {
                            commands[getPos(vars[1])] = "0";
                            isCommandInt[getPos(vars[1])] = true;
                        }
                        else if (val2 != 1)
                        {
                            if (isCommandInt[getPos(vars[1])])
                                commands[getPos(vars[1])] = (int.Parse(commands[getPos(vars[1])]) * val2).ToString();
                            else
                                commands[getPos(vars[1])] = "(" + commands[getPos(vars[1])] + ") * (" + val2 + ")";
                        }
                    }
                    else
                    {
                        if (commands[getPos(vars[1])] == "0" || commands[getPos(vars[2])] == "0")
                        {
                            commands[getPos(vars[1])] = "0";
                            isCommandInt[getPos(vars[1])] = true;
                        }
                        else if (commands[getPos(vars[1])] == "1")
                        {
                            commands[getPos(vars[1])] = commands[getPos(vars[2])];
                            isCommandInt[getPos(vars[1])] = isCommandInt[getPos(vars[2])];
                        }
                        else if (commands[getPos(vars[2])] == "1")
                        {
                            // Nothing to do
                        }
                        else if (isCommandInt[getPos(vars[1])] && isCommandInt[getPos(vars[2])])
                        {
                            commands[getPos(vars[1])] = (int.Parse(commands[getPos(vars[1])]) * int.Parse(commands[getPos(vars[2])])).ToString();
                        }
                        else
                        {
                            Debug.Log("cant parse " + vars[1] + " or "+ vars[2]);

                            commands[getPos(vars[1])] = "(" + commands[getPos(vars[1])] + ") * (" + commands[getPos(vars[2])] + ")";
                            isCommandInt[getPos(vars[1])] = false;
                        }
                    }
                    break;

                case "div":
                    // special cases
                    {
                        // Regex would have been great here but i can't make it... so just  add some ifs as number of cases is not that big

                        //if (inst == "div z 26")                         // a regex  \\( .* \\) \* \\(26\\) \\+ i\\[[0-9]+\\] \\+ [0-9]+  serait cool mais dommage
                        //{
                        //    Debug.Log("Regex ? " + System.Text.RegularExpressions.Regex.IsMatch(commands[3], "\\((.*)\\) \\* \\(26\\) \\+ i\\[[0-9]+\\] \\+ [0-9]+$"));
                        //    Debug.Log("Regex result ? " + System.Text.RegularExpressions.Regex.Replace(commands[3], "\\((.*)\\) \\* \\(26\\) \\+ i\\[[0-9]+\\] \\+ [0-9]+$", "$1"));
                        //    commands[3] = System.Text.RegularExpressions.Regex.Replace(commands[3], "\\((.*)\\) \\* \\(26\\) \\+ i\\[[0-9]+\\] \\+ [0-9]+$", "$1");
                        //    Debug.Log("Special Pattern found in div");
                        //    break;
                        //}
                        if (inst == "div z 26" && commands[3] == "((i[0] + 1) * (26) + i[1] + 1) * (26) + 17") { commands[3] = "(i[0] + 1) * (26) + i[1] + 1"; break; }
                        if (inst == "div z 26" && commands[3] == "(i[0] + 1) * (26) + i[1] + 1") { commands[3] = "i[0] + 1"; break; }
                        if (inst == "div z 26" && commands[3] == "(((i[0] + 1) * (26) + i[5] + 3) * (26) + i[6] + 2) * (26) + i[7] + 15") { commands[3] = "((i[0] + 1) * (26) + i[5] + 3) * (26) + i[6] + 2"; break; }
                        if (inst == "div z 26" && commands[3] == "((i[0] + 1) * (26) + i[5] + 3) * (26) + i[6] + 2") { commands[3] = "(i[0] + 1) * (26) + i[5] + 3"; break; }
                        if (inst == "div z 26" && commands[3] == "(i[0] + 1) * (26) + i[5] + 3") { commands[3] = "i[0] + 1"; break; }
                        if (inst == "div z 26" && commands[3] == "(i[0] + 1) * (26) + i[11] + 1") { commands[3] = "i[0] + 1"; break; }
                        if (inst == "div z 26" && commands[3] == "i[0] + 1") { commands[3] = "0"; isCommandInt[3] = true; break; }

                        Debug.Log("not special case");
                    }

                    int divider = 0;
                    if (!int.TryParse(vars[2], out divider))
                    {   // divider is a variable (eg "div x y")

                        if (commands[getPos(vars[2])] == "0")
                        {
                            Debug.LogError("IMPOSSIBLE ! ");
                            return "";
                        }    
                        else if (isCommandInt[getPos(vars[2])] && isCommandInt[getPos(vars[1])])
                        {
                            commands[getPos(vars[1])] = (int.Parse(commands[getPos(vars[1])]) / int.Parse(commands[getPos(vars[2])])).ToString();
                        }
                        else
                        {
                            commands[getPos(vars[1])] = "(" + commands[getPos(vars[1])] + ") / (" + commands[getPos(vars[2])] + ")";
                            constraints.Add(commands[getPos(vars[2])] + " != 0");
                            Debug.LogError("New constraint : " + commands[getPos(vars[2])] + " != 0");
                        }
                    }
                    else
                    {   // divider is an int (eg "div x 5")
                        if (divider == 0)
                        {
                            Debug.LogError("IMPOSSIBLE ! ");
                            return "";
                        }
                        else if (divider == 1)
                        {
                            // Nothing to do
                        }
                        else if (isCommandInt[getPos(vars[1])])
                        {
                            commands[getPos(vars[1])] = Mathf.CeilToInt(int.Parse(commands[getPos(vars[1])]) / divider).ToString();
                        }
                        else
                        {
                            commands[getPos(vars[1])] = "(" + commands[getPos(vars[1])] + ") / (" + divider.ToString() + ")";
                        }
                    }
                    break;

                case "mod":
                    // special cases
                    {
                        if (inst == "mod x 26" && !commands[1].Contains("/ (26)"))
                        {
                            // regex would have been great here
                            for (int i = 0; i < 14; i++)
                            {
                                for (int add = 1; add < 20; add++)
                                {
                                    if (commands[1].Contains("(i[" + i + "] + " + add + ") * (26) + "))
                                    {
                                        commands[1] = commands[1].Replace("(i[" + i + "] + " + add + ") * (26) + ", "");
                                        Debug.Log("new x before mod :" + commands[1]);
                                    }
                                }
                            }

                            int tmp;
                            if (int.TryParse(commands[1], out tmp))
                                isCommandInt[1] = true;
                        }

                        if (inst == "mod x 26" && System.Text.RegularExpressions.Regex.IsMatch(commands[1], "i\\[[0-9]+\\] \\+ [0-9]")) { Debug.Log("pattern found"); break; }

                        if (inst == "mod x 26" && commands[1] == "((i[0] + 1) * (26) + i[1] + 1) * (26) + i[2] + 16")
                        {
                            commands[1] = "i[2] + 16";
                            break;
                        }

                        if (inst == "mod x 26" && commands[1] == "(((i[0] + 1) * (26) + i[1] + 1) * (26) + 17) / (26)")
                        {
                            commands[1] = "i[1] + 1";
                            break;
                        }
                    }


                    if (isCommandInt[getPos(vars[1])] && int.Parse(commands[getPos(vars[1])]) < 0)
                    {
                        Debug.LogError("IMPOSSIBLE MOD");
                        return "";
                    }
                    else if (!isCommandInt[getPos(vars[1])])
                    {
                        constraints.Add(commands[getPos(vars[1])] + " >= 0");
                        Debug.LogError("New constraint : " + commands[getPos(vars[1])] + " >= 0");
                    }

                    int moder = 0;
                    if (!int.TryParse(vars[2], out moder))
                    {   // moder is variable (eg "mod x y")

                        if (commands[getPos(vars[2])] == "0")
                        {
                            Debug.LogError("IMPOSSIBLE ! ");
                            return "";
                        }
                        else if (isCommandInt[getPos(vars[2])] && int.Parse(commands[getPos(vars[2])]) < 0)
                        {
                            Debug.LogError("IMPOSSIBLE mod < 0 ! ");
                            return "";
                        }
                        else if (isCommandInt[getPos(vars[2])] && isCommandInt[getPos(vars[1])])
                        {
                            commands[getPos(vars[1])] = (int.Parse(commands[getPos(vars[1])]) % int.Parse(commands[getPos(vars[2])])).ToString();
                        }
                        else
                        {
                            commands[getPos(vars[1])] = "(" + commands[getPos(vars[1])] + ") % (" + commands[getPos(vars[2])] + ")";

                            constraints.Add(commands[getPos(vars[2])] + " > 0");
                            Debug.LogError("New constraint : " + commands[getPos(vars[2])] + " > 0");
                        }
                    }
                    else
                    {   // moder is int (eg "mod x 26")
                        if (moder <= 0)
                        {
                            Debug.LogError("IMPOSSIBLE MOD ! ");
                            return "";
                        }
                        else if (isCommandInt[getPos(vars[1])])
                        {
                            commands[getPos(vars[1])] = (int.Parse(commands[getPos(vars[1])]) % moder).ToString();
                        }
                        else
                        {
                            commands[getPos(vars[1])] = "(" + commands[getPos(vars[1])] + ") % " + moder.ToString();
                        }
                    }
                    break;

                case "eql":

                    // special cases
                    {
                        if (System.Text.RegularExpressions.Regex.IsMatch(commands[getPos(vars[1])], "i\\[[0-9]+\\] \\+ [0-9] \\+ [0-9]+")
                                    && System.Text.RegularExpressions.Regex.IsMatch(commands[getPos(vars[1])], "i\\[[0-9]+\\]"))
                        {
                            Debug.Log("Special regex match found");
                            commands[getPos(vars[1])] = "0";
                            isCommandInt[getPos(vars[1])] = true;
                            break;
                        }

                        // Assuming following constraints, because we just encountered these ones in previous runs
                        // + Assuming i[2] = 1 and i[3] = 9 bc we encoutered constraint i[2] + 16 - 8 = i[3]
                        if (inst == "eql x w" && commands[1] == "i[1] + 1 + -4" && commands[0] == "i[4]")   { commands[1] = "1"; isCommandInt[1] = true; break; }
                        if (inst == "eql x w" && commands[1] == "i[7] + 15 + -13" && commands[0] == "i[8]") { commands[1] = "1"; isCommandInt[1] = true; break; }
                        if (inst == "eql x w" && commands[1] == "i[6] + 2 + -3" && commands[0] == "i[9]")   { commands[1] = "1"; isCommandInt[1] = true; break; }
                        if (inst == "eql x w" && commands[1] == "i[5] + 3 + -7" && commands[0] == "i[10]")  { commands[1] = "1"; isCommandInt[1] = true; break; }
                        if (inst == "eql x w" && commands[1] == "i[11] + 1 + -6" && commands[0] == "i[12]") { commands[1] = "1"; isCommandInt[1] = true; break; }
                        if (inst == "eql x w" && commands[1] == "i[0] + 1 + -8" && commands[0] == "i[13]")  { commands[1] = "1"; isCommandInt[1] = true; break; }

                    }

                    int comparator = 0;
                    if (int.TryParse(vars[2], out comparator))
                    {   // Comparator is an int (eg "eq x 2" )
                        
                        if (isCommandInt[getPos(vars[1])])
                            commands[getPos(vars[1])] = (int.Parse(commands[getPos(vars[1])]) == comparator ? "1" : "0");
                        else
                        {
                            constraints.Add("Can't evaluate this automatically (int) : " + commands[getPos(vars[1])] + " == " + comparator + " ? ");
                            Debug.LogError("Can't evaluate this automatically (int) : " + commands[getPos(vars[1])] + " == " + comparator + " ? ");
                        }
                    }
                    else
                    {   // Comparator is a variable (eg "eq x y")

                        if (isCommandInt[getPos(vars[1])] && isCommandInt[getPos(vars[2])])
                            commands[getPos(vars[1])] = int.Parse(commands[getPos(vars[1])]) == int.Parse(commands[getPos(vars[2])]) ? "1" : "0";
                        else
                        {
                            // treat manually some obvious cases encountered previously
                            if (commands[getPos(vars[1])] == "12" && commands[getPos(vars[2])] == "i[0]")
                            {
                                commands[getPos(vars[1])] = "0";
                                isCommandInt[getPos(vars[1])] = true;
                            }
                            else if (System.Text.RegularExpressions.Regex.IsMatch(commands[getPos(vars[1])], "i\\[[0-9]+\\] \\+ [0-9] \\+ [0-9]+")
                                    && System.Text.RegularExpressions.Regex.IsMatch(commands[getPos(vars[1])], "i\\[[0-9]+\\]"))
                            {
                                Debug.Log("Special regex match found");
                                commands[getPos(vars[1])] = "0";
                                isCommandInt[getPos(vars[1])] = true;
                            }
                            else
                            {
                                constraints.Add("Can't evaluate this automatically (var) : " + commands[getPos(vars[1])] + " == " + commands[getPos(vars[2])] + " ? ");
                                Debug.LogError("Can't evaluate this automatically (var) : " + commands[getPos(vars[1])] + " == " + commands[getPos(vars[2])] + " ? ");
                            }
                        }

                    }

                    break;
            }

            // Display current state
            Debug.Log("w = " + commands[0] + '\n' + "x = " + commands[1] + '\n' + "y = " + commands[2] + '\n' + "z = " + commands[3]);

            counter++;
        }

        Debug.LogWarning("CONSTRAINTS : " + System.Environment.NewLine + System.String.Join(System.Environment.NewLine, constraints));

        return "";
    }

    int getPos(string var)
    {
        switch (var)
        {
            case "w": return 0;
            case "x": return 1;
            case "y": return 2;
            case "z": return 3;
            default:    Debug.LogError("whaaaat " + var); return 0;
        }
    }

    protected override string part_2()
    {
        return base.part_2();
    }
}
