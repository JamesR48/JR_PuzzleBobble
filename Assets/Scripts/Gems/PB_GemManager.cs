using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PB_GemManager : MonoBehaviour
{
    [SerializeField]
    public Tilemap _backgroundTilemap;

    [SerializeField]
    private PB_MapConfigSO _currentMapGems = null;

    [SerializeField]
    private PB_CharacterComponent _characterComponent = null;

    [SerializeField]
    private PB_GemComponent _gemPrefab;
    [SerializeField]
    private int _maxGemsInGame = 33;

    private List<PB_GemComponent> _gemsArray;
    public List<PB_GemComponent> gemsArray { get { return _gemsArray; } set { _gemsArray = value; } }

    private List<PB_GemComponent> _gemsToDestroy;
    public List<PB_GemComponent> gemsToDestroy { get { return _gemsToDestroy; } set { _gemsToDestroy = value; } }

    private int _currentGemsInGame = 0;

    int Right;
    int Left;
    int Up;
    int Down;
    int ceilingLevel = 0;


    private void OnEnable()
    {
        InitMapGems();

        if(_characterComponent)
        {
            _characterComponent.SetGemManager(this);
            _characterComponent.UpdateGemsToShoot();
        }
    }

    private void InitMapGems()
    {
        if (_currentMapGems != null)
        {
            int mapRows = _currentMapGems._mapRows;
            int mapCols = _currentMapGems._mapColumns;

            Right = (_backgroundTilemap.origin.x) + mapCols-1;
            Left = _backgroundTilemap.origin.x;
            Up = _backgroundTilemap.origin.y + mapRows-1;
            Down = _backgroundTilemap.origin.y;

            _gemsToDestroy = new List<PB_GemComponent>();
            _gemsArray = new List<PB_GemComponent>();
            PB_EGemType[,] mapGemTypes;
            PB_EGemColor[,] mapGemColors;
            _currentMapGems.GetMapGemData(out mapGemTypes, out mapGemColors);

            if(mapGemTypes == null || mapGemColors == null)
            {
                return;
            }

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
                            newGem.gemManager = this;
                            newGem.SetGemType(currentType);
                            newGem.SetGemColor(currentColor);

                            // assign the new position of the ball
                            Vector3 newPos = new Vector3(Left + col, Up - row, 0.0f);
                            if (_backgroundTilemap != null)
                            {
                                Vector2Int toTile = NearestTile(newPos.x, newPos.y);
                                newPos = TileToWorld(toTile.x, toTile.y);
                                newGem.gemTilePosition = toTile;
                            }
                            newGem.transform.position = (newPos);

                            _currentGemsInGame++;
                            _gemsArray.Add(newGem);
                        }
                    }
                }
            }
        }
    }

    public void UpdateShootableGems()
    {
        if (_characterComponent)
        {
            _characterComponent.UpdateGemsToShoot();
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
                _newGem.gemManager = this;
                //List<PB_EGemType> currentGemTypes = new List<PB_EGemType>();
                //List<PB_EGemColor> currentGemColors = new List<PB_EGemColor>();
                //Int32 foundTypesCount = 0;
                //Int32 foundColorsCount = 0;
                //foreach (PB_GemComponent gem in _gemsArray)
                //{
                //    if (!currentGemTypes.Contains(gem.GetGemType()))
                //    {
                //        currentGemTypes.Add(gem.GetGemType());
                //        foundTypesCount++;
                //        if (foundTypesCount >= 4)
                //        {
                //            break;
                //        }
                //    }

                //    if (!currentGemColors.Contains(gem.GetGemColor()))
                //    {
                //        currentGemColors.Add(gem.GetGemColor());
                //        foundColorsCount++;
                //    }

                //    if (foundTypesCount >= 4 && foundColorsCount >= 6)
                //    {
                //        break;
                //    }
                //}

                //if (foundTypesCount > 0)
                //{
                //    Int32 randomTypeIndex = UnityEngine.Random.Range(0, foundTypesCount - 1);
                //    _newGem.SetGemType(currentGemTypes[randomTypeIndex]);
                //}

                //if (foundColorsCount > 0)
                //{
                //    Int32 randomColorIndex = UnityEngine.Random.Range(0, foundColorsCount - 1);
                //    _newGem.SetGemColor(currentGemColors[randomColorIndex]);
                //    //Debug.Log("COLOR: " + _newGem.GetGemColor().ToString());
                //}

                Int32 currentGemsCount = _gemsArray.Count - 1;
                Int32 randomIndex = UnityEngine.Random.Range(0, currentGemsCount);
                //_newGem.SetGemType(_gemsArray[randomIndex].GetGemType());
                //_newGem.SetGemColor(_gemsArray[randomIndex].GetGemColor());
                _newGem.SetGemType(PB_EGemType.ROCK);
                _newGem.SetGemColor(PB_EGemColor.RED);

                Debug.Log("TYPE: " + _newGem.GetGemType().ToString() + " COLOR: " + _newGem.GetGemColor().ToString());

                _currentGemsInGame++;
            }
        }

        return _newGem;
    }

    public Vector3 TileToWorld(int InX, int InY)
    {
        Vector3Int tilePos = new Vector3Int(InX, InY, 0);
        Vector3 worldPos = _backgroundTilemap.GetCellCenterWorld(tilePos);
        //Debug.Log("Tile to World: " + worldPos);
        float oddOffset = _backgroundTilemap.cellSize.x * 0.5f;
        worldPos.x += (((InY - ceilingLevel) % 2) * oddOffset);
        //Debug.Log("Tile to World POST offset: " + worldPos);
        return worldPos;
    }

    public Vector2Int NearestTile(float InX, float InY)
    {
        Vector3 worldPos = new Vector3(InX, InY, 0.0f);
        //Debug.Log("initial: "+ worldPos +" Nearest Tile: " + _backgroundTilemap.WorldToCell(worldPos));
        //worldPos.x -= ((InY % 2) * 0.5f);
        Vector3Int cellPos = _backgroundTilemap.WorldToCell(worldPos);
        worldPos.x -= (((cellPos.y - ceilingLevel) % 2) * 0.5f);
        cellPos = _backgroundTilemap.WorldToCell(worldPos);
        //Debug.Log("Nearest Tile POST offset: " + cellPos);
        Vector2Int tilePos = new Vector2Int(cellPos.x, cellPos.y);
        return tilePos;
    }

    public int GetUpperLimit()
    {
        return Up - ceilingLevel;
    }

    public void UpdateUpperLimit()
    {
        ceilingLevel++;
        foreach(PB_GemComponent gem in _gemsArray)
        {
            //gem.gemTilePosition = new Vector2Int(gem.gemTilePosition.x, gem.gemTilePosition.y-1);
            //gem.transform.position = TileToWorld(gem.gemTilePosition.x, gem.gemTilePosition.y-1);
            Vector3 newWorldPos = gem.transform.position;
            newWorldPos.y -= 1;
            gem.gemTilePosition = NearestTile(newWorldPos.x, newWorldPos.y);
            gem.transform.position = TileToWorld(gem.gemTilePosition.x, gem.gemTilePosition.y);
        }
    }

    public int GetCeilingLevel()
    { return -ceilingLevel; }
}
