using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmpScoreP1;
    [SerializeField] TextMeshProUGUI tmpScoreP2;
    [SerializeField] GameObject pelota;
    int p1Score;
    int p2Score;

    bool running = false;

    public void addPointP1() {
        p1Score++;
        UpdateScores();
    }
    public void addPointP2() {
        p2Score++;
        UpdateScores();
    }
    
    void Start()
    {
        Cursor.visible = false;
        UpdateScores();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        
        if (!running && Input.GetKeyDown(KeyCode.Space))
        {
            pelota.SetActive(true);
            running = true;
        }
    }
    
    void UpdateScores() {
        tmpScoreP1.text = p1Score.ToString();
        tmpScoreP2.text = p2Score.ToString();
    }
}
