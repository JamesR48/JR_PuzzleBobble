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
        _onSpawnEventChannel.RaiseEvent(gameObject);

        if (_moveComponent != null)
        {
            _moveComponent.enabled = true;
            _moveComponent.OnStartMoving(transform.up);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_moveComponent != null && _moveComponent.isActiveAndEnabled) 
        {
            _moveComponent.OnStartMoving(Vector3.zero);
            _moveComponent.enabled = false;

            PB_GemManager gemM = FindObjectOfType<PB_GemManager>();
            if (gemM)
            {
                //Debug.Log("original: " + transform.position);
                //Debug.Log("TW from Original: " + gemM.TileToWorld(transform.position.x, transform.position.y));
                Vector2Int nt = gemM.NearestTile(transform.position.x, transform.position.y);
                //Debug.Log("NT: " + nt);
                transform.position = gemM.TileToWorld(nt.x, nt.y);
                //transform.position += new Vector3(0.5f, 0.5f, 0.0f);
                //Vector3Int cellPosition = gemM._backgroundTilemap.WorldToCell(transform.position);
                //transform.position = gemM._backgroundTilemap.GetCellCenterWorld(cellPosition);
                //Debug.Log("TW from NT: " + gemM.TileToWorld(nt.x, nt.y));
            }
        }
    }
}
