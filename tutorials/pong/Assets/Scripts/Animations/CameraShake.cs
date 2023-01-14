using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public float duration = .5f;
    public AnimationCurve curve;

    public void Shaking() {
        StartCoroutine(Shake());
    }

    public IEnumerator Shake() {
        
        Vector3 startPosition = transform.position;
        
        float elapsedTime = 0f;


        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = startPosition;
    }
    
}
