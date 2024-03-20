using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PB_CharacterComponent : MonoBehaviour
{
    [SerializeField]
    private SpriteLibrary _characterSpriteLib = default;
    [SerializeField]
    private PB_InputReaderSO _inputReader = default;
    [SerializeField]
    private PB_CannonComponent _cannonGO = default;
    [SerializeField]
    private Animator _AnimController = default;
    [SerializeField]
    private Transform _nextGemPosition;

    private PB_GemManager _gemManager;
    private PB_GemComponent _nextGem = null;
    private PB_GemComponent _currentGem = null;

    private void OnEnable()
    {
        if (_inputReader)
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
        if (_cannonGO)
        {
            _cannonGO.Shoot();
        }
    }

    private void OnTurn(float direction)
    {
        if (_cannonGO)
        {
            _cannonGO.Rotate(direction);
        }

        if(_AnimController)
        {
            _AnimController.SetFloat("TurnDirection", direction);
        }
    }

    public void UpdateGemsToShoot()
    {
        if (_cannonGO != null && _gemManager != null)
        {
            if (_nextGem != null)
            {
                _currentGem = _nextGem;
                _nextGem = null;
            }

            if (_nextGem == null)
            {
                _nextGem = _gemManager.SpawnNewGem();
                if (_nextGem != null)
                {
                    _nextGem.gameObject.transform.SetPositionAndRotation(_nextGemPosition.position, _nextGemPosition.rotation);
                }
            }
            if (_currentGem == null)
            {
                _currentGem = _gemManager.SpawnNewGem();
            }

            if (_currentGem != null)
            {
                _cannonGO.SetGemToShoot(_currentGem);
            }
        }
    }

    public void SetGemManager(PB_GemManager gemManager)
    {
        _gemManager = gemManager;
    }

    public void SetSpriteLib(SpriteLibraryAsset spriteLibAsset)
    {
        if (_characterSpriteLib != null && spriteLibAsset != null)
        {
            _characterSpriteLib.spriteLibraryAsset = spriteLibAsset;
        }
    }

    public void EnablePlayerInput(bool InEnable)
    {
        if(InEnable)
        {
            OnEnable();
        }
        else 
        {
            OnDisable();
        }
    }
}
