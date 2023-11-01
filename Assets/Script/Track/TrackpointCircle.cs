using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum TrackType
{
    Circular,
    Sprint
}

public class TrackpointCircle : MonoBehaviour
{
    public event UnityAction<TrackPoints> TrackPointTriggered;
    public event UnityAction<int> lapCompleted;

    [SerializeField] private TrackType trackType;
    public TrackType Type => trackType;
    
    private TrackPoints[] points;

    private int lapsCompleted = -1;

    private void Awake()
    {
        BuildCircuit();

    }

    private void Start()
    {
        for (int i = 0; i< points.Length; i++)
        {
            points[i].Triggered += OnTrackTointTriggered;
        }

        points[0].AssignAsTarget();
    }


    private void OnDestroy()
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i].Triggered -= OnTrackTointTriggered;
        }
    }

    private void OnTrackTointTriggered(TrackPoints trackPoints)
    {
        if (trackPoints.IsTarget == false) return;

        trackPoints.Passed();
        trackPoints.next?.AssignAsTarget();

        TrackPointTriggered?.Invoke(trackPoints);

        if ( trackPoints.IsLast == true)
        {
            lapsCompleted++;

            //проверка чекпоинтов
            if (trackType == TrackType.Sprint)
            {
                lapCompleted?.Invoke(lapsCompleted);
            }

            if (trackType == TrackType.Circular)
            {
                if (lapsCompleted > 0) 
                    lapCompleted?.Invoke(lapsCompleted);
            }
        }
    }


    [ContextMenu(nameof(BuildCircuit))]
    private void BuildCircuit()
    {
        points = TrackCircuitBuilder.Build(transform, trackType);
    }

}
