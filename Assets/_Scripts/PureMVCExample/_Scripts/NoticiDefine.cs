using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NotifyDefine : uint 
{
    Notify_NULL,                /// 没有命令
    Notify_Startup,               /// 开始命令
    Notify_refresh_bonus_items,
    Notify_Play,
    Notify_Update_Player_Data,
    Notify_refresh_bonus_ui,
    Notify_CREATE_BONUS_ITEMS,
    Notify_UPDATE_REWARD_TIP_VIEW,
    Notify_RewardTipView,
    Notify_Test = 10, // 测试消息
    Notify_Max = uint.MaxValue
}
