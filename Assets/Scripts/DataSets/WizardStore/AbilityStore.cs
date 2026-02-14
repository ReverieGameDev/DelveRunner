using UnityEngine;

[CreateAssetMenu(fileName = "AbilityStore", menuName = "Dialogue/AbilityStore")]
public class AbilityStore : ScriptableObject
{
    public string abilityName;
    public Sprite abilitySprite;
    public int abilityCost;
    public string abilityDescription;
    
}
