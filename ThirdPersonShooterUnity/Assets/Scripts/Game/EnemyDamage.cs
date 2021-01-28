using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float damage = 30.0f;        //  Enemy Damage

    Transform player;                   //  Position of the player
    PlayerHealth playerHealth;          //  Health of the player
    EnemyHealth enemyHealth;            //  Health of the enemy

    private void Start()
    {
        //  Get the player from the scene
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //  Get the components
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        if (!enemyHealth.isDeath)
        {
            //  Check if the enemy is close to the player
            if (Vector3.Distance(transform.position, player.position) <= 0.6f)
            {
                DamagePlayer();
            }
        }
    }

    public void DamagePlayer()
    {
        //  The player take the enemy damage is the enemy is not dead
        playerHealth.ReceiveDamage(damage);
    }
}
