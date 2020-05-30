using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GuardAI : MonoBehaviour
{
    Renderer renderer;


    public Text text;

    public float sightDistance;
    public float frontSightDistance;
    public float playerSpotDistance;

    RaycastHit leftHit, rightHit, frontHit;

    float forwardInput, sideInput;
    public float acceleration;
    public float forwardVel;
    Rigidbody rb;
    public bool playerControl = false;
    bool bump = false;

    public Material m1;
    public Material m2;

    float enemyLength = 0.3f;

    //three floats to represent distances to objects left, right, and front
    float l, r, f;

    private PhotonView PV;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        l = r = f = 0;

        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        LookForWalls();
        MovementLogic(f, l, r);

        if (PV == null || PV.IsMine)
        {

            if (playerControl)
            {
                GetInput();


                Run();


                Turn();
            }

            else
            {


                RunAI();
            }
        }
        if (!PV.IsMine)
        {
            SpotPlayer();

        }
    }


    void RunAI()
    {
        Vector3 desiredVel = (transform.forward * forwardInput * forwardVel + transform.right * sideInput * forwardVel) ;
        rb.velocity = desiredVel;

        Vector3 turnDir = new Vector3(sideInput, 0f, forwardInput);
        if (turnDir != Vector3.zero)
        {

            transform.rotation = Quaternion.LookRotation(transform.TransformDirection(turnDir));
        }


    }

    void Run()
    {
        Vector3 desiredVel = (Vector3.forward * forwardInput * forwardVel + Vector3.right * sideInput * forwardVel) ;
        //rb.velocity = Vector3.Slerp(rb.velocity, desiredVel, acceleration);
        rb.velocity = desiredVel;


    }

    public void changeColor(bool playerControl)
    {
        if (playerControl)
        {
            renderer.material = m1;
        }
        else
        {
            renderer.material = m2;
        }
    }


    void SpotPlayer()
    {
        bool spottedPlayer = false;

        //what's directly in front? (for seeing the player)
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out frontHit, playerSpotDistance))
        {
            if (frontHit.collider.gameObject.tag == "Player")
            {
                spottedPlayer = true;
                player = frontHit.collider.gameObject;
            }
            if (spottedPlayer)
            {
                //what to do? end game?
                Debug.Log("Game should be over now...spotted is true");
                player.GetComponent<MoveCharacter>().gameOver = true;
            }
        }


    }


    void LookForWalls()
    {
        

        l = r = f = 0;
        float r1, r2, l1, l2; //checking front side and back sie of the enamy, to avoid getting stuck.
        r1 = r2 = l1 = l2 = 0;

        //what's directly in front? (for walls)
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out frontHit, frontSightDistance))
        {
            f = frontHit.distance;


        }




        //what's directly to the left?
        if (Physics.Raycast(transform.position + transform.forward * enemyLength, transform.TransformDirection(Vector3.left), out leftHit, sightDistance))
        {
            l1 = leftHit.distance;

        }
        if (Physics.Raycast(transform.position - transform.forward * enemyLength, transform.TransformDirection(Vector3.left), out leftHit, sightDistance))
        {
            l2 = leftHit.distance;

        }

        //what's directly to the right?
        if (Physics.Raycast(transform.position + transform.forward * enemyLength, transform.TransformDirection(Vector3.right), out rightHit, sightDistance))
        {
            r1 = rightHit.distance;

        }
        if (Physics.Raycast(transform.position - transform.forward * enemyLength, transform.TransformDirection(Vector3.right), out rightHit, sightDistance))
        {
            r2 = rightHit.distance;

        }
        r = Mathf.Max(r1, r2);
        l = Mathf.Max(l1, l2);



    }


    void MovementLogic(float f, float l, float r)
    {
        sideInput = forwardInput = 0;
        gameObject.GetComponent<Renderer>().material.color = Color.grey;


        //if in a corridor with two walls left and right, no wall in front
        if (bump == true)
        {
            forwardInput = -1;
            bump = false;
        }
        else if (l > 0 && r > 0 && f == 0)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.green;

            //if and else if statements allow ai to reposition to middle of corridor.
            //this whole thing is to get some wiggle room
            //want strafing, not turning. work on later!
            //if (l - r > 0.1)
            //{
            //    sideInput = -0.2f;
            //}
            //else if (l - r < -0.1)
            //{
            //    sideInput = 0.2f;
            //}
            //else
            //{
            sideInput = 0;
            forwardInput = 1;
            //}

        }
        //if there is a wall in front
        else if (f > 0)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;


            //check for walls left and right
            if (l == 0 && r == 0) //if no walls on left or right
            {
                int randDir = Random.Range(0, 2);
                if (randDir == 0)
                {
                    randDir = -1;
                }
                sideInput = randDir;
            }

            else if (l == 0 && r > 0) //if no walls on left but wall on right
            {

                sideInput = -1;
            }
            else if (l > 0 && r == 0) //if no walls on right but wall on left
            {

                sideInput = 1;
            }


            else //if walls eveywhere, go backwards
            {
                gameObject.GetComponent<Renderer>().material.color = Color.cyan;
                forwardInput = -1;
            }
        }

        else if (l == 0 && r == 0 && f == 0) //in free space
        {
            //can randomly turn!!

            randomTurn();

            gameObject.GetComponent<Renderer>().material.color = Color.magenta;

        }

        else if (f == 0 && r == 0 && l > 0) //wall only on left
        {
            //can randomly turn!!
            randomTurnRight();
            gameObject.GetComponent<Renderer>().material.color = Color.white;
        }


        else if (f == 0 && l == 0 && r > 0) //wall only on right
        {
            //can randomly turn!!
            randomTurnLeft();
            gameObject.GetComponent<Renderer>().material.color = Color.white;
        }


        else //what could this be?
        {
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            sideInput = 0;
            forwardInput = 0;
        }
    }



    int randTurnFrequency = -30;



    void randomTurn() //goes randomly forward, right, or left.
    {
        int randDir = Random.Range(randTurnFrequency, 5);
        if (randDir < 3)
        {
            sideInput = 0;
            forwardInput = 1;
        }
        else if (randDir == 3)
        {
            sideInput = -1;
            forwardInput = 0;
        }
        else if (randDir == 4)
        {
            sideInput = 1;
            forwardInput = 0;
        }

    }
    void randomTurnLeft() //goes randomly forward or left.
    {
        int randDir = Random.Range(randTurnFrequency, 4);
        if (randDir < 3)
        {
            sideInput = 0;
            forwardInput = 1;
        }
        else if (randDir == 3)
        {
            sideInput = -1;
            forwardInput = 0;
        }


    }

    void randomTurnRight() //goes randomly forward or right
    {
        int randDir = Random.Range(randTurnFrequency, 4);
        if (randDir < 3)
        {
            sideInput = 0;
            forwardInput = 1;
        }
        else if (randDir == 3)
        {
            sideInput = 1;
            forwardInput = 0;
        }
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


    void Turn()
    {


        Vector3 turnDir = new Vector3(sideInput, 0f, forwardInput);
        if (turnDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(turnDir);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (playerControl == false && collision.gameObject.tag == "guard")
        {
            bump = true;
            Debug.Log("Bump");
        }
    }


}
