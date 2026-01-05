using UnityEngine;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip audioClip;
    public bool isLooping;
    
    [Range(0f, 1f)]
    public float volume;

    [HideInInspector]
    public AudioSource source;
}