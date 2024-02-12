using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_MoveComponent : MonoBehaviour
{
    [SerializeField]
    private float _maxSpeed = 5f;

    private Vector2 _position = Vector2.zero;
    private Vector2 _oldPosition = Vector2.zero;
    private Vector2 _acceleration = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
