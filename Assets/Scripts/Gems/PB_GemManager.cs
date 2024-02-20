using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;

public class PB_GemManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap _backgroundTilemap;

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
                            float yp = (-row + Up + 0.5f) / 1.0f;
                            float xp = ((col + Left + 0.5f - ((yp % 2) * 0.5f)) / 1.0f) + 0.25f;
                            newPos = new Vector3(xp, yp); //new Vector3((col*1.0f)+Left+((row%2)*0.5f), row*1.0f+Up, 0); // Map is 12x14. 7 free up. aprox 10 free laterally

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

                _gemsArray[0].transform.position = NearestTile(-4.49063206f, 3.59466577f);
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

    private void OnDrawGizmos()
    {
        //Gizmos.color = new Color(1, 1, 0, 1.0f);
        //Gizmos.DrawWireSphere(transform.position, GetCircleRadius());
    }

    public Vector3 NearestTile(float InX, float InY)
    {
        //float yp = (-InY + Up + 0.5f) / 1.0f;
        //float xp = ((InX + Left + 0.5f - ((yp % 2) * 0.5f)) / 1.0f) + 0.25f;
        float yp = (InY - Up + 8.0f) / 16.0f;
        float xp = ((InX - Left + 8.0f - ((yp % 2) * 8.0f)) / 16.0f);// + 0.25f;
        return new Vector3(xp, yp);
    }
}
