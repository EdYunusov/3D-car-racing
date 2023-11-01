using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITrackTime : MonoBehaviour, IDependency<RaceStateTraker>, IDependency<RaceTimeTraker>
{
    private RaceStateTraker raceStateTraker;
    public void Construct(RaceStateTraker obj) => this.raceStateTraker = obj;

    private RaceTimeTraker raceTimeTraker;
    public void Construct(RaceTimeTraker obj) => this.raceTimeTraker = obj;

    [SerializeField] private Text text;

    private void Start()
    {
        raceStateTraker.Started += OnRaceStarted;
        raceStateTraker.Complited += OnRaceFinished;

        text.enabled = false;
    }

    private void OnDestroy()
    {
        raceStateTraker.Started -= OnRaceStarted;
        raceStateTraker.Complited -= OnRaceFinished;
    }

    private void OnRaceFinished()
    {
        text.enabled = false;
        enabled = false;
    }

    private void OnRaceStarted()
    {
        text.enabled = true;
        enabled = true;
    }

    private void Update()
    {
        text.text = StringTime.SecondToTimeString(raceTimeTraker.CurrentTime);
    }
}
