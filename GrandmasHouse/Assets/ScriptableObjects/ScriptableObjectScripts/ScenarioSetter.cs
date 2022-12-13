using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Scenario Setter")]

public class ScenarioSetter : ScriptableObject
{
    public List<Scenario> scenarios;
    internal Scenario currentScenario;
    internal int currentScenarioIndex;

    void Start()
    {
        currentScenario = scenarios[0];
        currentScenarioIndex = scenarios.FindIndex(x => x.Equals(currentScenario));
    }

    internal void ChangeScenario(int scenarioIndex)
    {
        currentScenario = scenarios[scenarioIndex];
        currentScenarioIndex = scenarioIndex;
    }
}
