using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeLogic : MonoBehaviour
{
    public float speed = 10f;
    private BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontal, 0, vertical) * Time.deltaTime * speed);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Zone"))
        {
            Debug.Log("Entered Zone");
        }
    }

}
