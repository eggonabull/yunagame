using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_EasyCharacterAnim : MonoBehaviour
{
    public Animator CharacterAnimator;

    public void Anim_iDle()
    {
        CharacterAnimator.SetBool("Walk", false);
        CharacterAnimator.SetBool("Run", false);
        CharacterAnimator.SetBool("Death", false);
        CharacterAnimator.SetBool("Dance", false);
    }

    public void Anim_Trigger(string name)
    {
        CharacterAnimator.SetTrigger(name);
    }

    public void Anim_Bool(string name)
    {
        CharacterAnimator.SetBool("Walk", false);
        CharacterAnimator.SetBool("Run", false);
        CharacterAnimator.SetBool("Death", false);
        CharacterAnimator.SetBool("Dance", false);
        CharacterAnimator.SetBool(name, true);
    }
}
