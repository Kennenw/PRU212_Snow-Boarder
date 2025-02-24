using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject levelScreen;

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void Options()
    {

    }
    public void Levels()
    {
        if (mainScreen != null && levelScreen != null)
        {
            mainScreen.SetActive(false);
            levelScreen.SetActive(true);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
