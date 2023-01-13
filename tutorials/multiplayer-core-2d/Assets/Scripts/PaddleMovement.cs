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

    [SerializeField]
    public float maxY = 3.4f;

    private Rigidbody2D rb;
    KeyCode moveUp;
    KeyCode moveDown;

    private float time = 0;
    private float dashPressTime;
    private float dashReleaseTime;
    private KeyCode dashDirection = KeyCode.None;

    public void Reset() {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, 0);
        rb.position = new Vector2(rb.position.x, 0);
    }  

    public void Start() {
        if (isPlayer1)
        {
            moveUp = KeyCode.W;
            moveDown = KeyCode.S;
        }
        else
        {
            moveUp = KeyCode.I;
            moveDown = KeyCode.K;
        }
    }

    public void resetDashConditions() {
        dashDirection = KeyCode.None;
        dashPressTime = 0;
        dashReleaseTime = 0;
    }

    public void performDash(KeyCode direction) {
        if (direction == moveUp)
            rb.position = new Vector2(rb.position.x, rb.position.y + dashDistance);
        else if (direction == moveDown)
            rb.position = new Vector2(rb.position.x, rb.position.y - dashDistance);

        dashPressTime = 0;
        dashReleaseTime = 0;
    }

    public void Update() {
        float movement = 0f;
        time += Time.deltaTime;

        Debug.Log($"{time} {time - dashPressTime} {time - dashReleaseTime}");

        if (time - dashPressTime > 0.3f) {
            resetDashConditions();
        }

        if (Input.GetKey(moveUp)) {
            movement = 1f;

            if (dashDirection == moveDown)
                resetDashConditions();
            
            if (time - dashPressTime < 0.3f && time - dashReleaseTime < 0.15f)
                performDash(moveUp);

            if (dashPressTime == 0) {
                dashPressTime = time;
                dashDirection = moveUp;
            }

        }
        else if (Input.GetKey(moveDown)) {
            movement = -1f;

            if (dashDirection == moveUp)
                resetDashConditions();

            if (time - dashPressTime < 0.3f && time - dashReleaseTime < 0.15f)
                performDash(moveDown);

            if (dashPressTime == 0) {
                dashPressTime = time;
                dashDirection = moveDown;
            }
        }
        else {
            movement = 0f;
            if (dashPressTime != 0)
                dashReleaseTime = time;
        }

        float tempSpeed = speed;
        if ((isPlayer1 && Input.GetKey(KeyCode.LeftShift)) || (!isPlayer1 && Input.GetKey(KeyCode.RightShift)))
            tempSpeed = maxSpeed;

        float targetYPosition = rb.position.y + movement * tempSpeed * Time.deltaTime;
        if (targetYPosition > maxY)
            targetYPosition = maxY;
        else if (targetYPosition < -maxY)
            targetYPosition = -maxY;

        rb.position = new Vector2(rb.position.x, targetYPosition);

    }
}
