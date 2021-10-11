using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
  public GameObject levelHolder;
  public GameObject levelIcon;
  public GameObject canvas;
  public int numberOfLevels = 50;
  public int spacing;
  private Rect panelDimensions;
  private Rect iconDimensions;
  private int amountPerPage;
  private int currentLevelCount;
  private SaveInfo saveInfo;

  void Start()
  {
    saveInfo = new SaveInfo();
    saveInfo.LoadData();
    panelDimensions = levelHolder.GetComponent<RectTransform>().rect;
    iconDimensions = levelIcon.GetComponent<RectTransform>().rect;
    int maxInARow = Mathf.FloorToInt(panelDimensions.width / iconDimensions.width);
    int maxInACol = Mathf.FloorToInt(panelDimensions.height / iconDimensions.height);
    amountPerPage = maxInARow * maxInACol;
    int totalPages = Mathf.CeilToInt((float)numberOfLevels / amountPerPage);
    LoadPanels(totalPages);
  }

  void LoadPanels(int numberOfPanels)
  {
    GameObject panelClone = Instantiate(levelHolder) as GameObject;

    for (int i = 1; i <= numberOfPanels; i++)
    {
      currentLevelCount++;
      GameObject panel = Instantiate(panelClone) as GameObject;
      panel.transform.SetParent(canvas.transform, false);
      panel.transform.SetParent(levelHolder.transform);
      panel.name = "Page-" + i;
      panel.GetComponent<RectTransform>().localEulerAngles = new Vector2(panelDimensions.width * (i - 1), 0);
      SetupGrid(panel);
      int numberOfIcons = i == numberOfPanels ? numberOfLevels - currentLevelCount : amountPerPage;
      LoadIcons(numberOfIcons, panel);
    }
    Destroy(panelClone);
  }

  void SetupGrid(GameObject panel)
  {
    GridLayoutGroup grid = panel.AddComponent<GridLayoutGroup>();
    grid.cellSize = new Vector2(iconDimensions.width, iconDimensions.height);
    grid.childAlignment = TextAnchor.MiddleCenter;
    grid.spacing = new Vector2(spacing, spacing);
  }
  void LoadIcons(int numberOfIcons, GameObject parentObject)
  {
    for (int i = 0; i <= numberOfIcons; i++)
    {
      GameObject icon = Instantiate(levelIcon) as GameObject;
      icon.transform.SetParent(canvas.transform, false);
      icon.transform.SetParent(parentObject.transform);
      icon.name = "Level " + (i + 1);
      icon.GetComponentInChildren<Text>().text = (i + 1).ToString();
      icon.GetComponent<Button>().onClick.AddListener(delegate { GoToLevel(icon.name); });
      if (saveInfo.data.ContainsKey("Level" + (i + 1)) && saveInfo.data["Level" + (i + 1)] == 1)
      {
        icon.GetComponent<Image>().color = new Color32(147, 255, 170, 255);
      }
    }

  }

  public void GoToLevel(string level)
  {
    SceneManager.LoadScene(level.Replace(" ", ""));
  }

  public void BackToMenu()
  {
    SceneManager.LoadScene("MainMenu");
  }
}
