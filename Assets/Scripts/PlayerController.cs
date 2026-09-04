using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 250;
    [SerializeField] private float skillPushForward = 3;
    private InputSystem_Actions controls;
    private Rigidbody playerRb;
    private GameObject focusPoint;
    public bool isPushForwardEnabled = false;
    public bool isShieldSkillEnabled = false;
    public bool isKickStarEnabled = false;
    public bool isSlamming = false;
    private GameManager gameManager;

    // Abilities
    private Coroutine kickStar;
    private Coroutine speedBuff;

    void Awake()
    {
        controls = new InputSystem_Actions();
        playerRb = GetComponent<Rigidbody>();
        focusPoint = GameObject.Find("FocusPoint");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void FixedUpdate()
    {
        // Movement
        Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();
        float forwardInput = moveInput.y;
        if (!isSlamming)
        {
            playerRb.AddForce(focusPoint.transform.forward * moveSpeed * Time.deltaTime * forwardInput);
        }

        // Push forward skill
        if (controls.Player.Sprint.triggered && isPushForwardEnabled && !isSlamming)
        {
            playerRb.AddForce(focusPoint.transform.forward * skillPushForward * forwardInput, ForceMode.Impulse);
        }

        // Shield skill
        if (controls.Player.Jump.triggered && isShieldSkillEnabled && !isSlamming)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }
    }

    void Update()
    {
        if (transform.position.y < 0)
        {
            gameManager.GameOver();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isKickStarEnabled && collision.gameObject.CompareTag("Enemy"))
        {
            KickEnemyAway(collision.gameObject);
        }
    }

    void KickEnemyAway(GameObject enemy)
    {
        Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();

        Vector3 awayFromEnemy = enemy.transform.position - transform.position;

        enemyRb.AddForce(awayFromEnemy * 10, ForceMode.Impulse);
    }

    public void StartKickBuff(float time)
    {
        if (kickStar != null)
        {
            StopCoroutine(kickStar);
        }
        kickStar = StartCoroutine(KickBuff(time));
    }
    IEnumerator KickBuff(float time)
    {
        isKickStarEnabled = true;
        transform.Find("Buff1").gameObject.SetActive(true);

        yield return new WaitForSeconds(time);

        isKickStarEnabled = false;
        transform.Find("Buff1").gameObject.SetActive(false);
    }

    public void StartSlamDown(float time, float shockwaveRange, float shockwaveForce)
    {
        StartCoroutine(SlammDown(time, shockwaveRange, shockwaveForce));
    }

    IEnumerator SlammDown(float time, float shockwaveRange, float shockwaveForce)
    {
        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
        playerRb.AddForce(Vector3.up * 20, ForceMode.Impulse);
        isSlamming = true;

        yield return new WaitForSeconds(time);
        playerRb.angularVelocity = Vector3.zero;
        playerRb.AddForce(Vector3.down * 50, ForceMode.Impulse);
        isSlamming = false;


        Collider[] hits = Physics.OverlapSphere(transform.position, shockwaveRange);
        foreach (Collider hit in hits)
        {
            Rigidbody rb = hit.gameObject.GetComponent<Rigidbody>();
            if (rb != null && rb != playerRb)
            {
                rb.AddExplosionForce(
                    shockwaveForce, transform.position, shockwaveRange, 0.5f, ForceMode.Impulse
                );
            }
        }
    }

    public void StartSpeedBuff(float time, float speedIncreaseRate, float weightIncreaseRate)
    {
        if (speedBuff != null)
        {
            StopCoroutine(speedBuff);
        }
        speedBuff = StartCoroutine(SpeedBuff(time, speedIncreaseRate, weightIncreaseRate));
    }

    IEnumerator SpeedBuff(float time, float speedIncreaseRate, float weightIncreaseRate)
    {
        moveSpeed *= speedIncreaseRate;
        playerRb.mass *= weightIncreaseRate;
        transform.Find("Buff2").gameObject.SetActive(true);

        yield return new WaitForSeconds(time);

        moveSpeed /= speedIncreaseRate;
        playerRb.mass /= weightIncreaseRate;
        transform.Find("Buff2").gameObject.SetActive(false);
    }
}
