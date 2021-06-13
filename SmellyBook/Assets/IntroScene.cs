using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    [SerializeField] Transform box;
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(WaitTime(15f));
    }

    // Update is called once per frame
    void Update()
    {
       box.transform.position = Vector3.MoveTowards(box.transform.position, new Vector3(box.transform.position.x + 190, box.transform.position.y, box.transform.position.z), 5 * Time.deltaTime);
       transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + 190, transform.position.y, transform.position.z), 5 * Time.deltaTime);
       if(Input.GetButtonDown("Jump")) {
           StopAllCoroutines();
            SceneManager.LoadScene("1-1");
       }
    }


    IEnumerator WaitTime(float t) {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene("1-1");
    }
 
}
