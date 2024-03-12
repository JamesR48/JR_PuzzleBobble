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
    private PB_GemManager _gemManager = null;

    [SerializeField]
    private Vector3 _lowerPosition = Vector3.zero;

    [SerializeField]
    private PB_VoidEventChannelSO _onPlayerWinEvent = null;
    [SerializeField]
    private PB_VoidEventChannelSO _onPlayerLoseEvent = null;

    public void CheckGameWinningState()
    {
        if(_gemManager != null)
        {
            if(_gemManager.gemsArray.Count == 0)
            {
                _currentGameState = PB_EGameState.WINNING;
                _onPlayerWinEvent.RaiseEvent();
                Debug.Log("----- WINNING -----");
            }
        }
    }

    public void CheckGameLosingState()
    {
        if (_gemManager != null)
        {
            if (_gemManager.gemsArray.Count > 0)
            {
                foreach (PB_GemComponent gem in _gemManager.gemsArray)
                {
                    if(gem != null)
                    {
                        if(gem.transform.position.y-1.0f <= _lowerPosition.y)
                        {
                            _currentGameState = PB_EGameState.LOSING;
                            _onPlayerLoseEvent.RaiseEvent();
                            Debug.Log("----- LOSING -----");
                            break;
                        }
                    }
                }
            }
        }
    }

    public PB_EGameState GetGameState()
    {
        return _currentGameState;
    }

    public void SetGemManager(PB_GemManager gemManager)
    {
        _gemManager = gemManager;
    }
}
