using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviourPun
{
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float sensitivity = 0.1f;

    private float xRotation = 0f;

    void Start()
    {
        if (!photonView.IsMine) return;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        // Rotación horizontal → gira el cuerpo entero
        transform.Rotate(Vector3.up * mouseDelta.x * sensitivity);

        // Rotación vertical → solo la cámara, con límite para no dar vueltas raras
        xRotation -= mouseDelta.y * sensitivity;
        xRotation = Mathf.Clamp(xRotation, -40f, 40f); //-80f 80f
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}