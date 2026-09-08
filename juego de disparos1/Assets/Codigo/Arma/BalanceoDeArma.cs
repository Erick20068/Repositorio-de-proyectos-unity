using UnityEngine;

public class BalanceoDeArma : MonoBehaviour
{

    private Quaternion startPotation;

    public float swayAmount = 8;

    void Start()
    {
        startPotation = transform.localRotation;
    }


    void Update()
    {
        Sway();
    }


    private void Sway()
    {
        // Movimiento del mouse en X (izquierda/derecha)
        float mouseX = Input.GetAxis("Mouse X");
       
        // Movimiento del mouse en Y (arriba/abajo)
        float mouseY = Input.GetAxis("Mouse Y");

        Quaternion xAngle = Quaternion.AngleAxis(mouseX * -1.25f, Vector3.up);

        Quaternion yAngle = Quaternion.AngleAxis(mouseY * -1.25f, Vector3.right);

        Quaternion targetRotation = startPotation * xAngle * yAngle;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * swayAmount);
    }
}
