using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapController : MonoBehaviour
{
    public float lifespan = .6f;
    
    // Start is called before the first frame update
    void Start()
    {
         Destroy(gameObject, lifespan);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
