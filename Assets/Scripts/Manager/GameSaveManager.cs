using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
/// <summary>
/// 
/// </summary>
public class GameSaveManager : MonoBehaviour
{
    public Inventory myInventory; //可以用这种方法储存任意类都可以
    public void SaveGame()
    {
        Debug.Log(Application.persistentDataPath);
        if (!Directory.Exists(Application.persistentDataPath + "/saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }

        BinaryFormatter formatter = new BinaryFormatter();//二进制转化

        FileStream file = File.Create(Application.persistentDataPath + "/saves/inventory.txt");

        var json = JsonUtility.ToJson(myInventory);

        formatter.Serialize(file, json);
        file.Close();
    }

    public void LoadGame()
    {
        BinaryFormatter bf = new BinaryFormatter();//二进制转化
        //反序列化 先判断文件在不在 
        if (File.Exists(Application.persistentDataPath + "/saves/inventory.txt"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/saves/inventory.txt", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), myInventory);
            file.Close();
        }
    }
}
