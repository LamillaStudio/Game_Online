using Photon.Pun;
using UnityEngine;

public class PlayerMoney : MonoBehaviourPun
{
    public int startingMoney = 500;
    public int CurrentMoney { get; private set; }

    public System.Action<int> OnMoneyChanged;

    void Start()
    {
        if (photonView.IsMine)
        {
            CurrentMoney = startingMoney;
            UpdateProperty();
            OnMoneyChanged?.Invoke(CurrentMoney);
        }
    }

    public bool TrySpend(int amount)
    {
        if (!photonView.IsMine || CurrentMoney < amount) return false;

        CurrentMoney -= amount;
        UpdateProperty();
        OnMoneyChanged?.Invoke(CurrentMoney);
        return true;
    }

    public void AddMoney(int amount)
    {
        if (!photonView.IsMine) return;

        CurrentMoney += amount;
        UpdateProperty();
        OnMoneyChanged?.Invoke(CurrentMoney);
    }

    private void UpdateProperty()
    {
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
        {
            { "money", CurrentMoney }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }
}