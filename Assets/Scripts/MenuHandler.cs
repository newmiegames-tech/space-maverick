using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private InputField _playerNameInput;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _hallOfFameButton;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _startButton.onClick.AddListener(OnStartButton);
        _hallOfFameButton.onClick.AddListener(OnHallOfFameButton);
        _exitButton.onClick.AddListener(OnExitButton);
    }

    private void OnStartButton()
    {
        string playerName = _playerNameInput.text;
        GameManager.Instance.StartGame(playerName);
    }

    private void OnHallOfFameButton()
    {
        GameManager.Instance.ShowHallOfFame();
    }

    private void OnExitButton()
    {
        GameManager.Instance.QuitGame();
    }
}
