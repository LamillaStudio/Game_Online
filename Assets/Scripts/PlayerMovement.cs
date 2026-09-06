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

        // Animación: usa la magnitud total del input (detecta A/D también)
        float speedMultiplier = isSprinting ? 2f : 1f;
        float inputMagnitude = moveInput.magnitude; // detecta cualquier dirección, incluida lateral
        float direction = moveInput.y < -0.1f ? -1f : 1f; // solo negativo si hay componente "atrás"

        animator.SetFloat("VelocityZ", inputMagnitude * direction * speedMultiplier);
    }
}