using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class TowerPuzzleLogic : MonoBehaviour
{
    [SerializeField] GameObject towerA, towerB, towerC;
    public AudioClip RumbleSfx;
    [SerializeField] float towerABuriedHeight, towerBBuriedHeight, towerCBuriedHeight, towerRiseBurySpeed, Amp, freq, gainSpeed;

    Vector2 towerARisen, towerBRisen, towerCRisen;
    Vector2 towerABuried, towerBBuried, towerCBuried;

    [SerializeField] CinemachineCamera cam;
    CinemachineBasicMultiChannelPerlin perlin;

    bool towerAUp, towerBUp, towerCUp;

    bool shake=false;

    private void Start()
    {
        towerAUp = towerBUp = towerCUp = false;
        perlin = cam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        //perlin.NoiseProfile = null;

        towerARisen = towerA.transform.position;
        towerBRisen = towerB.transform.position;
        towerCRisen = towerC.transform.position;

        towerABuried = new Vector2(towerARisen.x, towerABuriedHeight);
        towerBBuried = new Vector2(towerBRisen.x, towerBBuriedHeight);
        towerCBuried = new Vector2(towerCRisen.x, towerCBuriedHeight);

        towerA.transform.position = towerABuried;
        towerB.transform.position = towerBBuried;
        towerC.transform.position = towerCBuried;



        /*StartCoroutine(RiseBuryTowers(towerA, towerABuried));
        StartCoroutine(RiseBuryTowers(towerB, towerBBuried));
        StartCoroutine(RiseBuryTowers(towerC, towerCBuried));*/


    }

    public void RiseTowerA(bool rise)
    {
        
        if (rise)
            StartCoroutine(RiseBuryTowers(towerA, towerARisen));

        else
            StartCoroutine(RiseBuryTowers(towerA, towerABuried));
    }

    public void RiseTowerB(bool rise)
    {
       
        if (rise)
            StartCoroutine(RiseBuryTowers(towerB, towerBRisen));

        else
            StartCoroutine(RiseBuryTowers(towerB, towerBBuried));
    }

    public void RiseTowerC(bool rise)
    {
        
        if (rise)
            StartCoroutine(RiseBuryTowers(towerC, towerCRisen));

        else
            StartCoroutine(RiseBuryTowers(towerC, towerCBuried));
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(RiseBuryTowers(towerA, towerARisen)); //bruh
           // Debug.Log("pressed");
        }*/


        if (towerCUp && towerBUp && towerAUp)
            Debug.Log("Puzzle solved");
    }

   /* IEnumerator waitTest()
    {
        yield return new WaitForSeconds(15f);

        RiseBuryTowers(towerA, towerARisen);
        RiseBuryTowers(towerB, towerBRisen);
        RiseBuryTowers(towerC, towerCRisen);
    }
*/
    IEnumerator ShakeValues(bool shake)
    {
        //Debug.Log("Shaking");
        if (shake)
        {
            perlin.FrequencyGain = freq;
            Debug.Log("Shaking");
            while (!Mathf.Approximately(perlin.AmplitudeGain, Amp))
            {
                perlin.AmplitudeGain += Time.deltaTime* gainSpeed;
                Debug.Log(perlin.AmplitudeGain);
                yield return null;
            }
        }
        
        else
        {
            perlin.FrequencyGain = 0;
            Debug.Log("stopping Shaking");
            while (!Mathf.Approximately(perlin.AmplitudeGain, 0))
            {
                perlin.AmplitudeGain -= Time.deltaTime* gainSpeed*2;
                perlin.FrequencyGain -= Time.deltaTime * gainSpeed*2;
                yield return null;
            }

        }
    }




    IEnumerator RiseBuryTowers(GameObject tower, Vector2 targetPos)
    {
        AudioManager.instance.PlaySoundFXClip(RumbleSfx, transform, 1, Random.Range(0.9f, 1.1f));
        // Start shaking
        perlin.FrequencyGain = freq;

        float currentAmplitude = 0f;
        while (currentAmplitude < Amp)
        {
            currentAmplitude = Mathf.MoveTowards(currentAmplitude, Amp, Time.deltaTime * gainSpeed);
            perlin.AmplitudeGain = currentAmplitude;
            yield return null;
        }

        // Move tower while shaking
        while (!Mathf.Approximately(tower.transform.position.y, targetPos.y))
        {
            tower.transform.position = Vector2.MoveTowards(tower.transform.position, targetPos, Time.deltaTime * towerRiseBurySpeed);
            yield return null;
        }

        // Smoothly stop shake
        while (perlin.AmplitudeGain > 0)
        {
            perlin.AmplitudeGain = Mathf.MoveTowards(perlin.AmplitudeGain, 0f, Time.deltaTime * gainSpeed * 2);
            perlin.FrequencyGain = Mathf.MoveTowards(perlin.FrequencyGain, 0f, Time.deltaTime * gainSpeed * 2);
            yield return null;
        }

        // Set tower state flags
        switch (tower.name)
        {
            case "TowerA":
                towerAUp = !towerAUp;
                break;
            case "TowerB":
                towerBUp = !towerBUp;
                break;
            case "TowerC":
                towerCUp = !towerCUp;
                break;
        }
    }
}
