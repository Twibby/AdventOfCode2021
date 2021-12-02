using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawn : MonoBehaviour
{
    public GameObject BubblePrefab;

    private float _timer = 0;

    private void OnEnable()
    {
        _timer = 0f; // force a bubble to be created on activation
    }
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            GameObject go = GameObject.Instantiate(BubblePrefab, this.transform);
            go.transform.Translate(0, Random.Range(-5f, 5f), 0);
            go.transform.Rotate(0, 0, Random.Range(-15f, 15f));
            _timer = Random.Range(0.15f, 0.55f);
        }
    }
}
