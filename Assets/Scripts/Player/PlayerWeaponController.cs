using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("Réglages Visuels")]
    public SpriteRenderer playerRenderer;

    [Header("Armes")]
    public List<Weapon> weapons;
    private Weapon currentWeapon;
    private int currentWeaponIndex = 0;

    private void Start()
    {
        if (playerRenderer == null)
            playerRenderer = GetComponent<SpriteRenderer>();

        InitializeWeapons();
    }

    void InitializeWeapons()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] != null)
            {
                weapons[i].gameObject.SetActive(false);
            }
        }

        if (weapons.Count > 0)
        {
            EquipWeapon(0);
        }
    }

    private void Update()
    {
        HandleWeaponSwitching();
        HandleShooting();
    }

    void HandleWeaponSwitching()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame) EquipWeapon(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) EquipWeapon(1);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) EquipWeapon(2);
    }

    void EquipWeapon(int index)
    {
        if (index >= weapons.Count || index < 0) return;

        if (currentWeapon != null)
            currentWeapon.gameObject.SetActive(false);

        currentWeaponIndex = index;
        currentWeapon = weapons[index];

        currentWeapon.gameObject.SetActive(true);

        if (playerRenderer != null && currentWeapon.playerSpriteWithWeapon != null)
        {
            playerRenderer.sprite = currentWeapon.playerSpriteWithWeapon;
        }

        Debug.Log($"Arme équipée : {currentWeapon.weaponName}");
    }

    void HandleShooting()
    {
        if (currentWeapon != null)
        {
            if (currentWeapon.isAutomatic)
            {
                if (Mouse.current.leftButton.isPressed)
                {
                    currentWeapon.TryAttack();
                }
            }
            else
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    currentWeapon.TryAttack();
                }
            }
        }
    }
}