using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WheelAxle
{
    [SerializeField] private WheelCollider leftWheelCollider;
    [SerializeField] private WheelCollider rightWheelCollider;

    [SerializeField] private Transform leftWheelMesh;
    [SerializeField] private Transform rightWheelMesh;

    [SerializeField] private bool isMotor;
    [SerializeField] private bool isSteer;

    [SerializeField] private float wheelWidth;
    [SerializeField] private float antiRollForce;

    [SerializeField] private float additionalWheelDownForce;

    [SerializeField] private float baseForwardStiffnes = 1.5f;
    [SerializeField] private float stabilityForwardFactor = 1.0f;

    [SerializeField] private float baseSidewaysStiffnes = 2.0f;
    [SerializeField] private float stabilitySidewaysFactor = 1.0f;

    private WheelHit leftWheelHit;
    private WheelHit rightWheelHit;

    public bool IsMotor => isMotor;
    public bool IsSteer => isSteer;

    //public API

    public void Update()
    {
        UpdateWheelHits();

        ApplyAntiRoll();
        ApplyDownForce();
        CorrectStiffnes();

        SyncMeshTransform();
    }

    public void ConfigureVehicleSubsteps(int speedThreshold, int speedBelowThreshold, int stepsAboveTreshold)
    {
        leftWheelCollider.ConfigureVehicleSubsteps(speedThreshold, speedBelowThreshold, stepsAboveTreshold);
        rightWheelCollider.ConfigureVehicleSubsteps(speedThreshold, speedBelowThreshold, stepsAboveTreshold);
    }

    public void ApplySteerAngle(float steerAngle, float wheelBaseLength)
    {
        if (isSteer == false) return;

        float radius = Mathf.Abs(wheelBaseLength * Mathf.Tan( Mathf.Deg2Rad  * ( 90 - Mathf.Abs(steerAngle))));
        float angleSign = Mathf.Sign(steerAngle);

        if(steerAngle > 0)
        {
            leftWheelCollider.steerAngle = Mathf.Rad2Deg * MathF.Atan(wheelBaseLength / (radius + (wheelWidth * 0.5f))) * angleSign;
            rightWheelCollider.steerAngle = Mathf.Rad2Deg * MathF.Atan(wheelBaseLength / (radius - (wheelWidth * 0.5f))) * angleSign; ;
        }
        
        else if(steerAngle < 0)
        {
            leftWheelCollider.steerAngle = Mathf.Rad2Deg * MathF.Atan(wheelBaseLength / (radius - (wheelWidth * 0.5f))) * angleSign; ;
            rightWheelCollider.steerAngle = Mathf.Rad2Deg * MathF.Atan(wheelBaseLength / (radius + (wheelWidth * 0.5f))) * angleSign; ;
        }
        else
        {
            leftWheelCollider.steerAngle = 0;
            rightWheelCollider.steerAngle = 0;
        }


    }

    public void AppluMotorTorque(float motorTorque)
    {
        if (isSteer == false) return;

        leftWheelCollider.motorTorque = motorTorque;
        rightWheelCollider.motorTorque = motorTorque;
    }

    public void ApplyBrakeTorque(float brakeTorque)
    {
        leftWheelCollider.brakeTorque = brakeTorque;
        rightWheelCollider.brakeTorque = brakeTorque;
    }

    public float GetAvargeRpm()
    {
        return (leftWheelCollider.rpm + rightWheelCollider.rpm) * 0.5f;
    }

    public float GetRadius()
    {
        return leftWheelCollider.radius;
    }

    //private

    private void UpdateWheelHits()
    {
        leftWheelCollider.GetGroundHit(out leftWheelHit);
        rightWheelCollider.GetGroundHit(out rightWheelHit);
    }

    private void CorrectStiffnes()
    {
        WheelFrictionCurve leftForward = leftWheelCollider.sidewaysFriction;
        WheelFrictionCurve rightForward = rightWheelCollider.sidewaysFriction;

        WheelFrictionCurve leftSideways = leftWheelCollider.sidewaysFriction;
        WheelFrictionCurve rightSideways = rightWheelCollider.sidewaysFriction;

        leftForward.stiffness = baseForwardStiffnes + Mathf.Abs(leftWheelHit.forwardSlip) * stabilityForwardFactor;
        rightForward.stiffness = baseForwardStiffnes + Mathf.Abs(rightWheelHit.forwardSlip) * stabilityForwardFactor;

        leftSideways.stiffness = baseSidewaysStiffnes + Mathf.Abs(leftWheelHit.forwardSlip) * stabilitySidewaysFactor;
        rightSideways.stiffness = baseSidewaysStiffnes + Mathf.Abs(rightWheelHit.forwardSlip) * stabilitySidewaysFactor;

        leftWheelCollider.forwardFriction = leftForward;
        rightWheelCollider.forwardFriction = rightForward;

        leftWheelCollider.sidewaysFriction = leftSideways;
        rightWheelCollider.sidewaysFriction = rightSideways;


    }

    private void ApplyDownForce()
    {
        if (leftWheelCollider.isGrounded == true)
            leftWheelCollider.attachedRigidbody.AddForceAtPosition(leftWheelHit.normal * -additionalWheelDownForce *
                leftWheelCollider.attachedRigidbody.velocity.magnitude, leftWheelCollider.transform.position);
        if (rightWheelCollider.isGrounded == true)
            rightWheelCollider.attachedRigidbody.AddForceAtPosition(rightWheelHit.normal * -additionalWheelDownForce *
                rightWheelCollider.attachedRigidbody.velocity.magnitude, rightWheelCollider.transform.position);
    }

    private void ApplyAntiRoll()
    {
        float travelL = 1.0f;
        float travelR = 1.0f;

        if (leftWheelCollider.isGrounded == true)
        {
            travelL = (-leftWheelCollider.transform.InverseTransformPoint(leftWheelHit.point).y - leftWheelCollider.radius) / leftWheelCollider.suspensionDistance;
        }

        if (rightWheelCollider.isGrounded == true)
        {
            travelR = (-rightWheelCollider.transform.InverseTransformPoint(rightWheelHit.point).y - rightWheelCollider.radius) / rightWheelCollider.suspensionDistance;
        }

        float forceDir = (travelL - travelR);

        if (leftWheelCollider.isGrounded == true)
            leftWheelCollider.attachedRigidbody.AddForceAtPosition(leftWheelCollider.transform.up * -forceDir * antiRollForce, leftWheelCollider.transform.position);
        if (rightWheelCollider.isGrounded == true)
            rightWheelCollider.attachedRigidbody.AddForceAtPosition(rightWheelCollider.transform.up * forceDir * antiRollForce, rightWheelCollider.transform.position);
    }

    private void SyncMeshTransform()
    {
        UpdateWheelTransform(leftWheelCollider, leftWheelMesh);
        UpdateWheelTransform(rightWheelCollider, rightWheelMesh);
    }

    private void UpdateWheelTransform(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;

        wheelCollider.GetWorldPose(out position, out rotation);

        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }
}
