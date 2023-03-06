using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    protected override void Death()
    {
        SavesManager.Instance.UpdateCharacter(this);
        if (LoadSceneManager.Instance.State == LoadSceneManager.LoadState.Fighting)
        {
            animator.SetTrigger(anim_death_trigger);
            SavesManager.Instance.UpdatePlayerPos(SavesManager.Instance.PlayerPrevNodeIndex);
        }
        else
        {
            
        }
    }

    protected override void LoadingWhenDead()
    {
        Stats.ChHealth.SetIsDead(false);
        Stats.ChHealth.SetCurrentHealth(40);
    }
}
