using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{ 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitTime(5f));
    }

    // Update is called once per frame
    void Update()
    {
       transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + 3), .2f * Time.deltaTime);
       if(Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1")) {
            StopAllCoroutines();
            SceneManager.LoadScene("IntroScene");
       }
    }


    IEnumerator WaitTime(float t) {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene("IntroScene");
    }
}
