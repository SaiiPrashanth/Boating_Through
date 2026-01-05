using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Singleton instance to access audio from anywhere
    public static AudioManager Instance;

    public Sound[] gameSounds;

    private void Awake()
    {
        // Basic Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Keep this object alive when changing scenes
        DontDestroyOnLoad(gameObject);

        // Setup audio sources for each sound
        foreach (Sound s in gameSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.volume = s.volume;
            s.source.loop = s.isLooping;
        }
    }

    public void PlaySound(string soundName)
    {
        // Placeholder for playing sounds logic
        // We will implement the actual playback when we have the audio files ready
    }
}