using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [Header("Paneles")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject createPanel;
    [SerializeField] private GameObject joinPanel;

    [Header("Main Panel")]
    [SerializeField] private TMP_InputField playerNameInput;

    [Header("Create Panel")]
    [SerializeField] private TMP_Dropdown maxPlayersDropdown;

    [Header("Join Panel")]
    [SerializeField] private TMP_InputField roomCodeInput;

    [Header("Status")]
    [SerializeField] private TextMeshProUGUI statusText;

    private const string GameSceneName = "OnlineTest";

    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
            playerNameInput.text = PlayerPrefs.GetString("PlayerName");

        ShowMainPanel();

        if (PhotonNetwork.IsConnected)
        {
            SetStatus("Ya conectado.");
        }
        else
        {
            SetStatus("Conectando a Photon...");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        SetStatus("Conectado. Listo para jugar.");
    }

    // --- Navegación entre paneles ---

    public void ShowMainPanel()
    {
        mainPanel.SetActive(true);
        createPanel.SetActive(false);
        joinPanel.SetActive(false);
    }

    public void ShowCreatePanel()
    {
        if (!ValidatePlayerName()) return;

        mainPanel.SetActive(false);
        createPanel.SetActive(true);
    }

    public void ShowJoinPanel()
    {
        if (!ValidatePlayerName()) return;

        mainPanel.SetActive(false);
        joinPanel.SetActive(true);
    }

    // --- Crear sala ---

    public void OnConfirmCreatePressed()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            SetStatus("Aún conectando, espera un momento...");
            return;
        }

        SavePlayerName();
        SetStatus("Creando sala...");

        string roomCode = RoomCodeGenerator.Generate();
        byte maxPlayers = GetSelectedMaxPlayers();

        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers };
        PhotonNetwork.CreateRoom(roomCode, options);
    }

    private byte GetSelectedMaxPlayers()
    {
        string selected = maxPlayersDropdown.options[maxPlayersDropdown.value].text;
        return byte.Parse(selected);
    }

    // --- Unirse a sala ---

    public void OnConfirmJoinPressed()
    {
        if (string.IsNullOrWhiteSpace(roomCodeInput.text))
        {
            SetStatus("Ingresa el código de sala.");
            return;
        }
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            SetStatus("Aún conectando, espera un momento...");
            return;
        }

        SavePlayerName();
        SetStatus("Uniéndose a sala...");

        PhotonNetwork.JoinRoom(roomCodeInput.text.ToUpper());
    }

    public override void OnJoinedRoom()
    {
        SetStatus("¡Conectado! Cargando juego...");
        PhotonNetwork.LoadLevel(GameSceneName);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetStatus("No se pudo crear la sala, intenta de nuevo.");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetStatus("Código inválido o sala llena.");
    }

    // --- Utilidades ---

    private bool ValidatePlayerName()
    {
        if (string.IsNullOrWhiteSpace(playerNameInput.text))
        {
            SetStatus("Ingresa un nombre de jugador.");
            return false;
        }
        return true;
    }

    private void SavePlayerName()
    {
        PhotonNetwork.NickName = playerNameInput.text;
        PlayerPrefs.SetString("PlayerName", playerNameInput.text);
    }

    private void SetStatus(string message)
    {
        if (statusText != null) statusText.text = message;
        Debug.Log(message);
    }

    public void OnQuitButtonPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}