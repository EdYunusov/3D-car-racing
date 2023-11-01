using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputController : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private AnimationCurve breakCruve;
    [SerializeField] private AnimationCurve steerCruve;

    [SerializeField] [Range(0.0f, 1.0f)] private float autoBreakStrenght = 0.5f;

    private float wheelSpeed;
    private float verticalAxis;
    private float horizontalAxis;
    private float handBreadAxis;

    private void Update()
    {
        wheelSpeed = car.WheelSpeed;

        UpdateAxis();

        UpdateTorqueAndBreak();
        UpdateSteer();
        UpdateAutoBreak();

        //to debug

        if (Input.GetKeyDown(KeyCode.E))
            car.UpGear();
        if (Input.GetKeyDown(KeyCode.Q))
            car.DownGear();
    }

    private void UpdateTorqueAndBreak()
    {
        if (Mathf.Sign(verticalAxis) == Mathf.Sign(wheelSpeed) || Mathf.Abs(wheelSpeed) < 0.5f)
        {
            car.ThrottleControl = Mathf.Abs(verticalAxis);
            car.BrakeControl = 0;
        }
        else
        {
            car.ThrottleControl = 0;
            car.BrakeControl = breakCruve.Evaluate(wheelSpeed / car.MaxSpeed);
        }

        //gears

        if (verticalAxis < 0 && wheelSpeed > -0.5f && wheelSpeed <= 0.5f) car.ShiftToReverseGear();

        if (verticalAxis > 0 && wheelSpeed > -0.5f && wheelSpeed < 0.5f) car.ShiftToFirstGear();
    }



    private void UpdateAutoBreak()
    {
        if (verticalAxis == 0)
        {
            car.BrakeControl = breakCruve.Evaluate(wheelSpeed / car.MaxSpeed) * autoBreakStrenght;
        }
    }

    private void UpdateSteer()
    {
        car.SteerControl = steerCruve.Evaluate(wheelSpeed / car.MaxSpeed) * horizontalAxis;
    }

    private void UpdateAxis()
    {
        verticalAxis = Input.GetAxis("Vertical"); 
        horizontalAxis = Input.GetAxis("Horizontal");
        handBreadAxis = Input.GetAxis("Jump");
    }

    public void Reset()
    {
        verticalAxis = 0;
        horizontalAxis = 0;
        handBreadAxis = 0;

        car.ThrottleControl = 0;
        car.SteerControl = 0;
        car.BrakeControl = 0;
        car.HandBrakeControl = 0;
    }
    public void Stop()
    {
        Reset();

        car.BrakeControl = 1;
    }
}
