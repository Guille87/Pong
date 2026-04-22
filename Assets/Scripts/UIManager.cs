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
    [SerializeField] private GameObject tmpStartText;

    public void ActualizarPuntuacion(int p1, int p2)
    {
        tmpScoreP1.text = p1.ToString();
        tmpScoreP2.text = p2.ToString();
    }
    
    public void ToggleStartText(bool visible)
    {
        tmpStartText.SetActive(visible);
    }

    public void MostrarMensajeVictoria(int ganador)
    {
        tmpWinText.gameObject.SetActive(true);
        tmpRestartText.gameObject.SetActive(true);
        tmpWinText.text = $"JUGADOR {(ganador == 1 ? "1 Y 3" : "2 Y 4")} GANAN";
        tmpRestartText.text = "PRESIONA 'R' PARA VOLVER A JUGAR";
    }

    public void OcultarMensajes()
    {
        tmpWinText.gameObject.SetActive(false);
        tmpRestartText.gameObject.SetActive(false);
    }
}
