using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject pauseMenuUI;
    private PlayerControls controls;
    private bool isPaused;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        base.OnEnable();
        controls.Player.Enable();
        controls.Player.Pause.performed += OnPausePerformed;
    }

    void OnDisable()
    {
        base.OnDisable();
        controls.Player.Pause.performed -= OnPausePerformed;
        controls.Player.Disable();
    }

    void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        if (isPaused) Resume();
        else Pause();
    }

    void Pause()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerMovement localPlayer = FindLocalPlayerMovement();
        if (localPlayer != null) localPlayer.SetMovementLocked(true);
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerMovement localPlayer = FindLocalPlayerMovement();
        if (localPlayer != null) localPlayer.SetMovementLocked(false);
    }

    private PlayerMovement FindLocalPlayerMovement()
    {
        // Busca entre todos los PlayerMovement de la escena el que es tuyo (IsMine)
        PlayerMovement[] allPlayers = FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);
        foreach (var p in allPlayers)
        {
            if (p.photonView.IsMine) return p;
        }
        return null;
    }

    // --- Botón "Salir" del menú de pausa ---
    public void OnExitButtonPressed()
    {
        if (!PhotonNetwork.InRoom) return;

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ForceReturnToMenu", RpcTarget.All);
        }
        else
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    [PunRPC]
    void ForceReturnToMenu()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MenuScene");
    }

    // --- Caso: el Master Client se desconecta abruptamente (corte de luz, Alt+F4) ---
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    // --- Notificación visual: un jugador cualquiera se desconecta ---
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        NotificationManager.Instance.ShowNotification($"{otherPlayer.NickName} se desconectó");
    }
}
