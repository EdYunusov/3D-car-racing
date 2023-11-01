using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraFovCorrection : CarCameraComponent
{
    [SerializeField] private new Camera camera;

    [SerializeField] private float minFOV;
    [SerializeField] private float maxFOV;

    private float defaultFOV;

    private void Start()
    {
        camera.fieldOfView = defaultFOV;
    }

    private void Update()
    {
        camera.fieldOfView = Mathf.Lerp(minFOV, maxFOV, car.NormalLinearVelocity);
    }
}
