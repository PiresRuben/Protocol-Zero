using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField]
    private GameObject Main;
    [SerializeField]
    private GameObject Option;

    public void SwitchPause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (!isPaused)
            {
                Time.timeScale = 0f;
                gameObject.SetActive(true);
                isPaused = true;
                Main.SetActive(true);
                Option.SetActive(false);
            }
            else
            {
                Time.timeScale = 1f;
                gameObject.SetActive(false);
                isPaused = false;
            }

        }
    }
}
