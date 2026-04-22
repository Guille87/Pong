using UnityEngine;
using UnityEngine.InputSystem;

public class PalaController : MonoBehaviour
{
    const float MAX_Y = 4.3f;
    const float MIN_Y = -4.3f;

    [SerializeField] float speed = 15f;

    private float inputY; // Guardamos el valor actual del eje

    // Referencia a la acción de movimiento
    public void OnMove(InputAction.CallbackContext context)
    {
        // Lee el valor del eje (1, -1 o 0)
        inputY = context.ReadValue<float>();
    }

    void Update()
    {
        if (inputY == 0) return;

        float newY = transform.position.y + (inputY * speed * Time.deltaTime);
        newY = Mathf.Clamp(newY, MIN_Y, MAX_Y);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
