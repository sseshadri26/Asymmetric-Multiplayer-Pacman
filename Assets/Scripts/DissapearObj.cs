using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearObj : MonoBehaviour
{

    int active = 1;
    public bool coll = false;
    public bool prev = false;
    ParticleSystem ps;
    float timeToStop = 0;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (coll&&prev==false)
        {
            prev = true;
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
            Destroy(gameObject);
            
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            coll = true;
        }

    }

}
