using System.Collections;
using UnityEngine;
using static AttackManager;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public AttackManager.WeaponType[] heldWeapons;
    public AttackManager.WeaponType currentWeapon;
    public int weaponIndex = 0;
    public bool switchingWeapons = false;
    public Sprite glitchSwordIcon;
    public Sprite blackHoleIcon;
    public Sprite shatterFrostIcon;
    public Image weaponSlot1;
    public Image weaponSlot2;
    public Image weaponSlot3;
    

    public float weaponSwitchCooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        UpdateWeaponUI();
    }
    private Sprite GetWeaponSprite(WeaponType weapon)
    {
        switch (weapon)
        {
            case WeaponType.GlitchSword: return glitchSwordIcon;
            case WeaponType.BlackHole: return blackHoleIcon;
            case WeaponType.ShatterFrost: return shatterFrostIcon;
            default: return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && switchingWeapons == false)
        {
            
            switchingWeapons = true;
            StartCoroutine("SwitchWeapons");
            
        }
    }

    IEnumerator SwitchWeapons()
    {
        if (weaponIndex >= 2)
        {
            weaponIndex = 0;
            currentWeapon = heldWeapons[weaponIndex];
            Debug.Log("Switching weapons" + currentWeapon);
            UpdateWeaponUI();
        }
        else
        {
            weaponIndex++;
            currentWeapon = heldWeapons[weaponIndex];
            Debug.Log("Switching weapons" + currentWeapon);
            UpdateWeaponUI();
        }
        yield return new WaitForSeconds(weaponSwitchCooldown);
        switchingWeapons = false;
    }

    private void UpdateWeaponUI()
    {
        int slot1Index = weaponIndex;
        int slot2Index = (weaponIndex + 1) % 3;
        int slot3Index = (weaponIndex + 2) % 3;

        weaponSlot1.sprite = GetWeaponSprite(heldWeapons[slot1Index]);
        weaponSlot2.sprite = GetWeaponSprite(heldWeapons[slot2Index]);
        weaponSlot3.sprite = GetWeaponSprite(heldWeapons[slot3Index]);
    }
}
