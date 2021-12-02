using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2021_02_Anim : DayAnimationScript
{
    public TMPro.TMP_Text DisplayLabel;
    public RectTransform Submarine;
    public BubbleSpawn BubbleSpawner;

    public override IEnumerator part_2()
    {
        //Init
        Submarine.anchorMin = new Vector2(0, 1f);
        Submarine.anchorMax = new Vector2(0, 1f);
        Submarine.gameObject.SetActive(false);
        DisplayLabel.text = "Preparing...";

        // make first loop for calculations of max depth and pos to have all game in screen
        float depth = 0, pos = 0, aim = 0;
        float maxDepth = 0, maxPos = 0;

        int counter = 20;   // add this counter to not make all input but just a sample

        #region calculations
        foreach (string command in input.Split('\n'))
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
                            maxDepth = Mathf.Max(maxDepth, depth);
                            maxPos = Mathf.Max(maxPos, pos);
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

            counter--;
            if (counter <= 0)
                break;
        }
        #endregion

        if (maxDepth > maxPos)
        {
            float ratio = maxDepth / maxPos; // Mathf.Min(maxDepth / maxPos, 2f);
            Submarine.parent.GetComponent<UnityEngine.UI.AspectRatioFitter>().aspectRatio = ratio;
            //Submarine.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * ratio - 100f, Submarine.parent.GetComponent<RectTransform>().sizeDelta.y);
            maxPos = maxDepth;// /ratio;
        }
        else
        {
            Submarine.parent.GetComponent<UnityEngine.UI.AspectRatioFitter>().aspectRatio = 1f;
            maxDepth = Mathf.Max(maxDepth, maxPos); // to ensure playing on square
            //maxPos = Mathf.Max(maxDepth, maxPos);   // to ensure playing on square
        }
        
        yield return new WaitForEndOfFrame();
        Submarine.gameObject.SetActive(true);

        counter = 20; depth = 0; aim = 0; pos = 0;   // reset values
        foreach (string command in input.Split('\n'))
        {
            string[] cmds = command.Split(' ');

            int val = int.Parse(cmds[1]);
            switch (cmds[0])
            {
                case "up": 
                    aim -= val;
                    DisplayLabel.text = "Command : " + command + System.Environment.NewLine + "Position: " + pos + System.Environment.NewLine + "Depth: " + depth + System.Environment.NewLine + "Aim: " + aim;
                    yield return coRotate(Mathf.Rad2Deg * Mathf.Acos(1f / aim));
                    break;
                case "down":
                    aim += val;
                    DisplayLabel.text = "Command : " + command + System.Environment.NewLine + "Position: " + pos + System.Environment.NewLine + "Depth: " + depth + System.Environment.NewLine + "Aim: " + aim;
                    yield return coRotate(Mathf.Rad2Deg * Mathf.Acos(1f/aim));
                    break;   
                case "forward":
                    pos += val;
                    depth += aim * val;
                    DisplayLabel.text = "Command : " + command + System.Environment.NewLine + "Position: " + pos + System.Environment.NewLine + "Depth: " + depth + System.Environment.NewLine + "Aim: " + aim;
                    yield return coTranslate(new Vector2( Mathf.Clamp01(pos/maxPos), 1f-Mathf.Clamp01(depth/maxDepth)));
                    break;
                default:
                    Debug.LogError("Command is not correct " + cmds[0]);
                    break;
            }


            counter--;
            if (counter <= 0)
                break;

            yield return new WaitForSeconds(0.4f);
        }
    }


    const float TRANSLATION_DURATION = 2f;
    IEnumerator coTranslate(Vector2 to)
    {
        Vector2 from = Submarine.anchorMin;
        Debug.LogWarning(from.ToString() + " -> " + to.ToString());

        // todo activate bubbles
        BubbleSpawner.enabled = true;

        float t0 = Time.time;
        while (Time.time - t0 < TRANSLATION_DURATION)
        {
            Vector2 ratio = Vector2.Lerp(from, to, (Time.time - t0) / TRANSLATION_DURATION);
            Submarine.anchorMin = ratio;
            Submarine.anchorMax = ratio;
            yield return new WaitForEndOfFrame();
        }

        Submarine.anchorMin = to;
        Submarine.anchorMax = to;

        // todo disables bubbles
        BubbleSpawner.enabled = false;
    }

    const float ROTATION_SPEED = 0.2f;
    IEnumerator coRotate(float to)
    {
        float startTime = Time.time;

        if (to > Submarine.transform.rotation.eulerAngles.z)
        {
            while (Submarine.transform.rotation.eulerAngles.z + ROTATION_SPEED < to)
            {
                Submarine.transform.Rotate(0, 0, ROTATION_SPEED);
                yield return new WaitForEndOfFrame();
            }
            Submarine.transform.Rotate(0, 0, to - Submarine.transform.rotation.eulerAngles.z);
        }
        else if (to < Submarine.transform.rotation.eulerAngles.z)
        {
            while (to < Submarine.transform.rotation.eulerAngles.z - ROTATION_SPEED)
            {
                Submarine.transform.Rotate(0, 0, -ROTATION_SPEED);
                yield return new WaitForEndOfFrame();
            }
            Submarine.transform.Rotate(0, 0, to - Submarine.transform.rotation.eulerAngles.z);
        }
        
        yield return new WaitForSeconds(Mathf.Max(0, 1.2f-(Time.time - startTime)));

    }
}
