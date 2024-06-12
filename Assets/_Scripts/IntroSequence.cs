using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroSequence : MonoBehaviour
{
    [SerializeField] private Image[] images;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Button beginButton;

    private float keyDownTime = 0;

    int maxIndex = 0;
    // Start is called before the first frame update

    private float[] timestamps = new float[]
    {
        0,
        7,
        15,
        18,
        22.3f,
        24.5f,
        34,
        38,
        500
    };
    // private int finish_index = 7;

    void Start()
    {
        audioSource.Play();
        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(false);
        }
        beginButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //print("time " + audioSource.time);
        for (int i = maxIndex; i < timestamps.Length - 1; i++)
        {
            if (audioSource.time >= timestamps[i] && audioSource.time <= timestamps[i + 1])
            {
                images[i].gameObject.SetActive(true);
                if (i == timestamps.Length - 2)
                {
                    maxIndex = i + 2;
                    beginButton.gameObject.SetActive(true);
                }
                else { maxIndex = i; }

            }
            else
            {
                images[i].gameObject.SetActive(false);
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (keyDownTime > 0)
            {
                if (Time.time - keyDownTime > 2f)
                {
                    LoadNextScene();
                }
            }
            else
            {
                keyDownTime = Time.time;
            }
        }
        else
        {
            keyDownTime = 0;
        }

    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
