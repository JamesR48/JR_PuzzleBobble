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
        int emittersNum = _soundEmitters.Count;
        if (audioCue != null && emittersNum > 0)
        {
            for(int idx = 0; idx < emittersNum; idx++)
            {
                PB_SoundEmitter soundEmitter = _soundEmitters[idx];
                if (soundEmitter != null)
                {
                    if(!(soundEmitter.IsInUse() && settings.CanBeInterrupted))
                    {
                        soundEmitter.PlayAudioClip(audioCue.GetAudioClip(), settings, audioCue.GetPlayLooping());
                        if (!audioCue.GetPlayLooping())
                        {
                            soundEmitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
                        }
                        break;
                    }
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
