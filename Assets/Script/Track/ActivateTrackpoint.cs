using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTrackpoint : TrackPoints
{
    [SerializeField] private GameObject hint;

    private void Start()
    {
        hint.SetActive(isTarget);
    }

    protected override void OnPassed()
    {
        hint.SetActive(false);
    }

    protected override void OnAssignAsTarget()
    {
        hint.SetActive(true);
    }
}
