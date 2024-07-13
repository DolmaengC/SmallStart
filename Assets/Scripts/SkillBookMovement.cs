using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBookMovement : MonoBehaviour
{
    public float rotSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rotSpeed = 100;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime, 0));
    }

}
