using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Puzzle Bobble/Audio/New Audio Config")]
public class PB_AudioConfigurationSO : ScriptableObject
{
    private enum PB_EPriorityLevel
    {
        Highest = 0,
        High = 64,
        Standard = 128,
        Low = 194,
        VeryLow = 256,
    }

    public AudioMixerGroup OutputAudioMixerGroup = null;

    [SerializeField] 
    private PB_EPriorityLevel _priorityLevel = PB_EPriorityLevel.Standard;
    [HideInInspector]
    public int Priority
    {
        get { return (int)_priorityLevel; }
        set { _priorityLevel = (PB_EPriorityLevel)value; }
    }

    public bool Mute = false;
    public bool CanBeInterrupted = false;
    [Range(0f, 1f)] public float Volume = 1f;
    [Range(-3f, 3f)] public float Pitch = 1f;

    public void ApplyTo(AudioSource audioSource)
    {
        audioSource.outputAudioMixerGroup = this.OutputAudioMixerGroup;
        audioSource.mute = this.Mute;
        audioSource.priority = this.Priority;
        audioSource.volume = this.Volume;
        audioSource.pitch = this.Pitch;
    }
}
