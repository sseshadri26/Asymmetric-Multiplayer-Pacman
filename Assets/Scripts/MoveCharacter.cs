using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveCharacter : MonoBehaviour
{
    public float forwardVel;
    public float rotateVel;

    public float acceleration;

    public Camera cam;

    public bool gameOver=false;
    bool GOprev = false;
    public int objLeft = 13;
    CameraFollowPlayer camScript;

    Quaternion targetRotation;
    Rigidbody rb;


    float forwardInput, sideInput;

    private PhotonView PV;

    float leaveGameCountdown=-1;
    float buffer = 2f;

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

        if (PV == null || PV.IsMine)
        {
            GetComponent<MeshRenderer>().enabled = true;
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

            if (GOprev == false)
            {
                checkEndPlayer();
            }
            GetInput();


            Run();


            Turn();

            if (buffer>0)
            {
                buffer -= Time.deltaTime;
            }

            if (leaveGameCountdown > 0)
            {
                leaveGameCountdown -= Time.deltaTime;
                if (leaveGameCountdown <= 0)
                {
                    PV.RPC("QuitGame", RpcTarget.All);
                }
            }
            if (GOprev == false)
            {
                checkEndGuard();
            }

        }




        



    }

    [PunRPC]
    void QuitGame()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);

    }

    [PunRPC]
    void EndGameGuardWins()
    {
        GameObject.Find("Canvas").GetComponentInChildren<Text>().text = "Guards Win!";
        leaveGameCountdown = 5;
        GOprev = true;

        GetComponent<MeshRenderer>().enabled = true;

    }

    [PunRPC]
    void EndGamePlayerWins()
    {
        GameObject.Find("Canvas").GetComponentInChildren<Text>().text = "Player Wins!";
        leaveGameCountdown = 5;
        GOprev = true;
        GetComponent<MeshRenderer>().enabled = true;

    }


    void checkEndGuard()
    {

        if (gameOver == true)
        {
            Debug.Log("checkEndGuard is true");
            PV.RPC("EndGameGuardWins", RpcTarget.All);
        }

    }

    void checkEndPlayer()
    {
        if (objLeft==0)
        {
            Debug.Log("checkEndPlayer is true");
            PV.RPC("EndGamePlayerWins", RpcTarget.All);
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


    private void OnTriggerEnter(Collider collision)
    {
        if (buffer <= 0)
        {
            if (collision.gameObject.tag.Equals("Objective"))
            {
                if(collision.gameObject.GetComponent<DissapearObj>().prev==false)
                {
                    objLeft -= 1;
                    buffer = 0.2f;
                }
                
            }
        }

    }

}
