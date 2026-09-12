using Photon.Pun;
using TMPro;
using UnityEngine;

public class MoneyHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;

    private PlayerMoney localPlayerMoney;

    void Update()
    {
        // Si aún no encontramos nuestro PlayerMoney, buscarlo
        if (localPlayerMoney == null)
        {
            FindLocalPlayerMoney();
        }
    }

    private void FindLocalPlayerMoney()
    {
        PlayerMoney[] allPlayers = FindObjectsByType<PlayerMoney>(FindObjectsSortMode.None);
        foreach (var p in allPlayers)
        {
            if (p.photonView.IsMine)
            {
                localPlayerMoney = p;
                localPlayerMoney.OnMoneyChanged += UpdateMoneyText;
                UpdateMoneyText(localPlayerMoney.CurrentMoney); // muestra el valor inicial de inmediato
                break;
            }
        }
    }

    private void UpdateMoneyText(int amount)
    {
        moneyText.text = $"$ {amount}";
    }

    void OnDestroy()
    {
        if (localPlayerMoney != null)
            localPlayerMoney.OnMoneyChanged -= UpdateMoneyText;
    }
}