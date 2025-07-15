using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using static Sentry.MeasurementUnit;

public class TowerPuzzleLogic : MonoBehaviour
{
    [SerializeField] GameObject towerA, towerB, towerC, WispA, WispB, WispC;

    [SerializeField] Color notSolved, solved;

    public AudioClip RumbleSfx;
    [SerializeField] float towerABuriedHeight, towerBBuriedHeight, towerCBuriedHeight, towerRiseBurySpeed, Amp, freq, gainSpeed;

    Vector2 towerARisen, towerBRisen, towerCRisen;
    Vector2 towerABuried, towerBBuried, towerCBuried;

    [SerializeField] CinemachineCamera cam;
    CinemachineBasicMultiChannelPerlin perlin;

    public bool towerAUp, towerBUp, towerCUp, puzzleSolved, towerAMoving, towerBMoving, towerCMoving;

    bool shake=false;


    SpriteRenderer towerARenderer, towerBRenderer, towerCRenderer;

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

        towerARenderer = towerA.transform.Find("star_0").GetComponent<SpriteRenderer>();
        towerBRenderer = towerB.transform.Find("star_0").GetComponent<SpriteRenderer>();
        towerCRenderer = towerC.transform.Find("star_0").GetComponent<SpriteRenderer>();

        /*StartCoroutine(RiseBuryTowers(towerA, towerABuried));
        StartCoroutine(RiseBuryTowers(towerB, towerBBuried));
        StartCoroutine(RiseBuryTowers(towerC, towerCBuried));*/


        towerARenderer.color = notSolved;
        towerBRenderer.color = notSolved;
        towerCRenderer.color = notSolved;


        if(WispA!=null)
            WispA.SetActive(false);
        if (WispB != null)
            WispB.SetActive(false);
        if(WispC!=null)
            WispC.SetActive(false);

        towerAMoving = false;
        towerBMoving = false;
        towerCMoving = false;

    }

    public void RiseTowerA(bool rise)
    {
        if (puzzleSolved || towerAMoving)
            return;

        GameObject.Find("CutsceneManagers").GetComponent<CutsceneManager>().towersMoving = true;


        if (rise)
            StartCoroutine(RiseBuryTowers(towerA, towerARisen));

       /* else if(!rise && !towerBUp)
            StartCoroutine(RiseBuryTowers(towerA, towerARisen));*/

        else
            StartCoroutine(RiseBuryTowers(towerA, towerABuried));
    }

    public void RiseTowerB(bool rise)
    {
        if (puzzleSolved || towerBMoving)
            return;

        GameObject.Find("CutsceneManagers").GetComponent<CutsceneManager>().towersMoving = true;


        if (rise)
            StartCoroutine(RiseBuryTowers(towerB, towerBRisen));

        else
            StartCoroutine(RiseBuryTowers(towerB, towerBBuried));
    }

    public void RiseTowerC(bool rise)
    {
        if (puzzleSolved || towerCMoving)
            return;

        GameObject.Find("CutsceneManagers").GetComponent<CutsceneManager>().towersMoving = true;

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


        if (towerCUp && towerBUp && towerAUp && !towerBMoving && !towerAMoving && !towerCMoving)
        {
            //Debug.Log("Puzzle solved");
            if (WispA != null)
                WispA.SetActive(true);
            if (WispB != null)
                WispB.SetActive(true);
            if (WispC != null)
                WispC.SetActive(true);

            puzzleSolved = true;
        }

        GameObject.Find("TopC").GetComponent<Collider2D>().enabled = towerCUp ? true:false;
        GameObject.Find("TopB").GetComponent<Collider2D>().enabled = towerBUp ? true : false;
        GameObject.Find("TopA").GetComponent<Collider2D>().enabled = towerAUp ? true : false;

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
        switch (tower.name)
        {
            case "TowerA":
                towerAUp = !towerAUp;
                towerAMoving = true;
                break;
            case "TowerB":
                towerBUp = !towerBUp;
                towerBMoving = true;
                break;
            case "TowerC":
                towerCUp = !towerCUp;
                towerCMoving = true;
                break;
        }

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
                if (towerAUp)
                {
                    StartCoroutine(changeColor(towerARenderer, solved));
                    //tower.transform.Find("eye bg_0 (2)").GetComponent<Animator>().enabled = true;
                }
                else
                {
                    //tower.transform.Find("eye bg_0 (2)").GetComponent<Animator>().enabled = false;
                    StartCoroutine(changeColor(towerARenderer, notSolved));
                }
                towerAMoving = false;
                break;
            case "TowerB":
                if (towerBUp)
                {
                    StartCoroutine(changeColor(towerBRenderer, solved));
                    //tower.transform.Find("eye bg_0 (2)").GetComponent<Animator>().enabled = true;
                }
                else
                {
                    //tower.transform.Find("eye bg_0 (2)").GetComponent<Animator>().enabled = false;
                    StartCoroutine(changeColor(towerBRenderer, notSolved));
                }
                towerBMoving = false;
                break;
            case "TowerC":
                if (towerCUp)
                {
                    StartCoroutine(changeColor(towerCRenderer, solved));
                    //tower.transform.Find("eye bg_0 (2)").GetComponent<Animator>().enabled = true;
                }
                else
                {
                    //tower.transform.Find("eye bg_0 (2)").GetComponent<Animator>().enabled = false;
                    StartCoroutine(changeColor(towerCRenderer, notSolved));
                }
                towerCMoving = false;
                break;
        }

    }


    IEnumerator changeColor(SpriteRenderer starRenderer, Color solColor)
    {
        Color currentColor = starRenderer.color;
        float elapsedTime = 0f;
        float duration = 1f;

        while (elapsedTime < duration)
        {
            
            // Calculate the lerp factor (0 to 1)
            float t = elapsedTime / duration;

            // Lerp between the colors
            starRenderer.color = Color.Lerp(currentColor, solColor, t);

            // Update elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Make sure it ends exactly on the target color
        starRenderer.color = solColor;
        GameObject.Find("CutsceneManagers").GetComponent<CutsceneManager>().towersMoving = false;
    }
}
