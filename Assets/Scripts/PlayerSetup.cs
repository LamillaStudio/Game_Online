using Photon.Pun;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPun
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private AudioListener audioListener;

    void Start()
    {
        Debug.Log($"IsMine: {photonView.IsMine} - {gameObject.name}");
        if (!photonView.IsMine)
        {
            playerCamera.gameObject.SetActive(false);
            if (audioListener != null) audioListener.enabled = false;
        }
    }
}