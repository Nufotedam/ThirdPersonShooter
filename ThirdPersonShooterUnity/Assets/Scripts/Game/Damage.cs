using UnityEngine;

public class Damage : MonoBehaviour
{
    public float damage = 30.0f;

    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, player.position) <= 0.6f)
        {
            DamagePlayer();
        }
    }

    public void DamagePlayer()
    {
        if(!enemyHealth.isDeath)
            playerHealth.ReceiveDamage(damage);
    }
}
