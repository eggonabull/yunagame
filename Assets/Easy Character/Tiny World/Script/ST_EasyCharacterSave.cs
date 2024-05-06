using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ST_EasyCharacterSave : MonoBehaviour
{
    public GameObject Character;
    private string Path = "Assets/Easy Character/Tiny World/Save Character/";
    public GameObject NameInput;
    private string Name;
    private Text NameText;

    public void SavePrefab()
    {
        NameText = NameInput.GetComponent<Text>();
        Name = NameText.text;
        if (string.IsNullOrEmpty(Name))
        {
            Debug.Log("Please set the name.");
        }
        else
        {
            PrefabUtility.SaveAsPrefabAsset(Character, Path + Name + ".prefab");
            Debug.Log("Your character has been saved. -> Assets/Easy Character/Tiny World/Save Character/" + Name);
        }
           
    }
}
