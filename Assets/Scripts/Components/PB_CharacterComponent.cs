using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_CharacterComponent : MonoBehaviour
{
    [SerializeField]
    private PB_InputReaderSO _inputReader = default;
    [SerializeField]
    private PB_VoidEventChannelSO _shootEventChannel = default;
    [SerializeField]
    private PB_FloatEventChannelSO _turnEventChannel = default;
    [SerializeField]
    private Animator _AnimController = default;
    [SerializeField]
    private Transform _nextGemPosition;

    private void OnEnable()
    {
        if (_inputReader && _shootEventChannel && _turnEventChannel)
        {
            _inputReader.shootEvent += OnShoot;
            _inputReader.turnEvent += OnTurn;
        }
    }

    private void OnDisable()
    {
        if (_inputReader)
        {
            _inputReader.shootEvent -= OnShoot;
            _inputReader.turnEvent -= OnTurn;
        }
    }

    private void OnShoot()
    {
        if (_shootEventChannel)
        {
            _shootEventChannel.RaiseEvent();
        }
    }

    private void OnTurn(float direction)
    {
        if (_turnEventChannel)
        {
            _turnEventChannel.RaiseEvent(direction);
        }

        if(_AnimController)
        {
            _AnimController.SetFloat("TurnDirection", direction);
        }
    }
}
