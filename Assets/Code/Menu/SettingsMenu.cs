using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsMenu : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider saturationSlider;

    [Header("Wwise RTPCs (Game Parameters, range 0-100)")]
    [SerializeField] private AK.Wwise.RTPC musicVolumeRTPC;
    [SerializeField] private AK.Wwise.RTPC soundVolumeRTPC;

    [Header("Post Processing (can be empty in scenes without it)")]
    [SerializeField] private Volume postProcessVolume;

    private const string MusicKey = "Settings_MusicVolume";
    private const string SoundKey = "Settings_SoundVolume";
    private const string SaturationKey = "Settings_Saturation";

    private ColorAdjustments colorAdjustments;

    private void Start()
    {
        SetupSlider(musicSlider, 0f, 1f);
        SetupSlider(soundSlider, 0f, 1f);
        SetupSlider(saturationSlider, -100f, 100f); // 0 = normal colours

        float music = PlayerPrefs.GetFloat(MusicKey, 1f);
        float sound = PlayerPrefs.GetFloat(SoundKey, 1f);
        float saturation = PlayerPrefs.GetFloat(SaturationKey, 0f);

        if (musicSlider != null) musicSlider.SetValueWithoutNotify(music);
        if (soundSlider != null) soundSlider.SetValueWithoutNotify(sound);
        if (saturationSlider != null) saturationSlider.SetValueWithoutNotify(saturation);

        SetMusicVolume(music);
        SetSoundVolume(sound);
        SetSaturation(saturation);

        if (musicSlider != null) musicSlider.onValueChanged.AddListener(SetMusicVolume);
        if (soundSlider != null) soundSlider.onValueChanged.AddListener(SetSoundVolume);
        if (saturationSlider != null) saturationSlider.onValueChanged.AddListener(SetSaturation);
    }

    private void SetupSlider(Slider slider, float min, float max)
    {
        if (slider == null) return;
        slider.minValue = min;
        slider.maxValue = max;
        slider.wholeNumbers = false;
    }

    public void SetMusicVolume(float value)
    {
        if (musicVolumeRTPC != null && musicVolumeRTPC.IsValid())
            musicVolumeRTPC.SetGlobalValue(value * 100f);
        PlayerPrefs.SetFloat(MusicKey, value);
    }

    public void SetSoundVolume(float value)
    {
        if (soundVolumeRTPC != null && soundVolumeRTPC.IsValid())
            soundVolumeRTPC.SetGlobalValue(value * 100f);
        PlayerPrefs.SetFloat(SoundKey, value);
    }

    public void SetSaturation(float value)
    {
        PlayerPrefs.SetFloat(SaturationKey, value);

        if (postProcessVolume == null) return;

        if (colorAdjustments == null)
        {
            VolumeProfile profile = postProcessVolume.profile;
            if (!profile.TryGet(out colorAdjustments))
                colorAdjustments = profile.Add<ColorAdjustments>(true);
        }

        colorAdjustments.active = true;
        colorAdjustments.saturation.overrideState = true;
        colorAdjustments.saturation.value = value;
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
}