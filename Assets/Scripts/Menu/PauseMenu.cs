using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField]
    private GameObject Main;
    [SerializeField]
    private GameObject Option;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private PlayerWeaponController playerWeaponController;
    [SerializeField]
    private InventoryManager inventoryManager;

    public void SwitchPause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (!isPaused)
            {
                Time.timeScale = 0f;
                gameObject.SetActive(true);
                isPaused = true;
                playerController.enabled = false;
                playerWeaponController.enabled = false;
                inventoryManager.CloseInventory();
                Main.SetActive(true);
                Option.SetActive(false);
            }
            else
            {
                Time.timeScale = 1f;
                gameObject.SetActive(false);
                playerController.enabled = true;
                playerWeaponController.enabled = true;
                isPaused = false;
            }

        }
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        isPaused = false;
        playerController.enabled = true;
        playerWeaponController.enabled = true;
    }
}
