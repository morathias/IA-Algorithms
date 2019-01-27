using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NNLander : MonoBehaviour {

    Rigidbody _rigidBody;
    NNAgent _geneticSelf;
    NeuralNetwork _brain;

    float _actionTimer = 0;

    int _landed = 1;
    float _flyingTime = 0;

    Vector3 _startPosition;

    Transform _platform;

    Text _score;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _platform = GameObject.FindGameObjectWithTag("platform").transform;
    }

    public void setNeuralNetwork(NeuralNetwork brain) {
        _brain = brain;
    }

    public NeuralNetwork getBrain() {
        return _brain;
    }

    public void setGAgent(NNAgent gAgent)
    {
        _geneticSelf = gAgent;
    }

    public void setStartPosition(Vector3 startPosition)
    {
        _startPosition = startPosition;
    }

    public void setScoreTxt(Text score) {
        _score = score;
    }

    void FixedUpdate()
    {
        if (transform.position.x <= -15)
            transform.position = new Vector3(15, transform.position.y, 0);
        if (transform.position.x >= 15)
            transform.position = new Vector3(-15, transform.position.y, 0);
        if (transform.position.y > 16)
            transform.position = new Vector3(transform.position.x, 16, 0);

        Vector3 platformDir = _platform.position - gameObject.transform.position;
       

        _geneticSelf.getChromosome().inSimulation(_rigidBody.velocity,
                                                  platformDir,
                                                  _flyingTime, _landed,
                                                  transform.up);

        _score.text = _geneticSelf.getChromosome().agentName + " " + _geneticSelf.getChromosome().getScore();
        platformDir.Normalize();
        List<float> inputs = new List<float>();
        inputs.Add(platformDir.x);
        inputs.Add(platformDir.y);
        inputs.Add(transform.up.x);
        inputs.Add(transform.up.y);

        List<float> outputs = _brain.update(inputs);

        //Debug.Log("thrust: " + outputs[0] + " rotate r: " + outputs[1] + " rotate l: " + outputs[2]);
        if (_landed == 2)
            return;

        if (gameObject.name == "agent0") {
            Debug.Log("thrust " + (float)outputs[0] + " rotate right: " + (float)outputs[1] + " rotate left" + (float)outputs[2]);
        }
        thrust(outputs[0]);
        rotateRight(outputs[1]);
        rotateLeft(outputs[2]);
    }

    public void reset()
    {
        transform.position = _startPosition;
        transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        _rigidBody.ResetCenterOfMass();
        _rigidBody.ResetInertiaTensor();
        _rigidBody.Sleep();
        _geneticSelf.resetGenIndex();
        _geneticSelf.getChromosome().ResetScore();
        _actionTimer = 0;
        _flyingTime = 0;
        _landed = 1;
    }

    void rotateLeft(float amount)
    {
        _rigidBody.AddTorque(transform.forward * amount);
    }

    void rotateRight(float amount)
    {
        _rigidBody.AddTorque(-transform.forward * amount);
    }

    void thrust(float amount)
    {
        _rigidBody.AddForce(transform.up * amount, ForceMode.Acceleration);
    }

    void waitForTime(float time)
    {
        _actionTimer += Time.fixedDeltaTime;

        if (_actionTimer >= time)
        {
            _geneticSelf.nextGen();
            _actionTimer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "platform")
        {
            _rigidBody.Sleep();
            _landed = 2;
        }

        //_landed = 0;
    }
}
