using UnityEngine;
using TMPro;

public class GamblingUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_InputField betInput;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI resultText;

    private SlotMachine currentMachine;
    private PlayerMoney currentPlayerMoney;

    void Start()
    {
        panel.SetActive(false);
    }

    public void Open(SlotMachine machine, PlayerMoney playerMoney)
    {
        currentMachine = machine;
        currentPlayerMoney = playerMoney;

        panel.SetActive(true);
        resultText.text = "";
        UpdateMoneyDisplay();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Close()
    {
        panel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnConfirmBetPressed()
    {
        if (!int.TryParse(betInput.text, out int betAmount) || betAmount <= 0)
        {
            resultText.text = "Ingresa una apuesta válida.";
            return;
        }

        if (!currentPlayerMoney.TrySpend(betAmount))
        {
            resultText.text = "No tienes suficiente dinero.";
            return;
        }

        float multiplier = currentMachine.Roll();
        int winnings = Mathf.RoundToInt(betAmount * multiplier);

        if (winnings > 0)
        {
            currentPlayerMoney.AddMoney(winnings);
            resultText.text = $"¡Ganaste x{multiplier}! +{winnings}";
        }
        else
        {
            resultText.text = "Perdiste la apuesta.";
        }

        UpdateMoneyDisplay();
    }

    private void UpdateMoneyDisplay()
    {
        moneyText.text = $"Dinero: {currentPlayerMoney.CurrentMoney}";
    }
}