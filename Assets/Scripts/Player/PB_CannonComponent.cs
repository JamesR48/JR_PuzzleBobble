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
            if (_shootableGO.TryGetComponent(out PB_IShootable shootableComp))
            {
                currentShootable = shootableComp.InstantiateShootable();
                if(currentShootable != null)
                {
                    currentShootable.gameObject.transform.SetPositionAndRotation(_shootPosition.position, _shootPosition.rotation);
                    currentShootable.gameObject.transform.SetParent(transform);
                }
            }
        }
    }
    public void Shoot()
    {
        if (_shootableGO != null && currentShootable == null)
        {
            if (_shootableGO.TryGetComponent(out PB_IShootable shootableComp))
            {
                currentShootable = shootableComp.InstantiateShootable();
                if (currentShootable != null)
                {
                    currentShootable.gameObject.transform.SetPositionAndRotation(_shootPosition.position, _shootPosition.rotation);
                    currentShootable.gameObject.transform.SetParent(transform);
                }
            }
        }

        if (currentShootable != null)
        {
            currentShootable.gameObject.transform.SetParent(null);
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
