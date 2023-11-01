using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRaceRecordTraker : MonoBehaviour, IDependency<RaceStateTraker>, IDependency<RaceResultTime>
{
    [SerializeField] private GameObject goldRecordObject;
    [SerializeField] private GameObject playerRecordObject;
    [SerializeField] private Text goldRecordTime;
    [SerializeField] private Text playerRecordTime;

    [SerializeField] private GameObject recordTime;
    [SerializeField] private GameObject currentPlayerTime;

    [SerializeField] private Text recordTimeText;
    [SerializeField] private Text currentPlayerTimeText;

    private RaceStateTraker raceStateTraker;
    public void Construct(RaceStateTraker obj) => this.raceStateTraker = obj;

    private RaceResultTime raceResultTime;
    public void Construct(RaceResultTime obj) => this.raceResultTime = obj;

    private void Start()
    {
        raceStateTraker.Started += OnRaceStarted;
        raceStateTraker.Complited += OnRaceFinished;
        raceResultTime.UpdateResults += OnRaceFinished;

        goldRecordObject.SetActive(false);
        playerRecordObject.SetActive(false);

        recordTime.SetActive(false);
        currentPlayerTime.SetActive(false);
    }


    private void OnDestroy()
    {
        raceStateTraker.Started -= OnRaceStarted;
        raceStateTraker.Complited -= OnRaceFinished;
        raceResultTime.UpdateResults -= OnRaceFinished;
    }

    private void OnRaceFinished()
    {
        recordTime.SetActive(true);
        currentPlayerTime.SetActive(true);

        recordTimeText.text = StringTime.SecondToTimeString(raceResultTime.PlayerRecordTime);
        currentPlayerTimeText.text = StringTime.SecondToTimeString(raceResultTime.CurrentTime);

        goldRecordObject.SetActive(false);
        playerRecordObject.SetActive(false);
    }

    private void OnRaceStarted()
    {
        if (raceResultTime.PlayerRecordTime > raceResultTime.GoldTime || raceResultTime.RecordWasSet == false)
        {
            goldRecordObject.SetActive(true);
            goldRecordTime.text = StringTime.SecondToTimeString(raceResultTime.GoldTime);
        }

        if (raceResultTime.PlayerRecordTime != 0)
        {
            playerRecordObject.SetActive(true);
            playerRecordTime.text = StringTime.SecondToTimeString(raceResultTime.PlayerRecordTime);
        }
    }
}
