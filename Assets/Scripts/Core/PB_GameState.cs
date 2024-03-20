using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PB_EGameState
{
    PLAYING,
    LOSING,
    WINNING,
    PAUSED
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
    private int _pointsPerGem = 50;

    [SerializeField]
    private PB_VoidEventChannelSO _onPlayerWinEvent = null;
    [SerializeField]
    private PB_VoidEventChannelSO _onPlayerLoseEvent = null;
    [SerializeField]
    private PB_VoidEventChannelSO _onPauseEvent = null;
    [SerializeField]
    private PB_VoidEventChannelSO _onResumeEvent = null;

    [SerializeField]
    private PB_IntVariable_SO _playerScore;

    [SerializeField]
    private PB_InputReaderSO _inputReader = null;

    public void OnEnable()
    {
        if (_playerScore != null)
        {
            _playerScore.SetValue(0);
        }

        if (_inputReader)
        {
            _inputReader.pauseEvent += OnPauseGame;
        }
    }

    public void OnGemsDestroyed(int numGemsDestroyed)
    {
        UpdatePlayerScore(numGemsDestroyed);
        CheckGameWinningState();
    }

    public void UpdatePlayerScore(int numGemsDestroyed)
    {
        _playerScore.ApplyChange(numGemsDestroyed * _pointsPerGem);
    }

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

    public void OnPauseGame()
    {
        if(GetGameState() == PB_EGameState.PLAYING)
        {
            _currentGameState = PB_EGameState.PAUSED;
            Time.timeScale = 0.0f;

            if (_onPauseEvent != null)
            {
                _onPauseEvent.RaiseEvent();
            }
        }
        else if (GetGameState() == PB_EGameState.PAUSED)
        {
            _currentGameState = PB_EGameState.PLAYING;
            Time.timeScale = 1.0f;

            if (_onResumeEvent != null)
            {
                _onResumeEvent.RaiseEvent();
            }
        }
    }
}
