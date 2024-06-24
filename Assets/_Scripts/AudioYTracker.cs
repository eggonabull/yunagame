using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioYTracker : MonoBehaviour
{
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        AudioClip clip = audioSource.clip;
        string clipName = clip.name;
        float y = player.position.y;
        if (clipName == "SFX_ambience_forest" && player.position.y < -1400)
        {
            y = -1400;
        }
        Vector3 pos = transform.position;
        pos.y = y;
        transform.position = pos;   
    }
}
