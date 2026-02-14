using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    // UI Panels
    public GameObject shopUI;
    public GameObject wizardShopUI;
    public GameObject questBoardShopUI;
    public GameObject itemDescription;
    public GameObject abilityDescription;
    public GameObject questDescription;

    // Item Description UI
    public TextMeshProUGUI itemDescriptionName;
    public TextMeshProUGUI itemDescriptionPrice;
    public TextMeshProUGUI itemDescriptionDescription;
    public TextMeshProUGUI itemDescriptionStats;
    public TextMeshProUGUI totalMoney;

    // Ability Description UI
    public TextMeshProUGUI abilityDescriptionName;
    public TextMeshProUGUI abilityDescriptionCost;
    public TextMeshProUGUI abilityDescriptionDescription;
    public TextMeshProUGUI totalMoneyWizardShop;

    // Quest Description UI
    public TextMeshProUGUI questDescriptionName;
    public TextMeshProUGUI questDescriptionReward;
    public TextMeshProUGUI questDescriptionDescription;

    // Shop Data
    public List<ItemData> itemList = new List<ItemData>();
    public List<Button> shopItemButtonList = new List<Button>();
    private List<ItemData> selectedItems = new List<ItemData>();

    // Wizard Shop Data
    public List<AbilityStore> abilityList = new List<AbilityStore>();
    public List<Button> wizardShopAbilityButtonList = new List<Button>();
    private List<AbilityStore> selectedAbilities = new List<AbilityStore>();

    // Quest board Data
    public List<QuestList> questList = new List<QuestList>();
    public List<Button> questBoardButtonList = new List<Button>();
    private List<QuestList> selectedQuests = new List<QuestList>();

    // References
    private PlayerCombat playerCombat;

    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
    }

    // ===== REGULAR SHOP =====

    public void OpenShop()
    {
        shopUI.SetActive(true);
        RandomizeItemSelection();
        PlaceItemsInSlots();
        totalMoney.text = ": " + playerCombat.playerMoney;
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        itemDescription.SetActive(false);
    }

    public void BuyItem1() { BuyItem(0); }
    public void BuyItem2() { BuyItem(1); }
    public void BuyItem3() { BuyItem(2); }
    public void BuyItem4() { BuyItem(3); }
    public void BuyItem5() { BuyItem(4); }
    public void BuyItem6() { BuyItem(5); }

    public void HoverItem1() { ShowItemDescription(0); }
    public void HoverItem2() { ShowItemDescription(1); }
    public void HoverItem3() { ShowItemDescription(2); }
    public void HoverItem4() { ShowItemDescription(3); }
    public void HoverItem5() { ShowItemDescription(4); }
    public void HoverItem6() { ShowItemDescription(5); }

    private void RandomizeItemSelection()
    {
        selectedItems.Clear();
        List<ItemData> tempList = new List<ItemData>(itemList);

        for (int i = 0; i < 6 && tempList.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            selectedItems.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
        }
    }

    private void PlaceItemsInSlots()
    {
        for (int i = 0; i < shopItemButtonList.Count && i < selectedItems.Count; i++)
        {
            shopItemButtonList[i].GetComponent<Image>().sprite = selectedItems[i].itemIcon;
        }
    }

    private void BuyItem(int slotIndex)
    {
        if (slotIndex >= selectedItems.Count) return;

        ItemData item = selectedItems[slotIndex];

        if (playerCombat.playerMoney >= item.itemPrice)
        {
            playerCombat.playerMoney -= item.itemPrice;
            ApplyItemEffect(item.itemEffect);
            totalMoney.text = ": " + playerCombat.playerMoney;
        }
    }

    private void ShowItemDescription(int index)
    {
        if (index >= selectedItems.Count) return;

        itemDescriptionName.text = selectedItems[index].itemName;
        itemDescriptionPrice.text = "$" + selectedItems[index].itemPrice.ToString();
        itemDescriptionDescription.text = selectedItems[index].itemDescription;
        itemDescriptionStats.text = selectedItems[index].itemEffect;
        itemDescription.SetActive(true);
    }

    private void ApplyItemEffect(string effectType)
    {
        switch (effectType)
        {
            case "Dash":
                break;
            case "Heal":
                break;
            case "DamageUp":
                playerCombat.attackDamage += 50f;
                break;
        }
    }

    // ================QUEST BOARD==================
    public void OpenQuestBoard()
    {
        questBoardShopUI.SetActive(true);
        RandomizeQuestSelection();
        PlaceQuestsInSlots();
        totalMoney.text = ": " + playerCombat.playerMoney;
    }
    public void CloseQuestBoard()
    {
        questBoardShopUI.SetActive(false);
        questDescription.SetActive(false);
    }
    private void RandomizeQuestSelection()
    {
        selectedQuests.Clear();
        List<QuestList> tempList = new List<QuestList>(questList);

        for (int i = 0; i < 6 && tempList.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            selectedQuests.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
        }
    }

    private void BuyQuest(int slotIndex)
    {
        if (slotIndex >= selectedQuests.Count) return;

        QuestList ability = selectedQuests[slotIndex];
    }

    public void BuyQuest1() { BuyQuest(0); }
    public void BuyQuest2() { BuyQuest(1); }
    public void BuyQuest3() { BuyQuest(2); }

    public void HoverQuest1() { ShowQuestDescription(0); }
    public void HoverQuest2() { ShowQuestDescription(1); }
    public void HoverQuest3() { ShowQuestDescription(2); }

    private void ShowQuestDescription(int index)
    {
        if (index >= selectedQuests.Count) return;

        questDescriptionName.text = selectedQuests[index].questName;
        questDescriptionReward.text = "Reward: $" + selectedQuests[index].questReward;
        questDescriptionDescription.text = selectedQuests[index].questDescription;
        questDescription.SetActive(true);
    }

    private void PlaceQuestsInSlots()
    {
        for (int i = 0; i < questBoardButtonList.Count && i < selectedQuests.Count; i++)
        {
            questBoardButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text = selectedQuests[i].questName;
        }
    }

    // ===== WIZARD SHOP =====

    public void OpenWizardShop()
    {
        wizardShopUI.SetActive(true);
        RandomizeAbilitySelection();
        PlaceAbilitiesInSlots();
        totalMoneyWizardShop.text = "$: " + playerCombat.playerMoney;
    }

    public void CloseWizardShop()
    {
        wizardShopUI.SetActive(false);
        abilityDescription.SetActive(false);
    }

    public void BuyAbility1() { BuyAbility(0); }
    public void BuyAbility2() { BuyAbility(1); }
    public void BuyAbility3() { BuyAbility(2); }

    public void HoverAbility1() { ShowAbilityDescription(0); }
    public void HoverAbility2() { ShowAbilityDescription(1); }
    public void HoverAbility3() { ShowAbilityDescription(2); }

    private void RandomizeAbilitySelection()
    {
        selectedAbilities.Clear();
        List<AbilityStore> tempList = new List<AbilityStore>(abilityList);

        for (int i = 0; i < 3 && tempList.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            selectedAbilities.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
        }
    }

    private void PlaceAbilitiesInSlots()
    {
        for (int i = 0; i < wizardShopAbilityButtonList.Count && i < selectedAbilities.Count; i++)
        {
            wizardShopAbilityButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text = selectedAbilities[i].abilityName;
        }
    }

    private void BuyAbility(int slotIndex)
    {
        if (slotIndex >= selectedAbilities.Count) return;

        AbilityStore ability = selectedAbilities[slotIndex];

        if (playerCombat.playerMoney >= ability.abilityCost)
        {
            playerCombat.playerMoney -= ability.abilityCost;
            ApplyAbilityEffect(ability.abilityName);
            totalMoneyWizardShop.text = ": " + playerCombat.playerMoney;
        }
    }

    private void ShowAbilityDescription(int index)
    {
        if (index >= selectedAbilities.Count) return;

        abilityDescriptionName.text = selectedAbilities[index].abilityName;
        abilityDescriptionCost.text = "$" + selectedAbilities[index].abilityCost.ToString();
        abilityDescriptionDescription.text = selectedAbilities[index].abilityDescription;
        abilityDescription.SetActive(true);
    }

    private void ApplyAbilityEffect(string abilityName)
    {
        switch (abilityName)
        {
            case "Dash":
                break;
            case "Teleport":
                break;
            case "Repel":
                break;
        }
    }
}