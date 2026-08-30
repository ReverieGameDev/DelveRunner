using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;
    public WeaponData currentWeapon;
    public bool switchingWeapons = false;
    public List<WeaponData> listOfWeapons = new List<WeaponData>();

    public List<Image> weaponSlotImages = new List<Image>();
    public List<WeaponData> currentWeapons = new List<WeaponData>();
    public float weaponSwitchCooldown;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;

        int loadout = PlayerPrefs.GetInt("SelectedLoadout", 0);
        currentWeapons.Clear();
        switch (loadout)
        {
            case 0: // Balanced
                currentWeapons.Add(listOfWeapons[0]);
                currentWeapons.Add(listOfWeapons[1]);
                currentWeapons.Add(listOfWeapons[2]);
                break;
            case 1: // High DPS
                currentWeapons.Add(listOfWeapons[0]);
                currentWeapons.Add(listOfWeapons[2]);
                currentWeapons.Add(listOfWeapons[4]);
                break;
            case 2: // Status Effects
                currentWeapons.Add(listOfWeapons[1]);
                currentWeapons.Add(listOfWeapons[0]);
                currentWeapons.Add(listOfWeapons[4]);
                break;
            case 3: // Scaling/Ramp
                currentWeapons.Add(listOfWeapons[0]);
                currentWeapons.Add(listOfWeapons[2]);
                currentWeapons.Add(listOfWeapons[3]);
                break;
        }
        currentWeapon = currentWeapons[0];
        UpdateWeaponUI(WeaponSwitch.Weapon1);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && switchingWeapons == false)
        {
            switchingWeapons = true;
            StartCoroutine(SwitchWeapons(WeaponSwitch.Weapon1));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && switchingWeapons == false)
        {
            switchingWeapons = true;
            StartCoroutine(SwitchWeapons(WeaponSwitch.Weapon2));

        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && switchingWeapons == false)
        {
            switchingWeapons = true;
            StartCoroutine(SwitchWeapons(WeaponSwitch.Weapon3));
        }
    }

    IEnumerator SwitchWeapons(WeaponSwitch weaponSwitch)
    {
        ResetWeaponUIHighlights();
        UpdateWeaponUI(weaponSwitch);
        yield return new WaitForSeconds(weaponSwitchCooldown);
        switchingWeapons = false;
    }

    private void UpdateWeaponUI(WeaponSwitch weaponSwitch)
    {
        //this will just highlight the currently selected weapon.
        switch (weaponSwitch)
        {
            case WeaponSwitch.Weapon1:
                currentWeapon = currentWeapons[0];
                UnityEngine.ColorUtility.TryParseHtmlString("#DBE02E", out Color selectedColor1);
                weaponSlotImages[0].color = selectedColor1;
                break;
            case WeaponSwitch.Weapon2:
                currentWeapon = currentWeapons[1];
                UnityEngine.ColorUtility.TryParseHtmlString("#DBE02E", out Color selectedColor2);
                weaponSlotImages[1].color = selectedColor2;
                break;
            case WeaponSwitch.Weapon3:
                currentWeapon = currentWeapons[2];
                UnityEngine.ColorUtility.TryParseHtmlString("#DBE02E", out Color selectedColor3);
                weaponSlotImages[2].color = selectedColor3;
                break; 
        }
        for (int i = 0; i < currentWeapons.Count; i++)
        {
            weaponSlotImages[i].sprite = currentWeapons[i].wIcon;
            TooltipTrigger wep1 = weaponSlotImages[i].GetComponent<TooltipTrigger>();
            wep1.title = currentWeapons[i].wName;
            wep1.body = currentWeapons[i].wDescription;
            wep1.secondary = currentWeapons[i].wSpecialEffectDescription;
        }
    }

    private void ResetWeaponUIHighlights()
    {
        UnityEngine.ColorUtility.TryParseHtmlString("#C68A44", out Color selectedColor1);
        weaponSlotImages[0].color = selectedColor1;
        UnityEngine.ColorUtility.TryParseHtmlString("#C68A44", out Color selectedColor2);
        weaponSlotImages[1].color = selectedColor2;
        UnityEngine.ColorUtility.TryParseHtmlString("#C68A44", out Color selectedColor3);
        weaponSlotImages[2].color = selectedColor3;
    }
    public enum WeaponSwitch
    {
        Weapon1, Weapon2, Weapon3
    }
}
