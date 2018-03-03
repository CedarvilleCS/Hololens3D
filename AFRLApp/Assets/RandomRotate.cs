using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotate : MonoBehaviour
{
    private Quaternion qTo;
    public float timer;
    public float rotateSpeed;
    public float speed;
    public float waitTime;

    void Start()
    {
        qTo = Quaternion.Euler(new Vector3(0.0f, Random.Range(-180.0f, 180.0f), 0.0f));
    }

    void Update()
    {

        timer += Time.deltaTime;


        if (timer > waitTime)
        { // timer resets at 2, allowing .5 s to do the rotating
            qTo = Quaternion.Euler(new Vector3(Random.Range(-180.0f, 180.0f), Random.Range(-180.0f, 180.0f), Random.Range(-180.0f, 180.0f)));
            timer = 0.0f;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, qTo, Time.deltaTime * rotateSpeed);
    }
}


