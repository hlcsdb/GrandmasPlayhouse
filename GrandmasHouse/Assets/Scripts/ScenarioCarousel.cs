using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScenarioCarousel : MonoBehaviour
{
    public ScenarioSetter scenarioSetter;
    internal List<Scenario> scenarios;
    public GameObject navDotContainer;
    public Sprite navImg;
    public GameObject draggableContainer;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI titleTextEngl;
    public Button rightButton;
    public Button leftButton;
    int numScenarios;
    List<Color> navDotColors = new List<Color> { new Color(0.78f, 0.78f, 0.875f, 1), new Color(0.78f, 0.78f, 0.875f, 0.4f) };
    List<GameObject> navDots = new List<GameObject>();
    List<Vector2> vocabPositions = new List<Vector2>() { new Vector2(-220, 46), new Vector2(-60, 46), new Vector2(100, 46), new Vector2(260, 46), new Vector2(-220, -74), new Vector2(-60, -74), new Vector2(100, -74), new Vector2(260, -74) };
    List<GameObject> draggableObjects;


    int activeScenarioIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        scenarios = new List<Scenario>(scenarioSetter.scenarios);
        numScenarios = scenarios.Count;
        SpawnNavDots();
        SpawnDraggables();
    }

    void SpawnNavDots()
    {
        float dotContainerWidth = navDotContainer.GetComponent<RectTransform>().rect.width;
        float dotSize = (dotContainerWidth * 0.7f) / numScenarios; // = 42
        Debug.Log(dotSize);
        float dotGapWidth = (dotContainerWidth * 0.3f) / (numScenarios-1); //18
        float x = -(dotContainerWidth / 2);

        for(int i = 0; i < numScenarios; i++)
        {
            GameObject dot = new GameObject();
            dot.transform.SetParent(navDotContainer.transform);
            
           
            dot.AddComponent<Image>().sprite = navImg;
            dot.transform.localScale = new Vector3(1, 1, 1);
            dot.GetComponent<RectTransform>().sizeDelta = new Vector2(dotSize, dotSize);
            dot.transform.localPosition = new Vector2( x, navDotContainer.transform.localPosition.y);
            dot.GetComponent<Image>().color = navDotColors[0];
            navDots.Add(dot);

            x += dotSize + dotGapWidth; // dot2: -48
            Debug.Log(x);
        }
        navDots[0].GetComponent<Image>().color = navDotColors[1];
    }

    internal List<string> DropdownOptionList()
    {
        List<string> dropOptions = new List<string>();
        foreach (Scenario scenario in scenarios)
        {
            dropOptions.Add(scenario.titleName[0]);
        }
        return dropOptions;
    }


    public void DoLeftButton()
    {
        activeScenarioIndex--;
        CheckNavBounaries();
        DestroyDraggables();
        SpawnDraggables();
    }

    public void DoRightButton()
    {
        activeScenarioIndex++;
        CheckNavBounaries();
        DestroyDraggables();
        SpawnDraggables();
    }

    public void CheckNavBounaries()
    {
        if (activeScenarioIndex == 0)
        {
            leftButton.interactable = false;
        }

        else if(activeScenarioIndex == numScenarios - 1)
        {
            rightButton.interactable = false;
        }

        else
        {
            rightButton.interactable = true;
            leftButton.interactable = true;
        }
    }

    public void ChangeNavDotColor()
    {
        for (int i = 0; i < numScenarios; i++)
        {
            if (i == activeScenarioIndex)
            {
                navDots[i].GetComponent<Image>().color = navDotColors[1];
            }
            else
            {
                navDots[i].GetComponent<Image>().color = navDotColors[0];
            }
        }
    }

    internal void SpawnDraggables()
    {
        Vector2 scale = new Vector2(1, 1);
        int i = 0;
        draggableObjects = new List<GameObject>(scenarios[activeScenarioIndex].scenarioDraggableObjects);
        foreach (GameObject draggable in draggableObjects)
        {
            GameObject ourDraggable = Instantiate(draggable);
            ourDraggable.transform.SetParent(draggableContainer.transform);
            ourDraggable.transform.localPosition = vocabPositions[i];
            ourDraggable.transform.localScale = scale;
            i++;
        }
    }

    private void DestroyDraggables()
    {
        foreach (GameObject draggable in draggableObjects)
        {
            Destroy(draggable);
        }
    }

}
