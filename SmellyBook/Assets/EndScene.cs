using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScene : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    // Start is called before the first frame update
    void Start()
    {
        int deaths = Stats.DeathCounter;
        textMeshProUGUI.text = $"You Died: {deaths} times";

    }

    // Update is called once per frame
    void Update()
    {
         if(Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1")) {
             Stats.DeathCounter = 0;
            SceneManager.LoadScene("1-1");
       }
    }
}
