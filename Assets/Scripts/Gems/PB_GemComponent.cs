using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.RuleTile.TilingRuleOutput;

public enum PB_EGemType
{
    NONE,
    DIRT,
    SILVER,
    GOLD,
    RUBY,
    AZURITE,
    EMERALD
}

public class PB_GemComponent : MonoBehaviour, PB_IShootable
{
    [SerializeField]
    private PB_GameObjectEventChannelSO _onSpawnEventChannel = default;

    [SerializeField]
    private PB_EGemType _gemType = PB_EGemType.NONE;

    private PB_GemManager _gemManager;
    public PB_GemManager gemManager { set { _gemManager = value; } }

    private PB_MoveComponent _moveComponent;

    private void OnEnable()
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
        if (_moveComponent != null)
        {
            _moveComponent.enabled = true;
            _moveComponent.OnStartMoving(transform.up);
        }
    }

    public PB_IShootable InstantiateShootable()
    {
        PB_IShootable spawnedGem = Instantiate(this);
        _onSpawnEventChannel.RaiseEvent(spawnedGem.gameObject);
        return spawnedGem;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_moveComponent != null && _moveComponent.isActiveAndEnabled) 
        {
            _moveComponent.OnStartMoving(Vector3.zero);
            _moveComponent.enabled = false;

            if (_gemManager != null)
            {
                Vector2Int nearesTile = _gemManager.NearestTile(transform.position.x, transform.position.y);
                transform.position = _gemManager.TileToWorld(nearesTile.x, nearesTile.y);
            }
        }
    }
}
