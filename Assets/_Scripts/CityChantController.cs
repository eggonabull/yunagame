using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityChantController : MonoBehaviour
{

    private AudioSource audioSource;
    public List<AudioClip> cityChants;

    int TIME_BETWEEN_CHANTS = 11;
    float timeSinceLastChant = 0;
    // Start is called before the first frame update

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastChant += Time.deltaTime;
        if (timeSinceLastChant > TIME_BETWEEN_CHANTS)
        {
            timeSinceLastChant = 0;
            PlayRandomChant();
        }
    }


    void PlayRandomChant()
    {
        int index = Random.Range(0, cityChants.Count);
        audioSource.clip = cityChants[index];
        audioSource.Play();
    }
}
