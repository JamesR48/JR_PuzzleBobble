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

    private int _currentGemsInGame = 0;

    int Right;
    int Left;
    int Up;
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
                            newGem.SetGemType(currentType);

                            // assign the new position of the ball
                            Vector3 newPos = Vector3Int.zero;
                            float xOffset = row % 2 != 0 ? 8.0f : 7.5f;
                            //newPos = new Vector3(col - xOffset, 6.0f - row, 0); // Map is 12x14. 7 free up. aprox 10 free laterally
                            //float yp = (-row + Up + 0.5f) / 1.0f;
                            //float xp = ((col + Left + 0.5f - ((yp % 2) * 0.5f)) / 1.0f) + 0.25f;
                            float yp = (-row + Up);
                            float xp = ((col + Left + ((yp % 2) * 0.5f)));
                            newPos = new Vector3(xp, yp); //new Vector3((col*1.0f)+Left+((row%2)*0.5f), row*1.0f+Up, 0); // Map is 12x14. 7 free up. aprox 10 free laterally

                            //newPos = NearestTile(Left + col, Up - row);
                            newPos = TileToWorld(col, row);
                            //newPos.x += 0.5f;
                            if (_backgroundTilemap != null)
                            {
                                //Vector3Int gridPos = _backgroundTilemap.WorldToCell(newPos);
                                Vector3 gridPos = _backgroundTilemap.origin;
                                //if(_backgroundTilemap.HasTile(gridPos))
                                //{
                                //newGem.transform.position = _backgroundTilemap.CellToWorld(offset - newPos);
                                newGem.transform.position = (newPos);// - new Vector3(0.5f, 0.5f, 0.0f);
                                //}
                            }

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

                //_gemsArray[0].transform.position = new Vector3(Right, Up, 0.0f) + offsetToWorld;
                //_gemsArray[1].transform.position = new Vector3(Right, Down, 0.0f) + offsetToWorld;
                //_gemsArray[2].transform.position = new Vector3(Left, Up, 0.0f) + offsetToWorld;
                //_gemsArray[3].transform.position = new Vector3(Left, Down, 0.0f) + offsetToWorld;

                //_gemsArray[0].transform.position = TileToWorld((int)-4.49063206f, (int)3.59466577f); //NearestTile(-4.49063206f, 3.59466577f);
                //Vector2Int nt = NearestTile(-4.49063206f, 3.59466577f);
                //Vector3 tw = TileToWorld(nt.x, nt.y);
                //_gemsArray[0].transform.position = tw;
                //Debug.Log("NT: " + nt + " TW: " + tw);
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
                    _gemsArray.Add(gemComp);
                    _currentGemsInGame++;
                }
            }
        }
    }

    public Vector3 TileToWorld(int InX, int InY)
    {
        float yp = (Up - InY * 1.0f) + 0.5f;
        float xp = (Left + InX * 1.0f) + 0.5f; //+ ((InY%2) * (1*0.5f));
        //xp -= (InY % 2) * 0.5f;

        Vector3Int tilePos = new Vector3Int(Left + InX, Up - InY, 0);
        Vector3 worldPos = _backgroundTilemap.GetCellCenterWorld(tilePos);
        //Debug.Log("pos in tilemap " + worldPos + " pos custom " + new Vector3(xp, yp));

        return new Vector3(xp, yp);

        //Vector3Int tilePos = new Vector3Int(InX, InY, 0);
        //Vector3 worldPos = _backgroundTilemap.GetCellCenterWorld(tilePos);
        //float oddOffset = _backgroundTilemap.cellSize.x * 0.5f;
        //worldPos.x += ((InY % 2) * oddOffset);
        //return worldPos;
    }

    public Vector2Int NearestTile(float InX, float InY)
    {
        int yp = (int)(InY - Up + (0.5f)) / 1;
        int xp = (int)((InX - Left + (0.5f)) / 1);
        //xp -= (int)((yp%2) * 0.5f);

        Vector3 worldPos = new Vector3(InX, InY, 0.0f);
        Vector2Int tilePos = new Vector2Int(_backgroundTilemap.WorldToCell(worldPos).x, _backgroundTilemap.WorldToCell(worldPos).y);
        Debug.Log("pos in tilemap " + tilePos + " pos custom " + new Vector2Int(xp, yp));

        return new Vector2Int(xp, yp);
        
        //Vector3 worldPos = new Vector3(InX, InY, 0.0f);
        //Vector2Int tilePos = new Vector2Int(_backgroundTilemap.WorldToCell(worldPos).x, _backgroundTilemap.WorldToCell(worldPos).y);
        //worldPos.x -= ((tilePos.y % 2) * 0.5f);
        //tilePos = new Vector2Int(_backgroundTilemap.WorldToCell(worldPos).x, _backgroundTilemap.WorldToCell(worldPos).y);
        //return tilePos;
    }
}
