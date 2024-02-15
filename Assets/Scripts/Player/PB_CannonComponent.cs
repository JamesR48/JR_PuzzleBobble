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
    private float _fireRate = 0.1f;

    private float currentFireRateTimer = 0.0f;
    private void Awake()
    {
        if (_shootableGO != null)
        {
            _shootableGO.transform.SetPositionAndRotation(_shootPosition.position, _shootPosition.rotation);
        }
    }
    public void OnShoot()
    {
        if (_shootableGO != null && _shootableGO.TryGetComponent(out PB_IShootable shootableComp))
        {
            _shootableGO.transform.SetPositionAndRotation(_shootPosition.position, _shootPosition.rotation);
            shootableComp.ShootResponse();
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
}
