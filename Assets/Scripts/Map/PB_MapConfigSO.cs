using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Puzzle Bobble/Maps/New Map Config")]
public class PB_MapConfigSO : ScriptableObject
{
    [SerializeField]
    private TextAsset _mapOrder;
    [field: SerializeField]
    public int _mapColumns { get; private set; }
    [field: SerializeField]
    public int _mapRows { get; private set; }
    [field: SerializeField]
    public int _leftPadding { get; private set; }
    [field: SerializeField]
    public int _rightPadding { get; private set; }
    [field: SerializeField]
    public int _upperPadding { get; private set;}

    private Dictionary<char, PB_EGemType> gemTypeMap = new Dictionary<char, PB_EGemType>()
    {
        {'N', PB_EGemType.NONE},
        {'D', PB_EGemType.DIRT},
        {'S', PB_EGemType.SILVER},
        {'G', PB_EGemType.GOLD},
        {'R', PB_EGemType.RUBY},
        {'A', PB_EGemType.AZURITE},
        {'E', PB_EGemType.EMERALD}
    };

    public PB_EGemType[,] GetMapGemTypes()
    {
        PB_EGemType[,] mapGemTypes;

        string mapOrderStr = _mapOrder.text;
        string[] textRows = mapOrderStr.Split('\n');

        mapGemTypes = new PB_EGemType[_mapRows, _mapColumns];

        for(int row = 0; row < _mapRows; row++)
        {
            if (row < textRows.Length)
            {
                string[] currentRowStr = textRows[row].Trim().Split(' ');
                for (int col = 0; col < _mapColumns; col++)
                {
                    if (col < textRows.Length)
                    {
                        char currentChar = currentRowStr[col][0];
                        PB_EGemType currentType = gemTypeMap[currentChar];
                        mapGemTypes[row, col] = currentType;
                    }
                }
            }
        }
        
        return mapGemTypes;
    }
}
