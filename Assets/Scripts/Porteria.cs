using UnityEngine;
using UnityEngine.Events;

public class Porteria : MonoBehaviour
{
    [SerializeField] private UnityEvent alMarcarGol;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pelota"))
        {
            alMarcarGol?.Invoke();
        }
    }
}
