/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System;
using System.Reflection;

using PureMVC.Interfaces;

#endregion

namespace PureMVC.Patterns
{
    /// <summary>
    /// A base <c>IObserver</c> implementation
    /// </summary>
    /// <remarks>
    ///     <para>An <c>Observer</c> is an object that encapsulates information about an interested object with a method that should be called when a particular <c>INotification</c> is broadcast</para>
    ///     <para>In PureMVC, the <c>Observer</c> class assumes these responsibilities:</para>
    ///     <list type="bullet">
    ///         <item>Encapsulate the notification (callback) method of the interested object</item>
    ///         <item>Encapsulate the notification context (this) of the interested object</item>
    ///         <item>Provide methods for setting the notification method and context</item>
    ///         <item>Provide a method for notifying the interested object</item>
    ///     </list>
    /// </remarks>
	/// <see cref="PureMVC.Core.View"/>
	/// <see cref="PureMVC.Patterns.Notification"/>
	public class Observer : IObserver
	{

        public virtual void OnNotify<Send, Param>(INotification<Send, Param> notification)
        {
                
        }
        #region Accessors
		/// <summary>
		/// Used for locking
		/// </summary>
		protected readonly object m_syncRoot = new object();

		#endregion
	}
}
