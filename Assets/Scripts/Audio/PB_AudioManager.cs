using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PB_AudioManager : MonoBehaviour
{
    [SerializeField]
    private List<PB_SoundEmitter> _soundEmitters;

    [SerializeField]
    private PB_AudioCueEventChannelSO _SFXEventChannel = default;
    [SerializeField] 
    private PB_AudioCueEventChannelSO _musicEventChannel = default;

    [SerializeField]
    private AudioMixer _audioMixer = default;
    [Range(0f, 1f)]
    [SerializeField] 
    private float _masterVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField]
    private float _musicVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField]
    private float _sfxVolume = 1f;

    private void Awake()
    {
        _SFXEventChannel.OnEventRaised += PlayAudioCue;
        _musicEventChannel.OnEventRaised += PlayAudioCue;
    }

    public void PlayAudioCue(PB_AudioCue audioCue, PB_AudioConfigurationSO settings)
    {
        if(audioCue != null && _soundEmitters.Count > 0)
        {
            foreach (PB_SoundEmitter soundEmitter in _soundEmitters)
            {
                if (soundEmitter != null)
                {
                    soundEmitter.PlayAudioClip(audioCue.GetAudioClip(), settings, audioCue);
                    if (!audioCue.GetPlayLooping())
                        soundEmitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
                }
            }
        }
    }

    private void OnSoundEmitterFinishedPlaying(PB_SoundEmitter soundEmitter)
    {
        soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;
        soundEmitter.Stop();
    }
}
