using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Vector3 newPos;
    public GameObject ghost;
    public float speed = 100.0f;
    private bool overloadCheck;
    private float playerX;
    private float playerZ;
    
    
    // Start is called before the first frame update
    void Start()
    {
        newPos = new Vector3(Random.Range(-67,66), 4, Random.Range(-51, 83));
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
            ghost.GetComponent<MeshRenderer>().material.color = new Color(168f, 0f, 255f, 100f);
            newPosition();
            moveToPosition();
        }
        else if (overloadCheck == true)
        {
            ghost.GetComponent<MeshRenderer>().material.color = new Color(168f, 0f, 255f, 255f);
            ChasePlayer();
        }
    }
    
    private void ChasePlayer()
    {
        playerX = GameObject.Find("Player").transform.position.x;
        playerZ = GameObject.Find("Player").transform.position.z;
        newPos = new Vector3(playerX, 4, playerZ);

        transform.position = Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);
        transform.LookAt(newPos);
    }
    
    private void newPosition()
    {
        if (gameObject.transform.position == newPos)
        {
            newPos = new Vector3(Random.Range(-67, 66), 4, Random.Range(-51, 83));
        }
    }

    private void moveToPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);
        transform.LookAt(newPos);
    }

    
}
