using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Moves_Player : MonoBehaviour
{
    
    public int runSpeed = 1;//speed of the character
    float horizontal;//movement of player left and right
    float vertical;//movement of player up and down
    Animator animator;//animator variable for movement
    bool faceRight;//checks if flip is needed
    bool isCrouching;//crouch trigger
    float countSlider = 0;//checks if player is sliding
    bool isSliding;//slide trigger
    Rigidbody2D rigidbody1;//Prevents player from falling through 'floor'
    public float jumpForce = 300;//how high the player can jump
    float axisY;//the movement in jump
    bool isJumping;//jump animation trigger
    

    void Awake()
    {
        animator = GetComponent<Animator>();//grabs animator
        rigidbody1 = GetComponent<Rigidbody2D>();//sets up rigidbody from Unity
        rigidbody1.Sleep();
    }
 
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");//sets horizontal as the movement in x-axis
        vertical = Input.GetAxis("Vertical");//sets vertical as the movement in y-axis

        animator.SetFloat("Speed", Mathf.Abs(horizontal != 0 ? horizontal : vertical));//sets the movement animation with speed
        if (Input.GetButton("Crouch") && (vertical == 0 && horizontal == 0))//statement for crouch, must be standing still
        {
            isCrouching = true;
            animator.SetBool("IsSliding", false);
            animator.SetBool("IsCrouching", isCrouching);
        }
        else if(Input.GetButtonDown("Crouch") && horizontal != 0 && !isCrouching)//statement for slide, mus be moving
        {
            countSlider = 0.5f;
            animator.SetFloat("Speed", 0.0f);
            animator.SetBool("IsSliding", true);
        }
        else if (Input.GetButtonUp("Crouch"))//To get up from crouch after releasing button
        {
            isCrouching = false;
            animator.SetBool("IsCrouching", isCrouching);
        }
        if(countSlider > 0)//To stop slide
        {
            animator.SetFloat("Speed", 0.0f);
            countSlider = countSlider - (1f * Time.deltaTime);
            if (countSlider <= 0)
                animator.SetBool("IsSliding", false);
        }
        if (transform.position.y <= axisY && isJumping)//landing after jump
            OnLanding();

        if (Input.GetButton("Jump") && !isJumping)//Triggers jump
        {
            axisY = transform.position.y;
            isJumping = true;
            rigidbody1.gravityScale = 1.5f;
            rigidbody1.WakeUp();
            rigidbody1.AddForce(new Vector2(0, jumpForce));
            animator.SetBool("IsJumping", isJumping);
        }
    }

    void FixedUpdate()
    {
        if ((horizontal != 0 || horizontal != 0) && !isCrouching)//movement math to move at a set speed
        {
            Vector3 movement = new Vector3(horizontal * runSpeed, vertical * runSpeed, 0.0f);
            transform.position = transform.position + movement * Time.deltaTime;
        }
            Flip(horizontal);
    }
    private void Flip(float horizontal)//flips character if going a direction the player isn't currently facing
    {
        if (horizontal < 0 && !faceRight || horizontal > 0 && faceRight)
        {
            faceRight = !faceRight;

            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
    void OnLanding()//has player stop on landing before falling through floor
    {
        isJumping = false;
        rigidbody1.gravityScale = 0f;
        rigidbody1.Sleep();
        axisY = transform.position.y;
        animator.SetBool("IsJumping", false);
    }

}
