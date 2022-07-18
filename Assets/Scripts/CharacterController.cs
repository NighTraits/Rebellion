using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
     // Move player in 2D space
    public float maxSpeed = 3.4f;
    public float jumpHeight = 6.5f;
    public float gravityScale = 1.5f;
    bool facingRight = false;
    float moveDirection = 0;
    bool isGrounded = false;
    Rigidbody2D r2d;
    CapsuleCollider2D mainCollider;
    Transform t;

    private Vector3 respawnPoint;
    public GameObject FallDetector;

    private AudioSource saltar;
   

    Animator playerAnim;

    private bool isPaused = false;
    public Canvas gamePausedCanvas, buttonADCanvas, buttonWCanvas, buttonFCanvas, messageCanvas, GameOverCanvas, GameClearCanvas;
    static public bool GotKey = false;
    public Image keyImage;
    GameObject chest1, chest2, chest3;

    // Use this for initialization
    void Start()
    {
        // Player
        t = transform;
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<CapsuleCollider2D>();
        playerAnim = GetComponent<Animator>();
        r2d.freezeRotation = true;
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        r2d.gravityScale = gravityScale;
        facingRight = t.localScale.x > 0;
        respawnPoint = transform.position;
        saltar = GetComponent<AudioSource>();
        
        
        // Items
        keyImage.enabled = false;

        // canvas
        gamePausedCanvas.gameObject.SetActive(false);
        buttonWCanvas.gameObject.SetActive(false);
        buttonFCanvas.gameObject.SetActive(false);
        messageCanvas.gameObject.SetActive(false);
        GameOverCanvas.gameObject.SetActive(false);
        GameClearCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Movement controls
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && (isGrounded || Mathf.Abs(r2d.velocity.x) > 0.01f))
        {
            moveDirection = Input.GetKey(KeyCode.A) ? -1 : 1;
            playerAnim.SetBool("IsRunning", true);
        }
        else
        {
            if (isGrounded || r2d.velocity.magnitude < 0.01f)
            {
                moveDirection = 0;
                playerAnim.SetBool("IsRunning", false);
                playerAnim.SetBool("IsJumping", false);
            }
        }

        // Change facing direction
        if (moveDirection != 0)
        {
            if (moveDirection < 0 && !facingRight)
            {
                facingRight = true;
                t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, transform.localScale.z);
            }
            if (moveDirection > 0 && facingRight)
            {
                facingRight = false;
                t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
            }
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
            playerAnim.SetBool("IsJumping", true);
            saltar.Play();
        }

        // Pause
        if(Input.GetKeyDown(KeyCode.P)){
            if(isPaused == false){
                Time.timeScale = 0;
                isPaused = true;
                gamePausedCanvas.gameObject.SetActive(true);
            }else
            {
                Time.timeScale = 1;
                isPaused = false;
                gamePausedCanvas.gameObject.SetActive(false);
            }
        }
        
        // Hide press button F to interact canvas
        if(ChestInteraction.chestOpen)
        {
            buttonFCanvas.gameObject.SetActive(false);
            keyImage.enabled = true;
        }

        // Game Clear 
        if (DoorInteraction.GameClear)
        {
            GameClearCanvas.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        //Game Over
        if(DoorInteraction.GameOver)
        {
            GameOverCanvas.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

    }

    void FixedUpdate()
    {
        Bounds colliderBounds = mainCollider.bounds;
        float colliderRadius = mainCollider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);
        
        // Check if player is grounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);
        
        //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        isGrounded = false;
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != mainCollider)
                {
                    isGrounded = true;
                    break;
                }
            }
        }

        // Apply movement velocity
        r2d.velocity = new Vector2((moveDirection) * maxSpeed, r2d.velocity.y);

        // Simple debug
        Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(0, colliderRadius, 0), isGrounded ? Color.green : Color.red);
        Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(colliderRadius, 0, 0), isGrounded ? Color.green : Color.red);
    }

    // Controlls tutorial
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Tutorial_1"))
        {
            buttonADCanvas.gameObject.SetActive(false);
            buttonWCanvas.gameObject.SetActive(true);
        }

        if(other.CompareTag("Tutorial_2"))
        {
            buttonWCanvas.gameObject.SetActive(false);
        }

        if(other.CompareTag("Tutorial_3"))
        {
            messageCanvas.gameObject.SetActive(false);
            buttonFCanvas.gameObject.SetActive(true);
        }

        if(other.CompareTag("Message_1"))
        {
            messageCanvas.gameObject.SetActive(true);
        }

        if(other.tag == "FallDetector")
        {
            transform.position = respawnPoint;

        }
    }
}