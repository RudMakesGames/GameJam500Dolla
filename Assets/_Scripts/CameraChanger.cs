using Unity.Cinemachine;
using UnityEngine;


public class CameraChanger : MonoBehaviour
{
    public CinemachineCamera ZoomedOutCamera,ZoomedInCamera, lakeViewCamera;
    public static CameraChanger instance;

    private void Awake()
    {
        instance = this;
    }
    public void ZoomIn()
    {
        ZoomedOutCamera.Priority = 0;
        ZoomedInCamera.Priority = 1;

        if(lakeViewCamera!=null) 
        lakeViewCamera.Priority = -1;


    }
    public void ZoomOut()
    {
        ZoomedInCamera.Priority = 0;
        ZoomedOutCamera.Priority = 1;
        if (lakeViewCamera != null)
            lakeViewCamera.Priority = -1;

    }

    public void LakePuzzleView()
    {
        if (lakeViewCamera != null)
            lakeViewCamera.Priority = 2;
    }
}
