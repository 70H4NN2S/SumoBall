using UnityEngine;

public class PickupsController : MonoBehaviour
{
    [SerializeField] private Color playerColor;
    [SerializeField] private Color playerColorFinal;
    [SerializeField] private bool changeColor;
    [SerializeField] private bool isTargetNPC;
    [SerializeField] private bool isTargetPlayer;
    [SerializeField] private string buffName;
    [SerializeField] private float abilityTime;
    public int price;

    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 50);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isTargetNPC && other.gameObject.CompareTag("Enemy"))
        {
            switch (buffName)
            {
                case "Kick":
                    other.gameObject.GetComponent<EnemyController>().StartKickBuff(abilityTime);
                    break;
                case "Speed":
                    other.gameObject.GetComponent<EnemyController>().StartSpeedBuff(abilityTime, 1.5f, 1.5f);
                    break;
            }

            Destroy(gameObject);
        }

        if (isTargetPlayer && other.gameObject.CompareTag("Player"))
        {
            if (changeColor)
            {
                ChangePlayerColor();
            }
            switch (buffName)
            {
                case "Shield":
                    AddPlayerShield();
                    break;
                case "Burst":
                    AddPlayerBurst();
                    break;
                case "Kick":
                    other.gameObject.GetComponent<PlayerController>().StartKickBuff(abilityTime);
                    break;
                case "Shockwave":
                    other.gameObject.GetComponent<PlayerController>().StartSlamDown(abilityTime, 6, 50);
                    break;
                case "Speed":
                    other.gameObject.GetComponent<PlayerController>().StartSpeedBuff(abilityTime, 2.5f, 2);
                    break;
            }
            Destroy(gameObject);
        }
    }

    void ChangePlayerColor()
    {
        Renderer playerRend = GameObject.Find("Player").GetComponent<Renderer>();
        if (playerRend.material.GetColor("_BaseColor") != Color.white)
        {
            playerRend.material.SetColor("_BaseColor", playerColorFinal);
        }
        else
        {
            playerRend.material.SetColor("_BaseColor", playerColor);
        }
    }

    void AddPlayerShield()
    {
        if (GameObject.Find("Player").GetComponent<PlayerController>().isShieldSkillEnabled == false)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().isShieldSkillEnabled = true;
        }
        else
        {
            GameObject.Find("Player").GetComponent<Rigidbody>().mass += 0.5f;
        }
    }

    void AddPlayerBurst()
    {
        if (GameObject.Find("Player").GetComponent<PlayerController>().isPushForwardEnabled == false)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().isPushForwardEnabled = true;
        }
        else
        {
            GameObject.Find("Player").GetComponent<PlayerController>().moveSpeed += 50;
        }
        GameObject.Find("Player").GetComponent<PlayerController>().isPushForwardEnabled = true;
    }
}
