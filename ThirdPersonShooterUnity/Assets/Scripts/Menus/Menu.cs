using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menuUI;
    public Text enemiesQuantity;
    public Slider quantity;
    public PlayerHealth player;

    bool isPause;
    GameSettings gameSettings;

    private void Awake()
    {
        gameSettings = GameObject.FindObjectOfType<GameSettings>();
        quantity.value = gameSettings.enemies;
        gameSettings.ApplySettings();

        Setup();
        player.GetComponent<PlayerHealth>();
    }

    public void Update()
    {
        enemiesQuantity.text = quantity.value.ToString();
        if (!player.playerDead)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isPause)
                {
                    Pause();
                }
                else
                {
                    Continue();
                }
            }
        }
        else
        {
            Pause();
        }
    }

    private void Setup()
    {
        if(gameSettings.enemies <= 0)
        {
            Pause();
        }
        else
        {
            Continue();
        }
    }

    private void Continue()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        menuUI.SetActive(false);
        isPause = false;
    }

    private void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        menuUI.SetActive(true);
        isPause = true;
    }

    public void Play()
    {
        int number = (int)quantity.value;
        gameSettings.Settings(number);
        ResetGame();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
