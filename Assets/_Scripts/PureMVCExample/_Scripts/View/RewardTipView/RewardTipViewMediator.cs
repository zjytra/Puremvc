using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Interfaces;


/// <summary>
/// RewardTipView的Mediator
/// </summary>
public class RewardTipViewMediator:PureMVC.Patterns.Mediator
{
	public new const string NAME = "RewardTipViewMediator";

	public RewardTipView View;

	public RewardTipViewMediator(object viewComponent)
		:base(NAME,viewComponent)
	{
		//前提只有ViewComponent初始化成功，才可以进行UI的操
		View = ((GameObject)ViewComponent).GetComponent<RewardTipView>();
		Debug.Log("RewardTip mediator");

		//绑定按钮事件
		View.Back.onClick.AddListener(OnClickBack);
	}

	public void OnClickBack()
	{
		View.gameObject.SetActive (false);

		  //重新随机奖励列表
		Facade.SendNotification(NotifyDefine.Notify_refresh_bonus_items);
	}
		
	/// <summary>
	/// 事件监听 
	/// 不可以为Null，否则无法注册
	/// </summary>
	/// <returns></returns>
	public override IList<NotifyDefine> ListNotificationInterests()
	{
		IList<NotifyDefine> list = new List<NotifyDefine>()
		{ NotifyDefine.Notify_RewardTipView};

		return list;
	}

    /// <summary>
    /// 监听消息
    /// </summary>
    /// <param name="notification"></param>
    public override void HandleNotify<SendEntity, Param>(INotification<SendEntity, Param> notification)
    {
        switch (notification.NotifiId)
        {
            case NotifyDefine.Notify_RewardTipView:
                if (!View.isActiveAndEnabled)
                {
                    View.gameObject.SetActive(true);
                }
                string text = notification.Body as string;
                //update text
                View.SetText(text);

                break;
        }
    }

}