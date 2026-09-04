using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public int price = 1;
    [SerializeField] public bool canCollectBuffs = false;
    [SerializeField] public int intelligenceType = 0; // 0: follows players; 1: collects buffs first; 2: tries to survive 3:2+1
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject[] buffs;
    [SerializeField] private int buffActivated; // 0: none; 1: first; 2: second; 3: 1+2; 4: 3; 5:3+1; 6: 
    private Rigidbody enemyRb;
    private GameObject player;
    private bool isKickStarEnabled = false;

    private Coroutine kickStar;
    private Coroutine speedBuff;

    void Awake()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        // Movement intelligence 1
        Vector3 target = player.transform.position - transform.position;
        Vector3 target1 = target.normalized;
        Vector3 target2 = target.normalized - enemyRb.linearVelocity;
        enemyRb.AddForce(target1 * Time.deltaTime * moveSpeed * 0.75f);
        enemyRb.AddForce(target2 * Time.deltaTime * moveSpeed * 0.25f);
    }

    void Update()
    {
        if (transform.position.y < -1)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player" && isKickStarEnabled)
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            Vector3 awayFromEnemy = collision.gameObject.transform.position - transform.position;

            playerRb.AddForce(awayFromEnemy * 10, ForceMode.Impulse);
        }
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
        enemyRb.mass *= weightIncreaseRate;
        transform.Find("Buff2").gameObject.SetActive(true);

        yield return new WaitForSeconds(time);

        moveSpeed /= speedIncreaseRate;
        enemyRb.mass /= weightIncreaseRate;
        transform.Find("Buff2").gameObject.SetActive(false);
    }
}
