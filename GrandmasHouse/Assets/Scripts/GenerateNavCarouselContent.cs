using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GenerateNavCarouselContent : MonoBehaviour
{
    public Sprite[] pictureFrameSprites;
    public GameObject scenarioImagePrefab;
    public ScenarioSetter scenarioSetter;
    public float moduleSpacing = 100f; // Adjust this value based on your desired spacing between modules

    float featureAreaWidth = 600f; //width between left and right navigation buttons (minus a bit to ensure margin)
    float featureAreaHeight = 400f;

    float pictureframeWidth; //to be calculated at start based on the width of the prefab
    float pictureframeHeight; //to be calculated at start based on the height of the prefab
    private float frameXMargin;//to be calculated at start based on the width of the prefab in relation to the total width
    private float frameYMargin;//to be calculated at start based on the width of the prefab in relation to the total width
    private GameObject[] scenarioFrameGOs;
    // Start is called before the first frame update
    void Start()
    {
        pictureframeWidth = scenarioImagePrefab.GetComponent<RectTransform>().rect.width;
        pictureframeHeight = scenarioImagePrefab.GetComponent<RectTransform>().rect.height;

        frameXMargin = (featureAreaWidth - (pictureframeWidth * 2)) / 3;
        frameYMargin = (featureAreaHeight - (pictureframeHeight * 2)) / 3; ;
        GenerateScenarioPictures();
    }

    private void GenerateScenarioPictures()
    {
        int scenarioCount = scenarioSetter.scenarios.Count;
        scenarioFrameGOs = new GameObject[scenarioCount];

        for (int i = 0; i < scenarioCount; i++)
        {
            int scenarioIndex = i;
            Vector3 offset = CalculateFrameOffsets(scenarioCount, scenarioIndex);

            GameObject scenarioFrameGO = Instantiate(scenarioImagePrefab, transform);
            scenarioFrameGOs[i] =scenarioFrameGO;
            scenarioFrameGO.transform.localPosition = offset;

            Image scenarioGOImage = scenarioFrameGO.GetComponent<Image>();
            scenarioGOImage.sprite = scenarioSetter.scenarios[scenarioIndex].homeImage;
            scenarioGOImage.preserveAspect = true;

            GameObject pictureFrameGO = new GameObject("PictureFrame");
            pictureFrameGO.transform.SetParent(scenarioGOImage.transform, false);

            RectTransform moduleRectTransform = scenarioGOImage.rectTransform;
            RectTransform pictureFrameRectTransform = pictureFrameGO.AddComponent<RectTransform>();
            pictureFrameRectTransform.sizeDelta = moduleRectTransform.sizeDelta;
            pictureFrameRectTransform.localScale = Vector3.one;

            Image pictureFrameImage = pictureFrameGO.AddComponent<Image>();
            pictureFrameImage.sprite = pictureFrameSprites[scenarioIndex % 4];
            pictureFrameImage.preserveAspect = true;

            Button scenarioButton = scenarioFrameGO.GetComponent<Button>();
            scenarioButton.onClick.AddListener(() => SetActiveScenario(scenarioIndex));

            Debug.Log("scenarioGO image name: " + scenarioGOImage.name + "; pictureFrameGO name: " + pictureFrameGO.name);
            scenarioFrameGO.SetActive(true);
        }

        for (int i = 0; i < scenarioCount; i++)
        {
            int quartetIndex = i / 4;
            float quartetOffset = 0.0f + (featureAreaWidth * quartetIndex);
            scenarioFrameGOs[i].transform.localPosition += new Vector3(quartetOffset, 0f, 0f);
        }
    }

    private Vector3 CalculateFrameOffsets(int scenarioCount, int scenarioIndex)
    {
        int numColumns = scenarioCount / 2;
        int xOrder = (scenarioIndex % 4 == 0 || scenarioIndex % 4 == 3) ? 1 : 2;
        int yOrder = (scenarioIndex % 4 == 1 || scenarioIndex % 4 == 2) ? 2 : 1;

        int quartetIndex = scenarioIndex / 4;
        float quartetOffset = featureAreaWidth * quartetIndex;

        Vector3 offset = new Vector3(quartetOffset, 0f, 0f);

        if(xOrder == 1)
        {
            offset.x += frameXMargin + (pictureframeWidth / 2);
        }
        else
        {
            offset.x += frameXMargin + pictureframeWidth + frameXMargin + (pictureframeWidth / 2);
        }

        if (yOrder == 1)
        {
            offset.y += frameYMargin + (pictureframeHeight / 2);
        }
        else
        {
            offset.y += frameYMargin + pictureframeHeight + frameYMargin + (pictureframeHeight / 2);
        }

        Debug.Log("index: " + scenarioIndex + "; side: " + xOrder + "; top bottom: " + yOrder + "; offset X: " +offset.x + "; offset Y: " +offset.y);

        return offset;
    }

    private Vector3 DoQuartetOffset(Vector3 frameOffset, int scenarioIndex)
    {
        int quartetIndex = scenarioIndex / 4;
        float quartetOffset = 0.0f + (featureAreaWidth * quartetIndex);
        frameOffset.x += quartetOffset;
        return frameOffset;
    }

    private void SetActiveScenario(int scenarioIndex)
    {
        Debug.Log("scenario index: " + scenarioIndex);
        scenarioSetter.currentScenario = scenarioSetter.scenarios[scenarioIndex];
        scenarioSetter.currentScenarioIndex = scenarioIndex;
        GoToScenario(scenarioIndex);
        // Add code here to transition to the play scene
    }

    private void GoToScenario(int activeScenarioIndex)
    {
        SceneManager.LoadScene("Home");

        scenarioSetter.ChangeScenario(activeScenarioIndex);
    }
}
