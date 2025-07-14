using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            if (this.gameObject.name == "LakeCamTrigger")
            {
                CameraChanger.instance.LakePuzzleView();
                return;
            }
            CameraChanger.instance.ZoomIn();
            Debug.Log("zooming in");
        }

        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if (this.gameObject.name == "LakeCamTrigger")
            {
                CameraChanger.instance.LakePuzzleView();
                return;
            }

            CameraChanger.instance.ZoomIn();
            Debug.Log("zooming zoomed in");

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CameraChanger.instance.ZoomOut();
            Debug.Log("zoomed out");

        }
    }

}
