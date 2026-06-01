using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadButton : MonoBehaviour
{
    [SerializeField] int _sceneIndex;

    public void Start()
    {
        Button button =  GetComponent<Button>();
        button.onClick.AddListener(LoadScene);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(_sceneIndex);
    }
}
