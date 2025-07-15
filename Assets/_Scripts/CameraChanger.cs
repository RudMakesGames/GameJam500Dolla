using Unity.Cinemachine;
using UnityEngine;


public class CameraChanger : MonoBehaviour
{
    public CinemachineCamera ZoomedOutCamera,ZoomedInCamera, lakeViewCamera, lakeViewCamera2;
    public static CameraChanger instance;

    private void Awake()
    {
        instance = this;
    }
    public void ZoomIn()
    {
        ZoomedOutCamera.Priority = 0;
        ZoomedInCamera.Priority = 1;
        if(lakeViewCamera != null ) 
        lakeViewCamera.Priority = -1;
        if (lakeViewCamera2 != null)
            lakeViewCamera2.Priority = -2;


    }
    public void ZoomOut()
    {
        ZoomedInCamera.Priority = 0;
        ZoomedOutCamera.Priority = 1;
        if (lakeViewCamera != null)
            lakeViewCamera.Priority = -1;

        if (lakeViewCamera2 != null)
            lakeViewCamera2.Priority =-2;

    }

    public void LakePuzzleView()
    {
        if (lakeViewCamera != null)
            lakeViewCamera.Priority = 2;
    }

    public void LakeView()
    {
        if(lakeViewCamera2 != null)
            lakeViewCamera2.Priority = 3;
    }
}
