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

    private void Start()
    {
        _oldPosition = transform.position;
    }

    private void FixedUpdate()
    {
        UpdateMovement(Time.fixedDeltaTime);
    }

    public void OnStartMoving(Vector3 direction)
    {
        _velocity = direction * _maxSpeed;
    }

    public void UpdateMovement(float DeltaTime)
    {
        Vector3 tempPosition = transform.position;
        //Debug.Log(_velocity);
        transform.position += _velocity * DeltaTime;
        _oldPosition = tempPosition;
    }
}
