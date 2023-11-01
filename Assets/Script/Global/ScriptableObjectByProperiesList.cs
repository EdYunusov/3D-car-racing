using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectByProperiesList : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject prefab;
    [SerializeField] private ScriptableObject[] properties;

    [ContextMenu(nameof(SpawnInEditor))]
    public void SpawnInEditor()
    {
        if (Application.isPlaying == true) return;

        GameObject[] allObjects = new GameObject[parent.childCount];

        for (int i = 0; i < parent.childCount; i++)
        {
            allObjects[i] = parent.GetChild(i).gameObject;
        }

        for (int i = 0; i < allObjects.Length; i++)
        {
            DestroyImmediate(allObjects[i]);
        }

        for (int i = 0; i < properties.Length; i++)
        {
            GameObject button = Instantiate(prefab, parent);
            IScriptableObjectProperty scriptableObjectProperty = button.GetComponent< IScriptableObjectProperty>();

            scriptableObjectProperty.ApplayProperty(properties[i]);
        }
    }
}
