using System.Collections;
using TMPro;
using UnityEngine;

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

    public int score = 0;
    public TMP_Text scoreText;
    public TMP_Text speedText;

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
    }

    void CalculateSpeedScore()
    {
        float speed = rb.linearVelocity.magnitude;
        if (speed > 3f)
        {
            score += (int)(speed * Time.deltaTime * 10);
        }
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
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
            score += 100;
            UpdateScoreUI();
            totalRotation = 0f;

            ShowSpinText();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            totalRotation = 0f;
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
}
