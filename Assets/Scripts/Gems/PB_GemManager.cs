using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using UnityEngine.U2D.Animation;

public class PB_GemManager : MonoBehaviour
{
    [SerializeField]
    private PB_GemComponent _gemPrefab;
    [SerializeField]
    private SpriteAtlas _gemsSpriteAtlas;
    [SerializeField]
    private List<SpriteLibraryAsset> _gemAnimations;
    [SerializeField]
    private int _maxGemsInGame = 33;

    private List<PB_GemComponent> _gemsArray;
    public List<PB_GemComponent> gemsArray { get { return _gemsArray; } set { _gemsArray = value; } }

    private List<PB_GemComponent> _gemsToDestroy;
    public List<PB_GemComponent> gemsToDestroy { get { return _gemsToDestroy; } set { _gemsToDestroy = value; } }

    private PB_CharacterComponent _playerCharacter = null;
    private PB_LevelManager _levelManager = null;

    private int _currentGemsInGame = 0;


    private void OnEnable()
    {
        if(_playerCharacter)
        {
            _playerCharacter.SetGemManager(this);
            _playerCharacter.UpdateGemsToShoot();
        }
    }

    public void InitGemManager(PB_EGemType[,] mapGemTypes, PB_EGemColor[,] mapGemColors, int mapRows, int mapCols, PB_CharacterComponent mapPlayer, PB_LevelManager levelManager)
    {
        _playerCharacter = mapPlayer;
        _levelManager = levelManager;

        if(_levelManager != null)
        {
            InitMapGems(mapGemTypes, mapGemColors, mapRows, mapCols);
        }
    }

    public void InitMapGems(PB_EGemType[,] mapGemTypes, PB_EGemColor[,] mapGemColors, int mapRows, int mapCols)
    {
        _gemsToDestroy = new List<PB_GemComponent>();
        _gemsArray = new List<PB_GemComponent>();

        if (mapGemTypes == null || mapGemColors == null)
        {
            return;
        }

        MapData mapData = _levelManager.GetCurrentMapData();

        for (int row = 0; row < mapRows; row++)
        {
            for (int col = 0; col < mapCols; col++)
            {
                PB_EGemType currentType = mapGemTypes[row, col];
                PB_EGemColor currentColor = mapGemColors[row, col];

                if (currentType != PB_EGemType.NONE && currentColor != PB_EGemColor.NONE)
                {
                    PB_GemComponent newGem = Instantiate(_gemPrefab);
                    if (newGem != null)
                    {
                        SetNewGemData(newGem, currentType, currentColor);

                        // assign the new position of the ball
                        Vector3 newPos = new Vector3(mapData._leftBound + col, mapData._topBound - row, 0.0f);
                        if (_levelManager != null)
                        {
                            Vector2Int toTile = _levelManager.NearestTile(newPos.x, newPos.y);
                            newPos = _levelManager.TileToWorld(toTile.x, toTile.y);
                            newGem.gemTilePosition = toTile;
                        }
                        newGem.transform.position = (newPos);

                        _currentGemsInGame++;
                        _gemsArray.Add(newGem);
                    }
                }
            }
        }

        if (_playerCharacter != null)
        {
            _playerCharacter.SetGemManager(this);
            _playerCharacter.UpdateGemsToShoot();
        }
    }

    public void UpdateShootableGems()
    {
        if (_playerCharacter)
        {
            _playerCharacter.UpdateGemsToShoot();
        }
    }

    public void SetNewGemData(PB_GemComponent InOutGem, PB_EGemType InType, PB_EGemColor InColor)
    {
        if(InOutGem != null)
        {
            InOutGem.gemManager = this;
            InOutGem.SetGemType(InType);
            InOutGem.SetGemColor(InColor);

            if (_gemsSpriteAtlas != null)
            {
                string newSpriteName = string.Format("{0}_{1}", InType.ToString()[0], (int)InColor - 1);
                Sprite newGemSprite = _gemsSpriteAtlas.GetSprite(newSpriteName);
                SpriteLibraryAsset newGemAnimation = null;
                if(_gemAnimations != null && _gemAnimations[(int)InType - 1] != null)
                {
                    newGemAnimation = _gemAnimations[(int)InType - 1];
                }
                InOutGem.SetGemVisuals(newGemSprite, newGemAnimation);
            }
        }
    }

    public PB_GemComponent SpawnNewGem()
    {
        PB_GemComponent _newGem = null;
        if (_gemPrefab != null)
        {
            _newGem = Instantiate(_gemPrefab);
            if (_newGem != null && !_gemsArray.Contains(_newGem))
            {
                Int32 currentGemsCount = _gemsArray.Count - 1;
                Int32 randomIndex = UnityEngine.Random.Range(0, currentGemsCount);
                SetNewGemData(_newGem, _gemsArray[randomIndex].GetGemType(), _gemsArray[randomIndex].GetGemColor());

                Debug.Log("TYPE: " + _newGem.GetGemType().ToString() + " COLOR: " + _newGem.GetGemColor().ToString());

                _currentGemsInGame++;
            }
        }

        return _newGem;
    }

    public Vector3 TileToWorld(int InX, int InY)
    {
        if (_levelManager)
        {
            return _levelManager.TileToWorld(InX, InY);
        }
        return new Vector3(InX, InY, 0.0f);
    }

    public Vector2Int NearestTile(float InX, float InY)
    {
        if (_levelManager)
        {
            return _levelManager.NearestTile(InX, InY);
        }
        return new Vector2Int((int)InX, (int)InY);
    }

    public int GetUpperLimit()
    {
        if(_levelManager)
        {
            return _levelManager.GetUpperLimit();
        }
        return 0;
    }

    public void UpdateUpperLimit()
    {
        if (_gemsArray != null)
        {
            foreach (PB_GemComponent gem in _gemsArray)
            {
                if(gem != null)
                {
                    //gem.gemTilePosition = new Vector2Int(gem.gemTilePosition.x, gem.gemTilePosition.y-1);
                    //gem.transform.position = TileToWorld(gem.gemTilePosition.x, gem.gemTilePosition.y-1);
                    Vector3 newWorldPos = gem.transform.position;
                    newWorldPos.y -= 1;
                    gem.gemTilePosition = _levelManager.NearestTile(newWorldPos.x, newWorldPos.y);
                    gem.transform.position = _levelManager.TileToWorld(gem.gemTilePosition.x, gem.gemTilePosition.y);
                }
            }
        }        
    }

    public int GetCeilingLevel()
    {
        if (_levelManager)
        {
            return _levelManager.GetCeilingLevel();
        }
        return 0;
    }
}
