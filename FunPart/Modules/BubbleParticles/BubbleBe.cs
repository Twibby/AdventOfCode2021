using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBe : MonoBehaviour
{
    public const float LIFETIME = 2.7f;
    public const float SPEED = 40f;

    private float _timer = LIFETIME;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.SetParent(this.transform.parent.parent.parent, true);
        _timer = LIFETIME;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer < 0)
            Destroy(this.gameObject);

        this.transform.Translate(Time.deltaTime * SPEED, 0, 0, Space.Self);
        _timer -= Time.deltaTime;
    }
}
