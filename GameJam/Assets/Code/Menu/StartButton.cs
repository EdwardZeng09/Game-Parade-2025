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
            LoadDemoScene();
        }
    }

    private IEnumerator WaitAndLoadScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadDemoScene();
    }

    private void LoadDemoScene()
    {
        SceneManager.LoadScene("BasicGameEnvironment");
    }
}
