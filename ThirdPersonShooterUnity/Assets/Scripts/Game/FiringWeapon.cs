using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringWeapon : MonoBehaviour
{
    public PlayerWeapon.weaponSlot weaponSlot;

    public float fireRate = 10.0f;
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f;
    public float weaponDamage = 10.0f;
    public int ammo = 10;
    public int maximumAmmo = 100;
    public float bulletForce = 5.0f;
    public Transform raycastOrigin;
    public Transform raycastDestination;
    public TrailRenderer bulletEffect;
    public ParticleSystem muzzleEffect;
    public ParticleSystem bulletHitEffect;
    [HideInInspector]
    public bool isFiring = false;

    Ray m_Ray;
    RaycastHit m_HitInfo;
    float m_AccumulatedTime = 1.0f;
    List<Bullet> bullets = new List<Bullet>();
    float m_MaxBulletLifeTime = 3.0f;

    RecoilWeapon recoil;
    PlayerWeapon playerWeapon;

    int m_ClipAmmo;
    int m_CurrentAmmo;

    class Bullet
    {
        public float time;
        public Vector3 inicialPosition;
        public Vector3 inicialVelocity;
        public TrailRenderer traicerBullet;
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.inicialPosition = position;
        bullet.inicialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.traicerBullet = Instantiate(bulletEffect, position, Quaternion.identity);
        bullet.traicerBullet.AddPosition(position);
        return bullet;
    }

    private void Awake()
    {
        recoil = GetComponent<RecoilWeapon>();
        playerWeapon = GetComponentInParent<PlayerWeapon>();
        m_ClipAmmo = ammo;
        m_CurrentAmmo = maximumAmmo;
    }

    private void Update()
    {
        if (isFiring)
        {
            m_AccumulatedTime += Time.deltaTime;
        }
    }

    public void StartFiring()
    {
        isFiring = true;
        //Firing();
        FiringUpdate();
        //BulletUpdate();
    }

    private void WeaponFiring()
    {
        if(m_ClipAmmo <= 0)
        {
            ReloadWeapon();
        }
        else
        {
            muzzleEffect.Emit(1);
            recoil.GenerateRecoil();

            Vector3 fireVelocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
            var bullet = CreateBullet(raycastOrigin.position, fireVelocity);
            bullets.Add(bullet);

            m_ClipAmmo--;
        }
    }

    public void BulletUpdate()
    {
        BulletSimulation();
        DestroyBullet();
    }

    void BulletSimulation()
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPositionBullet(bullet);
            bullet.time += Time.deltaTime;
            Vector3 p1 = GetPositionBullet(bullet);
            RaycastBulletSegment(p0, p1, bullet);
        });
    }

    void DestroyBullet()
    {
        bullets.RemoveAll(bullet => bullet.time >= m_MaxBulletLifeTime);
    }

    void RaycastBulletSegment(Vector3 origin, Vector3 end, Bullet bullet)
    {
        m_Ray.origin = origin;
        m_Ray.direction = end - origin;
        float distance = Vector3.Distance(end, origin);

        if (Physics.Raycast(m_Ray, out m_HitInfo, distance))
        {
            //Debug.Log(m_HitInfo.transform.name);
            //Debug.DrawLine(m_Ray.origin, m_HitInfo.point, Color.red, 3.0f);
            Rigidbody body = m_HitInfo.collider.GetComponent<Rigidbody>();
            if (body)
            {
                body.AddForceAtPosition(m_Ray.direction * bulletForce, m_HitInfo.point, ForceMode.Impulse);
            }

            var enemyBody = m_HitInfo.collider.GetComponent<HitDamage>();
            if (enemyBody)
            {
                //enemyBody.AddForceAtPosition(m_Ray.direction * bulletForce, m_HitInfo.point, ForceMode.Impulse);
                enemyBody.HitRaycast(this, m_Ray.direction);
            }

            bulletHitEffect.transform.position = m_HitInfo.point;
            bulletHitEffect.transform.forward = m_HitInfo.normal;
            bulletHitEffect.Emit(1);

            bullet.traicerBullet.transform.position = m_HitInfo.point;
            bullet.time = m_MaxBulletLifeTime;
        }
        else
        {
            bullet.traicerBullet.transform.position = end;
        }
    }

    public void FiringUpdate()
    {
        if(m_AccumulatedTime >= 1 / fireRate)
        {
            WeaponFiring();
            m_AccumulatedTime = 0;
            StopFiring();
        }
    }

    public void StopFiring()
    {
        isFiring = false;
    }

    Vector3 GetPositionBullet(Bullet bullet)
    {
        // p*v*t + 0.5*g*t*t
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.inicialPosition) + (bullet.inicialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    public void ReloadWeapon()
    {
        if(m_ClipAmmo != ammo && m_CurrentAmmo > 0)
        {
            int bullets = m_ClipAmmo;
            playerWeapon.Reloading();
            if(m_CurrentAmmo >= ammo)
            {
                m_ClipAmmo = ammo;
                m_CurrentAmmo = m_CurrentAmmo - (ammo - bullets);
            }
            else
            {                
                m_ClipAmmo = m_CurrentAmmo;
                m_CurrentAmmo = 0;
            }
        }
    }

    public int GetCurrentClip()
    {
        return m_ClipAmmo;
    }

    public int GetCurrentAmmo()
    {
        return m_CurrentAmmo;
    }
}
