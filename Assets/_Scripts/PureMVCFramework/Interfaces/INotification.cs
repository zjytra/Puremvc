/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System;

#endregion

namespace PureMVC.Interfaces
{
    /// <summary>
    /// The interface definition for a PureMVC Notification
    /// </summary>
    /// <remarks>
    ///     <para>PureMVC does not rely upon underlying event models</para>
    ///     <para>The Observer Pattern as implemented within PureMVC exists to support event-driven communication between the application and the actors of the MVC triad</para>
    ///     <para>Notifications are not meant to be a replacement for Events. Generally, <c>IMediator</c> implementors place event handlers on their view components, which they then handle in the usual way. This may lead to the broadcast of <c>Notification</c>s to trigger <c>ICommand</c>s or to communicate with other <c>IMediators</c>. <c>IProxy</c> and <c>ICommand</c> instances communicate with each other and <c>IMediator</c>s by broadcasting <c>INotification</c>s</para>
    /// </remarks>
	/// <see cref="PureMVC.Interfaces.IView"/>
	/// <see cref="PureMVC.Interfaces.IObserver"/>
    public interface INotification<SendEntity, Param>
    {
        /// <summary>
        /// The name of the <c>INotification</c> instance
        /// </summary>
        NotifyDefine NotifiId { get; set; }

        /// <summary>
        /// 发送消息的实体
        /// </summary>
        SendEntity Send { get; set; }
        /// <summary>
        /// The body of the <c>INotification</c> instance
        /// </summary>
		Param Body { get; set; }
        /// <summary>
        /// Get the string representation of the <c>INotification</c> instance
        /// </summary>
        /// <returns>The string representation of the <c>INotification</c> instance</returns>
        string ToString();
    }
}
