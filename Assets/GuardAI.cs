using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAI : MonoBehaviour
{

    //Variables
    //public float speed = 20;
    //public float rotateSpeed = 10;

    //public float directionDistance = 5;
    //public float targetDistance = 5;
    //public float followSpeed = 11;
    public GameObject player;

    public float sightDistance;

    RaycastHit leftHit, rightHit, frontHit;

    float forwardInput, sideInput;
    public float acceleration;
    public float forwardVel;
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //three floats to represent distances to objects left, right, and front
        float l, r, f;
        l = r = f = 0;

        //what's directly in front?
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out frontHit, sightDistance))
        {
            f = frontHit.distance;
            
        }

        //what's directly to the left?
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out leftHit, sightDistance))
        {
            l = leftHit.distance;

        }

        //what's directly to the right?
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out rightHit, sightDistance))
        {
            r = rightHit.distance;

        }


        if (l > 0 && r > 0 && f == 0)
        {
            if (l-r>0.1)
            {
                sideInput = -0.2f;
            }
            else if (l - r < -0.1)
            {
                sideInput = 0.2f;
            }
            else
            {
                sideInput = 0;
                forwardInput = 1;
            }
            Debug.Log(l - r);
        }
        else
        {
            sideInput = 0;
            forwardInput = 0;
        }
        Run();

    }

    void Run()
    {
        Vector3 desiredVel = transform.forward * forwardInput * forwardVel + transform.right * sideInput * forwardVel;
        rb.velocity = desiredVel;


    }
}
