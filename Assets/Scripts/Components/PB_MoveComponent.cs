using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class PB_MoveComponent : MonoBehaviour
{
    [SerializeField]
    private float _maxSpeed = 1.0f;

    private Vector3 _velocity = Vector3.zero;

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        UpdateMovement(Time.fixedDeltaTime);
    }

    public void OnStartMoving(Vector3 direction)
    {
        _velocity = direction * _maxSpeed;
    }

    private void UpdateMovement(float DeltaTime)
    {
        transform.position += _velocity * DeltaTime;
    }

    public Vector3 GetVelocity()
    { return _velocity; }
}
