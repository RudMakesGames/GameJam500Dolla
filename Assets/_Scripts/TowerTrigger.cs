using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;


public class TowerTrigger : MonoBehaviour
{
    public UnityEvent Event;
    public UnityEvent Exit;

    public Transform DesiredDestination;
    public Transform PlayerPos;

    [Header("Cinemachine Camera Reference")]
    public CinemachineCamera[] virtualCameras;

    public GameObject Black;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Event?.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Exit?.Invoke();
        }
    }

    public void Teleport()
    {
        StartCoroutine(TeleportPlayer());
    }

    IEnumerator TeleportPlayer()
    {
        if (PlayerPos != null && DesiredDestination != null)
        {
            Black?.SetActive(true);
            foreach (var Camera in virtualCameras)
            {
                Camera.GetComponent<CinemachineRotationComposer>().enabled = false;
            }
            PlayerPos.position = DesiredDestination.position;
            yield return new WaitForSeconds(1);
            Black?.SetActive(false);
           
            
        }
        
        //Invoke(nameof(WaitTillTeleport), 1.25f);
    }
    void WaitTillTeleport()
    {
        foreach (var Camera in virtualCameras)
        {
            Camera.GetComponent<CinemachineRotationComposer>().enabled = true;
        }
    }

}
