using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Vector3 newPos;
    public float speed = 100.0f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        newPos = new Vector3(Random.Range(-67,66), 4, Random.Range(-51, 83));
    }

    // Update is called once per frame
    void Update()
    {
        newPosition();
        moveToPosition();
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
