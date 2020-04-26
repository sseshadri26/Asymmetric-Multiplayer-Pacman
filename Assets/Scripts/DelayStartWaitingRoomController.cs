using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DelayStartWaitingRoomController : MonoBehaviourPunCallbacks
{

    private PhotonView myPhotonView;

    [SerializeField]
    private int multiplayerSceneIndex;

    [SerializeField]
    private int menuSceneIndex;

    private int playerCount;
    private int roomSize;


    public Text playerCountDisplay;

    private bool readyToStart;
    private bool startingGame;


    //go back to menu, leave waiting room
    public void DelayCancel()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(menuSceneIndex);

    }



    // Start is called before the first frame update
    void Start()
    {
        myPhotonView = GetComponent<PhotonView>();
        PlayerCountUpdate();

    }

    void PlayerCountUpdate()
    {
        //updates player count
        //displays player count
        //triggers countdown timer

        playerCount = PhotonNetwork.PlayerList.Length;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        playerCountDisplay.text = playerCount + ": " + roomSize;

        if (playerCount == roomSize)
        {
            readyToStart = true;
        }


    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerCountUpdate();

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerCountUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        WaitingForMorePlayers();
    }

    void WaitingForMorePlayers()
    {
        if (readyToStart)
        {
            if (startingGame)
            {
                return;
            }
            else
            {
                StartGame();
            }
        }
    }

    void StartGame()
    {
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        else
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);
        }
    }

    
}
