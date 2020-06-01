using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerGuardClickController : MonoBehaviour
{
    public GameObject currentGuard;
    GuardAI guardAIScript;
    public GameObject[] guards = new GameObject[5];
    private PhotonView PV;
    int currentGuardNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            findClick();
            spaceChange();
        }
    }

    void findClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log(hit.transform.gameObject.name);

                if (hit.transform.gameObject.tag == "guard")
                {
                    if (guardAIScript != null)
                    {
                        guardAIScript.playerControl = false;
                        guardAIScript.changeColor(false);
                    }
                    currentGuard = hit.transform.gameObject;
                    guardAIScript = currentGuard.GetComponent<GuardAI>();
                    guardAIScript.playerControl = true;
                    guardAIScript.changeColor(true);
                }
                else
                {
                    if (guardAIScript != null)
                    {
                        guardAIScript.playerControl = false;
                        guardAIScript.changeColor(false);
                    }
                    currentGuard = null;
                    guardAIScript = null;
                }
            }
        }
    }

    void spaceChange()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(currentGuardNum);
            currentGuardNum++;
            if(currentGuardNum==5)
            {
                currentGuardNum = 0;
            }
            


            if (guardAIScript != null)
            {
                guardAIScript.playerControl = false;
                guardAIScript.changeColor(false);
            }
            currentGuard = guards[currentGuardNum];
            guardAIScript = currentGuard.GetComponent<GuardAI>();
            guardAIScript.playerControl = true;
            guardAIScript.changeColor(true);

        }
    }
}
