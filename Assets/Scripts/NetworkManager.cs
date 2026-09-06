using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        // Si por algún motivo ya estamos listos al cargar (por ejemplo el host), instancia directo
        if (PhotonNetwork.InRoom)
        {
            SpawnPlayer();
        }
    }

    public override void OnJoinedRoom()
    {
        // Este callback garantiza que el join está confirmado a nivel de red
        SpawnPlayer();
    }

    private bool alreadySpawned;
    private void SpawnPlayer()
    {
        if (alreadySpawned) return;
        alreadySpawned = true;

        Debug.Log("¡Unido a la sala! Jugadores: " + PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-3, 3), 1, Random.Range(-3, 3)), Quaternion.identity);
    }

    //public override void OnConnectedToMaster()
    //{
    //    Debug.Log("Conectado a Photon, uniéndose a sala...");
    //    PhotonNetwork.JoinOrCreateRoom("SalaTest", new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
    //}

    //public override void OnJoinedRoom()
    //{
    //    Debug.Log("¡Unido a la sala! Jugadores: " + PhotonNetwork.CurrentRoom.PlayerCount);
    //    PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-3, 3), 1, Random.Range(-3, 3)), Quaternion.identity);
    //}
}