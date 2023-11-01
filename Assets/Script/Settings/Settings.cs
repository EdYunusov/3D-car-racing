using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class Difficulty : Settings
//{
//    public enum DifficultyType
//    {

//    }

//    public DifficultyType GetDifficulty()
//    {
//        return (DifficultyType) GetValue();
//    }
//}


public class Settings : ScriptableObject
{
    [SerializeField] protected string title;
    public string Title => title;

    public virtual bool isMinValue { get; }
    public virtual bool isMaxValue { get; }

    public virtual void SetNextValue() { }
    public virtual void SetPreviousValue() { }
    public virtual object GetValue() { return default(object); }
    public virtual string GetStringValue() { return string.Empty; }

    public virtual void Apply() { }
    public virtual void Load() { }
}
