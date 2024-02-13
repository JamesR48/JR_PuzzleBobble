using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class PB_MoveComponent : MonoBehaviour
{
    [SerializeField]
    private float _maxSpeed = 1.0f;

    private Vector3 _oldPosition = Vector3.zero;
    private Vector3 _velocity = Vector3.zero;

    private void OnEnable()
    {
        _oldPosition = transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 tempPosition = transform.position;
        Debug.Log(_velocity);
        transform.position += _velocity * Time.fixedDeltaTime;
        _oldPosition = tempPosition;
    }

    public void OnStartMoving(Vector3 direction)
    {
        _velocity = direction * _maxSpeed;
    }
}
