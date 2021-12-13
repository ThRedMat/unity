using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class okk : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject ball;
    public Rigidbody rb;
    public float speed = 2;
    float x;
    float y;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        rb.AddForce(new Vector3(x, 0, y) * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        ball.transform.position = teleportTarget.transform.position;
    }
}