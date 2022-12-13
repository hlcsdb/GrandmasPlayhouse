using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DisplayScenarioHome : MonoBehaviour
{
    public ScenarioSetter scenarioSetter;

    public Scenario scenario;
    public TextMeshProUGUI titleTextHulq;
    public TextMeshProUGUI titleTextEngl;
    public GameObject homeScreenBackground;
    internal AudioSource audSource;

    // Start is called before the first frame update
    void Start()
    {
        scenario = scenarioSetter.currentScenario;
    }

    public void SetScenario()
    {
        titleTextHulq.text = scenario.titleName[1];
        titleTextEngl.text = scenario.titleName[0];
        homeScreenBackground.GetComponent<Image>().sprite = scenario.homeImage;
        audSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
    }

    public void PlayTitleAudio()
    {
        audSource.PlayOneShot(scenario.GetTitleAudioClip());
    }

    public void GoToScene()
    {
        SceneManager.LoadScene(scenario.playSceneName);
    }

    public void GotoSelectionScene()
    {
        //Debug.Log("go home");
        SceneManager.LoadScene("ScenarioSelection");
    }
}
