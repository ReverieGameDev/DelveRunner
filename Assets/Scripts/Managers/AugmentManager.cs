using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem.Android;
using UnityEngine.UI;
using static UnityEngine.Rendering.HableCurve;

public class AugmentManager : MonoBehaviour
{
    public Button augment1;
    public Button augment2;
    public Button augment3;

    public Image augment1Icon;
    public Image augment2Icon;
    public Image augment3Icon;

    public TextMeshProUGUI augment1DisplayName;
    public TextMeshProUGUI augment2DisplayName;
    public TextMeshProUGUI augment3DisplayName;

    public TextMeshProUGUI augment1Description;
    public TextMeshProUGUI augment2Description;
    public TextMeshProUGUI augment3Description;

    public TextMeshProUGUI augment1AbilityDetails;
    public TextMeshProUGUI augment2AbilityDetails;
    public TextMeshProUGUI augment3AbilityDetails;

    public GameObject augmentSelect;
    public int augmentTierMaxRoll = 4;
    private int tierRandomizer;

    [SerializeField] private PlayerCombat playerCombat;

    private Dictionary<AugmentData, int> augmentDictionary = new Dictionary<AugmentData, int>();
    public List<AugmentData> augmentPool = new List<AugmentData>();
    private List<AugmentData> augmentSelection = new List<AugmentData>();

    private List<AugmentData> augmentSlots = new List<AugmentData>();
    public List<Button> augmentDisplaySlots;
    public List<TextMeshProUGUI> augmentDisplayLevels;
    private int maxAugmentCount = 3;

    private int currentSlotIndex = 0;

    public void AugmentSelectionStart()
    {
        Time.timeScale = 0;//we stop time for augment selection
        augmentSlots.Clear();// clear the currently held augment slots
        augmentSelection.Clear();// clear the previous tiers pool
        tierRandomizer = Random.Range(1, augmentTierMaxRoll);//picks a tier from tier 1-3
        RandomAugmentGenerator(tierRandomizer);//pick out 3 augments
    }
    public void RandomAugmentGenerator(int tier)
    {
        
        foreach (AugmentData augment in augmentPool)
        {
            if (augment.augmentTier == tier && !augmentDictionary.ContainsKey(augment)) //check if tier is correct 
            {
                if (augment.requiredAugmentWeapon == null)
                {
                    augmentSelection.Add(augment); //we simply add all of the available augments to this list
                }
                if (augment.requiredAugmentWeapon != null)
                {
                    foreach (WeaponData weapon in WeaponManager.Instance.currentWeapons)
                    {
                        if (augment.requiredAugmentWeapon.wName == weapon.wName)
                        {
                            augmentSelection.Add(augment);
                        }
                    }
                }
            }
        }
        augmentSelect.SetActive(true);//turn on the augment select screen.
        for (int i = 0; i < maxAugmentCount; i++)
        {
            int pick = Random.Range(0, augmentSelection.Count);
            augmentSlots.Add(augmentSelection[pick]);
            augmentSelection.RemoveAt(pick);
        }
        List<AugmentData> levelable = augmentDictionary.Where(pair => pair.Value < pair.Key.maxAugmentLevel).Select(pair => pair.Key).ToList();
        if (levelable.Count > 0)
        {
            int RandomPoolReplacement = Random.Range(0,augmentSlots.Count);
            int RandomPick = Random.Range(0, levelable.Count);
            augmentSlots[RandomPoolReplacement] = levelable[RandomPick];
        }
        if (tierRandomizer == 3) augmentTierMaxRoll--;
        DisplayAvailableAugments();
    }

    public void SelectAugment1() 
    {
        Time.timeScale = 1;
        augmentSelect.SetActive(false);

        playerCombat.augmentsOwed--;

        AugmentData picked = augmentSlots[0];

        if (augmentDictionary.ContainsKey(picked))
        {
            augmentDictionary[picked] = augmentDictionary[picked] + 1;
        }
        else
        {
            augmentDictionary[picked] = 1;
        }
        picked.Apply(playerCombat, augmentDictionary[picked]);
        if (playerCombat.augmentsOwed > 0)
        {
            Time.timeScale = 0;
            AugmentSelectionStart();
        }
        else
        {
            Time.timeScale = 1;
        }
            RefreshAvailableAugments();
    }

    public void SelectAugment2()
    {
        Time.timeScale = 1;
        augmentSelect.SetActive(false);

        playerCombat.augmentsOwed--;
        AugmentData picked = augmentSlots[1];

        if (augmentDictionary.ContainsKey(picked))
        {
            augmentDictionary[picked] = augmentDictionary[picked] + 1;
        }
        else
        {
            augmentDictionary[picked] = 1;
        }
        picked.Apply(playerCombat, augmentDictionary[picked]);
        if (playerCombat.augmentsOwed > 0)
        {
            Time.timeScale = 0;
            AugmentSelectionStart();
        }
        else
        {
            Time.timeScale = 1;
        }
        RefreshAvailableAugments();
    }

    public void SelectAugment3()
    {
        Time.timeScale = 1;
        augmentSelect.SetActive(false);

        playerCombat.augmentsOwed--;
        AugmentData picked = augmentSlots[2];

        if (augmentDictionary.ContainsKey(picked))
        {
            augmentDictionary[picked] = augmentDictionary[picked] + 1;
        }
        else
        {
            augmentDictionary[picked] = 1;
            
        }
        picked.Apply(playerCombat, augmentDictionary[picked]);
        if (playerCombat.augmentsOwed > 0)
        {
            Time.timeScale = 0;
            AugmentSelectionStart();
        }
        else
        {
            Time.timeScale = 1;
        }
        RefreshAvailableAugments();
    }

    public void DisplayAvailableAugments()
    {

        augment1Icon.sprite = augmentSlots[0].augmentIcon;
        augment2Icon.sprite = augmentSlots[1].augmentIcon;
        augment3Icon.sprite = augmentSlots[2].augmentIcon;

        augment1DisplayName.text = augmentSlots[0].augmentDescriptionName;
        augment2DisplayName.text = augmentSlots[1].augmentDescriptionName;
        augment3DisplayName.text = augmentSlots[2].augmentDescriptionName;

        augment1Description.text = augmentSlots[0].augmentDescription;
        augment2Description.text = augmentSlots[1].augmentDescription;
        augment3Description.text = augmentSlots[2].augmentDescription;

        augment1AbilityDetails.text = augmentSlots[0].augmentPerLevelDescription;
        augment2AbilityDetails.text = augmentSlots[1].augmentPerLevelDescription;
        augment3AbilityDetails.text = augmentSlots[2].augmentPerLevelDescription;
    }

    public void RefreshAvailableAugments()
    {
        int counter = 0;
        foreach (KeyValuePair<AugmentData, int> pair in augmentDictionary)
        {
            augmentDisplaySlots[counter].GetComponent<Image>().sprite = pair.Key.augmentIcon;
            augmentDisplayLevels[counter].text = (pair.Value.ToString());
            counter++;
        }
    }
}