using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    public void SwitchPause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (!isPaused)
            {
                Time.timeScale = 0f;
                gameObject.SetActive(true);
                isPaused = true;
                Debug.Log("Game Paused");
            }
            else
            {
                Time.timeScale = 1f;
                gameObject.SetActive(false);
                isPaused = false;
                Debug.Log("Game Resumed");
            }

        }
    }
}
