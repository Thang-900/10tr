using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        volumeSlider.onValueChanged.AddListener((value) =>
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.ChangeVolume(value);
        });

        // Đặt mặc định
        volumeSlider.value = 1f;
    }
}
