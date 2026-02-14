using UnityEngine;

[CreateAssetMenu(fileName = "QuestList", menuName = "Dialogue/QuestList")]
public class QuestList : ScriptableObject
{
    public string questName;
    public Sprite questSprite;
    public int questReward;
    public string questDescription;

}
