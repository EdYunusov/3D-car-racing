using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonSelectable : UIButton
{
    [SerializeField] private GameObject selectImage;

    public UnityEvent OnSelect;
    public UnityEvent OnUnselect;

    private void Start()
    {
        selectImage.SetActive(false);
    }

    public override void SetFocuse()
    {
        base.SetFocuse();

        selectImage.SetActive(true);

        OnSelect?.Invoke();
    }

    public override void SetUnfocuse()
    {
        base.SetUnfocuse();

        selectImage.SetActive(false);
        OnUnselect?.Invoke();
    }
    public static void ExitGame()
    {
        Application.Quit();
    }
}
