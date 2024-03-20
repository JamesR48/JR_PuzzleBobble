using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D.Animation;
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
    private PB_EGemType _gemType = PB_EGemType.NONE;

    [SerializeField]
    private PB_EGemColor _gemColor = PB_EGemColor.NONE;

    [SerializeField]
    private SpriteRenderer _rendererComponent = null;

    [SerializeField]
    private SpriteLibrary _spriteLibraryComp = null;

    [SerializeField]
    private Animator _gemAnimator = null;

    [SerializeField]
    private AnimationClip _gemDestroyedAnim = null;

    [SerializeField]
    private PB_VoidEventChannelSO _onGemShot = null;

    [SerializeField]
    private PB_VoidEventChannelSO _onGemCollided = null;

    private PB_GemManager _gemManager;
    public PB_GemManager gemManager { set { _gemManager = value; } }

    [SerializeField]
    private Vector2Int _gemTilePosition = Vector2Int.zero;
    public Vector2Int gemTilePosition { get { return _gemTilePosition; } set { _gemTilePosition = value; } }

    [SerializeField]
    private List<PB_GemComponent> _gemNeighbours;
    public List<PB_GemComponent> gemNeighbours { get { return _gemNeighbours; } }

    private bool _bMarkedToDestroy = false;
    public bool bMarkedToDestroy { get { return _bMarkedToDestroy; } set { _bMarkedToDestroy = value; } }

    private PB_MoveComponent _moveComponent;
    private CircleCollider2D _colliderComponent;

    private float _currentDestroyGemTimer = 0.0f;

    private void OnEnable()
    {
        _moveComponent = GetComponent<PB_MoveComponent>();
        _colliderComponent = GetComponent<CircleCollider2D>();
    }

    private void FixedUpdate()
    {
        if(_currentDestroyGemTimer > 0.0f)
        {
            _currentDestroyGemTimer -= Time.fixedDeltaTime;
            if(_currentDestroyGemTimer <= 0.0f)
            {
                _currentDestroyGemTimer = 0.0f;
                Destroy(gameObject);
            }
        }
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

    public void SetGemVisuals(Sprite InSprite, SpriteLibraryAsset InSpriteLib = null)
    {
        if(_rendererComponent != null && InSprite != null)
        {
            _rendererComponent.sprite = InSprite;
        }

        if(_spriteLibraryComp != null && InSpriteLib != null)
        {
            _spriteLibraryComp.spriteLibraryAsset = InSpriteLib;
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

            if(_onGemShot != null)
            {
                _onGemShot.RaiseEvent();
            }
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

                if (bIsStickedToWall)
                {
                    transform.position = _gemManager.TileToWorld(nearesTile.x, nearesTile.y);
                    _gemManager.gemsArray.Add(this);
                }

                PB_GemComponent collidedGem = null;
                if (bIsStickedToWall || collision.gameObject.TryGetComponent<PB_GemComponent>(out collidedGem))
                {
                    if (bIsStickedToWall || collidedGem != null)
                    {
                        List<PB_GemComponent> groupOfEquals = GetEqualNeighbours(collidedGem);
                        if (!bIsStickedToWall && groupOfEquals.Count < 3)
                        {
                            transform.position = _gemManager.TileToWorld(nearesTile.x, nearesTile.y);
                            _gemManager.gemsArray.Add(this);
                        }
                        else
                        {
                            _gemManager.UpdateGemsToDestroy(groupOfEquals);
                        }

                        if (_onGemCollided != null)
                        {
                            _onGemCollided.RaiseEvent();
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
                if(gem != null && gem != this && !_gemNeighbours.Contains(gem) && IsNextTo(gem))
                {
                    _gemNeighbours.Add(gem);
                }
            }
        }
    }

    private bool IsNextTo(PB_GemComponent gem)
    {
        float oddRowOffset = ((_gemTilePosition.y - _gemManager.GetCeilingLevel()) % 2) * 0.5f;
        Vector3 topLeft = new Vector3(transform.position.x - oddRowOffset - 0.5f , transform.position.y + 1.0f, 0.0f);
        if (gem.gemTilePosition == _gemManager.NearestTile(topLeft.x, topLeft.y))
        {
            return true;
        }

        Vector3 topRight = new Vector3(transform.position.x - oddRowOffset + 0.5f, transform.position.y + 1.0f, 0.0f);
        if (gem.gemTilePosition == _gemManager.NearestTile(topRight.x, topRight.y))
        {
            return true;
        }

        Vector3 Left = new Vector3(transform.position.x - oddRowOffset - 1.0f, transform.position.y, 0.0f);
        if (gem.gemTilePosition == _gemManager.NearestTile(Left.x, Left.y))
        {
            return true;
        }

        Vector3 Right = new Vector3(transform.position.x - oddRowOffset + 1.0f, transform.position.y, 0.0f);
        if (gem.gemTilePosition == _gemManager.NearestTile(Right.x, Right.y))
        {
            return true;
        }

        Vector3 botLeft = new Vector3(transform.position.x - oddRowOffset - 0.5f, transform.position.y - 1.0f, 0.0f);
        if (gem.gemTilePosition == _gemManager.NearestTile(botLeft.x, botLeft.y))
        {
            return true;
        }

        Vector3 botRight = new Vector3(transform.position.x - oddRowOffset + 0.5f, transform.position.y - 1.0f, 0.0f);
        if (gem.gemTilePosition == _gemManager.NearestTile(botRight.x, botRight.y))
        {
            return true;
        }

        return false;
    }

    public List<PB_GemComponent> GetEqualNeighbours(PB_GemComponent collidedGem = null)
    {
        List<PB_GemComponent> equalGemNeighbours = new List<PB_GemComponent>();
        equalGemNeighbours.Add(this);
        FindEqualNeighbours(equalGemNeighbours, this, collidedGem);

        return equalGemNeighbours;
    }

    private void FindEqualNeighbours(List<PB_GemComponent> equalGems, PB_GemComponent gem, PB_GemComponent collidedGem = null)
    {
        if(gem != null)
        {
            gem.FindNeighbours();

            if (collidedGem != null)
            {
                gemNeighbours.Add(collidedGem);
            }

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

    public void OnStartGemDestruction()
    {
        DisableCollision();

        if (_rendererComponent != null && _gemAnimator != null && _gemDestroyedAnim != null)
        {
            _rendererComponent.gameObject.SetActive(false);
            _gemAnimator.Play(_gemDestroyedAnim.name);
            _currentDestroyGemTimer = _gemDestroyedAnim.length;
        }
    }
}
