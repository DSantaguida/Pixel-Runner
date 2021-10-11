using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, Damageable
{
  public int Lives { get; set; }
  public int speed;
  public int diamonds;
  public int jumpForce;
  public float yLimit;
  [SerializeField]
  private bool isGrounded = true;
  private Transform playerTransform;
  private Rigidbody2D playerRigidBody;
  private Animator playerAnimator;
  private SpriteRenderer playerSprite;
  private BoxCollider2D playerCollider;



  // Start is called before the first frame update
  void Start()
  {
    Time.timeScale = 1;
    playerTransform = GetComponent<Transform>();
    playerRigidBody = GetComponent<Rigidbody2D>();
    playerAnimator = GetComponentInChildren<Animator>();
    playerSprite = GetComponentInChildren<SpriteRenderer>();
    playerCollider = GetComponent<BoxCollider2D>();
    Lives = 3;
  }

  // Update is called once per frame
  void Update()
  {
    Movement();
    CheckFall();
  }

  private void Movement() //All movement handled here
  {
    float direction = Input.GetAxisRaw("Horizontal"); //Left/Right movement
    int flip = playerSprite.flipX ? 1 : 0;
    if (direction != 0)
    {
      if (isOnWall())
      {
        if (((int)direction ^ flip) < 0) //Check for opposite directions to avoid walking directly into a wall
        {
          playerRigidBody.velocity = new Vector2(direction * speed, playerRigidBody.velocity.y);
        }
      }
      else
        playerRigidBody.velocity = new Vector2(direction * speed, playerRigidBody.velocity.y);
    }

    if (Input.GetKeyDown(KeyCode.Space) && isGrounded) //Jump
    {
      isGrounded = false;
      if (playerAnimator.GetBool("isOnWall")) //Diagonal jump if player is on a wall
      {
        if (playerSprite.flipX)
          playerRigidBody.velocity = new Vector2(Vector2.right.x * speed, jumpForce);
        else
          playerRigidBody.velocity = new Vector2(Vector2.left.x * speed, jumpForce);
      }
      else
        playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpForce);
    }

    if (Input.GetKeyDown(KeyCode.Space) && (playerAnimator.GetBool("isJumping") || playerAnimator.GetBool("isFalling")) && !playerAnimator.GetBool("isDoubleJumping"))
    {
      playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpForce);
      playerAnimator.SetBool("isDoubleJumping", true);
    }

    if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
      playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);

    //Animations
    if (Time.timeScale != 0)
    {
      playerAnimator.SetBool("isRunning", direction != 0 && isGrounded);
      playerAnimator.SetBool("isJumping", playerRigidBody.velocity.y > 0 && !isGrounded);
      playerAnimator.SetBool("isFalling", playerRigidBody.velocity.y < 0 && !isGrounded);
      playerAnimator.SetBool("isOnWall", isOnWall());
      FlipX(playerRigidBody.velocity.x);
    }
  }

  private void FlipX(float direction) //Check movement direction to flip sprite
  {
    if (direction > 0.1f)
    {
      playerSprite.flipX = false;
    }
    else if (direction < -0.1f)
    {
      playerSprite.flipX = true;
    }
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.transform.tag == "Ground")
    {
      isGrounded = true;
      playerAnimator.SetBool("isDoubleJumping", false);
    }
    playerRigidBody.velocity = new Vector2(0, 0);
  }

  private void OnCollisionStay2D(Collision2D other) //Might want a better solution for this
  {
    if (other.transform.tag == "Ground")
      isGrounded = true;

  }

  private void OnCollisionExit2D(Collision2D other) //Might want a better solution for this
  {
    if (other.transform.tag == "Ground")
      isGrounded = false;

  }

  private bool isOnWall() //Detect if player is touching a wall for the wall jump
  {
    RaycastHit2D rightHit = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - playerCollider.bounds.extents.y), Vector2.right, 0.75f, 1 << 6 | 1 << 8);
    RaycastHit2D leftHit = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - playerCollider.bounds.extents.y), Vector2.left, 0.75f, 1 << 6 | 1 << 8);

    return rightHit || leftHit;
  }

  public void Damage()
  {
    //Temporarily removed for gameplay design 
    // Lives--;
    // UIManager.Instance.SetLives(Lives);
    // playerAnimator.SetTrigger("Hit");

    // if (Lives == 0)
    // {
    playerAnimator.SetTrigger("PlayerDead");
    // }
  }

  public void AddDiamonds(int amount)
  {
    diamonds += amount;
    UIManager.Instance.UpdateDiamondCount(diamonds);
  }

  public void SetDiamonds(int amount)
  {
    diamonds = amount;
    UIManager.Instance.UpdateDiamondCount(diamonds);
  }

  public void AddForce(Vector2 velocity)
  {
    playerRigidBody.AddForce(velocity);
  }

  private void CheckFall()
  {
    if (transform.position.y < yLimit)
    {
      Lives = 0;
      UIManager.Instance.SetAllLives(false);
      playerAnimator.SetTrigger("PlayerDead");
    }
  }

}
