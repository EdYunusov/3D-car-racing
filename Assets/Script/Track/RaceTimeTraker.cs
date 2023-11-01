using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTimeTraker : MonoBehaviour, IDependency<RaceStateTraker>
{
    private RaceStateTraker raceStateTraker;
    public void Construct(RaceStateTraker obj) => this.raceStateTraker = obj;

    private float currenTime;
    public float CurrentTime => currenTime;

    [SerializeField] private GameObject resultPanel;

    private void Start()
    {
        raceStateTraker.Started += OnRaceStarted;
        raceStateTraker.Complited += OnRaceFinished;

        enabled = false;
        resultPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        raceStateTraker.Started -= OnRaceStarted;
        raceStateTraker.Complited -= OnRaceFinished;
    }

    private void OnRaceFinished()
    {
        enabled = false;
        resultPanel.SetActive(true);
    }

    private void OnRaceStarted()
    {
        enabled = true;
        resultPanel.SetActive(false);
        currenTime = 0;
    }

    private void Update()
    {
        currenTime += Time.deltaTime;
    }
}
