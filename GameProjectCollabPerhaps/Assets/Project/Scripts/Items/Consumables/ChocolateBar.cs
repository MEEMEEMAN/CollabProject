using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocolateBar : Equippable
{

    public override void OffensiveStanceUpdate(Animator CharacterModelAnimator)
    {
        if (CustomInputManager.GetMouseTap(0))
        {
            CharacterModelAnimator.SetTrigger("shot");
            StartCoroutine(AnimatorResetWait(0.05f, CharacterModelAnimator));
        }
    }

    public override IEnumerator AnimatorResetWait(float time = 0.05F, Animator characterModelAnimator = null)
    {
        yield return new WaitForSeconds(time);
        characterModelAnimator.ResetTrigger("shot");
    }
}
