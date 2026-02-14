using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor.SceneManagement;

public class AugmentManager : MonoBehaviour
{
    public Sprite AttackSpeed;
    public Sprite Attack;
    public Sprite Armor;
    public Sprite MagicResist;
    public Sprite MovementSpeed;
    public Sprite Size;

    public Button augment1;
    public Button augment2;
    public Button augment3;
    public TextMeshProUGUI augment1Text;
    public TextMeshProUGUI augment2Text;
    public TextMeshProUGUI augment3Text;
    private Dictionary<string, int> augmentDictionary = new Dictionary<string, int>();
    private List<string> augmentNames = new List<string>();
    public GameObject augmentSelect;
    private string augmentHold1;
    private string augmentHold2;
    private string augmentHold3;
    private PlayerCombat playerCombat;
    public List<Button> augmentDisplaySlots;
    private int currentSlotIndex = 0;

    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        augmentDictionary.Add("AttackSpeed", 0);
        augmentDictionary.Add("Attack", 0);
        augmentDictionary.Add("Armor", 0);
        augmentDictionary.Add("MagicResist", 0);
        augmentDictionary.Add("MovementSpeed", 0);
        augmentDictionary.Add("Size", 0);

        augmentNames = augmentDictionary.Keys.ToList();
    }

    void Update()
    {

    }

    public void RandomAugmentGenerator()
    {
        augmentSelect.SetActive(true);

        augmentHold1 = augmentNames[Random.Range(0, augmentNames.Count)];
        augment1Text.text = augmentHold1;
        augmentNames.Remove(augmentHold1);
        augmentHold2 = augmentNames[Random.Range(0, augmentNames.Count)];
        augment2Text.text = augmentHold2;
        augmentNames.Remove(augmentHold2);
        augmentHold3 = augmentNames[Random.Range(0, augmentNames.Count)];
        augment3Text.text = augmentHold3;
        augmentNames.Remove(augmentHold3);

    }

    public void SelectAugment1()
    {
        string selectedAugment = augment1Text.text;
        augmentDictionary[selectedAugment]++;
        Time.timeScale = 1;
        augmentSelect.SetActive(false);
        ReaddAugmentsToList(augmentHold1, augmentHold2, augmentHold3);
        playerCombat.ApplyAugment(selectedAugment);
        AddAugmentToUI(selectedAugment);
    }

    public void SelectAugment2()
    {
        string selectedAugment = augment2Text.text;
        augmentDictionary[selectedAugment]++;
        Time.timeScale = 1;
        augmentSelect.SetActive(false);
        ReaddAugmentsToList(augmentHold1, augmentHold2, augmentHold3);
        playerCombat.ApplyAugment(selectedAugment);
        AddAugmentToUI(selectedAugment);
    }

    public void SelectAugment3()
    {
        string selectedAugment = augment3Text.text;
        augmentDictionary[selectedAugment]++;
        Time.timeScale = 1;
        augmentSelect.SetActive(false);
        ReaddAugmentsToList(augmentHold1, augmentHold2, augmentHold3);
        playerCombat.ApplyAugment(selectedAugment);
        AddAugmentToUI(selectedAugment);
    }

    public void ReaddAugmentsToList(string augmentHold1, string augmentHold2, string augmentHold3)
    {
        augmentNames.Add(augmentHold1);
        augmentNames.Add(augmentHold2);
        augmentNames.Add(augmentHold3);
    }

    private Sprite GetAugmentSprite(string augmentName)
    {
        switch (augmentName)
        {
            case "AttackSpeed": return AttackSpeed;
            case "Attack": return Attack;
            case "Armor": return Armor;
            case "MagicResist": return MagicResist;
            case "MovementSpeed": return MovementSpeed;
            case "Size": return Size;
            default: return null;
        }
    }

    public void AddAugmentToUI(string augmentName)
    {
        if (currentSlotIndex < augmentDisplaySlots.Count)
        {
            augmentDisplaySlots[currentSlotIndex].GetComponent<Image>().sprite = GetAugmentSprite(augmentName);
            currentSlotIndex++;
        }
    }
}