using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public enum weaponSlot
    {
        AK = 0,
        Pistol = 1,
        Shotgun = 2
    }
    public FiringWeapon[] weapons;

    public AmmoInfo ammoUIInfo;

    [HideInInspector]
    public bool isShooting;
    [HideInInspector]
    public bool isReloading;
    [HideInInspector]
    public bool isChanging;
    [HideInInspector]
    public bool canShoot;

    PlayerAiming playerAiming;
    Animator m_Animator;
    bool m_IsAK;
    bool m_IsPistol;
    bool m_IsShotgun;
    bool m_ActiveWeapon;

    int indexWeapon = 0;
    
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        playerAiming = GetComponent<PlayerAiming>();
        ammoUIInfo.gameObject.SetActive(false);
    }

    void Update()
    {
        ActiveWeapon();
        Shooting();
        ReloadWeapon();
    }

    void Shooting()
    {
        if (Input.GetButton("Fire"))
        {
            if (m_ActiveWeapon && !isChanging && !isReloading)
            {                
                isShooting = true;
                playerAiming.isAiming = true;
                m_Animator.SetBool("IsAiming", true);
                AmmoInfo();
                if (canShoot)
                {
                    weapons[indexWeapon].StartFiring();
                }
            }
        }
        weapons[indexWeapon].BulletUpdate();
        if (Input.GetButtonUp("Fire"))
        {
            isShooting = false;
            canShoot = false;
        }
    }

    public void AmmoInfo()
    {
        ammoUIInfo.AmmoUIUpdate(weapons[indexWeapon].weaponSlot.ToString(), weapons[indexWeapon].GetCurrentClip(), weapons[indexWeapon].GetCurrentAmmo());
    }

    void ReloadWeapon()
    {
        if (Input.GetButtonDown("Reload"))
        {
            if (!isReloading && m_ActiveWeapon)
            {
                weapons[indexWeapon].ReloadWeapon();
            }
        }
    }

    public void Reloading()
    {
        isReloading = true;
        m_Animator.SetBool("IsAiming", false);
        m_Animator.SetTrigger("IsReload");
    }

    void ActiveWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            indexWeapon = 0;
            WeaponEquipped(weapons[indexWeapon]);
            AmmoInfo();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            indexWeapon = 1;
            WeaponEquipped(weapons[indexWeapon]);
            AmmoInfo();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            indexWeapon = 2;
            WeaponEquipped(weapons[indexWeapon]);
            AmmoInfo();
        }

        m_Animator.SetBool("AK", m_IsAK);
        m_Animator.SetBool("Pistol", m_IsPistol);
        m_Animator.SetBool("Shotgun", m_IsShotgun);
    }

    void ChangingWeapon(bool weaponA, bool weaponB)
    {
        if(weaponA || weaponB)
        {
            isChanging = true;
        }
    }

    void WeaponEquipped(FiringWeapon weapon)
    {
        if (weapon.weaponSlot.Equals(weaponSlot.AK))
        {
            ChangingWeapon(m_IsPistol, m_IsShotgun);
            m_IsPistol = false;
            m_IsShotgun = false;

            if (!m_IsAK)
            {
                m_IsAK = true;
                m_ActiveWeapon = true;
                ammoUIInfo.gameObject.SetActive(true);
            }
            else
            {
                m_IsAK = false;
                m_ActiveWeapon = false;
                ammoUIInfo.gameObject.SetActive(false);
            }
        }
        else if (weapon.weaponSlot.Equals(weaponSlot.Pistol))
        {
            ChangingWeapon(m_IsAK, m_IsShotgun);
            m_IsAK = false;
            m_IsShotgun = false;

            if (!m_IsPistol)
            {
                m_IsPistol = true;
                m_ActiveWeapon = true;
                ammoUIInfo.gameObject.SetActive(true);
            }
            else
            {
                m_IsPistol = false;
                m_ActiveWeapon = false;
                ammoUIInfo.gameObject.SetActive(false);
            }
        }
        else if (weapon.weaponSlot.Equals(weaponSlot.Shotgun))
        {
            ChangingWeapon(m_IsAK, m_IsPistol);
            m_IsAK = false;
            m_IsPistol = false;

            if (!m_IsShotgun)
            {
                m_IsShotgun = true;
                m_ActiveWeapon = true;
                ammoUIInfo.gameObject.SetActive(true);
            }
            else
            {
                m_IsShotgun = false;
                m_ActiveWeapon = false;
                ammoUIInfo.gameObject.SetActive(false);
            }
        }
    }

    public void ShootEvent()
    {
        canShoot = true;
    }
}
