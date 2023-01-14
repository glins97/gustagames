using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{

    [SerializeField]
    public bool isPlayer1 = false;
    public float speed = 10f;
    public float maxSpeed = 10f;
    public float dashDistance = 2f;
    public float dashCooldown = 0f;

    [SerializeField]
    public float maxY = 3.4f;

    public Rigidbody2D rb;

    private KeyCode moveUp;
    private KeyCode moveDown;
    private KeyCode lastDirection;

    private float time = 0;
    private float lastDashTime;
    private bool dashKeyIsPressed = false;
    private bool previousDashKeyIsPressed = false;


    public void Reset() {
        rb.velocity = new Vector2(0, 0);
            
        if (isPlayer1) {
            rb.position = new Vector2(-8, 0);
            moveUp = KeyCode.W;
            moveDown = KeyCode.S;
        }
        else {
            rb.position = new Vector2(8, 0);
            moveUp = KeyCode.I;
            moveDown = KeyCode.K;
        }
    }  

    public void Start() {
        rb = GetComponent<Rigidbody2D>();
        Reset();
    }

    public bool dashConditionsAreMet() {
        return (dashKeyIsPressed && !previousDashKeyIsPressed && time - lastDashTime > dashCooldown);
    }

    public void performDash(KeyCode direction) {
        if (direction == moveUp)
            rb.position = new Vector2(rb.position.x, rb.position.y + dashDistance);
        else if (direction == moveDown)
            rb.position = new Vector2(rb.position.x, rb.position.y - dashDistance);

        lastDashTime = time;
    }

    public void FixedUpdate() {
        float yMovementDirection = 0f;
        time += Time.deltaTime;

        previousDashKeyIsPressed = dashKeyIsPressed;
        dashKeyIsPressed = Input.GetKey(KeyCode.Space);
        // Debug.Log($"{time} {time - lastDashTime} {time - dashReleaseTime}");

        if (Input.GetKey(moveUp)) {
            yMovementDirection = 1f;
            lastDirection = moveUp;
        }
        else if (Input.GetKey(moveDown)) {
            yMovementDirection = -1f;
            lastDirection = moveDown;
        }
        else {
            yMovementDirection = 0f;
        }

        if (dashConditionsAreMet())
            performDash(lastDirection);
    
        float tempSpeed = speed;
        if ((isPlayer1 && Input.GetKey(KeyCode.LeftShift)) || (!isPlayer1 && Input.GetKey(KeyCode.RightShift)))
            tempSpeed = maxSpeed;

        float targetYPosition = rb.position.y + yMovementDirection * tempSpeed * Time.deltaTime;
        if (targetYPosition > maxY)
            targetYPosition = maxY;
        else if (targetYPosition < -maxY)
            targetYPosition = -maxY;
                    
        rb.position = new Vector2(rb.position.x, targetYPosition);

    }
}
