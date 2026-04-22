using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip != null) audioSource.PlayOneShot(clip, volume);
    }
}
