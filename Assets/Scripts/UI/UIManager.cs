using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
  private static UIManager instance;
  public Text playerMessageText;
  public Text diamondCount;
  public GameObject pausePanel;
  public GameObject completePanel;
  public Image[] lives;

  [SerializeField]
  private bool FPS;
  public static UIManager Instance
  {
    get
    {
      if (instance == null)
      {
        Debug.LogError("UI Manager is Null");
      }
      return instance;
    }
  }


  void Update()
  {
    if (FPS)
      Debug.Log(1.0 / Time.deltaTime);
  }

  private void Awake()
  {
    instance = this;
  }

  public void UpdateDiamondCount(int amount)
  {
    diamondCount.text = amount.ToString();
  }

  public void SetLives(int livesLeft)
  {
    lives[livesLeft].enabled = false;
  }

  public void SetAllLives(bool flag)
  {
    foreach (Image life in lives)
    {
      life.enabled = flag;
    }
  }

  public void PlayerMessage(string message, float duration)
  {
    playerMessageText.text = message;
    playerMessageText.enabled = true;
    StartCoroutine(DisablePlayerTextAfterSec(duration));
  }

  private IEnumerator DisablePlayerTextAfterSec(float duration)
  {
    yield return new WaitForSeconds(duration);
    playerMessageText.enabled = false;
  }

  public void PauseGame()
  {
    pausePanel.SetActive(true);
    Time.timeScale = 0;
  }

  public void ResumeGame()
  {
    pausePanel.SetActive(false);
    Time.timeScale = 1;
  }

  public void EndGame()
  {
    completePanel.SetActive(true);
    Time.timeScale = 0;
  }

  public void MainMenu()
  {
    SceneManager.LoadScene("MainMenu");
  }

  public void NextLevel()
  {
    int level = int.Parse(SceneManager.GetActiveScene().name.Substring(5, SceneManager.GetActiveScene().name.Length - 5));
    string scene = "Level" + (level + 1);

    if (Application.CanStreamedLevelBeLoaded(scene))
      SceneManager.LoadScene(scene);
    else
      SceneManager.LoadScene("MainMenu");
  }
}
