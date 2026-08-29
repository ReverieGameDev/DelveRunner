using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopManager : MonoBehaviour
{
    public List<WeaponLoadout> loadouts = new List<WeaponLoadout>();
    public List<Button> loadoutButtons = new List<Button>();
    public List<TextMeshProUGUI> loadoutButtonLabels = new List<TextMeshProUGUI>();

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public GameObject shopInterface;

    void Start()
    {
        for (int i = 0; i < loadoutButtons.Count; i++)
        {
            if (i >= loadouts.Count)
            {
                loadoutButtons[i].gameObject.SetActive(false);
                continue;
            }

            loadoutButtonLabels[i].text = loadouts[i].loadoutName;

            int index = i;
            loadoutButtons[i].onClick.AddListener(() => SelectLoadout(index));
        }
    }

    public void SelectLoadout(int index)
    {
        titleText.text = loadouts[index].loadoutName;
        descriptionText.text = loadouts[index].description;

        WeaponManager.Instance.currentWeapons.Clear();
        foreach (WeaponData weapon in loadouts[index].weapons)
        {
            WeaponManager.Instance.currentWeapons.Add(weapon);
        }
    }

    public void CloseShop()
    {
        shopInterface.SetActive(false);
        PlayerMovement.Instance.playerFrozen = false;
    }
}