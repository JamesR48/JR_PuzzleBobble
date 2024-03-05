using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NewSceneDatabase", menuName = "Puzzle Bobble/Scene Data/Database")]
public class PB_ScenesDataSO : ScriptableObject
{
    [SerializeField]
    private List<PB_LevelSceneSO> _levelScenes = new List<PB_LevelSceneSO>();
    [SerializeField]
    private List<PB_MenuSceneSO> _menuScenes = new List<PB_MenuSceneSO>();
    [SerializeField]
    private int _currentLevelIndex = 1;

    /*
     * _levelScenes
     */

    //Load a scene with a given index
    public void LoadLevelWithIndex(int index)
    {
        if (index <= _levelScenes.Count)
        {
            //Load Gameplay scene for the level
            SceneManager.LoadSceneAsync("Gameplay" + index.ToString());
            //Load first part of the level in additive mode
            SceneManager.LoadSceneAsync("Level" + index.ToString() + "Part1", LoadSceneMode.Additive);
        }
        //reset the index if we have no more _levelScenes
        else _currentLevelIndex = 1;
    }
    //Start next level
    public void NextLevel()
    {
        _currentLevelIndex++;
        LoadLevelWithIndex(_currentLevelIndex);
    }
    //Restart current level
    public void RestartLevel()
    {
        LoadLevelWithIndex(_currentLevelIndex);
    }
    //New game, load level 1
    public void NewGame()
    {
        LoadLevelWithIndex(1);
    }

    /*
     * _menuScenes
     */

    //Load main Menu
    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(_menuScenes[(int)PB_EMenuType.MAIN_MENU].GetSceneName());
    }
    //Load Pause Menu
    public void LoadPauseMenu()
    {
        SceneManager.LoadSceneAsync(_menuScenes[(int)PB_EMenuType.PAUSE_MENU].GetSceneName());
    }
}
