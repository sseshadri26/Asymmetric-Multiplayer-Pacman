using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    GameObject player;
    public int xOff;
    public int yOff;
    public int zOff;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3 (player.transform.position.x+xOff, player.transform.position.y+yOff, player.transform.position.z +zOff);
    
    }
}
