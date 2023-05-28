using DG.Tweening;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float horizontalSpeed = 10f;
    public float jumpForce = 10.0f;

    //private Rigidbody rb;
    private int position = 1; //center
    public float[] verticalPostios = new float[] { -1.5f, 0, 1.5f };
    private Vector3 targetPosition;

    //public bool isGround = false;
    public bool isJump = false;
    private float jumpStart;
    public float jumpLength = 20;
    public float jumpHeight = 10;

    public bool isSlide = false;
    private float slideStart;
    public float slideLength = 4;

    private void OnUpdate()
    {
        if (GameController.gameState == GameState.Pause)
            return;

        this.transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
        InputKeyBoard();

        float positionY = 1;
        if (isJump)
        {
            float ratioPositionJump = (this.transform.position.z - jumpStart) / jumpLength;
            if (ratioPositionJump <= 1)
            {
                positionY = Mathf.Sin(ratioPositionJump * Mathf.PI) * jumpHeight + 1;
            }    
            else
            {
                isJump = false;
            }    
        }

        if(isSlide)
        {
            float ratioPositionSlide = (this.transform.position.z - slideStart) / slideLength;
            if(ratioPositionSlide <= 1)
            {

            }
            else
            {
                isSlide = false;
                this.transform.DOKill();
                this.transform.DOScaleY(1, 0.3f);
            }
        }
        targetPosition = Vector3.zero.WithX(verticalPostios[position]).WithY(positionY).WithZ(this.transform.position.z);
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, horizontalSpeed * Time.deltaTime);
    }

    //private void FixedUpdate()
    //{
    //    //rb.velocity = Vector3.forward * moveSpeed;
    //    //if(isJump)
    //    //{
    //    //    isJump = false;
    //    //    Debug.Log("Jump");
    //    //    rb.AddForce(Vector3.up * jumpForce);
    //    //}
    //}

    private void InputKeyBoard()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }   
        else if(Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }   
        
        if(Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            Slide();
        }
    }

    private void MoveLeft()
    {
        if (position == 0)
            return;
        position -= 1;
        //targetPosition = Vector3.zero.WithX(positionArr[position]);
    }

    private void MoveRight()
    {
        if (position == 2)
            return;
        position += 1;
        //targetPosition = Vector3.zero.WithX(positionArr[position]);
    }

    private void Jump()
    {
        if(!isJump && !isSlide)
        {
            isJump = true;
            jumpStart = transform.position.z;
        }
    }

    private void Slide()
    {
        if(!isJump && !isSlide)
        {
            isSlide = true;
            this.transform.DOKill();
            this.transform.DOScaleY(0.3f, 0.3f);
            slideStart = transform.position.z;
        }
    }
}