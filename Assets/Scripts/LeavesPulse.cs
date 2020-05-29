using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavesPulse : MonoBehaviour
{

    int active = 1;
    bool coll=false;
    ParticleSystem ps;
    float timeToStop = 0;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        ps.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (coll)
        {
            timeToStop = 1.0f;
        }

        if (timeToStop > 0)
        {
            timeToStop -= Time.deltaTime;
            if (ps.isPlaying == false)
            {
                ps.Play();
            }
        }
        else if (timeToStop <= 0 && ps.isPlaying == true)
        {
            ps.Stop();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            coll = true;
        }

    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            coll = false;
        }
    }
   
}
