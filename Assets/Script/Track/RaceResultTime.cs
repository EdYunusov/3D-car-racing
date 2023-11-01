using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RaceResultTime : MonoBehaviour, IDependency<RaceStateTraker>, IDependency<RaceTimeTraker>
{
    public static string SaveMark = "player_best_time";

    public event UnityAction UpdateResults;

    [SerializeField] private float goldTime;


    private float playerRecordTime;
    public float GoldTime => goldTime;
    public float PlayerRecordTime => playerRecordTime;
    private float currentTime;
    public float CurrentTime => currentTime;

    public bool RecordWasSet => playerRecordTime != 0;

    private RaceStateTraker raceStateTraker;
    public void Construct(RaceStateTraker obj) => this.raceStateTraker = obj;

    private RaceTimeTraker raceTimeTraker;
    public void Construct(RaceTimeTraker obj) => this.raceTimeTraker = obj;

    private void Awake()
    {
        Load();
    }

    private void Start()
    {
        raceStateTraker.Complited += OnRaceComplited;
    }

    private void OnDestroy()
    {
        raceStateTraker.Complited -= OnRaceComplited;
    }

    private void OnRaceComplited()
    {
        float absoluteRecord = GetAbsoluteRecord();

        if (raceTimeTraker.CurrentTime < absoluteRecord || playerRecordTime == 0)
        {
            playerRecordTime = raceTimeTraker.CurrentTime;
            Save();
        }
        currentTime = raceTimeTraker.CurrentTime;

        UpdateResults?.Invoke();
    }

    public float GetAbsoluteRecord()
    {
        if (playerRecordTime < goldTime && playerRecordTime != 0)
        {
            return playerRecordTime;
        }
        else
        {
            return goldTime;
        }
    }

    private void Load()
    {
        playerRecordTime = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + SaveMark, 0);
    }

    private void Save()
    {
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + SaveMark, playerRecordTime);
    }
}
