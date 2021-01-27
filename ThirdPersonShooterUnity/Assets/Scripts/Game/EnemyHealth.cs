using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100.0f;                   //  Maximum health
    public float armor = 0f;                        //  Armor if the enemy is more dificult to kill
    public float damageEffectIntesity = 5.0f;       //  Intensity of effect when the player damages the enemy
    public float damageEffectDuration = 0.1f;       //  Duration of the effect
    public float forceDie = 5.0f;                   //  Force applies to the ragdoll when the enemy is dead

    [HideInInspector]
    public bool isDeath = false;

    Ragdoll m_Ragdoll;              //  Ragdoll component
    float currentHealth;            //  Current health of the enemy

    float m_DamageEffectTimer;
    float intensity;
    SkinnedMeshRenderer m_Mannequin;    //  Mesh of the object
    Color m_Color;                      //  Default color of the object, it's required to make the damage effect

    EnemyMovement m_EnemyMovement;

    private void Start()
    {
        m_Ragdoll = GetComponent<Ragdoll>();                            //  Get the Ragdoll component
        m_Mannequin = GetComponentInChildren<SkinnedMeshRenderer>();    //  Get the SkinnedMeshRenderer componet
        m_EnemyMovement = GetComponent<EnemyMovement>();
        //  Initialize variables
        currentHealth = health;
        intensity = damageEffectIntesity;
        m_Color = m_Mannequin.material.color;

        var rigibodies = GetComponentsInChildren<Rigidbody>();          //  Get all the Rigibodies of the object
        foreach(var body in rigibodies)
        {
            // The hit damage component is assigned to each Rigibody in the bones.
            HitDamage hitDamage = body.gameObject.AddComponent<HitDamage>();
            hitDamage.health = this;        //  Assign the health component to the Hit component
        }
    }

    private void Update()
    {
        //  If the enemy get damage apply the damage effect
        if(m_DamageEffectTimer > 0)
        {
            m_DamageEffectTimer -= Time.deltaTime;
            m_Mannequin.material.color *= intensity;        //  Change the brightness of the mesh material
        }
        else
        {
            m_Mannequin.material.color = m_Color;           //  Restart the color of the mesh material
        }
    }

    public void TakeDamage(float damage, Vector3 direction)
    {
        /*
         *  Apply the damage to the health of the enemy
         * */
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Death(direction);
        }
        m_DamageEffectTimer = damageEffectDuration;

        if (!m_EnemyMovement.playerInRange)
            m_EnemyMovement.playerInRange = true;
    }

    void Death(Vector3 forceDirection)
    {
        /*  
         *  Go to ragdoll state when the enemy dies
         * */
        isDeath = true;
        m_Ragdoll.ActivateRagdoll();
        forceDirection.y = 1;
        m_Ragdoll.ApplyForce(forceDirection * forceDie);
    }
}
