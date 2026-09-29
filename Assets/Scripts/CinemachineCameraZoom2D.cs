using Cinemachine;
using UnityEngine;

public class CinemachineCameraZoom2D : MonoBehaviour
{
    private const float NORMAL_ORTHOGRAPHIC_SIZE = 10f;
    public static CinemachineCameraZoom2D Instance { get; private set; }
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;

    private float targetOrthographicSize = 10f;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        float zoomSpeed=2f;
        cinemachineCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.m_Lens.OrthographicSize, targetOrthographicSize,zoomSpeed * Time.deltaTime) ;
    }

    public void SetCinemachineCameraTrackingTartget(Transform changedTarget)
    {
        cinemachineCamera.Follow = changedTarget;
    }

    public void SetTargetOrthographicSizew(float targetOrthoSize)
    {
        targetOrthographicSize = targetOrthoSize;
    }

      public void SetNormalOrthographicSizew()
    {
        targetOrthographicSize = NORMAL_ORTHOGRAPHIC_SIZE;
    }
}
