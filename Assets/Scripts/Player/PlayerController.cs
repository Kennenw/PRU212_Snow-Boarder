using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    SurfaceEffector2D se;
    [SerializeField] float TorqueAmount;
    [SerializeField] float boostSpeed;
    [SerializeField] float baseSpeed;
    [SerializeField] float slowSpeed;

    bool canMove = true;
    bool isJumping = false;
    float totalRotation = 0f;
    float lastRotation = 0f;
    float savedSpeed = 0f;
    int comboMultiplier = 1;

    public int score = 0;
    public TMP_Text scoreText;
    public TMP_Text speedText;
    public TMP_Text comboText;

    [SerializeField] GameObject spinTextPrefab;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        se = FindFirstObjectByType<SurfaceEffector2D>();
    }

    private void Update()
    {
        if (canMove)
        {
            RotatePlayer();
            Boost();
            CalculateSpeedScore();
            UpdateSpeedUI();
            PerformSpin();
        }
    }

    void Boost()
    {
        if (!isJumping)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                se.speed = boostSpeed;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                se.speed = slowSpeed;
            }
            else
            {
                se.speed = baseSpeed;
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(savedSpeed, rb.linearVelocity.y);
        }
    }

    void RotatePlayer()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddTorque(TorqueAmount);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddTorque(-TorqueAmount);
        }
    }

    public void DisableController()
    {
        canMove = false;
        ScoreManager.Instance.SaveFinalScore(score);  // Truyền điểm số hiện tại
    }

    void CalculateSpeedScore()
    {
        float speed = rb.linearVelocity.magnitude;
        int pointsToAdd = (int)(speed * Time.deltaTime * 10 * comboMultiplier);

        if (speed > 3f && pointsToAdd > 0) // Tránh cộng điểm 2 lần
        {
            score += pointsToAdd;
            ScoreManager.Instance.AddScore(pointsToAdd);
        }

        Debug.Log("Score hiện tại: " + score);
    }




    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        if (comboText != null)
        {
            comboText.text = "Combo x" + comboMultiplier;
        }
    }

    void UpdateSpeedUI()
    {
        if (speedText != null)
        {
            float speed = rb.linearVelocity.magnitude;
            speedText.text = "Speed: " + speed.ToString("F2") + " m/s";
        }
    }

    void PerformSpin()
    {
        float currentRotation = transform.rotation.eulerAngles.z;
        float rotationDifference = Mathf.DeltaAngle(lastRotation, currentRotation);
        totalRotation += Mathf.Abs(rotationDifference);
        lastRotation = currentRotation;

        if (totalRotation >= 290f)
        {
            comboMultiplier++;
            score += 100 * comboMultiplier;
            UpdateScoreUI();
            totalRotation = 0f;

            ShowSpinText();
            UpdateComboUI();
        }
    }

    void UpdateComboUI()
    {
        if (comboText != null)
        {
            comboText.text = "Combo x" + comboMultiplier;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            totalRotation = 0f;
            comboMultiplier = 1;
            UpdateComboUI();
            SurfaceEffector2D newSurface = collision.gameObject.GetComponent<SurfaceEffector2D>();
            if (newSurface != null)
            {
                se = newSurface;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = true;
            lastRotation = transform.rotation.eulerAngles.z;
            savedSpeed = rb.linearVelocity.x;
        }
    }

    void ShowSpinText()
    {
        if (spinTextPrefab != null)
        {
            GameObject spinText = Instantiate(spinTextPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
            TextMeshPro tmp = spinText.GetComponent<TextMeshPro>();

            if (tmp != null)
            {
                tmp.text = "+100 x" + comboMultiplier;
                Renderer renderer = tmp.GetComponent<Renderer>();
                renderer.sortingLayerName = "UI";
                renderer.sortingOrder = 100;

                StartCoroutine(AnimateSpinText(spinText));
            }
        }
    }

    IEnumerator AnimateSpinText(GameObject textObject)
    {
        float duration = 1f;
        float elapsedTime = 0f;
        Vector3 startPos = textObject.transform.position;
        Vector3 endPos = startPos + new Vector3(0, 5, 0);

        TextMeshPro textMesh = textObject.GetComponent<TextMeshPro>();
        if (textMesh == null)
        {
            Debug.LogError("TextMeshPro component not found!");
            yield break;
        }
        Color startColor = textMesh.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            textObject.transform.position = Vector3.Lerp(startPos, endPos, t);
            textMesh.color = new Color(startColor.r, startColor.g, startColor.b, 1 - t);

            yield return null;
        }

        Destroy(textObject);
    }

    void Start()
    {
        score = 0; // Đặt lại điểm khi game bắt đầu
        ScoreManager.Instance.ResetScore(); // Gọi reset từ ScoreManager
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game...");

        Time.timeScale = 1;
        Debug.Log("Current TimeScale: " + Time.timeScale);

        ScoreManager.Instance.ResetScore();  // Reset điểm ngay
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }




}
