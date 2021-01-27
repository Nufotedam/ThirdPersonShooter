using UnityEngine;

public class CrossHairPosition : MonoBehaviour
{
    Camera m_MainCamera;

    Ray m_Ray;
    RaycastHit m_HitInfo;

    private void Start()
    {
        m_MainCamera = Camera.main;
    }

    private void Update()
    {
        m_Ray.origin = m_MainCamera.transform.position;
        m_Ray.direction = m_MainCamera.transform.forward;
        Physics.Raycast(m_Ray, out m_HitInfo);
        transform.position = m_HitInfo.point;
    }
}
