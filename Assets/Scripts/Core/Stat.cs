using System;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float currentValue;

    [SerializeField] private float maxValue;

    [SerializeField] private GameEvent valueChanged;
    [SerializeField] private GameEvent valueIncreased; // Added
    [SerializeField] private GameEvent valueDecreased; // Added

    public float Value => currentValue;

    public float Max => maxValue;

    public void Add(float amount)
    {
        currentValue = Math.Min(currentValue + amount, maxValue);
        //RaiseEvent();
        RaiseChangeEvent(); // Changed
        RaiseIncreaseEvent(); // Added
    }

    public void Remove(float amount)
    {
        currentValue = Math.Max(currentValue - amount, 0);
        //RaiseEvent();
        RaiseChangeEvent();   // Changed
        RaiseDecreaseEvent(); // Added
    }

    public void ResetStat()
    {
        currentValue = maxValue;
        //RaiseEvent();
        RaiseChangeEvent();   // Changed
        RaiseIncreaseEvent(); // Added
    }

    public void ChangeMax(float amount)
    {
        float amountToAdd = amount - maxValue;
        maxValue = amount;
        currentValue = Math.Min(currentValue + amountToAdd, maxValue);
        //RaiseEvent();
        RaiseChangeEvent();   // Changed
        if (amountToAdd > 0) // Added
            RaiseIncreaseEvent(); // Added
        else if (amountToAdd < 0) // Added
            RaiseDecreaseEvent(); // Added
    }

    //private void RaiseEvent()
    private void RaiseChangeEvent()
    {
        if (valueChanged != null) valueChanged.Raise();
    }

    private void RaiseIncreaseEvent() // Added
    {
        if (valueIncreased != null) valueIncreased.Raise();
    }

    private void RaiseDecreaseEvent() // Added
    {
        if (valueDecreased != null) valueDecreased.Raise();
    }
}
