using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
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
        }
        else
        {
            titleText.text = "???";
            bodyText.text = "Click the node on the skill tree to unlock. " + node.unlockCost + " gold.";
        }
    }
    void RefreshDescription()
    {
        bodyText.text = node.description;
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
