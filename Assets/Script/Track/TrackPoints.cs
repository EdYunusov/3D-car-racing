using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(BoxCollider))]

public class TrackPoints : MonoBehaviour
{
    public event UnityAction<TrackPoints> Triggered;
    protected virtual void OnPassed() { }
    protected virtual void OnAssignAsTarget() { }

    public TrackPoints next;
    public bool IsFirst;
    public bool IsLast;

    protected bool isTarget;
    public bool IsTarget => isTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.GetComponent<Car>() == false) return;

        Triggered?.Invoke(this);
    }

    public void Passed()
    {
        isTarget = false;
        OnPassed();
    }

    public void AssignAsTarget()
    {
        isTarget = true;
        OnAssignAsTarget();
    }

    public void Reset()
    {
        next = null;
        IsFirst = false;
        IsLast = false;
    }
}
