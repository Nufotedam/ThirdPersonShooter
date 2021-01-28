using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100.0f;                   //  Maximum health
    public float armor = 0f;                        //  Armor if the enemy is more dificult to kill
    public float damageEffectIntesity = 5.0f;       //  Intensity of effect when the player damages the enemy
    public float damageEffectDuration = 0.1f;       //  Duration of the effect
    public float forceDie = 5.0f;                   //  Force applies to the ragdoll when the enemy is dead

    [HideInInspector]
    public bool isDeath = false;    //  Control if the enemy is dead

    Ragdoll m_Ragdoll;              //  Ragdoll component
    float currentHealth;            //  Current health of the enemy

    float m_DamageEffectTimer;
    float intensity;
    SkinnedMeshRenderer m_Mannequin;    //  Mesh of the object
    Color m_Color;                      //  Default color of the object, it's required to make the damage effect

    EnemyMovement m_EnemyMovement;      //  Movement  component of the enemy

    private void Start()
    {
        //  Get components
        m_Ragdoll = GetComponent<Ragdoll>();
        m_Mannequin = GetComponentInChildren<SkinnedMeshRenderer>();
        m_EnemyMovement = GetComponent<EnemyMovement>();
        //  Initialize variables
        currentHealth = health;
        intensity = damageEffectIntesity;
        m_Color = m_Mannequin.material.color;

        //  Get all the Rigibodies of the object
        var rigibodies = GetComponentsInChildren<Rigidbody>();
        foreach (var body in rigibodies)
        {
            // The hit damage component is assigned to each Rigibody in the bones.
            HitDamage hitDamage = body.gameObject.AddComponent<HitDamage>();
            //  Assign the health component to the Hit component
            hitDamage.health = this;
        }
    }

    private void Update()
    {
        //  If the enemy get damage apply the damage effect
        if (m_DamageEffectTimer > 0)
        {
            m_DamageEffectTimer -= Time.deltaTime;
            //  Change the brightness of the mesh material
            m_Mannequin.material.color *= intensity;
        }
        else
        {
            //  Restart the color of the mesh material
            m_Mannequin.material.color = m_Color;
        }
    }

    public void TakeDamage(float damage, Vector3 direction)
    {
        //  Apply the damage to the health of the enemy
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Death(direction);
        }
        m_DamageEffectTimer = damageEffectDuration;

        //  When the player shoots to the enemy, the enemy will chase him
        if (!m_EnemyMovement.playerInRange)
            m_EnemyMovement.playerInRange = true;
    }

    void Death(Vector3 forceDirection)
    {
        //  Go to ragdoll state when the enemy dies
        isDeath = true;
        m_Ragdoll.ActivateRagdoll();
        forceDirection.y = 1;
        m_Ragdoll.ApplyForce(forceDirection * forceDie);
    }
}
