using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    [SerializeField]
    private int currentScene = 1;

    void Start()
    {
        // get scene from database
    }
    public void StartButton()
    {
        SceneManager.LoadScene(currentScene);
        Debug.Log("Scene Loaded");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
