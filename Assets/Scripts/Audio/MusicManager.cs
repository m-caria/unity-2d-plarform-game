using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip bgLevelMusic;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
