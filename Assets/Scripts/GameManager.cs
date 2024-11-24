using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmpScoreP1;
    [SerializeField] TextMeshProUGUI tmpScoreP2;
    [SerializeField] TextMeshProUGUI tmpWinText;
    [SerializeField] TextMeshProUGUI tmpRestartText;
    [SerializeField] GameObject pelota;

    int p1Score;
    int p2Score;

    bool running = false;

    const int WIN_SCORE = 10;

    public void addPointP1() {
        p1Score++;
        UpdateScores();
        CheckWinCondition();
    }

    public void addPointP2() {
        p2Score++;
        UpdateScores();
        CheckWinCondition();
    }
    
    void Start()
    {
        Cursor.visible = false;
        UpdateScores();
        tmpWinText.gameObject.SetActive(false);
        tmpRestartText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Maneja el reinicio del juego después de un ganador
        if (IsGameOver() && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame(); // Reiniciar manualmente cuando presionas 'R'
        }
        
        // Solo inicia la pelota si el juego no ha terminado
        if (!running && !IsGameOver() && Input.GetKeyDown(KeyCode.Space))
        {
            pelota.SetActive(true);
            running = true;
        }
    }
    
    void UpdateScores() {
        tmpScoreP1.text = p1Score.ToString();
        tmpScoreP2.text = p2Score.ToString();
    }

    // Verificar si un jugador ha alcanzado el puntaje necesario para ganar
    void CheckWinCondition()
    {
        if (p1Score >= WIN_SCORE)
        {
            Win(1);
        }
        else if (p2Score >= WIN_SCORE)
        {
            Win(2);
        }
    }

    // Mostrar el mensaje de victoria y reiniciar el juego
    void Win(int ganador)
    {
        tmpWinText.gameObject.SetActive(true);
        tmpRestartText.gameObject.SetActive(true);

        // Actualiza el texto dependiendo del ganador
        if (ganador == 1)
        {
            tmpWinText.text = "JUGADOR 1 Y JUGADOR 3 GANAN";
        }
        else if (ganador == 2)
        {
            tmpWinText.text = "JUGADOR 2 Y JUGADOR 4 GANAN";
        }

        tmpRestartText.text = "PRESIONA 'R' PARA VOLVER A JUGAR";

        // Desactivar la pelota y detener el juego por un momento
        pelota.SetActive(false);
        running = false;
    }

    public bool IsGameOver()
    {
        return p1Score >= WIN_SCORE || p2Score >= WIN_SCORE;
    }

    // Método para reiniciar el juego (recarga la escena actual)
    void RestartGame()
    {
        // Reiniciar puntuaciones
        p1Score = 0;
        p2Score = 0;
        UpdateScores();

        // Reactivar pelota
        pelota.SetActive(true);

        // Obtener la referencia al script de la pelota
        PelotaController pelotaController = pelota.GetComponent<PelotaController>();

        // Reiniciar movimiento de la pelota
        if (pelotaController != null)
        {
            int direccionX = Random.Range(0, 2) == 0 ? -1 : 1;
            pelotaController.ResetPelota(direccionX);
        }

        // Ocultar el mensaje de victoria y reinicio
        tmpWinText.gameObject.SetActive(false);
        tmpRestartText.gameObject.SetActive(false);

        running = false; // Espera a que el jugador presione espacio para lanzar la pelota
    }
}
