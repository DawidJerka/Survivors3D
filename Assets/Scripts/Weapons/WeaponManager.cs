using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private PlayerLoadout playerLoadout;
    [SerializeField] private Transform weaponContainer;

    private readonly Dictionary<WeaponData, Weapon>
        activeWeapons = new();

    private void OnEnable()
    {
        if (playerLoadout != null)
        {
            playerLoadout.OnItemLevelChanged +=
                HandleItemLevelChanged;
        }
    }

    private void Start()
    {
        InitializeStartingWeapons();
    }

    private void OnDisable()
    {
        if (playerLoadout != null)
        {
            playerLoadout.OnItemLevelChanged -=
                HandleItemLevelChanged;
        }
    }

    private void InitializeStartingWeapons()
    {
        foreach (LevelUpItemData item in playerLoadout.OwnedItems)
        {
            if (item is WeaponData weaponData)
            {
                CreateWeapon(weaponData);
            }
        }
    }

    private void HandleItemLevelChanged(
        LevelUpItemData item,
        int newLevel)
    {
        if (item is not WeaponData weaponData)
            return;

        if (!activeWeapons.TryGetValue(
                weaponData,
                out Weapon weapon))
        {
            weapon = CreateWeapon(weaponData);
        }

        if (weapon != null)
        {
            weapon.HandleLevelChanged(newLevel);
        }
    }

    private Weapon CreateWeapon(WeaponData weaponData)
    {
        if (weaponData == null)
            return null;

        if (activeWeapons.TryGetValue(
                weaponData,
                out Weapon existingWeapon))
        {
            return existingWeapon;
        }

        if (weaponData.WeaponPrefab == null)
        {
            Debug.LogWarning(
                $"Weapon '{weaponData.DisplayName}' has no prefab."
            );

            return null;
        }

        Transform parent =
            weaponContainer != null
                ? weaponContainer
                : transform;

        Weapon weapon = Instantiate(
            weaponData.WeaponPrefab,
            parent
        );

        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        weapon.Initialize(
            weaponData,
            playerLoadout
        );

        activeWeapons.Add(
            weaponData,
            weapon
        );

        return weapon;
    }
}