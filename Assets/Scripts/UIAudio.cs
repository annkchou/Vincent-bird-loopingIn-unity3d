using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIAudio : MonoBehaviour
{
    //UI Components
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle muteToggle;
    [SerializeField] private UIManager uIManager;
    [SerializeField] private SoundController.Channel channel;
    public void UpdateAudio()
    {
        uIManager.UpdateAudio(channel, volumeSlider.value, muteToggle.isOn);
    }
}
