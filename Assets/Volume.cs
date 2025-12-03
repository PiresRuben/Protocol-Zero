using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    private Slider volumeSlider;

    private void Start()
    {
        volumeSlider = GetComponent<Slider>();
        AudioListener.volume = volumeSlider.value;
    }

    private void Update()
    {
        AudioListener.volume = volumeSlider.value;
        Debug.Log("Volume: " + AudioListener.volume);
    }

}
