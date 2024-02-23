using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;

public class PB_GemManager : MonoBehaviour
{
    [SerializeField]
    public Tilemap _backgroundTilemap;

    [SerializeField]
    private PB_GemPool _gemPool = null;
    [SerializeField]
    private PB_MapConfigSO _currentMapGems = null;
    [SerializeField]
    private PB_CharacterComponent _characterComponent = null;

    [SerializeField]
    private PB_GemComponent _gemPrefab;
    [SerializeField]
    private int _maxGemsInGame = 33;

    //private PB_GemComponent[,] _gemsArray;
    private List<PB_GemComponent> _gemsArray;
    public List<PB_GemComponent> gemsArray { get { return _gemsArray; } set { _gemsArray = value; } }

    private List<PB_GemComponent> _gemsToDestroy;
    public List<PB_GemComponent> gemsToDestroy { get { return _gemsToDestroy; } set { _gemsToDestroy = value; } }

    private int _currentGemsInGame = 0;

    int Right;
    int Left;
    public int Up;
    int Down;


    private void OnEnable()
    {
        InitMapGems();
       //UpdateShootableGems(true);
    }

    private void InitMapGems()
    {
        if (_gemPool != null && _currentMapGems != null)
        {
            int mapRows = _currentMapGems._mapRows;
            int mapCols = _currentMapGems._mapColumns;

            Right = (_backgroundTilemap.origin.x) + mapCols-1;
            Left = _backgroundTilemap.origin.x;
            Up = _backgroundTilemap.origin.y + mapRows-1;
            Down = _backgroundTilemap.origin.y;

            Vector3 offset = new Vector3(mapRows*0.5f+1.5f, -mapCols*0.5f+1.5f, 0);
            //_gemsArray = new PB_GemComponent[mapRows, mapCols];
            _gemsToDestroy = new List<PB_GemComponent>();
            _gemsArray = new List<PB_GemComponent>();
            PB_EGemType[,] mapGemTypes = _currentMapGems.GetMapGemTypes();

            Vector3 offsetToWorld = new Vector3(0.5f, 0.5f, 0.0f);

            for (int row = 0; row < mapRows; row++)
            {
                for (int col = 0; col < mapCols; col++)
                {
                    PB_EGemType currentType = mapGemTypes[row, col];
                    if(currentType != PB_EGemType.NONE)
                    {
                        //PB_GemComponent newGem = _gemPool.GetGem();
                        PB_GemComponent newGem = Instantiate(_gemPrefab);
                        if (newGem != null)
                        {
                            newGem.gemManager = this;
                            newGem.SetGemType(currentType);

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
                        //_gemsArray[row, col] = newGem;
                        //_gemsArray.Add(newGem);
                    }
                    else 
                    {
                        //_gemsArray[row, col] = null;
                    }
                }
            }
        }
    }

    public void UpdateShootableGems(bool bInitialSetup = false)
    {
        //if (_gemPool != null && _characterComponent != null)
        //{
        //    if(bInitialSetup)
        //    {
        //        PB_GemComponent[] gems = new PB_GemComponent[2];
        //        gems[0] = _gemPool.GetGem();
        //        gems[1] = _gemPool.GetGem();

        //        if (gems[0] != null && gems[1] != null)
        //        {
        //            _characterComponent.InitShootableGems(gems);
        //        }
        //    }
        //    else
        //    {
        //        PB_GemComponent newGem = _gemPool.GetGem();
        //        if (newGem != null)
        //        {
        //            _characterComponent.UpdateShootableGems(newGem);
        //        }
        //    }
        //}
    }

    public void UpdateGems(GameObject spawnedGO)
    {
        if (spawnedGO != null)
        {
            if (spawnedGO.TryGetComponent(out PB_GemComponent gemComp))
            {
                if (!_gemsArray.Contains(gemComp))
                {
                    gemComp.gemManager = this;
                    //gemComp.SetGemType(PB_EGemType.RUBY);
                    List<PB_EGemType> currentGemTypes = new List<PB_EGemType>();
                    Int32 foundTypesCount = 0;
                    foreach (PB_GemComponent gem in _gemsArray)
                    {
                        if(!currentGemTypes.Contains(gem.GetGemType()))
                        {
                            currentGemTypes.Add(gem.GetGemType());
                            foundTypesCount++;
                            if(foundTypesCount >= 6)
                            {
                                break;
                            }
                        }
                    }

                    if(foundTypesCount > 0)
                    {
                        Int32 randomTypeIndex = UnityEngine.Random.Range(0, foundTypesCount - 1);
                        gemComp.SetGemType(currentGemTypes[randomTypeIndex]);
                        Debug.Log("COLOR: " + gemComp.GetGemType().ToString());
                    }
                    //_gemsArray.Add(gemComp);
                    _currentGemsInGame++;
                }
            }
        }
    }

    public Vector3 TileToWorld(int InX, int InY)
    {
        Vector3Int tilePos = new Vector3Int(InX, InY, 0);
        Vector3 worldPos = _backgroundTilemap.GetCellCenterWorld(tilePos);
        float oddOffset = _backgroundTilemap.cellSize.x * 0.5f;
        worldPos.x += ((InY % 2) * oddOffset);
        return worldPos;
    }

    public Vector2Int NearestTile(float InX, float InY)
    {
        Vector3 worldPos = new Vector3(InX, InY, 0.0f);
        worldPos.x -= ((InY % 2) * 0.5f);
        Vector3Int cellPos = _backgroundTilemap.WorldToCell(worldPos);
        Vector2Int tilePos = new Vector2Int(cellPos.x, cellPos.y);
        return tilePos;
    }
}
