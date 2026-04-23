using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleSetup : MonoBehaviour
{
    [SerializeField] private GameObject[] palas;

    void Awake()
    {
        ConfigurarPala(0, GameSettings.P1);
        ConfigurarPala(1, GameSettings.P2);
        ConfigurarPala(2, GameSettings.P3);
        ConfigurarPala(3, GameSettings.P4);
    }

    void ConfigurarPala(int index, GameSettings.ControlType tipo)
    {
        GameObject pala = palas[index];
        
        switch (tipo)
        {
            case GameSettings.ControlType.Humano:
                pala.SetActive(true);
                pala.GetComponent<PalaController>().enabled = true;
                pala.GetComponent<IAController>().enabled = false;
                // Si es humano, el PlayerInput debe estar activo
                pala.GetComponent<PlayerInput>().enabled = true;
                break;

            case GameSettings.ControlType.IA:
                pala.SetActive(true);
                pala.GetComponent<PalaController>().enabled = false;
                pala.GetComponent<IAController>().enabled = true;
                pala.GetComponent<PlayerInput>().enabled = false;
                break;

            case GameSettings.ControlType.Desactivado:
                pala.SetActive(false);
                break;
        }
    }
}
