using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100.0f;
    public float timeDamageReset = 2.0f;
    public float damageEffectIntesity = 5.0f;       //  Intensity of effect when the player damages the enemy
    public float damageEffectDuration = 0.1f;       //  Duration of the effect
    [HideInInspector]
    public bool playerDead;

    float m_CurrentHealth;
    float m_AcumulateTime;

    float m_DamageEffectTimer;
    float intensity;
    SkinnedMeshRenderer m_Mannequin;    //  Mesh of the object
    Color m_Color;                      //  Default color of the object, it's required to make the damage effect

    private void Start()
    {
        m_CurrentHealth = health;
        intensity = damageEffectIntesity;
        m_Mannequin = GetComponentInChildren<SkinnedMeshRenderer>();    //  Get the SkinnedMeshRenderer componet
        m_Color = m_Mannequin.material.color;
    }

    private void Update()
    {
        if (m_AcumulateTime > 0)
        {
            m_AcumulateTime -= Time.deltaTime;
        }
        DamageEffect();
    }

    public void ReceiveDamage(float enemyDamage)
    {
        if (m_AcumulateTime <= 0)
        {
            m_CurrentHealth -= enemyDamage;
            m_AcumulateTime = timeDamageReset;
            m_DamageEffectTimer = damageEffectDuration;
            if (m_CurrentHealth <= 0)
            {
                PlayerDies();
            }
            //Debug.Log("Current Health: " + m_CurrentHealth);
        }
    }

    public void DamageEffect()
    {
        if (m_DamageEffectTimer > 0)
        {
            m_DamageEffectTimer -= Time.deltaTime;
            m_Mannequin.material.color *= intensity;        //  Change the brightness of the mesh material
        }
        else
        {
            m_Mannequin.material.color = m_Color;           //  Restart the color of the mesh material
        }
    }

    private void PlayerDies()
    {
        playerDead = true;
    }
}
