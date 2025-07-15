using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;
    public bool isCutsceneActive = false;
    public bool towersMoving = false;
    public PlayableDirector playableDirector;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }
    private void OnEnable()
    {
        playableDirector.played += OnCutsceneStarted;
        playableDirector.stopped += OnCutsceneStopped;
    }
    private void OnCutsceneStarted(PlayableDirector pd)
    {
        isCutsceneActive = true;
    }

    private void OnCutsceneStopped(PlayableDirector pd)
    {
        isCutsceneActive = false;
    }
    private void OnDisable()
    {
        if (playableDirector != null)
        {
            playableDirector.played -= OnCutsceneStarted;
            playableDirector.stopped -= OnCutsceneStopped;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (playableDirector != null)
        {
            isCutsceneActive = playableDirector.state == PlayState.Playing;
        }
    }
}
