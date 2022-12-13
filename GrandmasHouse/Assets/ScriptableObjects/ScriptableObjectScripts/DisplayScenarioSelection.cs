using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class DisplayScenarioSelection : MonoBehaviour
{
    public ScenarioSetter scenarioSetter;
    public List<Scenario> scenarios;
    //internal Scenario currentScenario;
    public TextMeshProUGUI m_TextA;
    public TextMeshProUGUI m_TextB;

    int m_DropdownValue;

    // Start is called before the first frame update
    void Start()
    {
        scenarios = new List<Scenario>(scenarioSetter.scenarios);

        SetDropdownOptions();
    }

    internal void SetDropdownOptions()
    {
        //Fetch the Dropdown GameObject the script is attached to
        TMP_Dropdown m_Dropdown = GetComponent<TMP_Dropdown>();
        //Clear the old options of the Dropdown menu
        //m_Dropdown.ClearOptions();
        //Add the options created in the List above
        m_Dropdown.AddOptions(DropdownOptionList());

        //Add listener for when the value of the Dropdown changes, to take action
        m_Dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(m_Dropdown);
        });

        //Initialise the Text to say the first value of the Dropdown
        m_TextA.text = "Selected Game : " + scenarios[m_Dropdown.value].titleName[1];
        m_TextB.text = scenarios[m_Dropdown.value].titleName[0];
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

    //Ouput the new value of the Dropdown into Text
    void DropdownValueChanged(TMP_Dropdown change)
    {
        m_DropdownValue = change.value;
        m_TextA.text = "Selected Game : " + scenarios[m_DropdownValue].titleName[1];
        m_TextB.text = scenarios[m_DropdownValue].titleName[0];
        
    }

    public void GoToScene()
    {
        SceneManager.LoadScene("Home");

        scenarioSetter.currentScenario = scenarios[m_DropdownValue];
    }
}