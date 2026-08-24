
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static AttackManager;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;
    public AttackManager.WeaponType[] heldWeapons;
    public WeaponData currentWeapon;
    public int weaponIndex = 0;
    public bool switchingWeapons = false;
    public Sprite twinShadowsIcon;
    public Sprite starDaggerIcon;
    public Sprite bloodMaceIcon;
    public Image weaponSlot1;
    public Image weaponSlot2;
    public Image weaponSlot3;
    public List<WeaponData> currentWeapons = new List<WeaponData>();


    public float weaponSwitchCooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        currentWeapon = currentWeapons[0];
        UpdateWeaponUI(WeaponSwitch.Weapon1);
    }
    private Sprite GetWeaponSprite(WeaponType weapon)
    {
        switch (weapon)
        {
            case WeaponType.TwinShadows: return twinShadowsIcon;
            case WeaponType.StarDagger: return starDaggerIcon;
            case WeaponType.BloodMace: return bloodMaceIcon;
            default: return null;
        }
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
                ColorUtility.TryParseHtmlString("#DBE02E", out Color selectedColor1);
                weaponSlot1.color = selectedColor1;
                break;
            case WeaponSwitch.Weapon2:
                currentWeapon = currentWeapons[1];
                ColorUtility.TryParseHtmlString("#DBE02E", out Color selectedColor2);
                weaponSlot2.color = selectedColor2;
                break;
            case WeaponSwitch.Weapon3:
                currentWeapon = currentWeapons[2];
                ColorUtility.TryParseHtmlString("#DBE02E", out Color selectedColor3);
                weaponSlot3.color = selectedColor3;
                break; 
        }
    }

    private void ResetWeaponUIHighlights()
    {
        ColorUtility.TryParseHtmlString("#C68A44", out Color selectedColor1);
        weaponSlot1.color = selectedColor1;
        ColorUtility.TryParseHtmlString("#C68A44", out Color selectedColor2);
        weaponSlot2.color = selectedColor2;
        ColorUtility.TryParseHtmlString("#C68A44", out Color selectedColor3);
        weaponSlot3.color = selectedColor3;
    }
    public enum WeaponSwitch
    {
        Weapon1, Weapon2, Weapon3
    }
}
