using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarChassis : MonoBehaviour
{
    [SerializeField] private WheelAxle[] wheelAxle;
    [SerializeField] private float wheelBaseLength;
    [SerializeField] private Transform centerOfMass;

    [Header("DownForce")]
    [SerializeField] private float downForceMin;
    [SerializeField] private float downForceMax;
    [SerializeField] private float downForceFactor;

    [Header("AngularDrag")]
    [SerializeField] private float angularDragMin;
    [SerializeField] private float angularDragMax;
    [SerializeField] private float angularDragFactor;

    public float MotorTorque;
    public float BrakeTorque;
    public float SteerAngle;

    public float LinearVelocity => rigidbody.velocity.magnitude * 3.6f;


    private new Rigidbody rigidbody;
    public Rigidbody Rigidbody => rigidbody == null? GetComponent<Rigidbody>(): rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        if (centerOfMass != null)
        {
            rigidbody.centerOfMass = centerOfMass.localPosition;
        }

        for( int i = 0; i < wheelAxle.Length; i++)
        {
            wheelAxle[i].ConfigureVehicleSubsteps(50, 50, 50);
        }
    }
    private void FixedUpdate()
    {
        UpdatingAngularDrag();
        UpdateDownForce();
        UpdateWheelAxles();
    }

    public float GetAverageRMP()
    {
        float sum = 0;
        
        for(int i = 0; i < wheelAxle.Length; i++)
        {
            sum += wheelAxle[i].GetAvargeRpm();
        }

        return sum / wheelAxle.Length;
    }

    public float GetWheelSpeed()
    {
        return GetAverageRMP() * wheelAxle[0].GetRadius() * 2 * 0.1885f;
    }    

    private void UpdateDownForce()
    {
        float downForce = Mathf.Clamp(downForceFactor * LinearVelocity, downForceMin, downForceMax);
        rigidbody.AddForce(-transform.up * downForce);
    }

    private void UpdatingAngularDrag()
    {
        rigidbody.angularDrag = Mathf.Clamp(angularDragFactor * LinearVelocity, angularDragMin, angularDragMax);

    }

    private void UpdateWheelAxles()
    {
        int amountMotorWheel = 0;

        for (int i = 0; i < wheelAxle.Length; i++)
        {
            if (wheelAxle[i].IsMotor == true)
                amountMotorWheel += 2;
        }

        for(int i = 0; i < wheelAxle.Length; i++)
        {
            wheelAxle[i].Update();

            wheelAxle[i].AppluMotorTorque(MotorTorque);
            wheelAxle[i].ApplyBrakeTorque(BrakeTorque);
            wheelAxle[i].ApplySteerAngle(SteerAngle, wheelBaseLength);
        }
    }

    public void Reset()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
}
