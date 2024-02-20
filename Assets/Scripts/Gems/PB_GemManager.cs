using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_GemManager : MonoBehaviour
{
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
            //_gemsArray = new PB_GemComponent[mapRows, mapCols];
            _gemsArray = new List<PB_GemComponent>();
            PB_EGemType[,] mapGemTypes = _currentMapGems.GetMapGemTypes();

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
                            Vector3 newPos = Vector3.zero;
                            float xOffset = row % 2 != 0 ? 8.0f : 7.5f;
                            newPos = new Vector3(col - xOffset, 6.0f - row, 0); // Map is 12x14. 7 free up. aprox 10 free laterally

                            newGem.transform.position = newPos;

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
}
