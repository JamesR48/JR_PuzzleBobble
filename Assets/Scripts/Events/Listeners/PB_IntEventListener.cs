using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntEvent : UnityEvent<int>
{

}

public class PB_IntEventListener : MonoBehaviour
{
    [SerializeField] 
    private PB_IntEventChannelSO _channel = default;

    public IntEvent OnEventRaised;

    private void OnEnable()
    {
        if (_channel != null)
            _channel.OnEventRaised += Respond;
    }

    private void OnDisable()
    {
        if (_channel != null)
            _channel.OnEventRaised -= Respond;
    }

    private void Respond(int value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
