using Photon.Pun;
using TMPro;
using UnityEngine;

public class RoomCodeBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI codeText;

    void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            codeText.text = $"Código: {PhotonNetwork.CurrentRoom.Name}";
        }
    }
}