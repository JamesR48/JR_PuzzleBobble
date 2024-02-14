using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PB_EGemType
{
    None,
    Dirt,
    Silver,
    Gold,
    Fire,
    Water,
    Emerald
}

public class PB_GemComponent : MonoBehaviour, PB_IShootable
{
    [SerializeField]
    private PB_EGemType _gemType = PB_EGemType.None;

    private PB_MoveComponent _moveComponent;

    private void Awake()
    {
        _moveComponent = GetComponent<PB_MoveComponent>();
    }

    public void SetGemType(PB_EGemType type)
    {
        _gemType = type;
    }

    public PB_EGemType GetGemType()
    {
        return _gemType;
    }

    public void ShootResponse()
    {
        if(_moveComponent != null)
        {
            _moveComponent.OnStartMoving(transform.up);
        }
    }
}
