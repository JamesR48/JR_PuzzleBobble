using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.RuleTile.TilingRuleOutput;

public enum PB_EGemColor
{
    NONE,
    DIRT,
    WHITE,
    YELLOW,
    RED,
    BLUE,
    GREEN
}

public enum PB_EGemType
{
    NONE,
    EARTH,
    ROCK,
    METAL,
    GEM
}

public class PB_GemComponent : MonoBehaviour
{
    [SerializeField]
    private PB_GameObjectEventChannelSO _onSpawnEventChannel = default;

    [SerializeField]
    private PB_EGemType _gemType = PB_EGemType.NONE;

    [SerializeField]
    private PB_EGemColor _gemColor = PB_EGemColor.NONE;

    [SerializeField]
    private SpriteRenderer _rendererComponent;

    private PB_GemManager _gemManager;
    public PB_GemManager gemManager { set { _gemManager = value; } }

    private Vector2Int _gemTilePosition = Vector2Int.zero;
    public Vector2Int gemTilePosition { get { return _gemTilePosition; } set { _gemTilePosition = value; } }

    private List<PB_GemComponent> _gemNeighbours;
    public List<PB_GemComponent> gemNeighbours { get { return _gemNeighbours; } }

    private bool _bMarkedToDestroy = false;
    public bool bMarkedToDestroy { get { return _bMarkedToDestroy; } set { _bMarkedToDestroy = value; } }

    private PB_MoveComponent _moveComponent;
    private CircleCollider2D _colliderComponent;

    private void OnEnable()
    {
        _moveComponent = GetComponent<PB_MoveComponent>();
        _colliderComponent = GetComponent<CircleCollider2D>();
    }

    public void SetGemType(PB_EGemType type)
    {
        _gemType = type;
    }

    public PB_EGemType GetGemType()
    {
        return _gemType;
    }

    public void SetGemColor(PB_EGemColor color)
    {
        _gemColor = color;
    }

    public PB_EGemColor GetGemColor()
    {
        return _gemColor;
    }

    public void SetGemSprite(Sprite InSprite)
    {
        if(_rendererComponent != null && InSprite != null)
        {
            _rendererComponent.sprite = InSprite;
        }
    }

    public void ShootResponse()
    {
        if (_colliderComponent != null)
        {
            _colliderComponent.enabled = true;
        }

        if (_moveComponent != null)
        {
            _moveComponent.enabled = true;
            _moveComponent.OnStartMoving(transform.up);
            _gemNeighbours = new List<PB_GemComponent>();
        }
    }

    public void DisableCollision()
    {
        if(_colliderComponent != null)
        {  
            _colliderComponent.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_moveComponent != null && _moveComponent.isActiveAndEnabled) 
        {
            bool bIsStickedToWall = false;

            PB_BoundComponent boundComp = null;
            if (collision.gameObject.TryGetComponent<PB_BoundComponent>(out boundComp))
            {
                if(boundComp != null)
                {
                    if(boundComp.GetBoundType() == PB_EBoundType.REPEL)
                    {
                        Vector3 newDirection = _moveComponent.GetVelocity().normalized;
                        newDirection.x *= -1.0f;
                        _moveComponent.OnStartMoving(newDirection);
                        return;
                    }
                    else 
                    {
                        bIsStickedToWall = true;
                    }
                }
            }

            _moveComponent.OnStartMoving(Vector3.zero);
            _moveComponent.enabled = false;

            if (_gemManager != null)
            {
                Vector2Int nearesTile = _gemManager.NearestTile(transform.position.x, transform.position.y);
                _gemTilePosition = nearesTile;

                _gemManager.UpdateShootableGems();

                if(bIsStickedToWall)
                {
                    transform.position = _gemManager.TileToWorld(nearesTile.x, nearesTile.y);
                    _gemManager.gemsArray.Add(this);
                    return;
                }

                List<PB_GemComponent> groupOfEquals = GetEqualNeighbours();
                if(groupOfEquals.Count < 3)
                {
                    transform.position = _gemManager.TileToWorld(nearesTile.x, nearesTile.y);
                    _gemManager.gemsArray.Add(this);
                    
                    //_gemManager.UpdateUpperLimit();
                }
                else
                {
                    _gemManager.gemsToDestroy = groupOfEquals;
                    _gemManager.gemsArray = new List<PB_GemComponent>(_gemManager.gemsArray.Except(groupOfEquals));

                    List<PB_GemComponent> floatingGems = new List<PB_GemComponent>();
                    List<PB_GemComponent> connectedGems = new List<PB_GemComponent>();

                    foreach(PB_GemComponent gem in _gemManager.gemsArray)
                    {
                        if (gem != null)
                        {
                            gem.bMarkedToDestroy = false;
                        }
                    }

                    bool bInCeiling = false;
                    foreach (PB_GemComponent gem in _gemManager.gemsArray)
                    {
                        bInCeiling = false;
                        if(gem != null &&  gem.gemTilePosition.y != _gemManager.GetUpperLimit())
                        {
                            connectedGems = gem.GetConnectedGems();
                            foreach (PB_GemComponent connected in connectedGems)
                            {
                                if(connected.gemTilePosition.y == _gemManager.GetUpperLimit())
                                {
                                    bInCeiling = true;
                                    break;
                                }
                            }

                            if(!bInCeiling)
                            {
                                gem.bMarkedToDestroy = true;
                                floatingGems.Add(gem);
                            }
                        }
                    }

                    _gemManager.gemsArray = new List<PB_GemComponent>(_gemManager.gemsArray.Except(floatingGems));
                    foreach (PB_GemComponent floating in floatingGems)
                    {
                        _gemManager.gemsToDestroy.Add(floating);
                    }

                    foreach (PB_GemComponent destroyG in _gemManager.gemsToDestroy)
                    {
                        if(destroyG != null)
                        {
                            Destroy(destroyG.gameObject);
                        }
                    }                    
                }
            }
        }
    }

