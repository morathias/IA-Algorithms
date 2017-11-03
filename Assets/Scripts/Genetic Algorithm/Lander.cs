using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lander : MonoBehaviour {

    Rigidbody _rigidBody;

	// Use this for initialization
	void Start () {
        _rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
            _rigidBody.AddForce(transform.up, ForceMode.Acceleration);

        if (Input.GetKey(KeyCode.D))
            _rigidBody.AddTorque(-transform.forward * 0.5f);
        if (Input.GetKey(KeyCode.A))
            _rigidBody.AddTorque(transform.forward * 0.5f);

        Debug.Log(Mathf.Abs(_rigidBody.velocity.y));

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (transform.rotation.eulerAngles.z < Quaternion.Euler(0, 0, 340).eulerAngles.z && 
            transform.rotation.eulerAngles.z > Quaternion.Euler(0, 0, 20).eulerAngles.z)
            Destroy(gameObject);

        if (_rigidBody.velocity.y > 1)
            Destroy(gameObject);

        if (collision.transform.tag == "platform") {
            _rigidBody.Sleep();
        }
    }
}
