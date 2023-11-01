using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PauseAudioSource : MonoBehaviour, IDependency<Pauser>
{

    private new AudioSource audio;

    private Pauser pauser;
    public void Construct(Pauser obj) => pauser = obj;

    private void Start()
    {
        audio = GetComponent<AudioSource>();

        pauser.PauseStateChange += OnPauseStateChange;
    }

    private void OnPauseStateChange(bool pause)
    {
        if (pause == true) audio.Play();
        if (pause == false) audio.Play();
    }
}
