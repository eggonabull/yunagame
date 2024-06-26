using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
 
public class videoscript : MonoBehaviour
{
 
     VideoPlayer video;
     [SerializeField] private int targetScene;
 
    void Start()
    {
        video = GetComponent<VideoPlayer>();
        video.loopPointReached += OnMovieEnded;
    }
 
     void OnMovieEnded(VideoPlayer vp)
    {
        SceneManager.LoadScene(targetScene);
    }
}
