using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviourPun
{
    private CharacterController controller;
    private PlayerControls controls;
    private Animator animator;

    private Vector2 moveInput;
    private bool isSprinting;
    private Vector3 velocity;

    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float gravity = -9.81f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        controls = new PlayerControls();
        animator = GetComponentInChildren<Animator>();
    }

    void OnEnable()
    {
        if (!photonView.IsMine) return;

        controls.Player.Enable();

        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;

        controls.Player.Sprint.performed += OnSprintPerformed;
        controls.Player.Sprint.canceled += OnSprintCanceled;
    }

    void OnDisable()
    {
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMove;

        controls.Player.Sprint.performed -= OnSprintPerformed;
        controls.Player.Sprint.canceled -= OnSprintCanceled;

        controls.Player.Disable();
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    void OnSprintPerformed(InputAction.CallbackContext ctx)
    {
        isSprinting = true;
    }

    void OnSprintCanceled(InputAction.CallbackContext ctx)
    {
        isSprinting = false;
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        // Movimiento relativo a hacia donde mira el jugador
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Gravedad
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // --- ANIMACIÓN: cada eje se manda a -1/0/1, sin diluir en diagonales ---
        float speedMultiplier = isSprinting ? 2f : 1f;

        float animX = Mathf.Abs(moveInput.x) > 0.1f ? Mathf.Sign(moveInput.x) : 0f;
        float animZ = Mathf.Abs(moveInput.y) > 0.1f ? Mathf.Sign(moveInput.y) : 0f;

        animator.SetFloat("VelocityX", animX * speedMultiplier);
        animator.SetFloat("VelocityZ", animZ * speedMultiplier);
    }
}