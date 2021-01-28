using UnityEngine;
using Cinemachine;
using UnityEngine.Animations.Rigging;

public class PlayerAiming : MonoBehaviour
{
    public float aimDuration = 0.4f;            //  How fast the player can aim
    public float turnSpeed = 30;                //  How fast the player can rotate with the camera

    //  Aim Rigs layer of each weapon (Animation rigging package)
    public Rig AKAimLayer;
    public Rig AKHandsLayer;
    public Rig ShotgunAimLayer;
    public Rig ShotgunHandsLayer;
    public Rig PistolAimLayer;
    public Rig PistolHandsLayer;

    public Transform cameraLookAt;              //  Where the camera will be looking at
    public CinemachineVirtualCamera aimCamera;  //  Virtual camera to make some aiming effects
    public AxisState xAxis;                     //  Get the x-axis movement of the input mouse movement
    public AxisState yAxis;                     //  Get the y-axis movement of the input mouse movement

    [HideInInspector]
    public bool isAiming;           //  Control if the player is aiming

    Camera m_MainCamera;            //  Current Main camera
    Animator m_Animator;            //  Animator component
    PlayerWeapon m_Weapon;          //  Player weapon component (this contol the interations of the player and the weapon)
    float m_ShoulderOffset = 0;     //  Shoulder aim effect
    bool cameraOffset;

    private void Start()
    {
        //  Get the components
        m_MainCamera = Camera.main;
        m_Animator = GetComponent<Animator>();
        m_Weapon = GetComponent<PlayerWeapon>();
    }

    private void FixedUpdate()
    {
        //  Every grame get the mouse x and y input
        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);
        //  Rotates the camera with the input
        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

        //  Get the y-axis of the camera and rotate the player
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
            //  If the player is aiming, then do the animation aiming and apply the active weapon aim pose
            m_Animator.SetBool("IsAiming", true);
            AimingPosesRigging(AKAimLayer, AKHandsLayer, m_Animator.GetBool("AK"));
            AimingPosesRigging(PistolAimLayer, PistolHandsLayer, m_Animator.GetBool("Pistol"));
            AimingPosesRigging(ShotgunAimLayer, ShotgunHandsLayer, m_Animator.GetBool("Shotgun"));
        }
        else
        {
            //  Disable the aim pose animation
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
            //  If the player is aiming, then apply the shoulder offset effect
            if (m_ShoulderOffset < 1.0)
            {
                m_ShoulderOffset += Time.deltaTime * 3;
                //  Move the camera in the z-axis to get closer to the player view
                aimCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset.z = m_ShoulderOffset;
            }
            else
            {
                m_ShoulderOffset = 1.0f;
            }
        }
        else
        {
            //  Disable the shoulder offset effect
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
            //  If the player has a weapon, then make the aim pose using the rig layer of every weapon
            if (!m_Weapon.isReloading)
            {
                //  Increment the weight of the layers
                weiponHandsLayer.weight = 1;
                weiponAimLayer.weight += Time.deltaTime / aimDuration;
                weiponHandsLayer.transform.GetChild(1).GetComponent<TwoBoneIKConstraint>().weight += Time.deltaTime * 8;
            }
            else
            {
                //  If the player is reloading, then decrement the left arm pose ans the aim pose to be able to do the reload animation
                weiponAimLayer.weight -= Time.deltaTime / aimDuration;
                weiponHandsLayer.transform.GetChild(1).GetComponent<TwoBoneIKConstraint>().weight -= Time.deltaTime / aimDuration;
            }
        }
        else
        {
            //  Decrement the weight of the layers
            weiponAimLayer.weight -= Time.deltaTime / aimDuration;
            weiponHandsLayer.weight -= Time.deltaTime / aimDuration;
        }
    }
}
