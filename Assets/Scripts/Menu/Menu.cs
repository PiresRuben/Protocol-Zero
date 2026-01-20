using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    [SerializeField]
    private int startScene = 1;

    void Start()
    {
        // get scene from database
    }
    public void StartButton()
    {
        SceneManager.LoadScene(startScene);
        Debug.Log("Scene Loaded");
    }

    public void MenuButton()
    {
        SceneManager.LoadScene(0);
        Debug.Log("Menu Loaded");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
