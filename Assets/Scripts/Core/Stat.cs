using System;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float currentValue;

    [SerializeField] private float maxValue;

    [SerializeField] private GameEvent valueChanged;

    public float Value => currentValue;

    public float Max => maxValue;

    public void Add(float amount)
    {
        currentValue = Math.Min(currentValue + amount, maxValue);
        RaiseEvent();
    }

    public void Remove(float amount)
    {
        currentValue = Math.Max(currentValue - amount, 0);
        RaiseEvent();
    }

    public void ResetStat()
    {
        currentValue = maxValue;
        RaiseEvent();
    }

    public void ChangeMax(float amount)
    {
        float amountToAdd = amount - maxValue;
        maxValue = amount;
        currentValue = Math.Min(currentValue + amountToAdd, maxValue);
        RaiseEvent();
    }

    private void RaiseEvent()
    {
        if (valueChanged != null) valueChanged.Raise();
    }
}
