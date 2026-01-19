using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("Armes")]
    public List<Weapon> weapons; // Liste de tes armes (enfants du player)
    private Weapon currentWeapon;
    private int currentWeaponIndex = 0;

    private void Start()
    {
        // Initialiser les armes
        InitializeWeapons();
    }

    void InitializeWeapons()
    {
        // Désactiver toutes les armes sauf la première
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] != null)
                weapons[i].gameObject.SetActive(i == 0);
        }

        if (weapons.Count > 0)
            currentWeapon = weapons[0];
    }

    private void Update()
    {
        HandleWeaponSwitching();
        HandleShooting();
    }

    void HandleWeaponSwitching()
    {
        // Touches clavier (New Input System via Keyboard direct pour simplifier ici)
        if (Keyboard.current.digit1Key.wasPressedThisFrame) EquipWeapon(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) EquipWeapon(1);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) EquipWeapon(2);
    }

    void EquipWeapon(int index)
    {
        if (index >= weapons.Count || index < 0) return;
        if (index == currentWeaponIndex) return;

        // Désactiver l'arme actuelle
        if (currentWeapon != null)
            currentWeapon.gameObject.SetActive(false);

        // Activer la nouvelle
        currentWeaponIndex = index;
        currentWeapon = weapons[index];
        currentWeapon.gameObject.SetActive(true);

        Debug.Log($"Arme équipée : {currentWeapon.weaponName}");
    }

    void HandleShooting()
    {
        // Si clic gauche maintenu (ou pressé selon l'arme, ici on gère le fireRate dans l'arme)
        if (Mouse.current.leftButton.isPressed)
        {
            if (currentWeapon != null)
            {
                currentWeapon.TryAttack();
            }
        }
    }
}