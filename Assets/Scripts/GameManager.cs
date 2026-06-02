using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameScene
{
    Title,
    Game,
    HallOfFame,
    MissionStart,
    MissionDefeat,
    MissionVictory
}

public enum GameState
{
    Running,
    Paused,
    Stopped
}

[Serializable]
public class HighScoreData
{
    public string PlayerName;
    public int Score;
}

[Serializable]
class SaveData
{
    public string PlayerName;
    public List<HighScoreData> HighScores;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private InputField _playerNameInput;

    public GameState GameState { get; private set; }
    public string PlayerName { get; private set; }
    public int StartingHealth { get; private set; }

    private const string _anonPlayerName = "Unknown Ace";
    private const string _scoreFile = "scores.json";
    private const int _highScoreLimit = 12;
    public IReadOnlyList<HighScoreData> HighScores { get; private set; }
    private List<HighScoreData> _highScores;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load saved state
        LoadState();

        // Setup game state
        GameState = GameState.Stopped;
        StartingHealth = 3;
    }

    public void StartGame(string playerName)
    {        
        if (playerName.Length > 1)
        {
            PlayerName = playerName;            
        }
        else
        {
            PlayerName = _anonPlayerName;
        }
        
        GameState = GameState.Running;
        SceneManager.LoadScene((int)GameScene.MissionStart);
    }

    public void EndGame()
    {
        // Stop the game
        GameState = GameState.Stopped;
        Time.timeScale = 0f;

        // Update high scores
        UpdateHighScores();

        // Save state for next time
        SaveState();
    }

    void UpdateHighScores()
    {
        // Get the final score
        ScoreboardController score = GameObject.Find("Scoreboard").GetComponent<ScoreboardController>();
        HighScoreData highScore = new HighScoreData();
        highScore.PlayerName = PlayerName;
        highScore.Score = score.Score;

        // Add high score if list is not at limit
        if (_highScores.Count < _highScoreLimit)
        {
            _highScores.Add(highScore);
        }
        // Update high score if this score is better than bottom one
        else if (_highScores[_highScoreLimit-1].Score < highScore.Score)
        {
            _highScores[_highScoreLimit - 1] = highScore;
        }

        // Keep high scores sorted (descending)
        _highScores.Sort((x, y) => y.Score.CompareTo(x.Score));
    }

    public void ShowHallOfFame()
    {
        SceneManager.LoadScene((int)GameScene.HallOfFame);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    void LoadState()
    {
        string path = Path.Combine(Application.persistentDataPath, _scoreFile);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // Reset player name prompt if last player was anonymous
            if (data.PlayerName == _anonPlayerName)
            {
                _playerNameInput.text = "";
            }
            else
            {
                _playerNameInput.text = data.PlayerName;
            }            

            _highScores = data.HighScores;
        }
        else
        {
            _playerNameInput.text = "";
            _highScores = new List<HighScoreData>();
        }

        HighScores = _highScores;
    }

    void SaveState()
    {
        SaveData data = new SaveData();
        data.PlayerName = PlayerName;
        data.HighScores = _highScores;
        string json = JsonUtility.ToJson(data);
        string path = Path.Combine(Application.persistentDataPath, _scoreFile);
        File.WriteAllText(path, json);
    }
}
