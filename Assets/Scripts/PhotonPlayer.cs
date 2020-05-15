using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView PV;

    public GameObject myAvatar;

    public GameObject[] guards;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        guards = new GameObject[GameSetup.gs.guardSpawnPoints.Length];

        //Master Client is assigned the player avatars
        //other person is assigned the guards



        if (PhotonNetwork.IsMasterClient)
        {
            if (PV.IsMine)
            {
                myAvatar = PhotonNetwork.Instantiate("player", GameSetup.gs.PlayerSpawnPoint.position, GameSetup.gs.PlayerSpawnPoint.rotation, 0);

            }
        }
        else
        {
            if (PV.IsMine)
            {
                for(int i = 0; i < guards.Length; i++)
                {
                    guards[i] = PhotonNetwork.Instantiate("guard", GameSetup.gs.guardSpawnPoints[i].position, GameSetup.gs.guardSpawnPoints[i].rotation, 0);

                }
                

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
