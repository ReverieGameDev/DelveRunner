using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class AttackManager : MonoBehaviour
{
    private PlayerCombat playerCombat;
    public GameObject blackHolePrefab;
    public GameObject glitchSwordPrefab;
    public GameObject glitchSwordEchoPrefab;
    public GameObject chakramPrefab;
    public GameObject shatterFrostPrefab;
    public GameObject fantasiaPrefab;

    private bool isEchoSwordActive = true;
    private bool isBlackHoleActive = true;
    private bool isChakramActive = true;
    private bool isShatterFrostActive = true;
    private bool isFantasiaActive = true;

    private bool isFiring = false;

    private float glitchSwordAS = .75f;
    private float blackHoleAS = 4f;
    private float chakramAS = 2f;
    private float shatterFrostAS = 2.5f;
    private float fantasiaAS = 10f;

    public Vector3 mousePos;
    public Vector3 playerPos;


    private WeaponManager weaponManager;
    public enum WeaponType
    {
        GlitchSword,
        BlackHole,
        ShatterFrost
    }
    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        weaponManager = FindFirstObjectByType<WeaponManager>();
        //if (isEchoSwordActive) StartCoroutine(GlitchSwordLoop());
        //if (isBlackHoleActive) StartCoroutine(BlackHoleLoop());
        //if (isChakramActive) StartCoroutine(ChakramLoop());
        //if (isShatterFrostActive) StartCoroutine(ShatterFrostLoop());
        //if (isFantasiaActive) StartCoroutine(FantasiaLoop());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isFiring == false && weaponManager.switchingWeapons == false)
        {
            playerCombat.GetComponent<Animator>().SetTrigger("Attack");
            isFiring = true;
            mouseAndPlayerPositionsATOF();//gets mouse and player position at the time the player fires
            Debug.Log("Firing: " + weaponManager.currentWeapon.ToString());
            StartCoroutine(weaponManager.currentWeapon.ToString());
        }
        else if (Input.GetMouseButtonDown(0) && isFiring == false && weaponManager.switchingWeapons == true)
        {
            Debug.Log("you are switching weapons, can't fire rn");
        }
        else if (Input.GetMouseButtonDown(0) && isFiring == true && weaponManager.switchingWeapons == false)
        {
            Debug.Log("Weapon fired too recently, it's on cooldown");
        }


    }
    IEnumerator GlitchSword()
    {
        Instantiate(glitchSwordPrefab, playerCombat.transform.position, Quaternion.identity);
        Instantiate(glitchSwordEchoPrefab, playerCombat.transform.position, Quaternion.identity);
        
        yield return new WaitForSeconds(glitchSwordAS);
        isFiring = false;
    }

    IEnumerator BlackHole()
    {
            
        GameObject spawnedBlackHole = Instantiate(blackHolePrefab, playerCombat.transform.position, Quaternion.identity);
        Destroy(spawnedBlackHole, 7f);
        yield return new WaitForSeconds(blackHoleAS);
        isFiring = false;
    }

    IEnumerator ChakramLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(chakramAS);
            if (playerCombat.closestCurrentEnemy != null)
            {
                Instantiate(chakramPrefab, playerCombat.transform.position, Quaternion.identity);
            }
        }
    }

    IEnumerator ShatterFrost()
    {
        Instantiate(shatterFrostPrefab, new Vector3(playerCombat.transform.position.x, playerCombat.transform.position.y - 0.5f, playerCombat.transform.position.z), Quaternion.identity);
        yield return new WaitForSeconds(shatterFrostAS);
        isFiring = false;
    }

    IEnumerator FantasiaLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(fantasiaAS);
            if (playerCombat.closestCurrentEnemy != null)
            {
                Instantiate(fantasiaPrefab, new Vector3(playerCombat.transform.position.x, playerCombat.transform.position.y - 0.5f, playerCombat.transform.position.z), Quaternion.identity);
            }
        }
    }

    private void mouseAndPlayerPositionsATOF() // AT TIME OF FIRING
    {
        mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        playerPos = transform.position;
    }
}