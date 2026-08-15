using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChestBehaviour : MonoBehaviour
{
    private bool isLooted = false;
    private Animator anim;
    [SerializeField] private DropTableData dropTable;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        anim.speed = 0;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E) && !isLooted)
            {
                isLooted = true;
                DropManager.Instance.RollDropTable(dropTable, transform.position);
                OpenChestAnimation();
            }
        }
    }

    private void OpenChestAnimation()
    {
        anim.speed = 1;
    }

    public void DestroyChest()
    {
        Destroy(gameObject);
    }
}
