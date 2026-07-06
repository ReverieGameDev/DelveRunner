using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;   // needed for the hover interface
using UnityEngine.UI;

public class SoulCoinTreeButton : MonoBehaviour, IPointerEnterHandler
{
    public SoulCoinNode node;
    private SoulCoinManager soulCoinManager;
    public Image iconImage;
    public TextMeshProUGUI levelDisplay;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bodyText;
    public TextMeshProUGUI abilityDetailsText;
    public TextMeshProUGUI levelCostText;
    public TextMeshProUGUI levelCostTitleText;
    private string costPerLevelText;
    private int counter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        soulCoinManager = FindFirstObjectByType<SoulCoinManager>();
    }
    void Start()
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        if (soulCoinManager.unlockedNodes.Contains(node.id))
        {
            bodyText.text = node.description;
            titleText.text = node.nodeName;
            abilityDetailsText.text = node.abilityDetails;
            levelCostTitleText.text = "Cost to buy per level:";
            costPerLevelText = "";
            counter = 0;
            for (int i = 0; i < node.cost.Count; i++)
            {
                counter++;
                costPerLevelText += node.cost[i];
                if (counter != node.cost.Count)
                {
                    costPerLevelText += " / ";
                }
            }
            levelCostText.text = costPerLevelText;
        }
        else
        {
            titleText.text = "???";
            bodyText.text = "Click the node on the skill tree to unlock.\nCost: " + node.unlockCost + " Soul Coins.";
            abilityDetailsText.text = "";
            levelCostTitleText.text = "";
            levelCostText.text = "";
            
        }
    }
    void RefreshDescription()
    {
        bodyText.text = node.description;
        titleText.text = node.nodeName;
        abilityDetailsText.text = node.abilityDetails;
        levelCostTitleText.text = "Cost to buy per level:";
        costPerLevelText = "";
        counter = 0;
        for (int i = 0; i < node.cost.Count; i++)
        {
            counter++;
            costPerLevelText += node.cost[i];
            if (counter != node.cost.Count)
            {
                costPerLevelText += " / ";
            }
        }
        levelCostText.text = costPerLevelText;
        titleText.text = node.nodeName;
    }
    public void OnClick()
    {
        
        if (!soulCoinManager.unlockedNodes.Contains(node.id))
        {
            
            if (soulCoinManager.unlock(node))
            {
                GetComponent<Image>().color = Color.clear;
                RefreshDescription();
            }
            
        }
        else 
        {
            soulCoinManager.Buy(node);
            levelDisplay.text = soulCoinManager.GetLevel(node.id) + "/" + node.maxLevel;
            RefreshDescription();
        }
    }

    public void SetNode(SoulCoinNode newNode)
    {
        node = newNode;
        if (!soulCoinManager.unlockedNodes.Contains(newNode.id))
        {
            GetComponent<Image>().color = Color.white;
        }
        else
        {
            GetComponent<Image>().color = Color.clear;
        }
        levelDisplay.text = soulCoinManager.GetLevel(newNode.id) + "/" + newNode.maxLevel;
        iconImage.sprite = node.icon;
    }
    // Update is called once per frame
    void Update()
    {
        
    }


}
