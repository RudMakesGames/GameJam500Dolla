using Unity.Cinemachine;
using UnityEngine;


public class CameraChanger : MonoBehaviour
{
    public CinemachineCamera ZoomedOutCamera,ZoomedInCamera;
    public static CameraChanger instance;

    private void Awake()
    {
        instance = this;
    }
    public void ZoomIn()
    {
        ZoomedOutCamera.Priority = 0;
        ZoomedInCamera.Priority = 1;
        
    }
    public void ZoomOut()
    {
        ZoomedInCamera.Priority = 0;
        ZoomedOutCamera.Priority = 1;
       
    }
}
