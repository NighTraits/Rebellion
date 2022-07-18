using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterControllerLvl2 : MonoBehaviour
{
     // Move player in 2D space
    public float maxSpeed = 8.5f;
    public float jumpHeight = 25f;
    public float gravityScale = 9f;
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
    public Canvas gamePausedCanvas, gameOverCanvas, gameClearCanvas;

    public Image keyImage;
    static public bool GotKey;


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
        
        GotKey = false;
        
        isPaused = false;
        Time.timeScale = 1;

        DoorInteraction.GameOver = false;
        DoorInteraction.GameClear = false;


        // Items
        keyImage.enabled = false;

        // canvas
        gamePausedCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        gameClearCanvas.gameObject.SetActive(false);
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
        if(Input.GetKeyDown(KeyCode.Escape)){
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
        
        // Game Clear 
        if (DoorInteraction2.GameClear)
        {
            Time.timeScale = 0;
            isPaused = true;
            gameClearCanvas.gameObject.SetActive(true);
        }

        //Game Over
        if(DoorInteraction2.GameOver || Health.dead)
        {
            Time.timeScale = 0;
            isPaused = true;
            gameOverCanvas.gameObject.SetActive(true);
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
    private void OnTriggerEnter2D(Collider2D other1)
    {
        if(other1.tag == "FallDetector")
        {
            transform.position = respawnPoint;

        }

        if(other1.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }

        if(other1.tag == "Item")
        {
            keyImage.enabled = true;
            GotKey = true;
            Destroy(other1.gameObject, 0.5f);
        }
    }
}