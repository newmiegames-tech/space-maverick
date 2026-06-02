using TMPro;
using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerNames;
    [SerializeField] private TextMeshProUGUI _playerScores;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(var highScore in GameManager.Instance.HighScores)
        {
            _playerNames.text += highScore.PlayerName + "\n";
            _playerScores.text += highScore.Score + "\n";
        }
    }
}
