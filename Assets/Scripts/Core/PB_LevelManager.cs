using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_LevelManager : MonoBehaviour
{
    [SerializeField]
    private PB_MapConfigSO _levelData = null;

    private int _currentLevelSection = 0;

    int Right;
    int Left;
    int Up;
    int Down;
    int ceilingLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
