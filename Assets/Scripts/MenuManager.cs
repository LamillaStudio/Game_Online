using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private TMP_InputField roomNameInput;
    [SerializeField] private TMP_Dropdown maxPlayersDropdown;
    [SerializeField] private TextMeshProUGUI statusText;

    private const string GameSceneName = "OnlineTest";

    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
            playerNameInput.text = PlayerPrefs.GetString("PlayerName");

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

    public void OnCreateRoomPressed()
    {
        if (!ValidateInputs()) return;

        SavePlayerName();
        SetStatus("Creando sala...");

        byte maxPlayers = GetSelectedMaxPlayers();

        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers };
        PhotonNetwork.CreateRoom(roomNameInput.text, options);
    }

    private byte GetSelectedMaxPlayers()
    {
        // Asume que las opciones del Dropdown son exactamente: "2", "4", "6", "8"
        string selected = maxPlayersDropdown.options[maxPlayersDropdown.value].text;
        return byte.Parse(selected);
    }

    public void OnJoinRoomPressed()
    {
        if (!ValidateInputs()) return;

        SavePlayerName();
        SetStatus("Uniéndose a sala...");

        PhotonNetwork.JoinRoom(roomNameInput.text);
    }

    public override void OnJoinedRoom()
    {
        SetStatus("¡Conectado! Cargando juego...");
        PhotonNetwork.LoadLevel(GameSceneName);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetStatus("No se pudo crear la sala: ya existe o hubo un error.");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetStatus("No se pudo unir: la sala no existe o está llena.");
    }

    private bool ValidateInputs()
    {
        if (string.IsNullOrWhiteSpace(playerNameInput.text))
        {
            SetStatus("Ingresa un nombre de jugador.");
            return false;
        }
        if (string.IsNullOrWhiteSpace(roomNameInput.text))
        {
            SetStatus("Ingresa un nombre de sala.");
            return false;
        }
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            SetStatus("Aún conectando, espera un momento...");
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