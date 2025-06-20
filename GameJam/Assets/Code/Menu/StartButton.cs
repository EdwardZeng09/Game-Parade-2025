using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(LoadDemoScene);
    }

    private void LoadDemoScene()
    {
        SceneManager.LoadScene("BasicGameEnvironment"); 
    }
}
