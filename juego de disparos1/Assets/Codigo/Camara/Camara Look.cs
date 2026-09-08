using UnityEngine;

public class CamaraLook : MonoBehaviour
{
    // Sensibilidad del mouse (qué tan rápido gira la cámara)
    public float mouseSensitivity = 90f;

    // Referencia al cuerpo del jugador (para rotarlo horizontalmente)
    public Transform playerBody;

    // Rotación en el eje X (arriba/abajo)
    float xRotation = 0;

    void Start()
    {
        // Oculta y bloquea el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Movimiento del mouse en X (izquierda/derecha)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        // Movimiento del mouse en Y (arriba/abajo)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Se acumula la rotación vertical
        xRotation += mouseY;

        // Limita la rotación para que no gire completamente (evita que se voltee)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Aplica la rotación vertical a la cámara (solo arriba/abajo)
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Rota el cuerpo del jugador horizontalmente (izquierda/derecha)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}