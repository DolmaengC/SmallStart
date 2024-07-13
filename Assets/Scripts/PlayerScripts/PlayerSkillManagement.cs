using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManagement : MonoBehaviour
{
    public GameObject firePosition;
    public float coolTime;
    public GameObject bullet;


    private void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "SkillBook")
        {   
            StartCoroutine(Fire(coolTime));   
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Item")
        {
            Destroy(other.gameObject);
        }
    }
    
    private IEnumerator Fire(float coolTime)
    {
        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                Instantiate(bullet, firePosition.transform.position, firePosition.transform.rotation);
                yield return new WaitForSeconds(coolTime);
            }
            yield return null;
        }
    }
}
