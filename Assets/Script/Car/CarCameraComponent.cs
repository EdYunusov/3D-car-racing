using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CarCameraController))]
public class CarCameraComponent : MonoBehaviour
{
    protected Car car;
    protected Camera camera;

    public virtual void SetProperties(Car car, Camera camera)
    {
        this.car = car;
        this.camera = camera;
    }
}
