using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SpeedVisualEffect : CarCameraComponent
{

    [Header("Flow Effect")]

    [SerializeField] [Range(0.0f, 1.0f)] private float normalizeSpeedShake;
    [SerializeField] private float shakeAmount;


    [Header("Sound")]
    [SerializeField] private AudioSource windSound;
    [SerializeField] private float minWindSound = 0.2f;
    [SerializeField] private float maxWindSound = 1.0f;


    private void Update()
    {
        if (car.NormalLinearVelocity >= normalizeSpeedShake)
        {
            transform.localPosition += Random.insideUnitSphere * shakeAmount * Time.deltaTime;

            if (windSound.isPlaying)
            {
                float windVolume = Mathf.Lerp(minWindSound, maxWindSound, car.NormalLinearVelocity);
                windSound.volume = windVolume;
                windSound.Play();
            }
        }
        else
        {

            if (windSound.isPlaying)
            {
                windSound.Stop();
            }
        }
            
    }
}
