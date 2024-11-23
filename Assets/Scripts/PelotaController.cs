using System.Collections;
using UnityEngine;

public class PelotaController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] GameManager manager;
    [SerializeField] float force;
    [SerializeField] float delay;
    [SerializeField] float speedIncrement = 1.05f; // Incremento del 5% por rebote

    [SerializeField] float minY = -2.5f;
    [SerializeField] float maxY = 2.5f;

    const float MIN_ANG = 25.0f;
    const float MAX_ANG = 40.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        if (manager == null) {
            manager = Object.FindFirstObjectByType<GameManager>();
        }
        int direccionX = Random.Range(0, 2) == 0 ? -1 : 1;
        StartCoroutine(LanzarPelota(direccionX));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        string tag = other.gameObject.tag;

        if (tag == "Pala1" || tag == "Pala2")
            IncrementarVelocidad();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Porteria2")) {
            manager.addPointP1();
            StartCoroutine(LanzarPelota(1));
        }
        else if(other.CompareTag("Porteria1")) {
            manager.addPointP2();
            StartCoroutine(LanzarPelota(-1));
        }
    }

    IEnumerator LanzarPelota(int direccionX)
    {
        yield return new WaitForSeconds(delay);
        
        // Cálculo de la posición vertical del lanzamiento
        float posY = Random.Range(minY, maxY);
        transform.position = new Vector3(0, posY, 0);

        Vector2 impulso = ObtenerDireccionAleatoria(direccionX);

        // Resetear la velocidad lineal de la pelota
        rb.linearVelocity = Vector2.zero;

        // Aplicamos el impulso
        rb.AddForce(impulso * force, ForceMode2D.Impulse);
    }

    private Vector2 ObtenerDireccionAleatoria(int direccionX)
    {
        // Cálculo del vector del lanzamiento
        float angulo = Random.Range(MIN_ANG, MAX_ANG) * Mathf.Deg2Rad;
        float x = Mathf.Cos(angulo) * direccionX;
        int direccionY = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Mathf.Sin(angulo) * direccionY;
        Debug.Log("angulo = "+ angulo + "; x = " + x + "; y = " + y);
        return new Vector2(x, y);
    }

    private void IncrementarVelocidad()
    {
        // Incrementa la velocidad actual de la pelota
        Vector2 velocidadActual = rb.linearVelocity;
        rb.linearVelocity = velocidadActual * speedIncrement;
        Debug.Log("Nueva velocidad: " + rb.linearVelocity);
    }
}
