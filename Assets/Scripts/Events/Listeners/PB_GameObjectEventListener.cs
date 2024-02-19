using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameObjectEvent : UnityEvent<GameObject>
{

}

public class PB_GameObjectEventListener : MonoBehaviour
{
    [SerializeField] 
    private PB_GameObjectEventChannelSO _channel = default;

    public GameObjectEvent OnEventRaised;

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

    private void Respond(GameObject value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
