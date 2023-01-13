using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    private AudioSource collisionSound;
    // Start is called before the first frame update
    void Start()
    {
        collisionSound = GetComponent<AudioSource>();
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        collisionSound.pitch = Random.Range(0.6f, 1.3f);
        collisionSound.Play();
    }
}
