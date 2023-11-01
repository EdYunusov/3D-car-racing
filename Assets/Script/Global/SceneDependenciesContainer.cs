using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDependency<T>
{
    void Construct(T obj);
}


public class SceneDependenciesContainer : Dependencies
{
    [SerializeField] private Car car;
    [SerializeField] private CarInputController inputController;
    [SerializeField] private CarCameraController carCameraController;
    [SerializeField] private RaceStateTraker raceStateTraker;
    [SerializeField] private RaceTimeTraker raceTimeTraker;
    [SerializeField] private RaceResultTime raceResultTime;
    [SerializeField] private TrackpointCircle trackpointCircle;

    protected override void BindAll(MonoBehaviour monoBehaivorInScene)
    {
        Bind<RaceStateTraker>(raceStateTraker, monoBehaivorInScene);
        Bind<RaceTimeTraker>(raceTimeTraker, monoBehaivorInScene);
        Bind<RaceResultTime>(raceResultTime, monoBehaivorInScene);
        Bind<TrackpointCircle>(trackpointCircle, monoBehaivorInScene);
        Bind<Car>(car, monoBehaivorInScene);
        Bind<CarInputController>(inputController, monoBehaivorInScene);
        Bind<CarCameraController>(carCameraController, monoBehaivorInScene);
    }


    private void Awake()
    {
        FindAllObjectToBind();
    }
}
