using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerGuardClickController : MonoBehaviour
{
    public GameObject currentGuard;
    GuardAI guardAIScript;

    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

    }

    // Update is called once per frame
    void Update()
    {
        if (PV == null || PV.IsMine)
        {
            findClick();
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
                    }
                    currentGuard = hit.transform.gameObject;
                    guardAIScript = currentGuard.GetComponent<GuardAI>();
                    guardAIScript.playerControl = true;
                }
                else
                {
                    if (guardAIScript != null)
                    {
                        guardAIScript.playerControl = false;
                    }
                    currentGuard = null;
                    guardAIScript = null;
                }
            }
        }
    }
}
