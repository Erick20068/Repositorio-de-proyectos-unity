using UnityEngine;

public class PlayerMovimiento : MonoBehaviour
{
    // Referencia al componente CharacterController (control del jugador)
    public CharacterController characterController;

    // Velocidad de movimiento del jugador
    public float Speed = 10f;

    // Fuerza de gravedad aplicada al jugador
    private float gravity = -9.81f;

    // Punto desde donde se verifica si el jugador está en el suelo
    public Transform groundCheck;

    // Radio de la esfera para detectar el suelo
    public float sphereRadius = 0.3f;

    // Capa que identifica qué es suelo
    public LayerMask groundMask;

    // Variable que indica si el jugador está tocando el suelo
    bool isGrounded;

    // Vector que almacena la velocidad (especialmente en Y para saltos)
    Vector3 velocity;

    // Altura del salto
    public float jumpHeight = 3;

    void Update()
    {
        // Verifica si el jugador está tocando el suelo usando una esfera
        isGrounded = Physics.CheckSphere(groundCheck.position, sphereRadius, groundMask);

        // Si está en el suelo y cayendo, se ajusta la velocidad en Y
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // pequeño valor para mantenerlo pegado al suelo
        }

        // Captura el movimiento horizontal (A/D o flechas)
        float x = Input.GetAxis("Horizontal");

        // Captura el movimiento vertical (W/S o flechas)
        float z = Input.GetAxis("Vertical");

        // Calcula la dirección del movimiento según la orientación del jugador
        Vector3 move = transform.right * x + transform.forward * z;

        // Si se presiona espacio y está en el suelo, el jugador salta
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Fórmula para calcular la fuerza del salto
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        // Mueve al jugador en X y Z (movimiento horizontal)
        characterController.Move(move * Speed * Time.deltaTime);

        // Aplica la gravedad constantemente
        velocity.y += gravity * Time.deltaTime;

        // Aplica el movimiento vertical (salto y caída)
        characterController.Move(velocity * Time.deltaTime);
    }
}