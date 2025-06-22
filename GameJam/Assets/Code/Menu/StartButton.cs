using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class StartButton : MonoBehaviour
{
    private Button button;

    [SerializeField] private AudioClip btnClick;
    private AudioSource audioSource;

    private void Start()
    {
        button = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (btnClick != null && audioSource != null)
        {
            audioSource.PlayOneShot(btnClick);
            StartCoroutine(WaitAndLoadScene(btnClick.length));
        }
        else
        {
            LoadSceneByButtonName();
        }
    }

    private IEnumerator WaitAndLoadScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadSceneByButtonName();
    }

    private void LoadSceneByButtonName()
    {
        string btnName = gameObject.name;

        switch (btnName)
        {
            case "btnStart":
                SceneManager.LoadScene("BasicGameEnvironment");
                break;
            case "btnHelp":
                SceneManager.LoadScene("Help");
                break;
            case "btnCredits":
                SceneManager.LoadScene("Members");
                break;
            case "btnBack":
                SceneManager.LoadScene("Mainmenu");
                break;
            default:
                Debug.LogWarning("Unrecognized button name: " + btnName);
                break;
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
