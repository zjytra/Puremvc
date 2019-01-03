using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Patterns;
using PureMVC.Interfaces;

public class RewardTipCommand:PureMVC.Patterns.SimpleCommand
{
    public override void Execute<SendEntity, Param>(INotification<SendEntity, Param> note)
    {
        //显示结算结果
        RewardTipViewMediator mediator = Facade.RetrieveMediator(RewardTipViewMediator.NAME) as RewardTipViewMediator;
        if (mediator == null)
        {
            GameObject obj = GameObjectUtility.Instance.CreateGameObject("_Prefabs/RewardTipView");
            mediator = new RewardTipViewMediator(obj);
            Facade.RegisterMediator(mediator);
        }
        //update reward tip view
       Facade. SendNotification(NotifyDefine.Notify_UPDATE_REWARD_TIP_VIEW, note.Body);
    }
 
}

