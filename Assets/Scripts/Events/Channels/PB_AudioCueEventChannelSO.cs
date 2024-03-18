using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Puzzle Bobble/Events/AudioCue Event Channel")]
public class PB_AudioCueEventChannelSO : ScriptableObject
{
    public UnityAction<PB_AudioCue, PB_AudioConfigurationSO> OnEventRaised;
    public void RaiseEvent(PB_AudioCue audioCue, PB_AudioConfigurationSO audioConfiguration)
    {
        OnEventRaised.Invoke(audioCue, audioConfiguration);
    }
}
