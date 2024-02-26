using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PB_EGameState
{
    PLAYING,
    LOSING,
    WINNING
}

public class PB_GameState : MonoBehaviour
{
    [SerializeField]
    private PB_EGameState _currentGameState = PB_EGameState.PLAYING;

    [SerializeField]
    private PB_GemManager _gemManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void CheckGameState()
    {
        if(_gemManager != null)
        {
            if(_gemManager.gemsArray.Count == 0)
            {
                _currentGameState = PB_EGameState.WINNING;
            }
        }
    }
}
