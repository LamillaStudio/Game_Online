using Photon.Pun;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPun
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private SkinnedMeshRenderer[] renderersToDisable;

    void Start()
    {
        if (!photonView.IsMine)
        {
            playerCamera.gameObject.SetActive(false);
            if (audioListener != null) audioListener.enabled = false;
        }
        else
        {
            foreach (var renderer in renderersToDisable)
            {
                renderer.enabled = false;
            }
        }
    }
}