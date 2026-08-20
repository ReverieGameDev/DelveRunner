using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SoulCoinManager : MonoBehaviour
{
    public int soulCoins;
    public SoulCoinTree selectedTree;
    public List<SoulCoinNode> allNodes;                              // the 36, dragged in via inspector
    public Dictionary<string, int> ownedLevels = new Dictionary<string, int>();
    public HashSet<string> unlockedNodes = new HashSet<string>();             // id is present = unlocked (NOT refundable)
    public List<GameObject> slots = new List<GameObject>();
    public TextMeshProUGUI flashMessage;
    public GameObject flashMessageBox;
    public CanvasGroup flashGroup;
    public int soulCoinsSpent;
    public GameObject respecWindow;
    public GameObject skillTreeWindow;
    public enum BuyResult { Success, CantAfford, Maxed, PrereqNotMet, SiblingLocked, UnlockBadReqs, UnlockCantAfford, MaxNodes, Respec }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soulCoinsSpent = PlayerPrefs.GetInt("SoulCoinsSpent");
        Refresh(selectedTree);   // assign a default tree in the inspector
        Load();
        Refresh(selectedTree);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenRespecWindow()
    {
        respecWindow.SetActive(true);
    }
    public void CloseRespecWindow()
    {
        respecWindow.SetActive(false);
    }

    public void Respec()
    {
        ownedLevels.Clear();
        soulCoins += soulCoinsSpent;
        soulCoinsSpent = 0;
        Refresh(selectedTree);
        Save();
        respecWindow.SetActive(false);
        MessageManager(BuyResult.Respec,null);
    }

    public int GetLevel(string id)
    {
        ownedLevels.TryGetValue(id, out int level);
        return level;
    }
    public void Save()
    {
        SoulSaveData data = new SoulSaveData();
        data.soulCoins = soulCoins;
        foreach (string unlocked in unlockedNodes)
        {
            data.unlocked.Add(unlocked);
        }
        foreach (var owned in ownedLevels)
        {
            data.ownedIds.Add(owned.Key);
            data.ownedValues.Add(owned.Value);
        }
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SoulSave", json);
    }
    public void ExitMenu()
    {
        skillTreeWindow.SetActive(false);
    }
    public void Load()
    {
        ownedLevels.Clear();
        unlockedNodes.Clear();
        string json = PlayerPrefs.GetString("SoulSave", "");
        if (json == "") return;   // nothing saved, bail

        SoulSaveData data = JsonUtility.FromJson<SoulSaveData>(json);
        soulCoins = data.soulCoins;
        
        foreach (string owned in data.unlocked)
        {
            unlockedNodes.Add(owned);
        }
        for (int i = 0; i < data.ownedIds.Count;i++)
        {
            ownedLevels.Add(data.ownedIds[i], data.ownedValues[i]);
        }
    }
    public bool Buy(SoulCoinNode node)
    {
        int level = GetLevel(node.id);
        if (level >= node.maxLevel)
        {
            MessageManager(BuyResult.Maxed, node);
            return false;
        }
        if (soulCoins < node.cost[level])
        {
            MessageManager(BuyResult.CantAfford, node);
            return false;
        }
        if (!RequirementMet(node)) 
        { 
            MessageManager(BuyResult.PrereqNotMet, node);
            return false;
        }
        if (ownedLevels.Count >= 8 && !ownedLevels.ContainsKey(node.id))
        {
            MessageManager(BuyResult.MaxNodes, node);
            return false;
        }
        foreach (SoulCoinNode siblingNodes in node.siblings)
        {

            if (GetLevel(siblingNodes.id) > 0) 
            {
                MessageManager(BuyResult.SiblingLocked, node);
                return false;
            }
           
        }
        soulCoinsSpent += node.cost[level];
        soulCoins -= node.cost[level];
        ownedLevels[node.id] = level + 1;
        return true;
    }

    public bool unlock(SoulCoinNode node)
    {
        if (unlockedNodes.Contains(node.id)) return false;//this one implies we already own the node and we're trying to buy it again, i'll add an error but it shouldnt show.
        if (!RequirementMet(node)) 
        {
            MessageManager(BuyResult.UnlockBadReqs, node);
            return false; 
        }
        if (soulCoins < node.unlockCost)
        {
            MessageManager(BuyResult.UnlockCantAfford, node);
            return false;
        }
        soulCoins -= node.unlockCost;
        unlockedNodes.Add(node.id);
        return true;
    }

    bool RequirementMet(SoulCoinNode node)
    {
        if (node.requires.Count == 0) return true;              // root, no gate
        foreach (SoulCoinNode req in node.requires)
            if (GetLevel(req.id) == req.maxLevel) return true;  // any one maxed = pass
        return false;
    }

    public void Refresh(SoulCoinTree tree)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].GetComponent<SoulCoinTreeButton>().SetNode(tree.nodes[i]);
        }
    }
    public IEnumerator FlashMessage(string message)
    {
        flashMessageBox.SetActive(true);
        flashMessage.text = message;
        flashGroup.alpha = 1f;
        float t = 1f;
        while (t > 0f)
        {
            flashGroup.alpha = t;
            t -= Time.deltaTime * 0.75f;
            yield return null;
        }
        flashMessageBox.SetActive(false);
    }
    public void MessageManager(BuyResult buyResult, SoulCoinNode node)
    {
        switch (buyResult)
        {
            case BuyResult.CantAfford: StartCoroutine(FlashMessage("Not enough gold.")); break;
            case BuyResult.Maxed: StartCoroutine(FlashMessage(node.nodeName + " is already max level!")); break;
            case BuyResult.PrereqNotMet: StartCoroutine(FlashMessage("Max the previous node before putting points into " + node.nodeName + ".")); break;
            case BuyResult.SiblingLocked: StartCoroutine(FlashMessage("You can only spec down one path — respec to choose " + node.nodeName + ".")); break;
            case BuyResult.UnlockBadReqs: StartCoroutine(FlashMessage ("You must unlock and max the previous node to unlock this node.")); break;
            case BuyResult.UnlockCantAfford: StartCoroutine(FlashMessage("Not enough gold to unlock!")); break;
            case BuyResult.MaxNodes: StartCoroutine(FlashMessage("Max 8 skills, respec if you'd like to change your loadout")); break;
            case BuyResult.Respec: StartCoroutine(FlashMessage("Your Soul Coins have returned and your skills have been reset!")); break;
        }
    }
}
