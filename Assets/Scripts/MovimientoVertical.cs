using UnityEngine;

public class MovimientoVertical : MonoBehaviour
{
    [SerializeField] private float velocidad = 2f; // Velocidad del movimiento
    [SerializeField] private float rangoSuperior = 3f; // Límite superior
    [SerializeField] private float rangoInferior = -3f; // Límite inferior
    [SerializeField] private int direccion = 1; // Dirección del movimiento (1 = arriba, -1 = abajo)

    // Update is called once per frame
    void Update()
    {
        // Actualiza la posición del objeto
        transform.position += new Vector3(0, velocidad * direccion * Time.deltaTime, 0);

        // Cambia la dirección si llega a los límites
        if (transform.position.y > rangoSuperior)
        {
            direccion = -1; // Cambiar a dirección hacia abajo
        }
        else if (transform.position.y < rangoInferior)
        {
            direccion = 1; // Cambiar a dirección hacia arriba
        }
    }
}
