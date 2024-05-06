using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntroAnimation : MonoBehaviour
{
    String _finalText = "Welcome to the game!";

    [SerializeField] private TextMeshProUGUI _text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        //Time.deltaTime;
        //_text.text = "Welcome to the game!";

        // gradually show each letter depending on the time
        for (int i = 0; i < _finalText.Length; i++)
        {
            if (Time.time * 10 > i)
            {
                _text.text = _finalText.Substring(0, i + 1);

            }
        }

        // scroll the text up the screen
        _text.transform.position += new Vector3(0, 0.1f, 0);
    }
}
