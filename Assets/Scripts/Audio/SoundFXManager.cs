using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFxObject;

    private bool wasGamePaused = false;
    private Dictionary<string, AudioSource> fxSoundsMap = new();

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        if (GameManager.IsGamePaused != wasGamePaused)
        {
            wasGamePaused = GameManager.IsGamePaused;

            if (wasGamePaused)
            {
                foreach (AudioSource audioSource in fxSoundsMap.Values)
                    audioSource.Pause();
            }
            else
            {
                foreach (AudioSource audioSource in fxSoundsMap.Values)
                    audioSource.UnPause();
            }
        }
    }

    public void PlaySoundFXClip(AudioClip clip, Vector2 position, float volume = 1.0F, float pitch = 1.0F, float destroyAfter = 0.0F, Transform parent = null, bool isAudio2D = false)
    {
        AudioSource audioSource = Instantiate(soundFxObject, parent);

        if (parent != null) audioSource.transform.localPosition = position;
        else audioSource.transform.position = position;

        if (isAudio2D) audioSource.spatialBlend = 0.0F;

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();
        
        fxSoundsMap[clip.name] = audioSource;
        float destroyTime = destroyAfter > 0 ? destroyAfter : clip.length;

        Destroy(audioSource.gameObject, destroyTime);

        StartCoroutine(RemoveSoundFromMap(clip.name, destroyTime));
    }

    public void PlaySoundFXClip(AudioClip clip, Vector2 position, Vector2 distance, float volume = 1.0F, float pitch = 1.0F, float destroyAfter = 0.0F, Transform parent = null, bool isAudio2D = false)
    {
        AudioSource audioSource = Instantiate(soundFxObject, parent);

        if (parent != null) audioSource.transform.localPosition = position;
        else audioSource.transform.position = position;

        if (isAudio2D) audioSource.spatialBlend = 0.0F;

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

        StartCoroutine(RemoveSoundFromMap(clip.name, destroyTime));
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

    private IEnumerator RemoveSoundFromMap(string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        fxSoundsMap.Remove(name);
    }
}
