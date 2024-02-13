using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_ShootComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject _shootableGO = null;
    [SerializeField]
    private Transform _shootPosition;
    [SerializeField]
    private float _fireRate = 0.1f;
    [SerializeField]
    private PB_Vector3EventChannelSO _aimDirectionEventChannel = default;

    private float currentFireRateTimer = 0.0f;

    public void OnShoot()
    {
        if (_aimDirectionEventChannel)
        {
            _aimDirectionEventChannel.RaiseEvent(transform.up);
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
