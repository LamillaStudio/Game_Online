using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado a Photon, uniéndose a sala...");
        PhotonNetwork.JoinOrCreateRoom("SalaTest", new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("¡Unido a la sala! Jugadores: " + PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-3, 3), 1, Random.Range(-3, 3)), Quaternion.identity);
    }
}