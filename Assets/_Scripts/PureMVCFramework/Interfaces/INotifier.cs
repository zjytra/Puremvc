/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System;
using Unity.Collections;

#endregion

namespace PureMVC.Interfaces
{
    /// <summary>
    /// The interface definition for a PureMVC Notifier
    /// </summary>
    /// <remarks>
    ///     <para><c>MacroCommand, Command, Mediator</c> and <c>Proxy</c> all have a need to send <c>Notifications</c></para>
    ///     <para>The <c>INotifier</c> interface provides a common method called <c>sendNotification</c> that relieves implementation code of the necessity to actually construct <c>Notifications</c></para>
    ///     <para>The <c>Notifier</c> class, which all of the above mentioned classes extend, also provides an initialized reference to the <c>Facade</c> Singleton, which is required for the convienience method for sending <c>Notifications</c>, but also eases implementation as these classes have frequent <c>Facade</c> interactions and usually require access to the facade anyway</para>
    /// </remarks>
	/// <see cref="PureMVC.Interfaces.IFacade"/>
	/// <see cref="PureMVC.Interfaces.INotification"/>
    public interface INotifier
    {

        void RegisterObserver(NotifyDefine notifi, IObserver observer);

        void RemoveObserver(NotifyDefine notifi, IObserver observer);
		void NotifyObservers<SendEntity, Param>( INotification<SendEntity, Param> note);
        /// <summary>
        ///  //发送 给观察者或命令
        /// </summary>
        /// <param name="notifi"></param>
		void SendNotification(NotifyDefine notifi); 
        void SendNotification<Param>(NotifyDefine notifiid, Param body);
		void SendNotification<SendEntity, Param>(NotifyDefine notifiid, SendEntity send, Param body);
    }
}
