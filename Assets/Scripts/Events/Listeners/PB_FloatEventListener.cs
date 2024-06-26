using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FloatEvent : UnityEvent<float>
{

}

public class PB_FloatEventListener : MonoBehaviour
{
    [SerializeField] 
    private PB_FloatEventChannelSO _channel = default;

    public FloatEvent OnEventRaised;

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

    private void Respond(float value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
