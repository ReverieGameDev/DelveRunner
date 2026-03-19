using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public float health;
    public float speed;
    public float damage;
    public string mobName;
    public bool isREE;
    public bool isBoss;
    public int goldValue;
    public int xpValue;
}
