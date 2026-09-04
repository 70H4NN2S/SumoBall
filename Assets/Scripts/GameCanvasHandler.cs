using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCanvasHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI roundText;

    [Header("UI")]
    [SerializeField] private GameObject gameOnCanvas;
    [SerializeField] private GameObject gamePauseCanvas;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject backdrop;

    private void Start()
    {
        ShowGameUI();
    }

    public void PauseGame()
    {
        gameManager.PauseGame();

        backdrop.SetActive(true);
        gameOnCanvas.SetActive(false);
        gamePauseCanvas.SetActive(true);
    }

    public void ContinueGame()
    {
        gameManager.ContinueGame();

        backdrop.SetActive(false);
        gameOnCanvas.SetActive(true);
        gamePauseCanvas.SetActive(false);
    }

    public void GameOver()
    {
        backdrop.SetActive(false);
        gameOnCanvas.SetActive(false);
        gamePauseCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
    }

    public void Restart()
    {
        // Important because timeScale is global and survives scene loads.
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void DrawRound(int round)
    {
        roundText.text = $"Round: {round}";
    }

    private void ShowGameUI()
    {
        backdrop.SetActive(false);
        gameOnCanvas.SetActive(true);
        gamePauseCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
    }
}