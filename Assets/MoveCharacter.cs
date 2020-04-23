using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    public float forwardVel;
    public float rotateVel;

    Quaternion targetRotation;
    Rigidbody rb;
    float forwardInput, turnInput;

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }



    // Start is called before the first frame update
    void Start()
    {
        targetRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();

        forwardInput = 0;
        turnInput = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Turn();
        Run();
        
        
    }

    private void FixedUpdate()
    {
        
    }

    void GetInput()
    {
        //forwardInput = Input.GetAxis("Vertical");
        //turnInput = Input.GetAxis("Horzontal");
        if (Input.GetKey(KeyCode.W))
        {
            forwardInput = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            forwardInput = -1;
        }
        else
        {
            forwardInput = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            turnInput = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            turnInput = 1;
        }
        else
        {
            turnInput = 0;
        }
    }

    void Run()
    {
        rb.velocity = transform.forward * forwardInput * forwardVel;
        Debug.Log(transform.forward );
        Debug.Log("fi" + forwardInput);
        Debug.Log("fv" + forwardVel);


    }

    void Turn()
    {


        targetRotation *= Quaternion.AngleAxis(rotateVel * turnInput * Time.deltaTime, Vector3.up);
        transform.rotation = targetRotation;

    }
}
