using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class PB_SoundEmitter : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource = null;

    [SerializeField]
    private bool _canBeInterrupted = false;

    public event UnityAction<PB_SoundEmitter> OnSoundFinishedPlaying;

    private float _currentAudioTimer = 0.0f;

    private void Awake()
    {
        if(_audioSource == null)
        {
            _audioSource = this.GetComponent<AudioSource>();
        }
        if (_audioSource != null)
        {
            _audioSource.playOnAwake = false;
        }            
    }

    private void FixedUpdate()
    {
        if(_currentAudioTimer > 0.0f)
        {
            _currentAudioTimer -= Time.fixedDeltaTime;
            if(_currentAudioTimer <= 0.0f)
            {
                _currentAudioTimer = 0.0f;
                FinishedPlaying();
            }
        }
    }

    public void PlayAudioClip(AudioClip clip, PB_AudioConfigurationSO settings, bool hasToLoop)
    {
        if(_audioSource != null && clip != null)
        {
            _currentAudioTimer = 0.0f;
            _audioSource.clip = clip;
            settings.ApplyTo(_audioSource);
            _audioSource.loop = hasToLoop;
            _audioSource.Play();
            
            if (!hasToLoop)
            {
                _currentAudioTimer = clip.length;
            }
        }
    }

    public void Resume()
    {
        _audioSource.Play();
    }

    public void Pause()
    {
        _audioSource.Pause();
    }

    public void Stop()
    {
        _currentAudioTimer = 0.0f;
        _audioSource.Stop();
    }

    public bool IsInUse()
    {
        return _audioSource.isPlaying;
    }

    public bool IsLooping()
    {
        return _audioSource.loop;
    }

    private void FinishedPlaying()
    {
        if(OnSoundFinishedPlaying.GetInvocationList().Length > 0)
        {
            OnSoundFinishedPlaying.Invoke(this);
        }
    }

    public bool CanBeInterrupted()
    {
        return _canBeInterrupted;
    }
}
