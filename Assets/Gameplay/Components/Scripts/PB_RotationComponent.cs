using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_RotationComponent : MonoBehaviour
{
    [SerializeField] 
    private float _angleLimit = 45.0f;

    [SerializeField]
    private float _rotationSpeed = 20.0f;

    private float _rotationDirection = 0;
    private float _rotationAngle = 0;

    private void OnEnable()
    {
        Vector3 _rotationAxis = Vector3.zero;
        transform.rotation.ToAngleAxis(out _rotationAngle, out _rotationAxis);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rotationAngle += _rotationDirection * _rotationSpeed * Time.fixedDeltaTime;
        _rotationAngle = Mathf.Clamp(_rotationAngle, -_angleLimit, _angleLimit);
        transform.rotation = Quaternion.AngleAxis(_rotationAngle, -Vector3.forward);
        Debug.Log("ROTATING: " + transform.rotation.eulerAngles);
    }

    public void OnRotate(float direction)
    {
        //Debug.Log("ROTATING: " + direction);
        _rotationDirection = direction;
    }
}
