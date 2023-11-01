using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TrackCircuitBuilder
{
    public static TrackPoints[] Build(Transform trackTransform, TrackType trackType)
    {
        TrackPoints[]  points = new TrackPoints[trackTransform.childCount];

        ResetPoint(trackTransform, points);
        MakeLinks(points, trackType);
        MarkPoints(points, trackType);

        return points;
    }

    private static void ResetPoint(Transform trackTransform, TrackPoints[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = trackTransform.GetChild(i).GetComponent<TrackPoints>();

            if (points[i] == null)
            {
                Debug.LogError("There is no TrackPoints script on one of the child objects");
                return;
            }

            points[i].Reset();
        }
    }

    private static void MakeLinks(TrackPoints[] points, TrackType trackType)
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            points[i].next = points[i + 1];
        }

        if (trackType == TrackType.Circular)
        {
            points[points.Length - 1].next = points[0];
        }
    }

    private static void MarkPoints(TrackPoints[] points, TrackType trackType)
    {
        points[0].IsFirst = true;

        if (trackType == TrackType.Sprint)
        {
            points[points.Length - 1].IsLast = true;
        }

        if (trackType == TrackType.Circular)
        {
            points[0].IsLast = true;
        }
    }
}
