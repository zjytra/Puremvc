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
    /// The interface definition for a PureMVC Controller
    /// </summary>
    /// <remarks>
    ///     <para>In PureMVC, an <c>IController</c> implementor follows the 'Command and Controller' strategy, and assumes these responsibilities:</para>
    ///     <list type="bullet">
    ///         <item>Remembering which <c>ICommand</c>s are intended to handle which <c>INotifications</c></item>
    ///         <item>Registering itself as an <c>IObserver</c> with the <c>View</c> for each <c>INotification</c> that it has an <c>ICommand</c> mapping for</item>
    ///         <item>Creating a new instance of the proper <c>ICommand</c> to handle a given <c>INotification</c> when notified by the <c>View</c></item>
    ///         <item>Calling the <c>ICommand</c>'s <c>execute</c> method, passing in the <c>INotification</c></item>
    ///     </list>
    /// </remarks>
	/// <see cref="PureMVC.Interfaces.INotification"/>
	/// <see cref="PureMVC.Interfaces.ICommand"/>
    public interface IController
    {
        void RegisterCommand(NotifyDefine notiid, ICommand command);
        void RemoveCommand(NotifyDefine notiid);
		bool HasCommand(NotifyDefine notiid);
        void ExcuteCmd<SendEntity, Param>(INotification<SendEntity, Param> note);
        ICommand GetCommand(NotifyDefine notifyid);

    }
}
