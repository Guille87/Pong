using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipalManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private RectTransform cursor;
    [SerializeField] private RectTransform contenedorBotones;
    private Image cursorImage;

    [Header("Ajustes de Distancia")]
    [SerializeField] private float margenAdicional = 20f;

    [Header("Efecto Selección")]
    [SerializeField] private int parpadeos = 4;
    [SerializeField] private float intervaloParpadeo = 0.1f;

    private GameObject ultimoSeleccionado;
    private bool seleccionando = false;

    void Start()
    {
        if (cursor != null) 
        {
            cursorImage = cursor.GetComponent<Image>();
            cursorImage.enabled = false;
        }

        if (contenedorBotones == null) contenedorBotones = GetComponent<RectTransform>();

        EventSystem.current.SetSelectedGameObject(null);

        if (contenedorBotones.childCount > 0)
        {
            GameObject primerBoton = contenedorBotones.GetChild(0).gameObject;
            EventSystem.current.SetSelectedGameObject(primerBoton);
            ultimoSeleccionado = primerBoton;
            // Un pequeño delay para dejar que el Layout Group organice los elementos antes de posicionar el cursor
            Invoke(nameof(InicializarPosicion), 0.05f);
        }
    }

    void InicializarPosicion()
    {
        if (ultimoSeleccionado != null)
        {
            ActualizarPosicionCursor(ultimoSeleccionado);
            if (cursorImage != null) cursorImage.enabled = true;
        }
    }

    void Update()
    {
        if (seleccionando) return;

        ManejarNavegacionUI();
        ManejarEntradaConfirmar();
    }

    private void ManejarNavegacionUI()
    {
        GameObject seleccionadoActual = EventSystem.current.currentSelectedGameObject;

        if (seleccionadoActual == null)
        {
            if (ultimoSeleccionado != null) EventSystem.current.SetSelectedGameObject(ultimoSeleccionado);
        }
        else if (seleccionadoActual != ultimoSeleccionado)
        {
            ultimoSeleccionado = seleccionadoActual;
            ActualizarPosicionCursor(ultimoSeleccionado);
        }
    }

    private void ManejarEntradaConfirmar()
    {
        bool enterPresionado = Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame;
        bool espacioPresionado = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool botonSurMando = Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame;

        if (enterPresionado || espacioPresionado || botonSurMando)
        {
            Button btn = ultimoSeleccionado.GetComponent<Button>();
            if (btn != null) btn.onClick.Invoke();
            
            ConfirmarSeleccion();
        }
    }

    void ConfirmarSeleccion()
    {
        if (ultimoSeleccionado != null && !seleccionando)
        {
            StartCoroutine(EfectoParpadeoYEntrar());
        }
    }

    IEnumerator EfectoParpadeoYEntrar()
    {
        seleccionando = true;

        for (int i = 0; i < parpadeos; i++)
        {
            cursorImage.enabled = false;
            yield return new WaitForSeconds(intervaloParpadeo);
            cursorImage.enabled = true;
            yield return new WaitForSeconds(intervaloParpadeo);
        }

        // Esperar un instante extra tras el parpadeo
        yield return new WaitForSeconds(0.2f);

        // Aquí guardaremos más adelante la configuración según el botón pulsado
        // Por ahora, solo cargamos la escena
        SceneManager.LoadScene("MainScene");
    }

    void ActualizarPosicionCursor(GameObject objetivo)
    {
        if (cursor == null || objetivo == null) return;

        TextMeshProUGUI textoBoton = objetivo.GetComponentInChildren<TextMeshProUGUI>();

        if (textoBoton != null)
        {
            // 1. Forzamos la actualización de la malla para tener datos reales
            textoBoton.ForceMeshUpdate();

            // 2. Obtenemos la información de los caracteres
            TMP_TextInfo textInfo = textoBoton.textInfo;

            // 3. Si hay texto, buscamos el origen del primer carácter
            if (textInfo.characterCount > 0)
            {
                // Buscamos la posición horizontal de la primera letra en el espacio local del texto
                float xPrimeraLetraLocal = textInfo.characterInfo[0].origin;

                // Convertimos esa posición local a posición de mundo (World Space)
                // Esto nos da el punto EXACTO donde empieza la primera letra en la pantalla
                Vector3 posicionInicioTextoMundo = textoBoton.transform.TransformPoint(new Vector3(xPrimeraLetraLocal, 0, 0));

                // 4. Aplicamos la posición al cursor
                // Usamos la X del inicio del texto menos el margen, y la Y del botón para que esté centrado verticalmente
                cursor.position = new Vector3(posicionInicioTextoMundo.x - margenAdicional, objetivo.transform.position.y, objetivo.transform.position.z);
            }
        }
    }

    public void SeleccionarModo(int modoID)
    {
        // Usamos un switch para configurar GameSettings según el botón pulsado
        // P1: Izq Abajo | P2: Der Abajo | P3: Izq Arriba | P4: Der Arriba
        
        switch (modoID)
        {
            case 1: // 1 Jugador vs IA
                ConfigurarPartida(GameSettings.ControlType.Humano, GameSettings.ControlType.IA, 
                                GameSettings.ControlType.Desactivado, GameSettings.ControlType.Desactivado);
                break;
            case 2: // 1 Jugador vs 1 Jugador
                ConfigurarPartida(GameSettings.ControlType.Humano, GameSettings.ControlType.Humano, 
                                GameSettings.ControlType.Desactivado, GameSettings.ControlType.Desactivado);
                break;
            case 3: // 1 Jugador 1 IA vs 2 IA
                ConfigurarPartida(GameSettings.ControlType.Humano, GameSettings.ControlType.IA, 
                                GameSettings.ControlType.IA, GameSettings.ControlType.IA);
                break;
            case 4: // 1 Jugador 1 IA vs 1 Jugador 1 IA
                ConfigurarPartida(GameSettings.ControlType.Humano, GameSettings.ControlType.Humano, 
                                GameSettings.ControlType.IA, GameSettings.ControlType.IA);
                break;
            case 5: // 2 Jugadores vs IA
                ConfigurarPartida(GameSettings.ControlType.Humano, GameSettings.ControlType.IA, 
                                GameSettings.ControlType.Humano, GameSettings.ControlType.Desactivado);
                break;
            case 6: // 2 Jugadores vs 1 Jugador 1 IA
                ConfigurarPartida(GameSettings.ControlType.Humano, GameSettings.ControlType.Humano, 
                                GameSettings.ControlType.Humano, GameSettings.ControlType.IA);
                break;
            case 7: // 2 Jugadores vs 2 Jugadores
                ConfigurarPartida(GameSettings.ControlType.Humano, GameSettings.ControlType.Humano, 
                                GameSettings.ControlType.Humano, GameSettings.ControlType.Humano);
                break;
        }
    }

    private void ConfigurarPartida(GameSettings.ControlType p1, GameSettings.ControlType p2, GameSettings.ControlType p3, GameSettings.ControlType p4)
    {
        GameSettings.P1 = p1;
        GameSettings.P2 = p2;
        GameSettings.P3 = p3;
        GameSettings.P4 = p4;
    }
}
