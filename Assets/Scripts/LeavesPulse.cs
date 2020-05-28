using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavesPulse : MonoBehaviour
{
    public Material grass1;
    public Material grass2;
    Renderer rend;
    int active = 1;
    bool coll;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //while (coll)
        //{
            float time = 0;
            time += Time.deltaTime;
            if (time > 1)
            {
                if (active == 1)
                {
                    rend.material = grass2;
                }
                else if (active == 2)
                {
                    rend.material = grass1;
                }
                time = 0;
            }
        //}
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            coll = true;
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            coll = false;
        }
    }
}
