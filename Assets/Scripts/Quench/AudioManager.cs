using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public AudioSource buttonDown;
    public AudioSource quenching;
    public AudioSource scoreUp;
    public AudioSource scoreDown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AudioPlay(AudioSource audiosource)
    {
        audiosource.Play();
    }
}
