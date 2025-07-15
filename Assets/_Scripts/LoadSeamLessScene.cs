using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSeamLessScene : MonoBehaviour
{
    [SerializeField] CinemachineCamera _camera;
    [SerializeField] Transform trackingTarget;
    private void OnTriggerExit2D(Collider2D collision)
    {
        Vector2 ogPos = trackingTarget.position;
        if(this.name=="startFall")
        {
            _camera.GetComponent<CinemachineRotationComposer>().Damping.y = 0;
            //_camera.GetComponent<CinemachineRotationComposer>().Composition.DeadZone

            trackingTarget.position = new Vector2(trackingTarget.position.x, trackingTarget.position.y + 15f);
            StartCoroutine(SceneLoadAsync());
        }

        else if(this.name=="endFall")
        {
            _camera.GetComponent<CinemachineRotationComposer>().Damping.y = 2;
            trackingTarget.position = ogPos;
        }
        
    }


    IEnumerator SceneLoadAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("FallScene", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return null;

    }
}
