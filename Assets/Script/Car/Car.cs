using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CarChassis))]
[RequireComponent(typeof(AudioSource))]
public class Car : MonoBehaviour
{
    public event UnityAction<string> GearChanged;


    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float maxBrakeTorque;


    [Header("Engine")]
    [SerializeField] private AnimationCurve engineTorqueCurve;
    [SerializeField] private float engineMaxTorque;
    [SerializeField] private float engineTorque;

    //Debug
    [SerializeField] private float engineRPM;
    [SerializeField] private float engineMinRMP;
    [SerializeField] private float engineMaxRMP;


    [Header("Gearbox")]
    [SerializeField] private float[] gears;
    [SerializeField] private AudioClip changeGearboxSound;
    [SerializeField] private float finalDriveRatio;

    //Debug
    [SerializeField] private float selectedGear;
    [SerializeField] private float rearGear;
    [SerializeField] private float upShiftEngineRPM;
    [SerializeField] private float downShiftEngineRPM;

    [SerializeField] private int selectedGearIndex;


    [SerializeField] private int maxSpeed;

    public float LinearVelocity => chassis.LinearVelocity;
    public float NormalLinearVelocity => chassis.LinearVelocity / maxSpeed;
    public float WheelSpeed => chassis.GetWheelSpeed();
    public float MaxSpeed => maxSpeed;

    public float EngineRMP => engineRPM;

    public float EngineMaxRMP => engineMaxRMP;


    private AudioSource audioSource;
    private CarChassis chassis;
    public Rigidbody Rigidbody => chassis == null ? GetComponent<CarChassis>().Rigidbody : chassis.Rigidbody;

    //Debug 
    [SerializeField] private float linearVelocity;
    public float ThrottleControl;
    public float SteerControl;
    public float BrakeControl;
    public float HandBrakeControl;

    private void Start()
    {
        chassis = GetComponent<CarChassis>();

        audioSource = GetComponent<AudioSource>();
    }

    
    private void Update()
    {
        linearVelocity = LinearVelocity;

        UpdateEngineTorque();
        AutoShiftGear();

        if (LinearVelocity >= maxSpeed)
            engineTorque = 0;

        chassis.MotorTorque = ThrottleControl * engineTorque;
        chassis.BrakeTorque = BrakeControl * maxBrakeTorque;
        chassis.SteerAngle = SteerControl * maxSteerAngle;
    }

    //Gearbox

    public string GetSelectedGearName()
    {
        if (selectedGear == rearGear) return "R";

        if (selectedGear == 0) return "N";

        return (selectedGearIndex + 1).ToString();
    }

    private void AutoShiftGear()
    {
        if (selectedGear < 0) return;

        if (engineRPM >= upShiftEngineRPM) UpGear();

        if (engineRPM < downShiftEngineRPM) DownGear();

        //selectedGearIndex = Mathf.Clamp(selectedGearIndex, 0, gears.Length - 1);
    }

    public void UpGear()
    {
        ShiftGear(selectedGearIndex + 1);
        PlayGearChangeSound();
    }

    public void DownGear()
    {
        ShiftGear(selectedGearIndex - 1);
        PlayGearChangeSound();
    }

    public void ShiftToReverseGear()
    {
        selectedGear = rearGear;

        GearChanged?.Invoke(GetSelectedGearName());
    }

    public void ShiftToFirstGear()
    {
        ShiftGear(0);
    }

    public void ShiftToNetral()
    {
        selectedGear = 0;
        GearChanged?.Invoke(GetSelectedGearName());
    }

    private void ShiftGear(int gearIndex)
    {
        gearIndex = Mathf.Clamp(gearIndex, 0, gears.Length - 1);
        selectedGear = gears[gearIndex];
        selectedGearIndex = gearIndex;

        GearChanged?.Invoke(GetSelectedGearName());
    }

    public void PlayGearChangeSound()
    {
        if (changeGearboxSound != null && selectedGearIndex != 0 && selectedGearIndex != 1 && selectedGearIndex != 6)
        {
            audioSource.PlayOneShot(changeGearboxSound);
        }
    }

    private void UpdateEngineTorque()
    {
        engineRPM = engineMinRMP + Mathf.Abs(chassis.GetAverageRMP() * selectedGear * finalDriveRatio);
        engineRPM = Mathf.Clamp(engineRPM, engineMinRMP, engineMaxRMP);

        engineTorque = engineTorqueCurve.Evaluate(LinearVelocity / maxSpeed) * engineMaxTorque * finalDriveRatio * Mathf.Sign(selectedGear);
    }

    public void Respawn(Vector3 position, Quaternion rotation)
    {
        Reset();
        
        transform.position = position;
        transform.rotation = rotation;
    }

    public void Reset()
    {
        chassis.Reset();

        chassis.MotorTorque = 0;
        chassis.BrakeTorque = 0;

        ThrottleControl = 0;
        BrakeControl = 0;
        SteerControl = 0;
        HandBrakeControl = 0;
    }
}
