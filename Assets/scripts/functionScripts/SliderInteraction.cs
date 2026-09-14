using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderInteraction : MonoBehaviour
{
    [Header("Slider references")]
    [SerializeField] private Slider volumeSlider;

    [Header("value variables")]
    public float AudioSliderValue;

    [Header("Script references")]
    [SerializeField] private Pause_Settings settings;

    private void OnEnable()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(OnAudioSliderValueChanged);
        }
    }

    private void OnDisable()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(OnAudioSliderValueChanged);
        }
    }

    public void OnAudioSliderValueChanged(float value)
    {
        settings.AudioLevelChange(value);
    }
}
