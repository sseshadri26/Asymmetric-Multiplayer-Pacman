using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView PV;

    public GameObject myAvatar;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient)
        {
            if (PV.IsMine)
            {
                myAvatar = PhotonNetwork.Instantiate("player", GameSetup.gs.spawnPoints[0].position, GameSetup.gs.spawnPoints[0].rotation, 0);

            }
        }
        else
        {
            if (PV.IsMine)
            {
                myAvatar = PhotonNetwork.Instantiate("guard", GameSetup.gs.spawnPoints[1].position, GameSetup.gs.spawnPoints[0].rotation, 0);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
