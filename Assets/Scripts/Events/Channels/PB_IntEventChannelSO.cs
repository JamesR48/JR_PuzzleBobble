using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Puzzle Bobble/Events/Int Event Channel")]
public class PB_IntEventChannelSO : ScriptableObject
{
    public UnityAction<int> OnEventRaised;
    public void RaiseEvent(int value)
    {
        OnEventRaised.Invoke(value);
    }
}
