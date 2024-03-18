using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_AudioCue : MonoBehaviour
{
    [SerializeField] 
    private AudioClip _audioClip = default;
    [SerializeField] 
    private bool _playOnStart = false;
    [SerializeField]
    private bool _playLooping = false;

    [Header("Configuration")]
    [SerializeField] 
    private PB_AudioCueEventChannelSO _audioCueEventChannel = default;
    [SerializeField] 
    private PB_AudioConfigurationSO _audioConfiguration = default;

    private void Start()
    {
        if (_playOnStart)
        {
            PlayAudioCue();
        }
    }

    public void PlayAudioCue()
    {
        if (_audioCueEventChannel != null)
        {
            _audioCueEventChannel.RaiseEvent(this, _audioConfiguration);
        }
    }

    public bool GetPlayLooping()
    {
        return _playLooping;
    }

    public AudioClip GetAudioClip() 
    {
        return _audioClip;
    }
}
