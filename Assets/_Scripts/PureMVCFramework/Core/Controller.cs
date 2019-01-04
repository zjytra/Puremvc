/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System;
using System.Collections.Generic;

using PureMVC.Interfaces;
using PureMVC.Patterns;

#endregion

namespace PureMVC.Core
{
    /// <summary>
    /// A Singleton <c>IController</c> implementation.
    /// </summary>
    /// <remarks>
    /// 	<para>In PureMVC, the <c>Controller</c> class follows the 'Command and Controller' strategy, and assumes these responsibilities:</para>
    /// 	<list type="bullet">
    /// 		<item>Remembering which <c>ICommand</c>s are intended to handle which <c>INotifications</c>.</item>
    /// 		<item>Registering itself as an <c>IObserver</c> with the <c>View</c> for each <c>INotification</c> that it has an <c>ICommand</c> mapping for.</item>
    /// 		<item>Creating a new instance of the proper <c>ICommand</c> to handle a given <c>INotification</c> when notified by the <c>View</c>.</item>
    /// 		<item>Calling the <c>ICommand</c>'s <c>execute</c> method, passing in the <c>INotification</c>.</item>
    /// 	</list>
    /// 	<para>Your application must register <c>ICommands</c> with the <c>Controller</c>.</para>
    /// 	<para>The simplest way is to subclass <c>Facade</c>, and use its <c>initializeController</c> method to add your registrations.</para>
    /// </remarks>
	/// <see cref="PureMVC.Core.View"/>
	/// <see cref="PureMVC.Patterns.Observer"/>
	/// <see cref="PureMVC.Patterns.Notification"/>
	/// <see cref="PureMVC.Patterns.SimpleCommand"/>
	/// <see cref="PureMVC.Patterns.MacroCommand"/>
    public class Controller : IController,ILife
	{
		#region Constructors
		protected Controller()
		{
			m_commandMap = new Dictionary<NotifyDefine, ICommand>();
            InitMe();
		}
		#endregion

		#region Public Methods

		public virtual void RegisterCommand(NotifyDefine  notifyid, ICommand command)
		{
				m_commandMap[notifyid] = command;
		}

		public virtual bool HasCommand(NotifyDefine notifyid)
		{
			lock (m_syncRoot)
			{
				return m_commandMap.ContainsKey(notifyid);
			}
		}

		public virtual void RemoveCommand(NotifyDefine notifyid)
		{
			lock (m_syncRoot)
			{
				if (m_commandMap.ContainsKey(notifyid))
				{
					m_commandMap.Remove(notifyid);
				}
			}
		}

        public virtual ICommand GetCommand(NotifyDefine notifyid)
        {
            if (!m_commandMap.ContainsKey(notifyid))
            {
                return null;
            }
            return m_commandMap[notifyid];
        }


        public void ExcuteCmd<SendEntity, Param>(INotification<SendEntity, Param> note)
        {
            if (!m_commandMap.ContainsKey(note.NotifiId))
            {
                return;
            }
            m_commandMap[note.NotifiId].Execute(note);
        }

		#endregion

		#region Accessors

		/// <summary>
		/// Singleton Factory method.  This method is thread safe.
		/// </summary>
		public static Controller Instance
		{
			get
			{
              
                return m_instance;
			}
		}

		#endregion

		#region Protected & Internal Methods

		/// <summary>
		/// Explicit static constructor to tell C# compiler
		/// not to mark type as beforefieldinit
		/// </summary>
		static Controller()
		{
            if (m_instance == null)
            {
                lock (m_staticSyncRoot)
                {
                    if (m_instance == null) m_instance = new Controller();
                }
            }
        }
		protected virtual void InitializeController()
		{
		}

        public void InitMe()
        {
            InitializeController();
        }

        public void OverLife()
        {
            m_commandMap.Clear();
        }

        #endregion

        #region Members


        /// <summary>
        /// Mapping of Notification names to Command Class references
        /// </summary>
        protected IDictionary<NotifyDefine, ICommand> m_commandMap;

        /// <summary>
        /// Singleton instance, can be sublcassed though....
        /// </summary>
		protected static volatile Controller m_instance;

		/// <summary>
		/// Used for locking
		/// </summary>
		protected readonly object m_syncRoot = new object();

		/// <summary>
		/// Used for locking the instance calls
		/// </summary>
		protected static readonly object m_staticSyncRoot = new object();

		#endregion
	}
}
