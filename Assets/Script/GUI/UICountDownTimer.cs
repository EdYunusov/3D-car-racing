using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICountDownTimer : MonoBehaviour, IDependency<RaceStateTraker>
{
    private RaceStateTraker raceStateTraker;

    public void Construct(RaceStateTraker obj) => raceStateTraker = obj;

    [SerializeField] private Text text;
    private Timer countDownTimer;

    private void Start()
    {
        raceStateTraker.PreperignStart += OnRacePreparingStarted;
        raceStateTraker.Started += OnStarted;

        text.enabled = false;
    }

    private void OnDestroy()
    {
        raceStateTraker.PreperignStart -= OnRacePreparingStarted;
        raceStateTraker.Started -= OnStarted;
    }

    private void OnRacePreparingStarted()
    {
        text.enabled = true;
        enabled = true;
    }

    private void OnStarted()
    {
        text.enabled = false;
        enabled = false;
    }

    private void Update()
    {
        text.text = raceStateTraker.CountDownTimer.Value.ToString("F0");

        if (text.text == "0") text.text = "GO!";
    }

    
}
