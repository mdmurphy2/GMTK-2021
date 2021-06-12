using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI textMeshPro;

    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
         textMeshPro.text = "Deaths: " + Stats.DeathCounter;
    }

    // Update is called once per frame
    void Update()
    {
        //textMeshPro.text = "Deaths: " + Stats.DeathCounter;
    }
}
