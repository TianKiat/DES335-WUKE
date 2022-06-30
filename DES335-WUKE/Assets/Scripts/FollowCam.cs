using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public float FollowSpeed = 5.0f;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position - (Vector3.forward * 10.0f), FollowSpeed * Time.deltaTime);
    }
}
