using System;
using System.Collections.Generic;
using PureMVC.Core;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using Unity.Collections;

namespace PureMVC.Patterns
{

    // 通知观察者的类
    /// <summary>
    /// A Base <c>INotifier</c> implementation
    /// </summary>
    /// <remarks>
    ///     <para><c>MacroCommand, Command, Mediator</c> and <c>Proxy</c> all have a need to send <c>Notifications</c></para>
    ///     <para>The <c>INotifier</c> interface provides a common method called <c>sendNotification</c> that relieves implementation code of the necessity to actually construct <c>Notifications</c></para>
    ///     <para>The <c>Notifier</c> class, which all of the above mentioned classes extend, provides an initialized reference to the <c>Facade</c> Singleton, which is required for the convienience method for sending <c>Notifications</c>, but also eases implementation as these classes have frequent <c>Facade</c> interactions and usually require access to the facade anyway</para>
    /// </remarks>
	/// <see cref="PureMVC.Patterns.Facade"/>
	/// <see cref="PureMVC.Patterns.Mediator"/>
	/// <see cref="PureMVC.Patterns.Proxy"/>
	/// <see cref="PureMVC.Patterns.SimpleCommand"/>
	/// <see cref="PureMVC.Patterns.MacroCommand"/>
    public class Notifier : INotifier,ILife
    {

        protected static Notifier m_instance;
        public static Notifier Instance
        {
            get
            {
                return m_instance;
            }
        }

        protected Notifier()
        {
            m_notifiMap = new Dictionary<NotifyDefine, List<IObserver>>();
            InitMe();
        }

        static Notifier()
        {
            if (m_instance == null)
            {
                lock (m_staticSyncRoot)
                {
                    if (m_instance == null)
                        m_instance = new Notifier();
                }
            }
        }

        /// <summary>
        ///  添加观察者
        /// </summary>
        /// <param name="notifi"></param>
        /// <param name="observer"></param>
        public virtual void RegisterObserver(NotifyDefine notifi, IObserver observer)
        {
            lock (m_syncRoot)
            {
                if (!m_notifiMap.ContainsKey(notifi))
                {
                    m_notifiMap[notifi] = new List<IObserver>();
                }

                m_notifiMap[notifi].Add(observer);
            }
        }

        /// <summary>
        /// 移除观察者
        /// </summary>
        /// <param name="notifyid"></param>
        /// <param name="observer"></param>
        public virtual void RemoveObserver(NotifyDefine notifyid, IObserver observer)
        {
            lock (m_syncRoot)
            {
                if (!m_notifiMap.ContainsKey(notifyid))
                {
                    return;
                }

                IList<IObserver> observers = m_notifiMap[notifyid];
                // find the observer for the notifyContext
                for (int i = 0; i < observers.Count; i++)
                {
                    if (observers[i].Equals(observer))
                    {
                        observers.RemoveAt(i);
                        break;
                    }
                }
                
                if (observers.Count == 0)
                {
                    m_notifiMap.Remove(notifyid);
                }

            }
        }

        public virtual void NotifyObservers<SendEntity, Param>(INotification<SendEntity, Param> note)
        {
            IList<IObserver> observers = null;

            lock (m_syncRoot)
            {
                if (m_notifiMap.ContainsKey(note.NotifiId))
                {
                    // Get a reference to the observers list for this notification name
                    IList<IObserver> observers_ref = m_notifiMap[note.NotifiId];
                    // Copy observers from reference array to working array, 
                    // since the reference array may change during the notification loop
                    observers = new List<IObserver>(observers_ref);
                }
            }

            // Notify outside of the lock
            if (observers != null)
            {
                // Notify Observers from the working array				
                for (int i = 0; i < observers.Count; i++)
                {
                    IObserver observer = observers[i];
                    observer.OnNotify(note);
                }
            }
            //查看命令里面是否有这个
            Facade.Instance.ExcuteCmd(note);
        }

        public void SendNotification(NotifyDefine notifiid)
        {
            Notification<object, object> notification = Notification<object, object>.createObject();
            notification.InitData(notifiid, null, null);
            NotifyObservers(notification);
        }

        public void SendNotification<Param>(NotifyDefine notifiid, Param body)
        {
            Notification<object, Param> notification = Notification<object, Param>.createObject();
            notification.InitData(notifiid, null, body);
            NotifyObservers(notification);
        }

        public void SendNotification<SendEntity, Param>(NotifyDefine notifiid, SendEntity send, Param body)
        {

            Notification<SendEntity, Param> notification = Notification<SendEntity, Param>.createObject();
            notification.InitData(notifiid, send, body);
            NotifyObservers(notification);
        }


        /// <summary>
        /// 初始化
        /// </summary>
        public void InitMe()
        {
     
        }

        public void OverLife()
        {
            foreach (var item in m_notifiMap)
            {
                item.Value.Clear();
            }
            m_notifiMap.Clear();
        }

        #region Accessors

        /// <summary>
        /// Local reference to the Facade Singleton
        /// </summary>

        private Dictionary<NotifyDefine,List<IObserver>> m_notifiMap;

        protected readonly object m_syncRoot = new object();
        /// <summary>
        /// Used for locking the instance calls
        /// </summary>
        protected static readonly object m_staticSyncRoot = new object();
        #endregion
    }
}
