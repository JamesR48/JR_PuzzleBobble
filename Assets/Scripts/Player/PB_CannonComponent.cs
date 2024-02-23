using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_CannonComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject _shootableGO = null;
    [SerializeField]
    private Transform _shootPosition;
    [SerializeField]
    private PB_RotationComponent _RotationComp = null;
    [SerializeField]
    private float _fireRate = 0.1f;

    private float currentFireRateTimer = 0.0f;

    private PB_IShootable _currentShootable = null;
    private PB_IShootable _nextShootable = null;

    private void Start()
    {
        if (_shootableGO != null && _currentShootable == null)
        {
            if (_shootableGO.TryGetComponent(out PB_IShootable shootableComp))
            {
                _currentShootable = shootableComp.InstantiateShootable();
                if(_currentShootable != null)
                {
                    _currentShootable.gameObject.transform.SetPositionAndRotation(_shootPosition.position, _shootPosition.rotation);
                    _currentShootable.gameObject.transform.SetParent(transform);
                }
            }
        }
    }

    public void spawnbullet()
    {
        if (_shootableGO != null && _currentShootable == null)
        {
            if (_shootableGO.TryGetComponent(out PB_IShootable shootableComp))
            {
                _currentShootable = shootableComp.InstantiateShootable();
                if (_currentShootable != null)
                {
                    _currentShootable.gameObject.transform.SetPositionAndRotation(_shootPosition.position, _shootPosition.rotation);
                    _currentShootable.gameObject.transform.SetParent(transform);
                }
            }
        }
    }

    public void Shoot()
    {        
        if (_currentShootable != null)
        {
            _currentShootable.gameObject.transform.SetParent(null);
            _currentShootable.ShootResponse();
            _currentShootable = null;
        }
    }

    public void SetShootableGO(GameObject shootable)
    {
        _shootableGO = shootable;

        if (_shootableGO != null)
        {
            _shootableGO.transform.SetPositionAndRotation(_shootPosition.position, _shootPosition.rotation);
            _shootableGO.transform.SetParent(transform, true);
        }
    }

    public void Rotate(float direction)
    {
        if(_RotationComp)
        {
            _RotationComp.SetRotationDirection(direction);
        }
    }
}
