using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Scenario Setter")]

public class ScenarioSetter : ScriptableObject
{
    public List<Scenario> scenarios;
    internal Scenario currentScenario;
    void Start()
    {
        currentScenario = scenarios[0];
    }
}
