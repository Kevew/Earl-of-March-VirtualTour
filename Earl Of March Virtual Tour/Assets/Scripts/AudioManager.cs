using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> shuffle = new List<AudioClip>();

    private AudioSource audio;

    float currsong = 0f;
    float songlength = 0f;
    void Start()
    {
        currsong = 0f;
        audio = GetComponent<AudioSource>();
        songlength = audio.clip.length;
        realSuffle();
    }

    void Update()
    {
        if (currsong < songlength)
        {
            currsong += Time.deltaTime;
        }
        else
        {
            realSuffle();
        }
    }

    public void realSuffle()
    {
        currsong = 0f;
        int a = Random.Range(0, shuffle.Count);
        while (shuffle[a].length == songlength)
        {
            a = Random.Range(0, shuffle.Count);
        }
        songlength = shuffle[a].length;
        audio.clip = shuffle[a];
        audio.Play();
    }
}
