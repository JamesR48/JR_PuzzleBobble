using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Puzzle Bobble/Scene Data/Level")]
public class PB_LevelSceneSO : PB_GameSceneSO
{
    [Header("Level specific")]

    [SerializeField]
    private PB_MapConfigSO _levelData = null;
}
