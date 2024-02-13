using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Puzzle Bobble/Events/Vector3 Event Channel")]
public class PB_Vector3EventChannelSO : ScriptableObject
{
    public UnityAction<Vector3> OnEventRaised;
    public void RaiseEvent(Vector3 value)
    {
        OnEventRaised.Invoke(value);
    }
}
