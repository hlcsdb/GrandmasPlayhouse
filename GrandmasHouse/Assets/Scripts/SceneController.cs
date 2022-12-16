using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneController : MonoBehaviour
{

    public void GotoHomeScene()
    {
        //Debug.Log("go home");
        SceneManager.LoadScene("Home");
    }

    public void GotoPlayScene()
    {
        SceneManager.LoadScene("Play");
    }

    public void GotoSelectionScene()
    {
        //Debug.Log("go to scenario selection");
        SceneManager.LoadScene("ScenarioSelection");
    }
}
