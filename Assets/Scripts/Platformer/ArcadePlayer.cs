using UnityEngine;
using UnityEngine.InputSystem;

public class ArcadePlayer : MonoBehaviour
{
    Rigidbody2D body;

    [SerializeField] private float moveSpeed = 5.0f; 
    [SerializeField] private float jumpHeight = 3.0f;

    private Vector2 direction;

    public InputActionReference move;
    public InputActionReference jump;

    [SerializeField] private bool canJump = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        direction = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate(){
        body.linearVelocityX = -direction.x * moveSpeed;
    }

    private void OnEnable(){
        jump.action.started += Jump;
    }

    private void Jump(InputAction.CallbackContext context){
        if(canJump){
            body.linearVelocityY += jumpHeight;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Platform")){
            canJump = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Platform")){
            canJump = false;
        }
    }
}
