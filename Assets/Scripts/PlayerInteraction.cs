using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviourPun
{
    private PlayerControls controls;
    private SlotMachine nearbyMachine;
    public float interactRadius = 2f;

    [SerializeField] private GamblingUI gamblingUI;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void Start()
    {
        if (gamblingUI == null)
            gamblingUI = FindFirstObjectByType<GamblingUI>();

        Debug.Log($"GamblingUI encontrado: {(gamblingUI != null)}");
    }

    void OnEnable()
    {
        if (!photonView.IsMine) return;
        controls.Player.Enable();
        controls.Player.Interact.performed += OnInteract;
    }

    void OnDisable()
    {
        controls.Player.Interact.performed -= OnInteract;
        controls.Player.Disable();
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, interactRadius);
        nearbyMachine = null;
        foreach (var hit in hits)
        {
            SlotMachine machine = hit.GetComponent<SlotMachine>();
            if (machine != null)
            {
                nearbyMachine = machine;
                break;
            }
        }
        // Debug con tecla R
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            Debug.Log($"Máquina cercana: {(nearbyMachine != null ? nearbyMachine.name : "Ninguna")} | Jugadores/colliders detectados: {hits.Length}");
        }
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        if (nearbyMachine != null)
        {
            gamblingUI.Open(nearbyMachine, GetComponent<PlayerMoney>());
        }
    }
    // --- Gizmo para visualizar el radio de interacción en el editor ---
    void OnDrawGizmosSelected()
    {
        Gizmos.color = nearbyMachine != null ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}