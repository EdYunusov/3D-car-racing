using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceKeyboardStarted : MonoBehaviour, IDependency<RaceStateTraker>
{
    private RaceStateTraker raceStateTraker;
    public void Construct(RaceStateTraker obj) => this.raceStateTraker = obj;

    [SerializeField] private GameObject hintText;

    private void Start()
    {
        hintText.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) == true)
        {
            raceStateTraker.StartTimer();
            hintText.SetActive(false);
        }
    }
}
