using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_CharacterComponent : MonoBehaviour
{
    [SerializeField]
    private PB_InputReaderSO _inputReader = default;

    private PB_ShootComponent _shooter;
    private PB_RotationComponent _rotator;

    private void OnEnable()
    {
        _shooter = GetComponent<PB_ShootComponent>();
        _rotator = GetComponent<PB_RotationComponent>();
        if (_inputReader && _shooter && _rotator)
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

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnShoot()
    {
        if (_shooter)
        {
            _shooter.OnShoot();
        }
    }

    private void OnTurn(float direction)
    {
        if (_rotator)
        {
            _rotator.OnRotate(direction);
        }
    }
}
