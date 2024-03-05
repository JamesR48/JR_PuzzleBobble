using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_GameSceneSO : ScriptableObject
{
    [Header("Information")]
    
    [SerializeField]
    private string _sceneName = "";
    [SerializeField]
    private string _shortDescription = "";

    [Header("Sounds")]
    
    [SerializeField]
    private AudioClip _sceneMusic = null; 
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _musicVolume = 0.2f;

    public string GetSceneName() {  return _sceneName; }
}
