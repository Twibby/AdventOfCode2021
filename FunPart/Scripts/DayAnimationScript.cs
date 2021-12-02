using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAnimationScript : MonoBehaviour
{
    [HideInInspector]
    public string input;
    public virtual IEnumerator part_1() { Debug.LogError("[" + this.GetType().ToString() + "] Animation part_1 is not defined"); yield break; }
    public virtual IEnumerator part_2() { Debug.LogError("[" + this.GetType().ToString() + "] Animation part_2 is not defined"); yield break; }
}
