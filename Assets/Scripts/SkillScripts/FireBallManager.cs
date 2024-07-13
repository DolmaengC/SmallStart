using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallManager : MonoBehaviour
{
    public float bulletForce;
    public float damage;

    private void Awake() {
        
        GetComponent<Rigidbody>().AddForce(transform.forward * bulletForce, ForceMode.Impulse);
        Destroy(this.gameObject, 2.0f);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Monster") {
            
            // StartCoroutine(DestroyObjectAfterDelay(other.gameObject, 0.5f));
            StartCoroutine(DestroyObjectAfterDelay(this.gameObject, 0.5f));
        }
    }

    private IEnumerator DestroyObjectAfterDelay(GameObject objectToDestroy, float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(objectToDestroy);
    }
}
