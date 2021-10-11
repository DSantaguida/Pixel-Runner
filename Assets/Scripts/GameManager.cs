using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

  private Vector2 currentCheckpoint;
  public Vector2 initialSpawn;
  private Player currentPlayer;
  private Player playerPrefab;
  private int checkpointDiamonds;
  private static GameManager instance;
  [SerializeField]
  private List<GameObject> DestroyedObjects;

  [SerializeField]
  private List<Player> playerPrefabList;
  private SaveInfo saveInfo;

  public static GameManager Instance
  {
    get
    {
      if (instance == null)
      {
        Debug.LogError("Game Manager is Null");
      }
      return instance;
    }
  }

  private void Awake() //triggered before Start() of other classes
  {
    saveInfo = new SaveInfo();
    saveInfo.LoadData();

    playerPrefab = playerPrefabList[saveInfo.data["selectedChar"]];

    instance = this;
    currentCheckpoint = initialSpawn;
    currentPlayer = Instantiate(playerPrefab, currentCheckpoint, Quaternion.identity);
  }

  public void updateCheckpoint(Vector2 checkpoint, int diamonds)
  {
    currentCheckpoint = checkpoint;
    checkpointDiamonds = diamonds;
  }

  public void SpawnPlayer()
  {
    currentPlayer = Instantiate(playerPrefab, currentCheckpoint, Quaternion.identity);
    currentPlayer.SetDiamonds(checkpointDiamonds);
    RespawnObjects();
    ClearDestroyedObjects();
  }

  public void EndLevel()
  {

    if (!saveInfo.data.ContainsKey(SceneManager.GetActiveScene().name))
    {
      saveInfo.data[SceneManager.GetActiveScene().name] = 1;
      saveInfo.data["diamonds"] += currentPlayer.diamonds;
      saveInfo.SaveData();
      Debug.Log("Data saved to key " + SceneManager.GetActiveScene().name);
    }
    UIManager.Instance.EndGame();
  }

  public void DestroyObject(GameObject destroyed)
  {
    if (destroyed.tag != "Player")
      DestroyedObjects.Add(destroyed);
  }

  public void ClearDestroyedObjects()
  {
    DestroyedObjects.Clear();
  }

  private void RespawnObjects()
  {
    foreach (GameObject obj in DestroyedObjects)
    {
      obj.SetActive(true);
      Enemy enemy = obj.GetComponent<Enemy>();
      if (enemy != null)
      {
        enemy.health = 1;
      }
    }
  }

  //temp
  void Update()
  {
    if (currentPlayer == null)
    {
      SpawnPlayer();
    }
  }
}
