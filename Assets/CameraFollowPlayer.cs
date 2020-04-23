using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    GameObject player;
    public float xOff;
    public float yOff;
    public float zOff;

    [Range(0.01f, 1f)]
    public float lag;

    Vector3 camOffset;

    Vector3 destination = Vector3.zero;
    float rotateVel;
    Camera mycam;

    bool topDown = false;





    // Start is called before the first frame update
    void Start()
    {
        camOffset = new Vector3(xOff, yOff, zOff);
        player = GameObject.Find("player");
        mycam = GetComponent<Camera>();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            topDown = !topDown;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPos;

        if (topDown)
        {
            
            newPos = new Vector3(0, 21, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(90, 0, 0), lag);

            
        }
        else
        {
            newPos = player.transform.position + camOffset;
            transform.LookAt(mycam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mycam.nearClipPlane)), Vector3.up);

            Quaternion turnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotateVel, Vector3.up);
            camOffset = turnAngle * camOffset;
            transform.LookAt(player.transform);
            
        }

        transform.position = Vector3.Slerp(transform.position, newPos, lag);



    }
}
