using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIRaceButton : UIButtonSelectable, IScriptableObjectProperty
{
    [SerializeField] private RaceInfo raceInfo;
    [SerializeField] private Image icon;
    [SerializeField] private Text title;

    private void Start()
    {
        ApplayProperty(raceInfo);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (raceInfo == null) return;

        SceneManager.LoadScene(raceInfo.SceneName);
    }

    public void ApplayProperty(ScriptableObject property)
    {
        if (property == null) return;

        if (property is RaceInfo == false) return;

        raceInfo = property as RaceInfo;

        icon.sprite = raceInfo.Sprite;
        title.text = raceInfo.Title;
    }
}
