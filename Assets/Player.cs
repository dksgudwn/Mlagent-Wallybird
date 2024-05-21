using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Player : Agent
{

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpForce;
    private float fallThreshold = 20f;
    [SerializeField]
    float previousHeight;

    private Rigidbody2D _rb;

    public int Count = 0;

    public GameObject image;
    Camera _camera;

    float moveInput;

    GameObject recentGround;

    GroundSpawner groundSpawner;

    private void Update()
    {
        if (transform.position.y > previousHeight)
        {
            previousHeight = transform.position.y; // 새로운 높이로 업데이트
        }

        if (transform.position.y < previousHeight - fallThreshold) // 최고높이에서 좀 빼고 그거보다 작으면 떨어진걸로
        {
            AddReward(-1f);
            print("사망");
            EndEpisode();
        }

        if (transform.position.y > 39900)
        {
            image.SetActive(true);
            print("클리어");
        }

        //if (transform.position.x > 7)
        //    transform.position = new Vector3(-7, transform.position.y);

        //if (transform.position.x < -7)
        //    transform.position = new Vector3(7, transform.position.y);
    }

    void FixedUpdate()
    {
        //moveInput = Input.GetAxisRaw("Horizontal");

        _rb.velocity = new Vector2(moveInput * moveSpeed, _rb.velocity.y);
    }

    public override void Initialize()
    {
        _camera = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
        groundSpawner = transform.parent.GetComponent<GroundSpawner>();
    }

    public override void OnEpisodeBegin()
    {
        previousHeight = transform.position.y;
        transform.position = new Vector3(transform.parent.position.x, 0, 0);

        groundSpawner.Destory();
        groundSpawner.Spawn();
        print(Count);
        Count = 0;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(transform.position);0
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var DiscreteActions = actions.DiscreteActions;

        switch (DiscreteActions[0])
        {
            case 0:
                moveInput = 0;
                break;
            case 1:
                moveInput = -1;
                break;
            case 2:
                moveInput = 1;
                break;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var DiscreteActionsOut = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
            DiscreteActionsOut[0] = 0;
        else if (Input.GetKey(KeyCode.D))
            DiscreteActionsOut[0] = 2;
        else if (Input.GetKey(KeyCode.A))
            DiscreteActionsOut[0] = 1;
        else
            DiscreteActionsOut[0] = 0;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_rb.velocity.y < 0 && collision.gameObject.CompareTag("Ground"))
        {
            if (collision.gameObject == recentGround)
            {
                AddReward(-1f);
            }
            else
            {
                Count++;
                recentGround = collision.gameObject;
                print("보상");
                AddReward(10f);
            }
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        }

        if (collision.gameObject.CompareTag("DestroyZone"))
        {
            AddReward(-1f);
            print("사망");
            EndEpisode();
        }
    }
}
