using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFxObject;

    private Dictionary<string, AudioSource> fxSoundsMap = new();

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void PlaySoundFXClip(AudioClip clip, Vector2 position, float volume = 1.0F, float pitch = 1.0F, float destroyAfter = 0.0F, Transform parent = null)
    {
        AudioSource audioSource = Instantiate(soundFxObject, parent);

        if (parent != null) audioSource.transform.localPosition = position;
        else audioSource.transform.position = position;

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();
        
        fxSoundsMap[clip.name] = audioSource;
        float destroyTime = destroyAfter > 0 ? destroyAfter : clip.length;

        Destroy(audioSource.gameObject, destroyTime);

        fxSoundsMap.Remove(clip.name);
    }

    public void PlaySoundFXClip(AudioClip clip, Vector2 position, Vector2 distance, float volume = 1.0F, float pitch = 1.0F, float destroyAfter = 0.0F, Transform parent = null)
    {
        AudioSource audioSource = Instantiate(soundFxObject, parent);

        if (parent != null) audioSource.transform.localPosition = position;
        else audioSource.transform.position = position;

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.minDistance = distance.x;
        audioSource.maxDistance = distance.y;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.Play();

        fxSoundsMap[clip.name] = audioSource;
        float destroyTime = destroyAfter > 0 ? destroyAfter : clip.length;
        Destroy(audioSource.gameObject, destroyTime);

        fxSoundsMap.Remove(clip.name);
    }

    public void DestroySoundFXClip(AudioClip clip)
    {
        if (fxSoundsMap.TryGetValue(clip.name, out AudioSource audioSource))
        {
            audioSource.Stop();
            Destroy(audioSource.gameObject);
            fxSoundsMap.Remove(clip.name);
        }
    }
}
