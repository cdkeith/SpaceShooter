using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 3f;
    public float offset = 1f;
    public float direction = -1f;
    public float angle = 0f;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        angle += Time.deltaTime * direction * speed;
        float x = Mathf.Cos(angle) * offset;
        float y = Mathf.Sin(angle) * offset;

        this.transform.position = new Vector2(transform.parent.position.x + x, transform.parent.position.y + y);
        if(angle >= MathF.PI*2 || angle <= MathF.PI*-2)
        {
            angle = 0f;
        }
    }
}
