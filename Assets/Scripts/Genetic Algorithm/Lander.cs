using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LanderAction {
    None,
    Thrust,
    RotateRight,
    RotateLeft,
    actionsCount
}

public class Lander : MonoBehaviour {

    Rigidbody _rigidBody;
    GAgent _geneticSelf;

    float _actionTimer = 0;

	void Start () {
        _rigidBody = GetComponent<Rigidbody>();
        _geneticSelf = GetComponent<GAgent>();
	}
	
	void Update () {
        executeAction(_geneticSelf.getChromosome().getGenes()[_geneticSelf.getGenIndex()].getAction(), 
                      _geneticSelf.getChromosome().getGenes()[_geneticSelf.getGenIndex()].getTime());
    }

    void rotateLeft() {
        _rigidBody.AddTorque(transform.forward * 0.5f);
    }

    void rotateRight() {
        _rigidBody.AddTorque(-transform.forward * 0.5f);
    }

    void thrust() {
        _rigidBody.AddForce(transform.up, ForceMode.Acceleration);
    }

    void executeAction(LanderAction action, float time) {
        switch (action)
        {
            case LanderAction.None:
                waitForTime(time);
                break;

            case LanderAction.Thrust:
                thrust();
                waitForTime(time);
                break;

            case LanderAction.RotateRight:
                rotateRight();
                waitForTime(time);
                break;

            case LanderAction.RotateLeft:
                rotateLeft();
                waitForTime(time);
                break;

            default:
                break;
        }
    }

    void waitForTime(float time) {
        _actionTimer += Time.deltaTime;

        if (_actionTimer >= time)
        {
            _geneticSelf.nextGen();
            _actionTimer = 0;
        }
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
