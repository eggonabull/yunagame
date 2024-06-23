using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        AudioSource introHook = audioSources[0];
        AudioSource menuMusic = audioSources[1];

        double playTime = AudioSettings.dspTime + introHook.clip.samples / introHook.clip.frequency - 2.60d;
        // print("Current time: " + AudioSettings.dspTime + " Play time: " + playTime);
        menuMusic.PlayScheduled(playTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
