using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] float floatDelay = 2f;
    [SerializeField] ParticleSystem crashEffect;
    bool hasCrashed = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ground" && hasCrashed == false)
        {
            hasCrashed = true;
            crashEffect.Play();
            GetComponent<AudioSource>().Play();
            FindFirstObjectByType<PlayerController>().DisableController();
            Invoke("ReloadLevel", floatDelay);
        }
    }
    void ReloadLevel()
    {
        SceneManager.LoadScene(1);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
