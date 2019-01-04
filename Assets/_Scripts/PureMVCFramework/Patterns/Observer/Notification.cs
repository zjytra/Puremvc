/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System;

using PureMVC.Interfaces;

#endregion

namespace PureMVC.Patterns
{
    /// <summary>
    /// A base <c>INotification</c> implementation
    /// </summary>
    /// <remarks>
    ///     <para>PureMVC does not rely upon underlying event models</para>
    ///     <para>The Observer Pattern as implemented within PureMVC exists to support event-driven communication between the application and the actors of the MVC triad</para>
    ///     <para>Notifications are not meant to be a replacement for Events. Generally, <c>IMediator</c> implementors place event handlers on their view components, which they then handle in the usual way. This may lead to the broadcast of <c>Notification</c>s to trigger <c>ICommand</c>s or to communicate with other <c>IMediators</c>. <c>IProxy</c> and <c>ICommand</c> instances communicate with each other and <c>IMediator</c>s by broadcasting <c>INotification</c>s</para>
    /// </remarks>
	/// <see cref="PureMVC.Patterns.Observer"/>
    public class Notification<SendEntity, Param> : ObjectPool<Notification<SendEntity, Param>>, INotification<SendEntity, Param> 
    {
		#region Constructors

        public Notification()
        {

        }
		/// <summary>
        /// Constructs a new notification with the specified name, default body and type
        /// </summary>
        /// <param name="name">The name of the <c>Notification</c> instance</param>
        public Notification(NotifyDefine notifi)
		{
            m_notifi = notifi;
        }

        /// <summary>
        /// Constructs a new notification with the specified name and body, with the default type
        /// </summary>
        /// <param name="name">The name of the <c>Notification</c> instance</param>
        /// <param name="body">The <c>Notification</c>s body</param>
        public Notification(NotifyDefine notifi,  Param body)
            :this(notifi)
		{
            m_body = body;
        }

        /// <summary>
        /// Constructs a new notification with the specified name, body and type
        /// </summary>
        /// <param name="name">The name of the <c>Notification</c> instance</param>
        /// <param name="body">The <c>Notification</c>s body</param>
        /// <param name="type">The type of the <c>Notification</c></param>
        public Notification(NotifyDefine notifi , SendEntity send,  Param body)
           :this(notifi,body)
		{
			m_send = send;
		}

        /// <summary>
        /// 初始化变量
        /// </summary>
        /// <param name="notifi"></param>
        /// <param name="send"></param>
        /// <param name="body"></param>
        public void InitData(NotifyDefine notifi, SendEntity send, Param body)
        {
            m_notifi = notifi;
            m_send = send;
            m_body = body;
        }
		#endregion

		#region Public Methods

		/// <summary>
		/// Get the string representation of the <c>Notification instance</c>
		/// </summary>
		/// <returns>The string representation of the <c>Notification</c> instance</returns>
		public override string ToString()
		{
			string msg = "Notification Name: " + NotifiId;
			msg += "\nBody:" + ((Body == null) ? "null" : Body.ToString());
			msg += "\nSend:" + ((Send == null) ? "null" : Send.ToString());
			return msg;
		}

        public void  RecycleObj()
        {
            m_notifi = 0;
            reclaimObject(this);
        }

		#endregion

		#region Accessors

		/// <summary>
        /// The name of the <c>Notification</c> instance
        /// </summary>
		public virtual NotifyDefine NotifiId
		{
			get { return m_notifi; }
            set { m_notifi = value; }
		}
		
        /// <summary>
        /// The body of the <c>Notification</c> instance
        /// </summary>
		/// <remarks>This accessor is thread safe</remarks>
		public virtual Param Body
		{
			get
			{
				// Setting and getting of reference types is atomic, no need to lock here
				return m_body;
			}
			set
			{
				m_body = value;
			}
		}



        public SendEntity Send
        {
            get
            {
                return m_send;
            }

            set
            {
                m_send = value;
            }
        }

        private NotifyDefine m_notifi;
        //发送者
        private SendEntity m_send;
        //参数
		private Param m_body;

		#endregion
	}
}
