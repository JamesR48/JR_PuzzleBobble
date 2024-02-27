using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Puzzle Bobble/Maps/New Map Config")]
public class PB_MapConfigSO : ScriptableObject
{
    [SerializeField]
    private TextAsset _mapOrder;
    [SerializeField]
    private Grid _levelGridPrefab;
    [field: SerializeField]
    public int _mapColumns { get; private set; }
    [field: SerializeField]
    public int _mapRows { get; private set; }

    private Dictionary<char, PB_EGemType> _gemTypeMap = new Dictionary<char, PB_EGemType>()
    {
        {'N', PB_EGemType.NONE},
        {'E', PB_EGemType.EARTH},
        {'R', PB_EGemType.ROCK},
        {'M', PB_EGemType.METAL},
        {'G', PB_EGemType.GEM}
    };

    private Dictionary<char, PB_EGemColor> _gemColorMap = new Dictionary<char, PB_EGemColor>()
    {
        {'N', PB_EGemColor.NONE},
        {'D', PB_EGemColor.DIRT},
        {'W', PB_EGemColor.WHITE},
        {'Y', PB_EGemColor.YELLOW},
        {'R', PB_EGemColor.RED},
        {'B', PB_EGemColor.BLUE},
        {'G', PB_EGemColor.GREEN}
    };

    public void GetMapGemData(out PB_EGemType[,] mapGemTypes, out PB_EGemColor[,] mapGemColors)
    {
        string mapOrderStr = _mapOrder.text;
        string[] textRows = mapOrderStr.Split('\n');

        mapGemTypes = new PB_EGemType[_mapRows, _mapColumns];
        mapGemColors = new PB_EGemColor[_mapRows, _mapColumns];

        for (int row = 0; row < _mapRows; row++)
        {
            if (row < textRows.Length)
            {
                string[] currentRowStr = textRows[row].Trim().Split(' ');
                for (int col = 0; col < _mapColumns; col++)
                {
                    if (col < textRows.Length)
                    {
                        char currentTypeChar = currentRowStr[col][0];
                        PB_EGemType currentType = _gemTypeMap[currentTypeChar];
                        mapGemTypes[row, col] = currentType;

                        char currentColorChar = currentRowStr[col][1];
                        PB_EGemColor currentColor = _gemColorMap[currentColorChar];
                        mapGemColors[row, col] = currentColor;
                    }
                }
            }
        }
    }

    public Grid GetMapGridPrefab()
    {
        return _levelGridPrefab;
    }
}
