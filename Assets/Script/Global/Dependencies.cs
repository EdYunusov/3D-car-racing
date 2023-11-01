using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dependencies : MonoBehaviour
{
    protected virtual void BindAll(MonoBehaviour monoBehaivorInScene)
    {

    }

    protected void FindAllObjectToBind()
    {
        MonoBehaviour[] allMonoBehavioursInScene = FindObjectsOfType<MonoBehaviour>();

        for (int i = 0; i < allMonoBehavioursInScene.Length; i++)
        {
            BindAll(allMonoBehavioursInScene[i]);
        }
    }

    protected void Bind<T>(MonoBehaviour bindObject, MonoBehaviour target) where T : class
    {
        if (target is IDependency<T>) (target as IDependency<T>).Construct(bindObject as T);
    }

}
