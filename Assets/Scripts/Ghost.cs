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
    NavMeshAgent ghostNavMesh;

    public Appliance[] appliance = new Appliance[38]; 
    // Start is called before the first frame update
    void Start()
    {
        appliance = GameManager.instance.appliances;
        ghostNavMesh = GetComponent<NavMeshAgent>();

        newPos = appliance[Random.Range(0, 39)].transform.position;
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
            ghost.GetComponent<MeshRenderer>().material.color = new Color(0.66f, 0f, 1f, 0.5f);
            newPosition();
            moveToPosition();
        }
        else if (overloadCheck == true)
        {
            ghost.GetComponent<MeshRenderer>().material.color = new Color(0.66f, 0f, 1f, 1f);
            ChasePlayer();
        }
    }
    
    private void ChasePlayer()
    {
        playerX = GameObject.Find("Player").transform.position.x;
        playerZ = GameObject.Find("Player").transform.position.z;
        newPos = new Vector3(playerX, 4, playerZ);

        ghostNavMesh.SetDestination(player.transform.position);
        transform.LookAt(newPos);
    }
    
    private void newPosition()
    {
        if (Vector3.Distance(ghost.transform.position, newPos) <= 3)
        {
            newPos = appliance[Random.Range(0, 39)].transform.position;
        }
    }

    private void moveToPosition()
    {
        ghostNavMesh.SetDestination(newPos);
        //transform.LookAt(newPos);
    }

    
}
