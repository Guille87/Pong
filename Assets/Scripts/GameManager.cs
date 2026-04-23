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
    private bool partidaTerminada = false;

    void Start()
    {
        Cursor.visible = false;
        PrepararMenuInicio();
    }

    private void PrepararMenuInicio()
    {
        p1Score = 0;
        p2Score = 0;
        juegoEnCurso = false;
        partidaTerminada = false;
        uiManager.ActualizarPuntuacion(0, 0);
        uiManager.OcultarMensajes();
        uiManager.ToggleStartText(true);
        pelota.gameObject.SetActive(false);
    }

    public void SumarPuntoP1() => ProcesarPunto(ref p1Score, 1);
    public void SumarPuntoP2() => ProcesarPunto(ref p2Score, -1);

    private void ProcesarPunto(ref int score, int direccionSiguienteSaque)
    {
        score++;
        uiManager.ActualizarPuntuacion(p1Score, p2Score);

        if (score >= puntosParaGanar)
        {
            int ganador = (p1Score >= puntosParaGanar) ? 1 : 2;
            TerminarJuego(ganador);
        }
        else
        {
            pelota.LanzarDesdeCentro(direccionSiguienteSaque);
        }
    }

    private void TerminarJuego(int ganador)
    {
        juegoEnCurso = false;
        partidaTerminada = true;
        pelota.gameObject.SetActive(false);
        uiManager.MostrarMensajeVictoria(ganador);
    }

    public void OnReiniciar(InputAction.CallbackContext context)
    {
        if (context.started && partidaTerminada) ReiniciarPartida();
    }

    public void OnSalir(InputAction.CallbackContext context)
    {
        if (context.started) Application.Quit();
    }

    public void OnLanzar(InputAction.CallbackContext context)
    {
        if (context.started && !juegoEnCurso && !partidaTerminada)
        {
            juegoEnCurso = true;
            pelota.gameObject.SetActive(true);
            uiManager.ToggleStartText(false);

            int direccionAleatoria = Random.value > 0.5f ? 1 : -1;
            pelota.LanzarDesdeCentro(direccionAleatoria);
        }
    }

    private void ReiniciarPartida()
    {
        PrepararMenuInicio();
    }
}
