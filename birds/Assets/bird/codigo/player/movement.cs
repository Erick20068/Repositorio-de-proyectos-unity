using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;
    private Vector2 movementInput;

    public Transform camara;
    public float sensibilidadMouse = 200f;

    private float rotacionVertical = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse * Time.deltaTime;
      
        rotacionVertical -= mouseY;
        rotacionVertical = Mathf.Clamp(rotacionVertical, -80f, 80f);

        camara.localRotation = Quaternion.Euler(rotacionVertical, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void FixedUpdate()
    {
        Vector3 direccion = transform.right * movementInput.x + transform.forward * movementInput.y;

        rb.MovePosition(
            rb.position + direccion.normalized * speed * Time.fixedDeltaTime
        );
    }
}