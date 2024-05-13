using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public GameObject[] waypoints;
    int current = 0;
    float rotSpeed;
    public float speed;
    float WPRadius = 1.0f;
    void Update()
    {
        if (Vector2.Distance(waypoints[current].transform.position, transform.position) < WPRadius)
        current++;
        if (current >= waypoints.Length)
        {
            current = 0;
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime *speed);
    }
}
