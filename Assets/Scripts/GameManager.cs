using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PelotaController pelota;

    [Header("Ajustes")]
    [SerializeField] private int puntosParaGanar = 10;

    private int p1Score;
    private int p2Score;
    private bool juegoEnCurso = false;

    void Start()
    {
        Cursor.visible = false;
        uiManager.ActualizarPuntuacion(0, 0);
        uiManager.OcultarMensajes();
    }

    public void SumarPuntoP1() => ProcesarPunto(ref p1Score, 1);
    public void SumarPuntoP2() => ProcesarPunto(ref p2Score, 2);

    private void ProcesarPunto(ref int score, int direccionLanzamiento)
    {
        score++;
        uiManager.ActualizarPuntuacion(p1Score, p2Score);

        if (score >= puntosParaGanar)
        {
            TerminarJuego(direccionLanzamiento == 1 ? 1 : 2);
        }
        else
        {
            pelota.LanzarDesdeCentro(direccionLanzamiento);
        }
    }

    private void TerminarJuego(int ganador)
    {
        juegoEnCurso = false;
        pelota.gameObject.SetActive(false);
        uiManager.MostrarMensajeVictoria(ganador);
    }

    public void OnReiniciar(InputAction.CallbackContext context)
    {
        if (context.started && !juegoEnCurso) ReiniciarPartida();
    }

    // Métodos para el sistema de eventos
    public void OnSalir(InputAction.CallbackContext context)
    {
        if (context.started) Application.Quit();
    }

    public void OnLanzar(InputAction.CallbackContext context)
    {
        if (context.started && !juegoEnCurso)
        {
            juegoEnCurso = true;
            pelota.gameObject.SetActive(true);
            uiManager.ToggleStartText(false);
        }
    }

    private void ReiniciarPartida()
    {
        p1Score = 0;
        p2Score = 0;
        uiManager.ActualizarPuntuacion(0, 0);
        uiManager.OcultarMensajes();
        uiManager.ToggleStartText(true);

        juegoEnCurso = true;
        pelota.gameObject.SetActive(true);
    }
}
