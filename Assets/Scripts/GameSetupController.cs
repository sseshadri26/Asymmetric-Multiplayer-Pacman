using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;

public class GameSetupController : MonoBehaviour
{





    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player");
        PhotonNetwork.Instantiate("PhotonPlayer", Vector2.zero, Quaternion.identity);
        //PhotonNetwork.Instantiate("player", new Vector3(0, -1.23f, 1.12f), Quaternion.identity);
        //PhotonNetwork.Instantiate("Maze", new Vector3(0, -1.23f, 1.12f), Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {

    }


}
