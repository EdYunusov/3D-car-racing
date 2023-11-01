using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum RaceState
{
    Preparation,
    CoudnDown,
    Race,
    Passed
}


public class RaceStateTraker : MonoBehaviour, IDependency<TrackpointCircle> 
{
    public event UnityAction PreperignStart;
    public event UnityAction Started;
    public event UnityAction Complited;
    public event UnityAction<TrackPoints> TrackPointPassed;
    public event UnityAction<int> LapComplited;

    private TrackpointCircle trackPointCircale;
    public void Construct(TrackpointCircle obj) => this.trackPointCircale = obj;

    [SerializeField] private Timer countdownTimer;
    [SerializeField] private int lapsToComplited;

    private RaceState state;
    public RaceState State => state;

    public Timer CountDownTimer => countdownTimer;

    private void StartState(RaceState state)
    {
        this.state = state;
    }

    private void Start()
    {
        countdownTimer.enabled = false;

        countdownTimer.Finished += OnCountdownTimerFinished;

        StartState(RaceState.Preparation);

        trackPointCircale.TrackPointTriggered += OnTrackPointTriggered;
        trackPointCircale.lapCompleted += OnLapComplited;
    }

    private void OnCountdownTimerFinished()
    {
        StartRace();
    }

    private void OnLapComplited(int lapAmount)
    {
        if (trackPointCircale.Type == TrackType.Sprint)
        {
            ComplitedRace();
        }

        if (trackPointCircale.Type == TrackType.Circular)
        {
            if (lapAmount == lapsToComplited) ComplitedRace();
            else ComplitedLap(lapAmount);
        }
    }

    private void OnTrackPointTriggered(TrackPoints trackPoint)
    {
        TrackPointPassed?.Invoke(trackPoint);
    }

    private void OnDestroy()
    {
        countdownTimer.Finished -= OnCountdownTimerFinished;
        trackPointCircale.TrackPointTriggered -= OnTrackPointTriggered;
        trackPointCircale.lapCompleted -= OnLapComplited;
    }

    //Public method

    public void StartTimer()
    {
        if (state != RaceState.Preparation) return;

        StartState(RaceState.CoudnDown);

        countdownTimer.enabled = true;
        PreperignStart?.Invoke();
    }

    private void StartRace()
    {
        if (state != RaceState.CoudnDown) return;

        StartState(RaceState.Race);

        Started?.Invoke();
    }

    private void ComplitedRace()
    {
        if (state != RaceState.Race) return;

        StartState(RaceState.Passed);

        Complited?.Invoke();
    }

    private void ComplitedLap(int lapAmount)
    {
        LapComplited?.Invoke(lapAmount);
    }
}





