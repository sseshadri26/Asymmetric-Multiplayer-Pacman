using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject player;

    [Range(0.01f, 1f)]
    public float lag;

    public Vector3 camOffset;

    Vector3 destination = Vector3.zero;

    [Range(5f, 30f)]
    public float rotateVel = 10;
    Camera mycam;

    public bool topDown = false;
    public float tiltUp;
    Rigidbody playerRb;



    // Start is called before the first frame update
    void Start()
    {

        playerRb = player.GetComponent<Rigidbody>();
        mycam = GetComponent<Camera>();
        //Cursor.visible = false;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            topDown = !topDown;
        }

        //Debug.Log(Input.GetAxis("Mouse X"));
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPos;

        if (topDown)
        {

            newPos = new Vector3(0, 21, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(90, 0, 0), lag / 2);
            transform.position = Vector3.Slerp(transform.position, newPos, lag / 3);


        }
        else
        {
            //transform.LookAt(mycam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mycam.nearClipPlane)), Vector3.up);




            Quaternion turnAngleHorizontal = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotateVel, Vector3.up);
            camOffset = turnAngleHorizontal * camOffset;

            transform.LookAt(playerRb.transform.position + Vector3.up * tiltUp);

            newPos = playerRb.transform.position + camOffset;

            transform.position = newPos;
            //transform.position = Vector3.Slerp(transform.position, newPos, lag);



        }





    }
}
