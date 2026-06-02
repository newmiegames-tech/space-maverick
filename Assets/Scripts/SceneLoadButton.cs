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
        // Destroy GameManager if transition back to title this ensures a clean restart
        if (_sceneIndex == (int)GameScene.Title)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        SceneManager.LoadScene(_sceneIndex);
    }
}
