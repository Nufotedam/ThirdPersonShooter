using UnityEngine;
using Cinemachine;

public class RecoilWeapon : MonoBehaviour
{
    public CinemachineImpulseSource cameraShake;        //  Camera shake effect

    public PlayerAiming playerAimingCamera;             //  Player aim component  
    public float verticalRecoil;                        //  How strong is the vertical
    public float horizontalRecoil;                      //  How strong is the horizontal
    public float recoilDuration;                        //  How fast the camera will move following the recoil

    //  Variables
    float time;
    float horizontalRandom;

    private void Awake()
    {
        //  Get components
        if(cameraShake == null)
            cameraShake = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        if (time > 0)
        {
            //  Add the values to the input axis camera
            playerAimingCamera.yAxis.Value -= ((verticalRecoil / 10) * Time.deltaTime) / recoilDuration;
            playerAimingCamera.xAxis.Value -= ((horizontalRandom / 10) * Time.deltaTime) / recoilDuration;
            time -= Time.deltaTime;
        }
    }

    public void GenerateRecoil()
    {
        time = recoilDuration;
        //  Make a impulse effect to the main camera when the playerr is shooting
        cameraShake.GenerateImpulse(Camera.main.transform.forward);

        //  Generate a ramdon number with the horizontal recoil
        horizontalRandom = Random.Range(-horizontalRecoil, horizontalRecoil);
    }
}