    private void FindNeighbours()
    {
        if(_gemManager != null && _gemManager.gemsArray != null && _gemManager.gemsArray.Count > 0)
        {
            if(_gemNeighbours != null)
            {
                _gemNeighbours.Clear();
            }
            else 
            {
                _gemNeighbours = new List<PB_GemComponent>();
            }
            
            foreach (PB_GemComponent gem in _gemManager.gemsArray)
            {
                if(gem != null && gem != this && IsNextTo(gem))
                {
                    _gemNeighbours.Add(gem);
                }
            }
        }
    }

    private bool IsNextTo(PB_GemComponent gem)
    {
        Vector2Int topLeft = new Vector2Int( _gemTilePosition.x - ((_gemTilePosition.y + 1 + _gemManager.GetCeilingLevel()) % 2) , _gemTilePosition.y + 1);
        if(gem.gemTilePosition == topLeft)
        {
            return true;
        }

        Vector2Int topRight = new Vector2Int(_gemTilePosition.x - ((_gemTilePosition.y + 1 + _gemManager.GetCeilingLevel()) % 2) + 1, _gemTilePosition.y + 1);
        if (gem.gemTilePosition == topRight)
        {
            return true;
        }

        Vector2Int Left = new Vector2Int(_gemTilePosition.x - 1, _gemTilePosition.y);
        if (gem.gemTilePosition == Left)
        {
            return true;
        }

        Vector2Int Right = new Vector2Int(_gemTilePosition.x + 1, _gemTilePosition.y);
        if (gem.gemTilePosition == Right)
        {
            return true;
        }

        Vector2Int botLeft = new Vector2Int(_gemTilePosition.x - ((_gemTilePosition.y + 1 + _gemManager.GetCeilingLevel()) % 2), _gemTilePosition.y - 1);
        if (gem.gemTilePosition == botLeft)
        {
            return true;
        }

        Vector2Int botRight = new Vector2Int(_gemTilePosition.x - ((_gemTilePosition.y + 1 + _gemManager.GetCeilingLevel()) % 2) + 1, _gemTilePosition.y - 1);
        if (gem.gemTilePosition == botRight)
        {
            return true;
        }

        return false;
    }

    public List<PB_GemComponent> GetEqualNeighbours()
    {
        List<PB_GemComponent> equalGemNeighbours = new List<PB_GemComponent>();
        equalGemNeighbours.Add(this);
        FindEqualNeighbours(equalGemNeighbours, this);

        return equalGemNeighbours;
    }

    private void FindEqualNeighbours(List<PB_GemComponent> equalGems, PB_GemComponent gem)
    {
        if(gem != null)
        {
            gem.FindNeighbours();

            if (gem.gemNeighbours != null && gem.gemNeighbours.Count > 0)
            {
                foreach (PB_GemComponent neighbour in gem.gemNeighbours)
                {
                    if (GetGemType() == neighbour.GetGemType() && GetGemColor() == neighbour.GetGemColor() && !equalGems.Contains(neighbour))
                    {
                        equalGems.Add(neighbour);
                        FindEqualNeighbours(equalGems, neighbour);
                    }
                }
            }
        }
    }

    public List<PB_GemComponent> GetConnectedGems()
    {
        List<PB_GemComponent> connectedGems = new List<PB_GemComponent>();
        connectedGems.Add(this);
        FindConnectedGems(connectedGems, this);

        return connectedGems;
    }

    private void FindConnectedGems(List<PB_GemComponent> connectedGems, PB_GemComponent gem)
    {
        if (gem != null)
        {
            gem.FindNeighbours();

            if (gem.gemNeighbours != null && gem.gemNeighbours.Count > 0)
            {
                foreach (PB_GemComponent neighbour in gem.gemNeighbours)
                {
                    if (!neighbour.bMarkedToDestroy && !connectedGems.Contains(neighbour))
                    {
                        connectedGems.Add(neighbour);
                        if (neighbour.gemTilePosition.y == _gemManager.GetUpperLimit())
                        {
                            break;
                        }
                        FindConnectedGems(connectedGems, neighbour);
                    }
                }
            }
        }
    }
}
