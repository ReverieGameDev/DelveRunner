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
    public enum BuyResult { Success, CantAfford, Maxed, PrereqNotMet, SiblingLocked }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Refresh(selectedTree);   // assign a default tree in the inspector
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetLevel(string id)
    {
        ownedLevels.TryGetValue(id, out int level);
        return level;
    }

    public void Buy(SoulCoinNode node)
    {
        int level = GetLevel(node.id);
        if (level >= node.maxLevel)
        {
            MessageManager(BuyResult.Maxed, node);
            return;
        }
        if (soulCoins < node.cost[level])
        {
            MessageManager(BuyResult.CantAfford, node);
            return;
        }
        if (!RequirementMet(node)) 
        { 
            MessageManager(BuyResult.PrereqNotMet, node); 
            return;
        }
        foreach (SoulCoinNode siblingNodes in node.siblings)
        {

            if (GetLevel(siblingNodes.id) > 0) 
            {
                MessageManager(BuyResult.SiblingLocked, node);
                return;
            }
           
        }

        soulCoins -= node.cost[level];
        ownedLevels[node.id] = level + 1;
    }

    public bool unlock(SoulCoinNode node)
    {
        Debug.Log("UNLOCK called: " + node.id + " | already? " + unlockedNodes.Contains(node.id));
        if (unlockedNodes.Contains(node.id)) return false;
        if (!RequirementMet(node)) return false;
        if (soulCoins < node.unlockCost) return false;
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
            Debug.Log(i + " slot:" + (slots[i] == null) + " node:" + (tree.nodes[i] == null));
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
        }
    }
}
