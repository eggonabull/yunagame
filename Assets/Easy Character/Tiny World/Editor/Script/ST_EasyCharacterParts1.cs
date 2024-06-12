using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;

public class ST_EasyCharacterParts1 : MonoBehaviour
{
    public UnityEngine.U2D.Animation.SpriteResolver SpriteResolver;
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
        SpriteResolver.SetCategoryAndLabel(Category, Label);
        SpriteResolver.ResolveSpriteToSpriteRenderer();
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
        SpriteResolver.SetCategoryAndLabel(Category, Label);
        SpriteResolver.ResolveSpriteToSpriteRenderer();
    }

}

