using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Puzzle Bobble/Events/GameObject Event Channel")]
public class PB_GameObjectEventChannelSO : ScriptableObject
{
    public UnityAction<GameObject> OnEventRaised;
    public void RaiseEvent(GameObject value)
    {
        OnEventRaised.Invoke(value);
    }
}
