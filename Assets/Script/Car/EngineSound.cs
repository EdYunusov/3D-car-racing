using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EngineSound : MonoBehaviour
{
    [SerializeField] private Car car;

    [SerializeField] private float pitchModifier;
    [SerializeField] private float volumeModifier;
    [SerializeField] private float rmpModifier;

    [SerializeField] private float basePitch = 1.0f;
    [SerializeField] private float baseVolume = 0.4f;


    private AudioSource engineAS;

    private void Start()
    {
        engineAS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        engineAS.pitch = basePitch + pitchModifier * ((car.EngineRMP / car.EngineMaxRMP) * rmpModifier);
        engineAS.volume = baseVolume + volumeModifier * (car.EngineRMP / car.EngineMaxRMP);
    }
}
