using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceInputController : MonoBehaviour, IDependency<RaceStateTraker>, IDependency<CarInputController>
{

    private CarInputController controller;
    public void Construct(CarInputController obj) => this.controller = obj;

    private RaceStateTraker raceStateTraker;
    public void Construct(RaceStateTraker obj) => this.raceStateTraker = obj;

    private void Start()
    {
        raceStateTraker.Started += OnRaceStarted;
        raceStateTraker.Complited += OnRaceFinished;

        controller.enabled = false;
    }

    private void OnRaceFinished()
    {
        controller.enabled = false;
    }

    private void OnRaceStarted()
    {
        controller.enabled = true;
        controller.Stop();
    }

    private void OnDestroy()
    {
        raceStateTraker.Started -= OnRaceStarted;
        raceStateTraker.Complited -= OnRaceFinished;
    }
}
