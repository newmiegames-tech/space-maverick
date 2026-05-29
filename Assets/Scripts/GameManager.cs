using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Running,
    Paused,
    Stopped
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private InputField _playerNameInput;

    public GameState GameState { get; private set; }
    public string PlayerName { get; private set; }
    public int StartingHealth { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        GameState = GameState.Stopped;
        StartingHealth = 3;
    }

    public void StartGame(string playerName)
    {        
        if (playerName.Length <= 1)
            return;

        PlayerName = playerName;
        GameState = GameState.Running;
        SceneManager.LoadScene(1);
    }

    public void EndGame(int score)
    {
        GameState = GameState.Stopped;
    }

    public void ShowHallOfFame()
    {
        // TODO - load hall of fame scene
        Debug.Log("Loading Hall Of Fame Scene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}
