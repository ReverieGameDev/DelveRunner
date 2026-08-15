using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AugmentManager : MonoBehaviour
{
    public Button augment1;
    public Button augment2;
    public Button augment3;

    public GameObject augment1Icon;
    public GameObject augment2Icon;
    public GameObject augment3Icon;

    public TextMeshProUGUI augment1Text;
    public TextMeshProUGUI augment2Text;
    public TextMeshProUGUI augment3Text;

    public GameObject augmentSelect;
    public List<Button> augmentDisplaySlots;

    [SerializeField] private PlayerCombat playerCombat;

    private Dictionary<AugmentData, int> augmentDictionary = new Dictionary<AugmentData, int>();
    private List<AugmentData> augmentPool = new List<AugmentData>();

    private AugmentData augmentHold1;
    private AugmentData augmentHold2;
    private AugmentData augmentHold3;

    private int currentSlotIndex = 0;

    public void RandomAugmentGenerator()
    {
        augmentSelect.SetActive(true);
    }

    public void SelectAugment1()
    {
        Time.timeScale = 1;
        augmentSelect.SetActive(false);

        playerCombat.augmentsOwed--;

        if (playerCombat.augmentsOwed > 0)
        {
            Time.timeScale = 0;
            RandomAugmentGenerator();
        }
    }

    public void SelectAugment2()
    {
        Time.timeScale = 1;
        augmentSelect.SetActive(false);

        playerCombat.augmentsOwed--;

        if (playerCombat.augmentsOwed > 0)
        {
            Time.timeScale = 0;
            RandomAugmentGenerator();
        }
    }

    public void SelectAugment3()
    {
        Time.timeScale = 1;
        augmentSelect.SetActive(false);

        playerCombat.augmentsOwed--;

        if (playerCombat.augmentsOwed > 0)
        {
            Time.timeScale = 0;
            RandomAugmentGenerator();
        }
    }

    public void AddAugmentToUI(AugmentData augment)
    {
    }
}