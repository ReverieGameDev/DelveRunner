using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AttackManager : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private PlayerMovement playerMovement;
    public GameObject chargeBar;
    public Image chargeBarFill;
    public GameObject starDaggerPrefab;
    public GameObject bloodMacePrefab;
    public GameObject twinShadowsPrefab;
    public GameObject shadowEcho;

    public bool isFiring = false;

    private float starDaggerAS = 1f;
    private float bloodMaceAS = 6f;
    private float twinShadowsAS = .75f;
    private float currentCharge;
    private WeaponData currentWeapon;

    public Vector3 mousePos;
    public Vector3 playerPos;
    private Image chargeBarFillRef;
    

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
        if (currentCharge > .25f)
        {
            if (!chargeBar.activeInHierarchy) { chargeBar.SetActive(true); }
            chargeBarFill.fillAmount = currentCharge/currentWeapon.wChargeTime;
            chargeBarFill.color = new Color32(210, (byte)(255 * (chargeBarFill.fillAmount)), 55, 255);
        }
        if (playerStatusEffects.isStunned == false && Input.GetMouseButton(0) && isFiring == false && weaponManager.switchingWeapons == false && !playerMovement.playerFrozen && weaponManager.currentWeapon != null && weaponManager.currentWeapon.hasChargeAttack)
        {
            currentWeapon = weaponManager.currentWeapon;
            currentCharge += Time.deltaTime;
        }
        if (playerStatusEffects.isStunned == false && Input.GetMouseButtonUp(0) && isFiring == false && weaponManager.switchingWeapons == false && !playerMovement.playerFrozen)
        {
            currentWeapon = weaponManager.currentWeapon;
            if (currentCharge >= currentWeapon.wChargeTime/2)//wont keep it as /2, this will be weapon specific, out of scope for now
            {
                Debug.Log(currentWeapon.wChargeTime + "current charge time");
                playerCombat.GetComponent<Animator>().SetTrigger("Attack");
                isFiring = true;
                mouseAndPlayerPositionsATOF();
                StartCoroutine(FireWeapon(true));
            }
            else
            {
                playerCombat.GetComponent<Animator>().SetTrigger("Attack");
                isFiring = true;
                mouseAndPlayerPositionsATOF();
                StartCoroutine(FireWeapon(false));
            }

        }
    }
    private IEnumerator FireWeapon(bool charged)
    {

        if (!charged) { Instantiate(currentWeapon.wProjectilePrefab, playerCombat.transform.position, Quaternion.identity); }
        if (charged) 
        {
            GameObject go = Instantiate(currentWeapon.wChargeProjectilePrefab, playerCombat.transform.position, Quaternion.identity);
            go.GetComponent<ChargeAttackBase>().Fire(currentCharge, currentWeapon.wChargeTime);
        }
        if (shadowEcho.activeInHierarchy) shadowEcho.GetComponent<ShadowEcho>().ShadowAttack(starDaggerPrefab, "StarDagger"); // ignore this line for now.
        currentCharge = 0;
        //chargeBarFill.fillAmount = 0;
        yield return new WaitForSeconds(currentWeapon.wAS * playerCombat.attackSpeed);
        isFiring = false;
        if (chargeBar.activeInHierarchy) chargeBar.SetActive(false);
    }
    /*IEnumerator StarDagger()
    {
        Instantiate(starDaggerPrefab, playerCombat.transform.position, Quaternion.identity);
        if (shadowEcho.activeInHierarchy) shadowEcho.GetComponent<ShadowEcho>().ShadowAttack(starDaggerPrefab, "StarDagger");
        yield return new WaitForSeconds(starDaggerAS * playerCombat.attackSpeed);
        isFiring = false;
    }*/

    private void mouseAndPlayerPositionsATOF()
    {
        mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        playerPos = transform.position;
    }
}