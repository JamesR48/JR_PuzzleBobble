using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewIntVariable", menuName = "Puzzle Bobble/Variables/IntVariable")]
public class PB_IntVariable_SO : ScriptableObject
{
    public int Value;
    public event UnityAction valueChangeEvent = delegate { };

    public void SetValue(int value)
    {
        Value = value;
        valueChangeEvent.Invoke();
    }

    public void SetValue(PB_IntVariable_SO value)
    {
        Value = value.Value;
        valueChangeEvent.Invoke();
    }

    public void ApplyChange(int amount)
    {
        Value += amount;
        valueChangeEvent.Invoke();
    }

    public void ApplyChange(PB_IntVariable_SO amount)
    {
        Value += amount.Value;
        valueChangeEvent.Invoke();
    }
}
