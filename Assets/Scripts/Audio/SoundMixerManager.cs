using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
    public enum Mixers
    {
        MASTER,
        SFX,
        MUSIC
    }

    private Dictionary<Mixers, string> mixers = new() 
    {
        { Mixers.MASTER, "masterVolume" },
        { Mixers.SFX, "soundFXVolume" },
        { Mixers.MUSIC, "bgMusicVolume" },
    };

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider globalVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    private void Start()
    {
        if (globalVolumeSlider != null)
            globalVolumeSlider.value = GetMixerVolume(Mixers.MASTER);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.value = GetMixerVolume(Mixers.SFX);

        if (musicVolumeSlider != null)
            musicVolumeSlider.value = GetMixerVolume(Mixers.MUSIC);
    }

    public void SetMasterVolume(Slider slider) => audioMixer.SetFloat(mixers[Mixers.MASTER], slider.value);

    public void SetSoundFXVolume(Slider slider) => audioMixer.SetFloat(mixers[Mixers.SFX], slider.value);

    public void SetBGMusicVolume(Slider slider) => audioMixer.SetFloat(mixers[Mixers.MUSIC], slider.value);

    public float GetMixerVolume(Mixers mixer)
    {
        if (audioMixer.GetFloat(mixers[mixer], out float volume))
            return volume;

        return 0.0F;
    }
}
