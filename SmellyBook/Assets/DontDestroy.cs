using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DontDestroy : MonoBehaviour
{
    Character currentCharacter;
    PlayerController playerController;
    // Start is called before the first frame update

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("DontDestroy");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
    

        DontDestroyOnLoad(this.gameObject);
    }

    // private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    //     playerController = FindObjectOfType<PlayerController>();
    //     Debug.Log(currentCharacter);
        
    //     playerController.SetCharacter(currentCharacter);
    // }


    // Update is called once per frame
    void Update()
    {   
        // Debug.Log(currentCharacter);
        // currentCharacter = playerController.GetCharacter();
    }

    
}
