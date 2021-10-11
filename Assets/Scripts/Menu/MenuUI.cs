using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{

  public int characterCost;

  [SerializeField]
  private GameObject shopPanel;

  [SerializeField]
  private Text diamondCount;

  [SerializeField]
  private List<Button> characterButtonList;

  private int selectedChar;
  private int purchaseChar;
  private SaveInfo saveInfo;

  [SerializeField]
  private GameObject purchasePanel;

  [SerializeField]
  private Text insufficientText;

  private string[] unlockableChar = {
    "Ninja Frog",
    "Virtual Guy",
    "Mask Dude",
    "Pink Man"
  };

  void Start()
  {
    saveInfo = new SaveInfo();
    saveInfo.LoadData();
    InitSave();
    selectedChar = saveInfo.data["selectedChar"];
    Time.timeScale = 0;
  }

  private void InitSave()
  {
    if (!saveInfo.data.ContainsKey("diamonds"))
      saveInfo.data["diamonds"] = 0;

    if (!saveInfo.data.ContainsKey("selectedChar"))
      saveInfo.data["selectedChar"] = 0;

    foreach (string index in unlockableChar)
    {
      if (!saveInfo.data.ContainsKey(index))
        saveInfo.data[index] = 0;
    }
    saveInfo.data[unlockableChar[0]] = 1;
    saveInfo.SaveData();
  }

  public void StartLevelSelect()
  {
    SceneManager.LoadScene("LevelSelect");
  }

  public void StartShop()
  {
    saveInfo.LoadData();
    diamondCount.text = saveInfo.data["diamonds"].ToString();
    SelectCharacter(selectedChar);
    shopPanel.SetActive(true);
    purchasePanel.SetActive(false);
  }

  public void ConfirmShop()
  {
    if (!purchasePanel.activeSelf)
    {
      saveInfo.LoadData();
      saveInfo.data["selectedChar"] = selectedChar;
      saveInfo.SaveData();
      shopPanel.SetActive(false);
    }
  }

  public void SelectCharacter(int selected)
  {

    if (saveInfo.data[unlockableChar[selected]] == 1)
    {
      selectedChar = selected;
    }
    else
    {
      purchasePanel.SetActive(true);
      insufficientText.enabled = false;
      purchaseChar = selected;
      SetPurchaseText(purchaseChar);
    }

    for (int i = 0; i < characterButtonList.Count; i++)
    {
      if (saveInfo.data[unlockableChar[i]] == 0)
        characterButtonList[i].GetComponentInChildren<Image>().color = new Color32(159, 30, 30, 255);
      else
        characterButtonList[i].GetComponentInChildren<Image>().color = new Color32(255, 255, 255, 255);
    }

    characterButtonList[selectedChar].GetComponentInChildren<Image>().color = new Color32(147, 255, 170, 255);
  }

  private void SetPurchaseText(int character)
  {
    purchasePanel.GetComponentInChildren<Text>().text = "Purchase " + unlockableChar[character] + " for " + characterCost + " diamonds?";
  }

  public void PurchaseCharacter()
  {
    if (saveInfo.data["diamonds"] > characterCost)
    {
      saveInfo.LoadData();
      saveInfo.data[unlockableChar[purchaseChar]] = 1;
      saveInfo.data["diamonds"] -= characterCost;
      diamondCount.text = saveInfo.data["diamonds"].ToString();
      saveInfo.SaveData();
      purchasePanel.SetActive(false);
      SelectCharacter(purchaseChar);
    }
    else
    {
      insufficientText.enabled = true;
    }
  }

  public void CancelPurchase()
  {
    purchasePanel.SetActive(false);
  }


}
