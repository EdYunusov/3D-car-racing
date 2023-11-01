using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRespawn : MonoBehaviour, IDependency<RaceStateTraker>, IDependency<Car>, IDependency<CarInputController>
{
    private TrackPoints respawnTrackPoint;
    [SerializeField] private float respawnHeight;

    private RaceStateTraker raceStateTraker;
    public void Construct(RaceStateTraker obj) => this.raceStateTraker = obj;

    private Car car;
    public void Construct(Car obj) => this.car = obj;

    private CarInputController controller;
    public void Construct(CarInputController obj) => this.controller = obj;

    private void Start()
    {
        raceStateTraker.TrackPointPassed += OnTrackPointPassed;
    }

    private void OnDestroy()
    {
        raceStateTraker.TrackPointPassed -= OnTrackPointPassed;
    }

    private void OnTrackPointPassed(TrackPoints points)
    {
        respawnTrackPoint = points;
    }

    public void Respawn()
    {
        if (respawnTrackPoint == null) return;

        if (raceStateTraker.State != RaceState.Race) return;

        car.Respawn(respawnTrackPoint.transform.position + respawnTrackPoint.transform.up * respawnHeight, respawnTrackPoint.transform.rotation);

        controller.Reset();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) == true)
        {
            Respawn();
        }
    }
}
