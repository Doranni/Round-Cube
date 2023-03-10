using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character
{
    public (Card card, bool useOnYourself) ChooseCard()
    {
        return (Equipment.Storages[IStorage.StorageNames.WeaponSlot].Cards[0], false);
    }

    protected override void Death()
    {
        SavesManager.Instance.UpdateCharacter(this);
        if (LoadSceneManager.Instance.State == LoadSceneManager.LoadState.Fighting)
        {
            animator.SetTrigger(anim_death_trigger);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
