using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;

public class Player1Move : MonoBehaviour
{
    private Animator Anim;
    private AnimatorStateInfo Player1Layer0;
    private Vector3 OppPosition;
    private bool IsJumping = false;
    private bool CanWalkLeft = true;
    private bool CanWalkRight = true;
    private bool FacingLeft = false;
    private bool FacingRight = true;

    public GameObject Player1;
    public GameObject Opponent;
    public float WalkSpeed = 0.0025f;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Listen to Animator
        Player1Layer0 = Anim.GetCurrentAnimatorStateInfo(0);

        ScreenBounds();
        HorizontalMovement();
        VerticalMovement();
    }

    // Create Bounds for Screen
    void ScreenBounds()
    {
        Vector3 ScreenBounds = Camera.main.WorldToScreenPoint(this.transform.position);

        if (ScreenBounds.x > Screen.width)
        {
            CanWalkRight = false;
        }
        else if (ScreenBounds.x < 0)
        {
            CanWalkLeft = false;
        }
        else
        {
            CanWalkRight = true;
            CanWalkLeft = true;
        }

        // Get Opponent Position
        OppPosition = Opponent.transform.position;
        
        // Facing Left or Right of Opponent
        if (OppPosition.x > Player1.transform.position.x)
        {
            StartCoroutine(FaceLeft());
        }
        
        if (OppPosition.x < Player1.transform.position.x)
        {
            StartCoroutine(FaceRight());
        }
    }

    // Walking left and right
    void HorizontalMovement()
    {
        if (Player1Layer0.IsTag("Motion"))
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                if (CanWalkRight)
                {
                    Anim.SetBool("Forward", true);
                    transform.Translate(WalkSpeed, 0, 0);
                }
            }

            if (Input.GetAxis("Horizontal") < 0)
            {
                if (CanWalkLeft)
                {
                    Anim.SetBool("Backward", true);
                    transform.Translate(-WalkSpeed, 0, 0);
                }
            }
        }

        if (Input.GetAxis("Horizontal") == 0)
        {
            Anim.SetBool("Forward", false);
            Anim.SetBool("Backward", false);
        }
    }

    // Jumping and Crouching
    void VerticalMovement()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            if (!IsJumping)
            {
                IsJumping = true;
                Anim.SetTrigger("Jump");
                StartCoroutine(JumpPause());
            }
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            Anim.SetBool("Crouch", true);
        }

        if (Input.GetAxis("Vertical") == 0)
        {
            Anim.SetBool("Crouch", false);
        }
    }

    IEnumerator JumpPause()
    {
        yield return new WaitForSeconds(1.0f);
        IsJumping = false;
    }

    IEnumerator FaceLeft()
    {
        if (FacingLeft)
        {
            FacingLeft = false;
            FacingRight = true;
            
            yield return new WaitForSeconds(0.15f);
            
            Player1.transform.Rotate(0, 180, 0);
            
            Anim.SetLayerWeight(1, 0);
        }
    }

    IEnumerator FaceRight()
    {
        if (FacingRight)
        {
            FacingRight = false;
            FacingLeft = true;
            
            yield return new WaitForSeconds(0.15f);

            Player1.transform.Rotate(0, -180, 0);
            
            Anim.SetLayerWeight(1, 1);
        }
    }
}