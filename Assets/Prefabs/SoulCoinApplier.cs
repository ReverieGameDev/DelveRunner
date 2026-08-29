using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoulCoinApplier : MonoBehaviour
{
    public List<SoulCoinNode> allNodes = new List<SoulCoinNode>();
    public List<SoulCoinNode> ownedNodes = new List<SoulCoinNode>();
    public List<Image> soulCoinSlots = new List<Image>();
    public List<TextMeshProUGUI> soulCoinLevelSlots = new List<TextMeshProUGUI>();
    private List<int> soulCoinLevels = new List<int>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ReadSoulCoinData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ReadSoulCoinData()
    {
        string json = PlayerPrefs.GetString("SoulSave", "");
        PlayerCombat.Instance.ResetStatBonuses();
        if (json == "") return;
        SoulSaveData data = JsonUtility.FromJson<SoulSaveData>(json);
        PlayerCombat.Instance.soulCoins = data.soulCoins;

        for (int i = 0; i< allNodes.Count; i++)
        {
            for (int x = 0; x < data.ownedIds.Count; x++)
            {
                if (allNodes[i].id == data.ownedIds[x])
                {
                    allNodes[i].Apply(PlayerCombat.Instance, data.ownedValues[x]);
                    ownedNodes.Add(allNodes[i]);
                    soulCoinLevels.Add(data.ownedValues[x]);
                }
            }
        }
        for (int i = 0;i < ownedNodes.Count; i++)
        {
            TooltipTrigger tooltip = soulCoinSlots[i].GetComponent<TooltipTrigger>();
            soulCoinLevelSlots[i].text = soulCoinLevels[i].ToString();
            soulCoinSlots[i].sprite = ownedNodes[i].icon;
            tooltip.title = ownedNodes[i].nodeName;
            tooltip.body = ownedNodes[i].description;
            tooltip.secondary = ownedNodes[i].abilityDetails;
        }
    }
}
