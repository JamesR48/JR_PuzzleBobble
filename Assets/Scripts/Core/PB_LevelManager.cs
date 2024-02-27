using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_LevelManager : MonoBehaviour
{
    [SerializeField]
    private List<PB_MapConfigSO> _levelData = null;
    [SerializeField]
    private PB_GemManager _gemManagerPrefab = null;
    [SerializeField]
    private PB_CharacterComponent _playerCharacterPrefab = null;

    private int _currentLevelSection = 0;
    private PB_GemManager _currentLevelGemManager = null;
    private PB_CharacterComponent _currentLevelPlayer = null;

    int Right;
    int Left;
    int Up;
    int Down;
    int ceilingLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(_levelData != null && _levelData.Count > _currentLevelSection && _levelData[_currentLevelSection] != null)
        {
            InitGemManager();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void InitGemManager()
    {
        if (_gemManagerPrefab != null)
        {
            _currentLevelGemManager = Instantiate(_gemManagerPrefab);

            if(_currentLevelGemManager)
            {
                PB_EGemType[,] mapGemTypes;
                PB_EGemColor[,] mapGemColors;
                PB_MapConfigSO currentSection = _levelData[_currentLevelSection];
                currentSection.GetMapGemData(out mapGemTypes, out mapGemColors);

                _currentLevelGemManager.InitMapGems(mapGemTypes, mapGemColors, currentSection._mapRows, currentSection._mapColumns);
            }
        }
    }
}
