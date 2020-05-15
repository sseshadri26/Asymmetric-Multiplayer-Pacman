using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    public float forwardVel;
    public float rotateVel;

    public float acceleration;

    public Camera cam;
    CameraFollowPlayer camScript;

    Quaternion targetRotation;
    Rigidbody rb;


    float forwardInput, sideInput;

    private PhotonView PV;

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }



    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        if (cam==null)
        {
            cam = (GameObject.Find("Main Camera").GetComponent<Camera>());
        }
        

        targetRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();

        forwardInput = 0;
        sideInput = 0;
        camScript = cam.GetComponent<CameraFollowPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PV == null || PV.IsMine)
        {
            GetInput();


            Run();


            Turn();
        }

        



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
            sideInput = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            sideInput = 1;
        }
        else
        {
            sideInput = 0;
        }
    }

    void Run()
    {
        
        if (camScript==null || camScript.topDown)
        {
            Vector3 desiredVel = (Vector3.forward * forwardInput * forwardVel + Vector3.right * sideInput * forwardVel) + new Vector3(0, rb.velocity.y, 0);
            
            //implement acceleration later
            //rb.velocity = Vector3.Slerp(rb.velocity, desiredVel, acceleration);
            rb.velocity = desiredVel;
            







        }
        else
        {
            Vector3 desiredVel = (transform.forward * forwardInput * forwardVel + transform.right * sideInput * forwardVel) + new Vector3(0, rb.velocity.y, 0);
            rb.velocity = desiredVel;
            //rb.velocity = Vector3.Slerp(rb.velocity, desiredVel, acceleration);
        }




    }

    void Turn()
    {

        //Quaternion turnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotateVel, Vector3.up);
        //transform.rotation = targetRotation;
        ////targetRotation *= Quaternion.AngleAxis(rotateVel * turnInput * Time.deltaTime, Vector3.up);
        ////transform.rotation = targetRotation;
        if (camScript==null || camScript.topDown)
        {
            Vector3 turnDir = new Vector3(sideInput, 0f, forwardInput);
            if (turnDir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(turnDir);
            }
        }
        else
        {
            Vector3 cameraAtPlayerLevelPos = new Vector3(cam.transform.position.x, transform.position.y, cam.transform.position.z);

            transform.LookAt(2 * transform.position - cameraAtPlayerLevelPos);
        }

    }
}
