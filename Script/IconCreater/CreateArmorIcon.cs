using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

#if UNITY_EDITOR
public class CreateArmorIcon : MonoBehaviour
{
    public CharacterBase CharacterBase;
    public EquipData EquipData;
    private string IconPath = @"Assets/Assets/UIAssets/ArmorIcon/";
    int IconIndex = 0;
    public int ArmorIndex = 0;
    public ArmorType ArmorType;
    private void Awake()
    {
        this.CharacterBase = GetComponent<CharacterBase>();
    }
    // Start is called before the first frame update
    void Start()
    {
        CharacterBase.InitCharacter();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(TakeShot(this.IconIndex));
            this.IconIndex++;
        }
        if(Input.GetKeyDown(KeyCode.L)) 
        {
            StartCoroutine(TakeShot(this.ArmorIndex));
        }
    }
    IEnumerator TakeShot(int iconIndex)
    {
        if (EquipData.AllEquips[iconIndex].armorType == ArmorType)
        {
            CharacterBase.SceenShotEquip(iconIndex, EquipData.AllEquips[iconIndex].armorType);
        }
        else
            yield break;
        yield return new WaitForEndOfFrame();
        TakeSceenShot(EquipData.AllEquips[iconIndex].itemName);
        AssetDatabase.Refresh();
    }
    public void TakeSceenShot(string IconName)
    {
        int height = 512;
        int width = 512;
        Texture2D texture2D = new Texture2D(width, height,TextureFormat.ARGB32, false);
        Rect rect = new Rect((1920 - width) / 2, (1080 - height) / 2, width, height);
        texture2D.ReadPixels(rect, 0, 0);
        texture2D.Apply();

        byte[] bytes = texture2D.EncodeToPNG();
        System.IO.File.WriteAllBytes(IconPath + IconName + ".png", bytes);
        this.IconIndex++;
    }
}
#endif