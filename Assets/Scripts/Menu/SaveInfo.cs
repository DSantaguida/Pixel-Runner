using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

public class SaveInfo : MonoBehaviour
{
  //   public SlotLoader SlotLoader;
  //data is what is finally saved
  public Dictionary<string, int> data;

  private string saveName = "PixelRunner.save";

  void Awake()
  {
    //LoadUpgradeData();
    LoadData();

    if (data == null)
      data = new Dictionary<string, int>();
    //WARNING! data.Clear() deletes EVERYTHING
    //data.Clear();
    //SaveData();
  }

  public void LoadData()
  {
    //this loads the data
    data = SaveInfo.DeserializeData<Dictionary<string, int>>(saveName);
    if (data == null)
      data = data = new Dictionary<string, int>();
  }

  public void SaveData()
  {
    //this saves the data
    SaveInfo.SerializeData(data, saveName);
  }
  public static void SerializeData<T>(T data, string path)
  {
    //this is just magic to save data.
    //if you're interested read up on serialization
    FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
    BinaryFormatter formatter = new BinaryFormatter();
    try
    {
      formatter.Serialize(fs, data);
      Debug.Log("Data written to " + path + " @ " + DateTime.Now.ToShortTimeString());
    }
    catch (SerializationException e)
    {
      Debug.LogError(e.Message);
    }
    finally
    {
      fs.Close();
    }
  }

  public static T DeserializeData<T>(string path)
  {
    //this is the magic that deserializes the data so we can load it
    T data = default(T);

    if (File.Exists(path))
    {
      FileStream fs = new FileStream(path, FileMode.Open);
      try
      {
        BinaryFormatter formatter = new BinaryFormatter();
        data = (T)formatter.Deserialize(fs);
        Debug.Log("Data read from " + path);
      }
      catch (SerializationException e)
      {
        Debug.LogError(e.Message);
      }
      finally
      {
        fs.Close();
      }
    }
    return data;
  }
}
