using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Vector3Event : UnityEvent<Vector3>
{

}

public class PB_Vector3EventListener : MonoBehaviour
{
    [SerializeField] 
    private PB_Vector3EventChannelSO _channel = default;

    public Vector3Event OnEventRaised;

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

    private void Respond(Vector3 value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
