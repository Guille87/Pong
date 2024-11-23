using UnityEngine;

public class PalaController : MonoBehaviour
{
    const float MAX_Y = 4.3f;
    const float MIN_Y = -4.3f;

    [SerializeField] float speed = 15f;

    // Configuración de las teclas para cada pala (puedes añadir más teclas si es necesario)
    [SerializeField] KeyCode upKey;
    [SerializeField] KeyCode downKey;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Movimiento hacia arriba
        if (Input.GetKey(upKey) && transform.position.y < MAX_Y)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }

        // Movimiento hacia abajo
        if (Input.GetKey(downKey) && transform.position.y > MIN_Y)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }
}
