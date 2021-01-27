using UnityEngine;
using Cinemachine;
using UnityEngine.Animations.Rigging;

public class PlayerAiming : MonoBehaviour
{
    public float aimDuration = 0.4f;
    public float turnSpeed = 30;

    public Rig AKAimLayer;
    public Rig AKHandsLayer;
    public Rig ShotgunAimLayer;
    public Rig ShotgunHandsLayer;
    public Rig PistolAimLayer;
    public Rig PistolHandsLayer;
    public Transform cameraLookAt;
    public CinemachineVirtualCamera aimCamera;
    public AxisState xAxis;
    public AxisState yAxis;

    public bool isAiming;

    Camera m_MainCamera;
    Animator m_Animator;
    PlayerWeapon m_Weapon;
    float m_ShoulderOffset = 0;
    bool cameraOffset;

    private void Start()
    {
        m_MainCamera = Camera.main;

        m_Animator = GetComponent<Animator>();
        m_Weapon = GetComponent<PlayerWeapon>();
    }

    private void FixedUpdate()
    {
        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);
        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

        float axisCamera = m_MainCamera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, axisCamera, 0), turnSpeed * Time.deltaTime);
    }

    private void Update()
    {
        if (Input.GetButton("Aim"))
        {
            isAiming = true;
            cameraOffset = true;
        }
        else
        {
            if (!m_Weapon.isShooting)
            {
                isAiming = false;
            }
            cameraOffset = false;
        }
        PlayerAim();
        ShoulderOffsetCamera();
    }

    private void PlayerAim()
    {
        if (isAiming)
        {
            m_Animator.SetBool("IsAiming", true);
            AimingPosesRigging(AKAimLayer, AKHandsLayer, m_Animator.GetBool("AK"));
            AimingPosesRigging(PistolAimLayer, PistolHandsLayer, m_Animator.GetBool("Pistol"));
            AimingPosesRigging(ShotgunAimLayer, ShotgunHandsLayer, m_Animator.GetBool("Shotgun"));

            
            
        }
        else
        {
            m_Animator.SetBool("IsAiming", false);
            AimingPosesRigging(AKAimLayer, AKHandsLayer, false);
            AimingPosesRigging(PistolAimLayer, PistolHandsLayer, false);
            AimingPosesRigging(ShotgunAimLayer, ShotgunHandsLayer, false);
            
        }
    }

    private void ShoulderOffsetCamera()
    {
        if (cameraOffset)
        {
            if (m_ShoulderOffset < 1.0)
            {
                m_ShoulderOffset += Time.deltaTime * 3;
                aimCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset.z = m_ShoulderOffset;
            }
            else
            {
                m_ShoulderOffset = 1.0f;
            }
        }
        else
        {
            if (m_ShoulderOffset > 0)
            {
                m_ShoulderOffset -= Time.deltaTime * 3;
                aimCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset.z = m_ShoulderOffset;
            }
            else
            {
                m_ShoulderOffset = 0f;
            }
        }
    }

    private void AimingPosesRigging(Rig weiponAimLayer, Rig weiponHandsLayer, bool activeWeapon)
    {
        if (activeWeapon && !m_Weapon.isChanging)
        {
            if (!m_Weapon.isReloading)
            {
                weiponHandsLayer.weight = 1;
                weiponAimLayer.weight += Time.deltaTime / aimDuration;
                weiponHandsLayer.transform.GetChild(1).GetComponent<TwoBoneIKConstraint>().weight += Time.deltaTime * 8;
            }
            else
            {
                weiponAimLayer.weight -= Time.deltaTime / aimDuration;
                weiponHandsLayer.transform.GetChild(1).GetComponent<TwoBoneIKConstraint>().weight -= Time.deltaTime / aimDuration;
            }
        }
        else
        {
            weiponAimLayer.weight -= Time.deltaTime / aimDuration;
            weiponHandsLayer.weight -= Time.deltaTime / aimDuration;
        }
    }
}
