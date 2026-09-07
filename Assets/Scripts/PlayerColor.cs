using Photon.Pun;
using UnityEngine;

public class PlayerColor : MonoBehaviourPun
{
    [SerializeField] private SkinnedMeshRenderer bodyRenderer;

    void Start()
    {
        if (photonView.IsMine)
        {
            Color randomColor = Random.ColorHSV(0f, 1f, 0.7f, 1f, 0.8f, 1f);
            photonView.RPC("SetPlayerColor", RpcTarget.AllBuffered, randomColor.r, randomColor.g, randomColor.b);
        }
    }

    [PunRPC]
    void SetPlayerColor(float r, float g, float b)
    {
        Debug.Log($"Aplicando color: R={r} G={g} B={b} al objeto {gameObject.name}");
        Material instanceMaterial = bodyRenderer.material;
        instanceMaterial.SetColor("_BaseColor", new Color(r, g, b));
    }
}
