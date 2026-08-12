using UnityEngine;

public class DropOnDeath : MonoBehaviour
{
    [SerializeField] private DropTableData dropTable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DropItems(Vector2 positionAtDeath)
    {
        Debug.Log("Drop Items fired");
        DropManager.Instance.RollDropTable(dropTable, positionAtDeath);
    }
}
