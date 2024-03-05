using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PB_EMenuType
{
    MAIN_MENU,
    PAUSE_MENU
}

[CreateAssetMenu(fileName = "NewMenu", menuName = "Puzzle Bobble/Scene Data/Menu")]
public class PB_MenuSceneSO : PB_GameSceneSO
{
    [Header("Menu specific")]

    [SerializeField]
    private PB_EMenuType _menuType = PB_EMenuType.MAIN_MENU;
}
