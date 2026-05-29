using UnityEngine;
using UnityEngine.UI;

public class ScoreboardController : MonoBehaviour
{    
    public int Score { get; private set; }
    private string _playerName;
    private Text _scoreLabelText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.Instance != null)
        {
            _playerName = GameManager.Instance.PlayerName;
        }
        else
        {
            // TODO throw an exception, here for faster dev
            _playerName = "Unknown";
        }

        // Initialize scoreboard
        Score = 0;
        _scoreLabelText = GameObject.Find("ScoreboardLabel").GetComponent<Text>();
        AddScore(0);
    }

    public void AddScore(int value)
    {
        // No negative scoring atm
        if (value < 0)
            return;

        // Update the value then the display
        Score += value;
        _scoreLabelText.text = _playerName + ": " + Score;
    }
}
