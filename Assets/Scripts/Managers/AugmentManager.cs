using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public List<Button> augmentDisplaySlots;

    [SerializeField] private PlayerCombat playerCombat;

    private Dictionary<AugmentData, int> augmentDictionary = new Dictionary<AugmentData, int>();
    public List<AugmentData> augmentPool = new List<AugmentData>();
    private List<AugmentData> augmentSelection = new List<AugmentData>();

    private List<AugmentData> augmentSlots = new List<AugmentData>();
    private int maxAugmentCount = 3;

    private int currentSlotIndex = 0;

    public void AugmentSelectionStart()
    {
        Time.timeScale = 0;//we stop time for augment selection
        augmentSlots.Clear();// clear the currently held augment slots
        augmentSelection.Clear();// clear the previous tiers pool
        int tierRandomizer = Random.Range(1,4);//picks a tier from tier 1-3
        RandomAugmentGenerator(1);//pick out 3 augments
    }
    public void RandomAugmentGenerator(int tier)
    {
        
        foreach (AugmentData augment in augmentPool)
        {
            if (augment.augmentTier == tier) //check if tier is correct 
            {
                augmentSelection.Add(augment); //we simply add all of the available augments to this list
            }
        }
        augmentSelect.SetActive(true);//turn on the augment select screen.
        for (int i = 0; i < maxAugmentCount; i++)
        {
            int pick = Random.Range(0, augmentSelection.Count);
            augmentSlots.Add(augmentSelection[pick]);
            augmentSelection.RemoveAt(pick);
        }
        AddAugmentToUI();
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

        if (playerCombat.augmentsOwed > 0)
        {
            Time.timeScale = 0;
        }
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
        if (playerCombat.augmentsOwed > 0)
        {
            Time.timeScale = 0;
        }
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
        if (playerCombat.augmentsOwed > 0)
        {
            Time.timeScale = 0;
        }
    }

    public void AddAugmentToUI()
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
}