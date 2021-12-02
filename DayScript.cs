using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayScript : MonoBehaviour
{
    public bool IsDebug = false;
    public bool IsTestInput = false;
    public bool Part1 = true;
    public bool Part2 = false;

    protected string _input;
    protected int _day
    {
        get { return int.Parse(this.GetType().ToString().Substring(this.GetType().ToString().Length-2)); }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(coDay());
    }

    protected IEnumerator coDay()
    {
        Debug.LogWarning("Day is : " + _day);

        if (!IsTestInput)
        {
            yield return StartCoroutine(Tools2021.Instance.GetInput(_day));
            _input = Tools2021.Instance.Input;
        }
        else
        {
            yield return StartCoroutine(Tools2021.Instance.GetTestInput("https://ollivier.iiens.net/AoC/2021/" + _day + ".txt"));
            _input = Tools2021.Instance.Input;
        }

        float t0;
        string log = "", result = "";

        if (Part1)
        {
            t0 = Time.realtimeSinceStartup;
            log = "Started at " + t0;

            result = part_1();

            log += " | Ended at " + Time.realtimeSinceStartup;
            log += " | Part 1 duration is : " + (Time.realtimeSinceStartup - t0).ToString();
            if (IsDebug)
                Debug.Log(log);

            Debug.LogWarning("[Day " + _day.ToString() + "] Part 1 result is : " + result);
        }

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        if (Part2)
        {
            t0 = Time.realtimeSinceStartup;
            log = "Started at " + t0;

            result = part_2();

            log += " | Ended at " + Time.realtimeSinceStartup;
            log += " | Part 1 duration is : " + (Time.realtimeSinceStartup - t0).ToString();
            if (IsDebug)
                Debug.Log(log);

            Debug.LogWarning("[Day " + _day.ToString() + "] Part 2 result is : " + result);
        }

        //yield return new WaitForEndOfFrame();
        //yield return new WaitForSeconds(5f);
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    protected virtual string part_1() { Debug.LogError("[" + this.GetType().ToString() + "] part_1 is not defined"); return "N/A"; }
    protected virtual string part_2() { Debug.LogError("[" + this.GetType().ToString() + "] part_2 is not defined"); return "N/A"; }
}
