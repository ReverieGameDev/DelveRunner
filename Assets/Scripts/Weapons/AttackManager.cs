using UnityEngine;
using System.Collections;

public class AttackManager : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private PlayerMovement playerMovement;
    public GameObject starDaggerPrefab;
    public GameObject bloodMacePrefab;
    public GameObject twinShadowsPrefab;
    public GameObject shadowEcho;

    public bool isFiring = false;

    private float starDaggerAS = 1f;
    private float bloodMaceAS = 6f;
    private float twinShadowsAS = .75f;

    public Vector3 mousePos;
    public Vector3 playerPos;

    private PlayerStatusEffects playerStatusEffects;

    private WeaponManager weaponManager;
    public enum WeaponType
    {
        TwinShadows,
        StarDagger,
        BloodMace
    }
    void Start()
    {
        playerStatusEffects = FindAnyObjectByType<PlayerStatusEffects>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        weaponManager = FindFirstObjectByType<WeaponManager>();
    }

    private void Update()
    {
        if (playerStatusEffects.isStunned == false && Input.GetMouseButtonDown(0) && isFiring == false && weaponManager.switchingWeapons == false && !playerMovement.playerFrozen)
        {
            playerCombat.GetComponent<Animator>().SetTrigger("Attack");
            isFiring = true;
            mouseAndPlayerPositionsATOF();
            StartCoroutine(weaponManager.currentWeapon.ToString());
        }
    }

    IEnumerator TwinShadows()
    {
        Instantiate(twinShadowsPrefab, playerCombat.transform.position, Quaternion.identity);
        if (shadowEcho.activeInHierarchy) shadowEcho.GetComponent<ShadowEcho>().ShadowAttack(twinShadowsPrefab, "TwinShadows");
        yield return new WaitForSeconds(twinShadowsAS * playerCombat.attackSpeed);
        isFiring = false;
    }

    IEnumerator BloodMace()
    {
        Instantiate(bloodMacePrefab, playerCombat.transform.position, Quaternion.identity);
        if (shadowEcho.activeInHierarchy) shadowEcho.GetComponent<ShadowEcho>().ShadowAttack(bloodMacePrefab, "BloodMace");
        yield return new WaitForSeconds(bloodMaceAS * playerCombat.attackSpeed);
        isFiring = false;
    }

    IEnumerator StarDagger()
    {
        Instantiate(starDaggerPrefab, playerCombat.transform.position, Quaternion.identity);
        if (shadowEcho.activeInHierarchy) shadowEcho.GetComponent<ShadowEcho>().ShadowAttack(starDaggerPrefab, "StarDagger");
        yield return new WaitForSeconds(starDaggerAS * playerCombat.attackSpeed);
        isFiring = false;
    }

    private void mouseAndPlayerPositionsATOF()
    {
        mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        playerPos = transform.position;
    }
}