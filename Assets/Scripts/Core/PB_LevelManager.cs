using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct MapData
{
    public int _rightBound;
    public int _leftBound;
    public int _topBound;
    public int _bottomBound;
    public int _mapCols;
    public int _mapRows;
}

public class PB_LevelManager : MonoBehaviour
{
    [SerializeField]
    private List<PB_MapConfigSO> _levelData = null;
    [SerializeField]
    private PB_GemManager _gemManagerPrefab = null;
    [SerializeField]
    private PB_CharacterComponent _playerCharacterPrefab = null;
    [SerializeField]
    private PB_CharacterDataSO _currentLevelCharacterData = null;
    [SerializeField]
    private PB_GameState _currentLevelGameState = null;

    [SerializeField]
    private PB_BoundComponent _ceilingBound = null;
    [SerializeField]
    private PB_VoidEventChannelSO _onLowerCeilingEvent = null;
    [SerializeField]
    private float _timeToLowerCeiling = 10.0f;

    private int _currentLevelSection = 0;
    private PB_GemManager _currentLevelGemManager = null;
    private PB_CharacterComponent _currentLevelPlayer = null;
    private Tilemap _currentLevelTilemap = null;

    private MapData _currentMapData;

    private float _currentTimeToLowerCeiling = 0.0f;
    private int _ceilingLevel = 0;
    private bool _canLowerCeiling = true;

    // Start is called before the first frame update
    void Start()
    {
        if(_levelData != null && _levelData.Count > _currentLevelSection && _levelData[_currentLevelSection] != null)
        {
            CreateLevel();
            InitGemManager();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_currentLevelGameState != null && _currentLevelGameState.GetGameState() == PB_EGameState.PLAYING && _canLowerCeiling)
        {
            _currentTimeToLowerCeiling += Time.fixedDeltaTime;
            if (_currentTimeToLowerCeiling >= _timeToLowerCeiling)
            {
                _currentTimeToLowerCeiling = 0.0f;
                UpdateUpperLimit();
            }
        }
    }

    private void CreateLevel()
    {
        if(_levelData[_currentLevelSection].GetMapGridPrefab() != null)
        {
            Grid currentLevelGrid = Instantiate(_levelData[_currentLevelSection].GetMapGridPrefab());
            if (currentLevelGrid != null)
            {
                _currentLevelTilemap = currentLevelGrid.gameObject.GetComponentInChildren<Tilemap>();

                PB_MapConfigSO currentSection = _levelData[_currentLevelSection];
                _currentMapData._rightBound = (_currentLevelTilemap.origin.x) + currentSection._mapColumns - 1;
                _currentMapData._leftBound = _currentLevelTilemap.origin.x;
                _currentMapData._topBound = _currentLevelTilemap.origin.y + currentSection._mapRows - 1;
                _currentMapData._bottomBound = _currentLevelTilemap.origin.y;
                _currentMapData._mapCols = currentSection._mapColumns;
                _currentMapData._mapRows = currentSection._mapRows;

                if(_ceilingBound != null)
                {
                    _ceilingBound.SetBoundsSize(_currentMapData._mapCols);
                    Vector3 ceilingPosition = TileToWorld(_currentMapData._leftBound + _currentMapData._mapCols / 2, _currentMapData._topBound);
                    ceilingPosition.x -= 0.5f;
                    ceilingPosition.y += 1.0f;
                    _ceilingBound.transform.position = ceilingPosition;
                }
            }
        }

        if(_playerCharacterPrefab != null)
        {
            _currentLevelPlayer = Instantiate(_playerCharacterPrefab);
            
            if(_currentLevelPlayer != null && _currentLevelCharacterData != null) 
            {
                _currentLevelPlayer.SetSpriteLib(_currentLevelCharacterData.GetCharacterSpriteLib());
            }
        }
    }

    private void InitGemManager()
    {
        if (_gemManagerPrefab != null)
        {
            _currentLevelGemManager = Instantiate(_gemManagerPrefab);

            if(_currentLevelGemManager != null)
            {
                PB_EGemType[,] mapGemTypes;
                PB_EGemColor[,] mapGemColors;
                PB_MapConfigSO currentSection = _levelData[_currentLevelSection];
                currentSection.GetMapGemData(out mapGemTypes, out mapGemColors);

                _currentLevelGemManager.InitGemManager(mapGemTypes, mapGemColors, currentSection._mapRows, currentSection._mapColumns, _currentLevelPlayer, this);
            }
        }

        if (_currentLevelGameState != null)
        {
            _currentLevelGameState.SetGemManager(_currentLevelGemManager);
        }
    }

    public MapData GetCurrentMapData()
    {
        return _currentMapData;
    }

    public Vector3 TileToWorld(int InX, int InY)
    {
        Vector3Int tilePos = new Vector3Int(InX, InY, 0);
        Vector3 worldPos = _currentLevelTilemap.GetCellCenterWorld(tilePos);
        float oddOffset = _currentLevelTilemap.cellSize.x * 0.5f;
        worldPos.x += (((InY - _ceilingLevel) % 2) * oddOffset);
        return worldPos;
    }

    public Vector2Int NearestTile(float InX, float InY)
    {
        Vector3 worldPos = new Vector3(InX, InY, 0.0f);
        Vector3Int cellPos = _currentLevelTilemap.WorldToCell(worldPos);
        worldPos.x -= (((cellPos.y - _ceilingLevel) % 2) * 0.5f);
        cellPos = _currentLevelTilemap.WorldToCell(worldPos);
        Vector2Int tilePos = new Vector2Int(cellPos.x, cellPos.y);
        return tilePos;
    }

    public void UpdateUpperLimit()
    {
        _ceilingLevel++;

        if (_ceilingBound != null)
        {
            _ceilingBound.transform.position -= transform.up;
        }

        if (_onLowerCeilingEvent != null)
        {
            _onLowerCeilingEvent.RaiseEvent();
        }
    }

    public int GetUpperLimit()
    {
        return _currentMapData._topBound - _ceilingLevel;
    }

    public int GetCeilingLevel()
    { 
        return -_ceilingLevel;
    }

    public void SetCanLowerCeiling(bool InCanLower)
    {
        _canLowerCeiling = InCanLower;
    }
}
