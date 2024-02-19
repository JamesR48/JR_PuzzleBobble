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

    private PB_IShootable currentShootable = null;

    private void Start()
    {
        if (_shootableGO != null && currentShootable == null)
        {
            GameObject newShootableGO = Instantiate(_shootableGO);

            if (newShootableGO != null && newShootableGO.TryGetComponent(out PB_IShootable shootableComp))
            {
                currentShootable = shootableComp;
                currentShootable.gameObject.transform.SetPositionAndRotation(_shootPosition.position, _shootPosition.rotation);
            }
        }

        //if (_shootableGO != null)
        //{
        //    _shootableGO.transform.SetPositionAndRotation(_shootPosition.position, _shootPosition.rotation);
        //}
    }
    public void Shoot()
    {
        if (_shootableGO != null && currentShootable == null)
        {
            GameObject newShootableGO = Instantiate(_shootableGO);

            if (newShootableGO != null && newShootableGO.TryGetComponent(out PB_IShootable shootableComp))
            {
                currentShootable = shootableComp;
                currentShootable.gameObject.transform.SetPositionAndRotation(_shootPosition.position, _shootPosition.rotation);
            }
        }

        //if (_shootableGO != null && _shootableGO.TryGetComponent(out PB_IShootable shootableComp))
        //{
        //    _shootableGO.transform.SetPositionAndRotation(_shootPosition.position, _shootPosition.rotation);
        //    shootableComp.ShootResponse();
        //}

        if (currentShootable != null)
        {
            currentShootable.ShootResponse();
            currentShootable = null;
        }

    }

    public void SetShootableGO(GameObject shootable)
    {
        _shootableGO = shootable;

        if (_shootableGO != null)
        {
            _shootableGO.transform.SetPositionAndRotation(_shootPosition.position, _shootPosition.rotation);
        }
    }

    private void FixedUpdate()
    {
        if (currentShootable != null)
        {
            currentShootable.gameObject.transform.SetPositionAndRotation(_shootPosition.position, _shootPosition.rotation);
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
