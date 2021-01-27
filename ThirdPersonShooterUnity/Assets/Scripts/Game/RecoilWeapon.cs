using UnityEngine;
using Cinemachine;

public class RecoilWeapon : MonoBehaviour
{
    public CinemachineImpulseSource cameraShake;

    public PlayerAiming playerAimingCamera;
    public float verticalRecoil;
    public float horizontalRecoil;
    public float recoilDuration;

    float time;
    float horizontalRandom;

    private void Awake()
    {
        cameraShake = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        if(time > 0)
        {
            playerAimingCamera.yAxis.Value -= ((verticalRecoil / 10) * Time.deltaTime) / recoilDuration;
            playerAimingCamera.xAxis.Value -= ((horizontalRandom / 10) * Time.deltaTime) / recoilDuration;
            time -= Time.deltaTime;
        }
    }

    public void GenerateRecoil()
    {
        time = recoilDuration;
        cameraShake.GenerateImpulse(Camera.main.transform.forward);

        horizontalRandom = Random.Range(-horizontalRecoil, horizontalRecoil);
    }
}
