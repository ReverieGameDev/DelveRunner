using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopManager : MonoBehaviour
{
    public GameObject shopInterface;
    public List<GameObject> buttonHighlight = new List<GameObject>();

    void Start()
    {

    }


    public void CloseShop()
    {
        shopInterface.SetActive(false);
        PlayerMovement.Instance.playerFrozen = false;
    }
    public void Loadout1()
    {
        PlayerPrefs.SetInt("SelectedLoadout", 0);
        ResetColors();
        buttonHighlight[0].GetComponent<Image>().color = Color.yellow;
    }
    public void Loadout2()
    {
        PlayerPrefs.SetInt("SelectedLoadout", 1);
        ResetColors();
        buttonHighlight[1].GetComponent<Image>().color = Color.yellow;
    }
    public void Loadout3()
    {
        PlayerPrefs.SetInt("SelectedLoadout", 2);
        ResetColors();
        buttonHighlight[2].GetComponent<Image>().color = Color.yellow;
    }
    public void Loadout4()
    {
        PlayerPrefs.SetInt("SelectedLoadout", 3);
        ResetColors();
        buttonHighlight[3].GetComponent<Image>().color = Color.yellow;
    }
    
    private void ResetColors()
    {
        foreach(GameObject button in buttonHighlight)
        {
            button.GetComponent<Image>().color = Color.white;
        }
    }
}