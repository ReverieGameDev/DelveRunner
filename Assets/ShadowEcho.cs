using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShadowEcho : MonoBehaviour
{
    private Animator anim;
    private AttackManager attackManager;
    private WeaponManager weaponManager;
    private PlayerMovement playerMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        attackManager = FindFirstObjectByType<AttackManager>();
        weaponManager = FindFirstObjectByType<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerMovement.xInput < 0)
        {
            transform.position = new Vector2(playerMovement.transform.position.x + 1f, playerMovement.transform.position.y);
            transform.rotation = new Quaternion(0, 180, 0, 0);
            anim.SetFloat("IsMoving", Mathf.Abs(playerMovement.xInput) + Mathf.Abs(playerMovement.yInput));
        }
        else if (playerMovement.xInput >= 0)
        {
            transform.position = new Vector2(playerMovement.transform.position.x - 1f, playerMovement.transform.position.y);
            transform.rotation = new Quaternion(0, 0, 0, 0);
            anim.SetFloat("IsMoving", Mathf.Abs(playerMovement.xInput) + Mathf.Abs(playerMovement.yInput));
            
        }
        if (Input.GetMouseButtonDown(0) && attackManager.isFiring == false && weaponManager.switchingWeapons == false)
        {
            anim.SetTrigger("Attack");
        }
    }

    public void ShadowAttack(GameObject weaponPrefab, string weaponName)
    {
        StartCoroutine(DelayedAttack(weaponPrefab, weaponName));
    }
    IEnumerator DelayedAttack(GameObject weaponPrefab, string weaponName)
    {
        yield return new WaitForSeconds(0.15f);
        GameObject shadow = Instantiate(weaponPrefab, PlayerCombat.Instance.transform.position, Quaternion.identity);
        shadow.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0f, 1f, 0.6f);
        switch (weaponName)
        {
            case "TwinShadows":
                shadow.GetComponent<TwinShadowsAttack>().damageMultiplier = 0.25f;
                break;
            case "StarDagger":
                shadow.GetComponent<StarDaggerAttack>().damageMultiplier = 0.25f;
                break;
            case "BloodMace":
                shadow.GetComponent<BloodMaceAttack>().damageMultiplier = 0.25f;
                break;
        }
    }
}
