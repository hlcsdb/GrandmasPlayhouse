using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PassSplashScreen : MonoBehaviour
{
    void Start() {
        var videoPlayer = gameObject.GetComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "iyusLogo.mp4");

        videoPlayer.Play();
        StartCoroutine(WaitForLogoToFinish());
        IEnumerator WaitForLogoToFinish()
        {
            yield return new WaitForSeconds(5);
            yield return new WaitUntil(() => !videoPlayer.isPlaying);
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene("Home");
        }
    }
}
