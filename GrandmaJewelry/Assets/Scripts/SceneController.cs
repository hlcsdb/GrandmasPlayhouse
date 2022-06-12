using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneController : MonoBehaviour
{
    //public int gotoScene;
    //public Button playButton;
    //public GameObject endlessFly;
    //public GameObject challengeFly;

    //public void SetSceneNum(int sceneNum)
    //{
    //    gotoScene = sceneNum;
    //    playButton.interactable = true;
    //    if (gotoScene == 1)
    //    {
    //        endlessFly.SetActive(true);
    //        challengeFly.SetActive(false);
    //    }
    //    if (gotoScene == 2)
    //    {
    //        challengeFly.SetActive(true);
    //        endlessFly.SetActive(false);
    //    }
    //}

    public void GotoHomeScene()
    {
        Debug.Log("go home");
        SceneManager.LoadScene("Home");
    }

    public void GotoPlayScene()
    {
        SceneManager.LoadScene("Jewelry");
        //if (gotoScene == 1)
        //{
        //    SceneManager.LoadScene("Freeplay");
        //}
        //else if (gotoScene == 2)
        //{
        //    SceneManager.LoadScene("Challenge");
        //}
    }
}
