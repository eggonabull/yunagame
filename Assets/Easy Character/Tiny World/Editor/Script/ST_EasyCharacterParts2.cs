using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;

public class ST_EasyCharacterParts2 : MonoBehaviour
{
    public UnityEngine.U2D.Animation.SpriteResolver SpriteResolver1;
    public UnityEngine.U2D.Animation.SpriteResolver SpriteResolver2;
    public string Category;
    public string Label;
    public int MaxLabel;
    public Text LabelTxt;

    public void SwitchParts_Add()
    {
        int num = int.Parse(Label);
        num += 1;
        if (num > MaxLabel)
        {
            num = 1;
        }
        LabelTxt.text = num.ToString();
        Label = num.ToString();
        SpriteResolver1.SetCategoryAndLabel(Category, Label);
        SpriteResolver1.ResolveSpriteToSpriteRenderer();
        SpriteResolver2.SetCategoryAndLabel(Category, Label);
        SpriteResolver2.ResolveSpriteToSpriteRenderer();
    }

    public void SwitchParts_Subtract()
    {
        int num = int.Parse(Label);
        num -= 1;
        if (num == 0)
        {
            num = MaxLabel;
        }
        LabelTxt.text = num.ToString();
        Label = num.ToString();
        SpriteResolver1.SetCategoryAndLabel(Category, Label);
        SpriteResolver1.ResolveSpriteToSpriteRenderer();
        SpriteResolver2.SetCategoryAndLabel(Category, Label);
        SpriteResolver2.ResolveSpriteToSpriteRenderer();
    }
}

