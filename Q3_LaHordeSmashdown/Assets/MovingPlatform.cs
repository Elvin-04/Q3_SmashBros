using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Parameters")]
    public List<Transform> waypoints;
    public float speed;

    Vector3 destination;
    Vector3 newPos;
    float distance;
    int index = 0;
    bool goBack = false;

    private void Update()
    {
        //destination = waypoints[index].position;
        //newPos = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        //transform.position = newPos;

        //distance = Vector2.Distance(transform.position, destination);
        //if (distance <= 0.05f)
        //{
        //    if (!goBack && index < waypoints.Count - 1)
        //        index++;
        //    else if (index == waypoints.Count - 1)
        //        goBack = true;

        //    if(goBack && index > 0)
        //        index--;
        //    else if(index == 0)
        //        goBack = false;
        //}
    }
}
