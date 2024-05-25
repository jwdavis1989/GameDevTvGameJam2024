using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    public bool allowRotation = true;
    [SerializeField] public CinemachineFreeLook cinemachineFreeLook;
    [SerializeField] float yAxisSpeed = 2;
    [SerializeField] float xAxisSpeed = 200;

    void Start()
    {
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
    }

    private void FixedUpdate()
    {
        HandleCamera();
    }
    void HandleCamera()
    {
        if (allowRotation)
        {
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = xAxisSpeed;
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = yAxisSpeed;

        }
        else
        {
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = 0;
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0;
        }
    }
}