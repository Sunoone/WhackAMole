using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField]
    private Text _scoreDisplay;
#pragma warning restore 649

    public float Score { get; private set; }

    public void Refresh()
    {
        Score = 0;
        UpdateScoreDisplay();
    }

    public void AddScore(float score)
    {
        Score += score;
        UpdateScoreDisplay();
    }
    private void UpdateScoreDisplay() => _scoreDisplay.text = "Score: " + ((int)Score).ToString();


    private void SavetyChecks()
    {
        if (_scoreDisplay == null)
        {
            Debug.LogError("ScoreDisplay has not been assigned in the editor. Disabling " + GetType() + " on GameObject " + gameObject.name + ".");
            enabled = false;
        }
    }
}
