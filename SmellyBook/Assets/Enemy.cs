using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float leftX;
    public float rightX;
    public float speed = 1.0f;

    private Vector3 pos1, pos2;
    // Start is called before the first frame update
    void Start()
    {
        pos1 = new Vector3(transform.position.x - leftX, transform.position.y, transform.position.z);
        pos2 = new Vector3(transform.position.x + rightX, transform.position.y, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
         transform.position = Vector3.Lerp (pos1, pos2, Mathf.PingPong(Time.time*speed, 1.0f));
    }
}
