using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    public string CurrentLevel;
    public string NextLevel;
    public bool isALoadingScreen, isAMenu;
    public float TimeDelay = 3;
    private void Awake()
    {
        if (instance == null)
            instance = this;

    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(CurrentLevel);
    }
    public void ProceedToAnotherLevel()
    {
        if (NextLevel != null)
            SceneManager.LoadScene(NextLevel);
    }
 
    public void Play()
    {
        Invoke("ProceedToAnotherLevel", 1.25F);
    }
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Invoke("Quit", 1.25f);
    }
    private void Quit()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }
    public void LoadALevel(string levelName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelName);
        
    }
    IEnumerator StartScreen()
    {
        yield return new WaitForSeconds(TimeDelay);
        ProceedToAnotherLevel();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isALoadingScreen)
        {
            StartCoroutine(StartScreen());
        }
    }
}
