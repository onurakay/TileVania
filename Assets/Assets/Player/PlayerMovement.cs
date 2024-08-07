using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2 (10f, 10f);
    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowSpawn;
    [SerializeField] float shootCooldown = 1f;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;
    float direction;

    bool isAlive = true;
    public bool IsAlive { get { return isAlive;} }
    bool canShoot = true;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }

    void Update()
    {
        if(!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        if(!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if(!isAlive) { return; }
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if(value.isPressed)
        {
            myRigidbody.velocity += new Vector2 (0, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if(myAnimator.GetBool("isClimbing") || !canShoot || !isAlive) { return; }

        direction = transform.localScale.x;

        Instantiate(arrow, arrowSpawn.position, Quaternion.Euler(0, 180*(direction < 0 ? 1 : 0), 0));
        myAnimator.SetTrigger("Shooting");

        StartCoroutine(ShootCooldown());
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", hasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if(hasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidbody.velocity.x), 1f);        
        }
    }

    void ClimbLadder()
    {
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return; 
        }

        Vector2 climbVelocity = new Vector2 (myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0;

        bool hasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", hasVerticalSpeed);
    }

    void Die()
    {
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathKick;
            FindObjectOfType<GameSession>().HandlePlayerDeath();
        }
    }
    IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
