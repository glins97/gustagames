using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BallMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private AudioSource paddleTick;
    private CameraShake camshake;
    public Animator squashStretchAnimator;
    
    [SerializeField]
    public float speed = 10f;
    public float maxSpeed = 10f;
    public float minSpeed = 5f;
    public float speedIncrease = 0.1f;
    public float maxCollisionAngle = 45f;

    public float releaseTime = 1f;

    public TextMeshProUGUI player1Score;
    public TextMeshProUGUI player2Score;

    private Vector2 _previousVelocity;
    private bool _isReleased = false;
    private float _hitTime = 0f;
    private int _player1Score = 0;
    private int _player2Score = 0;

    public void Update() {

        if (!_isReleased) 
            return;
        
        Debug.DrawRay(rb.position, rb.velocity, Color.green, 1f);

            // position code below is wrong lol... position is already handled by velocity
            // ... buts its more fun this way :P
            // rb.position = new Vector2(rb.position.x, rb.position.y) + rb.velocity * Time.deltaTime;
            // ------------------

            _previousVelocity = rb.velocity;
    }

    public void Start() {
        rb = GetComponent<Rigidbody2D>();
        paddleTick = GetComponent<AudioSource>();
        Reset();
        Release();
    }
    
    public void Reset() {
        _isReleased = false;
        rb.velocity = new Vector2(0, 0);
        rb.position = new Vector2(0, 0);
    }

    public void Release() {
        _isReleased = true;
        rb.velocity = new Vector2(-speed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log($"BallMovement OnTriggerEnter2D {collision.gameObject.tag}");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log($"BallMovement OnCollisionEnter2D {collision.gameObject.tag} {rb.velocity} {_previousVelocity}");

        if (collision.gameObject.tag == "Paddle") {
            FindObjectOfType<HitStop>().Stop(0.1f);
            FindObjectOfType<CameraShake>().Shaking();
            FindObjectOfType<SimpleFlash>().Flash();
            squashStretchAnimator.SetTrigger("collision");

            paddleTick.pitch = Random.Range(1.1f, 1.7f);
            paddleTick.Play();

            Vector2 median = Vector2.zero;
            foreach (ContactPoint2D point in collision.contacts)
            {
                median += point.point;
                Debug.DrawRay(point.point, Vector3.right, Color.red, 1f);
            }
            median /= collision.contactCount;
            Debug.DrawRay(median, Vector3.right, Color.cyan, 1f);

            // calculate relative distance from center (between -1 and 1)
            float absoluteDistanceFromCenter = median.y - collision.transform.position.y;
            float relativeDistanceFromCenter = absoluteDistanceFromCenter * 2 / collision.collider.bounds.size.y;

            // calculate rotation using quaternion
            int angleSign = transform.position.x < 0 ? 1 : -1;
            float angle = relativeDistanceFromCenter * maxCollisionAngle * angleSign;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            Debug.DrawRay(median, Vector3.forward, Color.yellow, 1f);

            // calculate direction / velocity
            Vector2 dir = transform.position.x < 0 ? Vector2.right : Vector2.left;
            Vector2 velocity = rot * dir * (rb.velocity.magnitude + speedIncrease);
            rb.velocity = velocity;
            Debug.DrawRay(median, velocity, Color.green, 1f);
        }
        else if (collision.gameObject.tag == "TopWall" || collision.gameObject.tag == "BottomWall") {
            rb.velocity = new Vector2(_previousVelocity.x, -_previousVelocity.y);
        }
        else if (collision.gameObject.tag == "LeftWall") {
            rb.velocity = new Vector2(speed, 0);
            rb.position = new Vector2(0, 0);
            _hitTime = Time.time;

            _player2Score += 1;
            player2Score.text = _player2Score.ToString();
        }
        else if (collision.gameObject.tag == "RightWall") {
            rb.velocity = new Vector2(-speed, 0);
            rb.position = new Vector2(0, 0);
            _hitTime = Time.time;
            
            _player1Score += 1;
            player1Score.text = _player1Score.ToString();
        }
    }

}

