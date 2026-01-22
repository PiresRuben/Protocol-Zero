using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    [SerializeField]
    private int startScene = 1;
    [SerializeField]
    private Slider volumeSlider;

    void Start()
    {
        if (volumeSlider != null)
            volumeSlider.value = AudioListener.volume;
    }
    public void StartButton()
    {
        SceneManager.LoadScene(startScene);
        Time.timeScale = 1f;
        Debug.Log("Scene Loaded");
    }

    public void MenuButton()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        Debug.Log("Menu Loaded");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void UpdateSoundVolume()
    {
        float volume = volumeSlider.value;
        AudioListener.volume = volume;
        Debug.Log("Volume set to: " + volume);
    }
}
