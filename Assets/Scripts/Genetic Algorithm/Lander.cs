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

    int _landed = 1;
    float _flyingTime = 0;

    Vector3 _startPosition;

	void Start () {
        _rigidBody = GetComponent<Rigidbody>();
	}

    public void setGAgent(GAgent gAgent) {
        _geneticSelf = gAgent;
    }

    public void setStartPosition(Vector3 startPosition) {
        _startPosition = startPosition;
    }
	
	void FixedUpdate () {
        if (transform.position.x <= -15)
            transform.position = new Vector3(15, transform.position.y, 0);
        if(transform.position.x >= 15)
            transform.position = new Vector3(-15, transform.position.y, 0);
        if(transform.position.y > 16)
            transform.position = new Vector3(transform.position.x, 16, 0);

        if (_geneticSelf.getGenIndex() < _geneticSelf.getChromosome().getGenes().Count)
            executeAction(_geneticSelf.getChromosome().getGenes()[_geneticSelf.getGenIndex()].getAction(),
                          _geneticSelf.getChromosome().getGenes()[_geneticSelf.getGenIndex()].getTime());

        _geneticSelf.getChromosome().inSimulation(_rigidBody.velocity,
                                                  GameObject.FindGameObjectWithTag("platform").transform.position - gameObject.transform.position,
                                                  _flyingTime, _landed,
                                                  transform.rotation.eulerAngles.z);
    }

    public void reset() {
        transform.position = _startPosition;
        transform.rotation = Quaternion.identity;
        _rigidBody.ResetCenterOfMass();
        _rigidBody.ResetInertiaTensor();
        _rigidBody.Sleep();
        _geneticSelf.resetGenIndex();
        _actionTimer = 0;
        _flyingTime = 0;
        Debug.Log(_geneticSelf.getGenIndex());
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
        _actionTimer += Time.fixedDeltaTime;
        
        if (_actionTimer >= time)
        {
            _geneticSelf.nextGen();
            _actionTimer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "platform") {
            _rigidBody.Sleep();
            _landed = 100;
        }

        _landed = 0;
    }
}
