using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{   
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpSpeed = 16f; 

    [SerializeField] private Transform groundCheck; //aqui fazemos uma var em transform pra conseguirmos pegar a posição do transform do gameobject
    [SerializeField] private LayerMask groundLayer; //serve para filtrar uma layer
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.4f, 0.1f); //aqui é só um jeito de definir o tamanho do checker
    [SerializeField] private bool grounded;
    [SerializeField] private Animator visualAnimator;//arrastar o animator pra cá
    [SerializeField] private Transform visualTransform;
    private bool wasGrounded;
    
    private Rigidbody2D rb; 
    private float movementX;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grounded = IsGrounded();
        wasGrounded = grounded;
        rb = GetComponent<Rigidbody2D>();
    }
    private bool IsGrounded()
    {
        Collider2D ground = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer); //aqui ele devolve o collider na overlap box criada
        return ground != null;
    }
    private void OnMove(InputValue movementValue)
    {
        movementX = movementValue.Get<float>();
        visualAnimator.SetFloat("Speed", Mathf.Abs(movementX));

        if (movementX != 0)
        {
            Vector3 scale = visualTransform.transform.localScale; //atribui os anteriores valores de scale 
            scale.x = movementX > 0 ? 1 : -1; //ve se o x é maior ou menor
            visualTransform.transform.localScale = scale; //atribui a scale pro transformador.
        }

    }

    private void OnJump(InputValue jumpValue)
    {
        if(jumpValue.isPressed && grounded){
            rb.linearVelocityY = jumpSpeed;

            
            visualAnimator.ResetTrigger("Land");
            visualAnimator.SetTrigger("Jump");
        }
    }


    void FixedUpdate()
    {
        wasGrounded = grounded;
        grounded = IsGrounded();
        

        rb.linearVelocityX = movementX * speed;
        
        if(!wasGrounded && grounded)
        {
            visualAnimator.ResetTrigger("Jump");
            visualAnimator.SetTrigger("Land");
        }
       
    }


    
}

