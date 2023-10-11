using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    private Vector3 newPos;
    public GameObject ghost;
    public GameObject player;
    public float speed = 100.0f;
    private bool overloadCheck;
    private float playerX;
    private float playerZ;
    public NavMeshAgent ghostNavMesh;

    private float speedTime;
    private float speedTimer = 15f;
    private float speedRate = 2f;


    public Appliance[] appliance = new Appliance[45]; 
    // Start is called before the first frame update
    void Start()
    {
        appliance = GameManager.instance.appliances;
        ghostNavMesh = GetComponent<NavMeshAgent>();

        newPos = appliance[Random.Range(0, 45)].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        checkOverload();
    }

    private void checkOverload()
    {
        overloadCheck = GameManager.instance.overload;
        if (overloadCheck == false)
        {
            ghost.GetComponent<MeshRenderer>().material.color = new Color(0.66f, 0f, 1f, 0f);
            newPosition();
            moveToPosition();
            updateSpeed();
        }
        else if (overloadCheck == true)
        {
            ghost.GetComponent<MeshRenderer>().material.color = new Color(0.66f, 0f, 1f, 0.7f);
            ChasePlayer();
            ghostNavMesh.speed = 20f;
        }
    }
    
    private void updateSpeed()
    {
        speedTime = speedTime + Time.deltaTime;
        if (speedTime > speedTimer)
        {
            ghostNavMesh.speed = ghostNavMesh.speed + speedRate;
            speedTime = 0;
        }
    }
    
    private void ChasePlayer()
    {
        ghostNavMesh.SetDestination(player.transform.position);
        transform.LookAt(player.transform.position);
    }
    
    private void newPosition()
    {
        if (Vector3.Distance(ghost.transform.position, newPos) <= 3)
        {
            newPos = appliance[Random.Range(0, 45)].transform.position;
        }
    }

    private void moveToPosition()
    {
        ghostNavMesh.SetDestination(newPos);
        //transform.LookAt(newPos);
    }

    
}
