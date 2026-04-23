using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI tmpScoreP1;
    [SerializeField] private TextMeshProUGUI tmpScoreP2;

    [Header("Status UI")]
    [SerializeField] private TextMeshProUGUI tmpWinText;
    [SerializeField] private TextMeshProUGUI tmpRestartText;
    [SerializeField] private TextMeshProUGUI tmpStartText;

    public void ActualizarPuntuacion(int p1, int p2)
    {
        tmpScoreP1.text = p1.ToString();
        tmpScoreP2.text = p2.ToString();
    }
    
    public void ToggleStartText(bool visible)
    {
        tmpStartText.gameObject.SetActive(visible);
    }

    public void MostrarMensajeVictoria(int ganador)
    {
        tmpWinText.gameObject.SetActive(true);
        tmpRestartText.gameObject.SetActive(true);

        // Comprobamos si el modo de juego es 2vs2 mirando las palas secundarias
        // Si P3 o P4 no están desactivados, es que es modo equipos.
        bool esModoEquipos = GameSettings.P3 != GameSettings.ControlType.Desactivado || 
                             GameSettings.P4 != GameSettings.ControlType.Desactivado;
        
        if (ganador == 1)
        {
            tmpWinText.text = esModoEquipos ? "JUGADOR 1 Y 3 GANAN" : "JUGADOR 1 GANA";
        }
        else
        {
            tmpWinText.text = esModoEquipos ? "JUGADOR 2 Y 4 GANAN" : "JUGADOR 2 GANA";
        }

        tmpRestartText.text = "PRESIONA 'R' PARA VOLVER A JUGAR";
    }

    public void OcultarMensajes()
    {
        tmpWinText.gameObject.SetActive(false);
        tmpRestartText.gameObject.SetActive(false);
        tmpStartText.gameObject.SetActive(false);
    }
}
