using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryShower : MonoBehaviour
{
    public Image background;
    public Text intro;
    public Text outro;
    public Text credits;

    float fadeTracker = 0;
    public float fadeTime = 3;

    PlayerController player;
    Pause pauser;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        pauser = GetComponent<Pause>();
        StartCoroutine("IntroCoroutine");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !background.gameObject.activeSelf)
        {
            if (Pause.isPaused) pauser.UnpauseGame();
            else pauser.PauseGame(true);
        }
    }

    IEnumerator IntroCoroutine()
    {
        player.SetLook(false);
        player.SetMovement(false);
        background.gameObject.SetActive(true);
        intro.gameObject.SetActive(true);
        background.color = Color.black;
        fadeTracker = 0;
        while(fadeTracker < 1 || (!Input.GetMouseButtonDown(0) && !Input.GetKeyDown(KeyCode.E)))
        {
            if (fadeTracker < fadeTime)
            {
                fadeTracker += Time.deltaTime;
                intro.color = new Color(1, 1, 1, fadeTracker / fadeTime);
            }
            yield return null;
        }
        pauser.UnpauseGame();
        background.gameObject.SetActive(false);
        intro.gameObject.SetActive(false);
    }

    public void Outro()
    {
        StartCoroutine("OutroCoroutine");
    }

    IEnumerator OutroCoroutine()
    {
        player.SetLook(false);
        player.SetMovement(false);
        background.gameObject.SetActive(true);
        fadeTracker = 0;
        while (fadeTracker < fadeTime)
        {

            fadeTracker += Time.deltaTime;
            background.color = new Color(0, 0, 0, fadeTracker / fadeTime);
            yield return null;
        }
        outro.gameObject.SetActive(true);
        fadeTracker = 0;
        while (fadeTracker < 1 || (!Input.GetMouseButtonDown(0) && !Input.GetKeyDown(KeyCode.E)))
        {
            if (fadeTracker < fadeTime)
            {
                fadeTracker += Time.deltaTime;
                outro.color = new Color(1, 1, 1, fadeTracker / fadeTime);
            }
            yield return null;
        }
        while (fadeTracker > 0)
        {
            fadeTracker -= Time.deltaTime;
            outro.color = new Color(1, 1, 1, fadeTracker / fadeTime);
            yield return null;
        }
        outro.gameObject.SetActive(false);
        credits.gameObject.SetActive(true);
        while (!Input.GetMouseButtonDown(0) && !Input.GetKeyDown(KeyCode.E))
        {
            if (fadeTracker < fadeTime)
            {
                fadeTracker += Time.deltaTime;
                credits.color = new Color(1, 1, 1, fadeTracker / fadeTime);
            }
            yield return null;
        }
        Application.Quit();
    }
}
