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
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        se = FindFirstObjectByType<SurfaceEffector2D>();
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        if(canMove == true)
        {
            RotatePlayer();
            Boost();
        }
    }
    void Boost()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) 
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
}
