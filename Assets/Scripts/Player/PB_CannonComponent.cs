using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_CannonComponent : MonoBehaviour
{
    [SerializeField]
    private PB_GemComponent _gemGO = null;
    [SerializeField]
    private Transform _shootPosition;
    [SerializeField]
    private PB_RotationComponent _RotationComp = null;
    [SerializeField]
    private float _fireRate = 0.1f;

    private float currentFireRateTimer = 0.0f;

    private PB_GemComponent _gemToShoot = null;

    private void Start()
    {
    }

    public void Shoot()
    {        
        if (_gemToShoot != null)
        {
            _gemToShoot.gameObject.transform.SetParent(null);
            _gemToShoot.ShootResponse();
            _gemToShoot = null;
        }
    }

    public void SetGemToShoot(PB_GemComponent newGem)
    {
        _gemToShoot = newGem;

        if (_gemToShoot != null)
        {
            _gemToShoot.DisableCollision();
            _gemToShoot.transform.SetPositionAndRotation(_shootPosition.position, _shootPosition.rotation);
            _gemToShoot.transform.SetParent(transform);
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
