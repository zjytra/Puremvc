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
    /// A Singleton <c>IView</c> implementation.
    /// </summary>
    /// <remarks>
    ///     <para>In PureMVC, the <c>View</c> class assumes these responsibilities:</para>
    ///     <list type="bullet">
    ///         <item>Maintain a cache of <c>IMediator</c> instances</item>
    ///         <item>Provide methods for registering, retrieving, and removing <c>IMediators</c></item>
    ///         <item>Managing the observer lists for each <c>INotification</c> in the application</item>
    ///         <item>Providing a method for attaching <c>IObservers</c> to an <c>INotification</c>'s observer list</item>
    ///         <item>Providing a method for broadcasting an <c>INotification</c></item>
    ///         <item>Notifying the <c>IObservers</c> of a given <c>INotification</c> when it broadcast</item>
    ///     </list>
    /// </remarks>
	/// <see cref="PureMVC.Patterns.Mediator"/>
	/// <see cref="PureMVC.Patterns.Observer"/>
	/// <see cref="PureMVC.Patterns.Notification"/>
    public class View : IView,ILife
    {
		#region Constructors

		/// <summary>
        /// Constructs and initializes a new view
        /// </summary>
        /// <remarks>
        /// <para>This <c>IView</c> implementation is a Singleton, so you should not call the constructor directly, but instead call the static Singleton Factory method <c>View.Instance</c></para>
        /// </remarks>
		protected View()
		{
			m_mediatorMap = new Dictionary<string, IMediator>();
            InitMe();
        }

		#endregion
		/// <summary>
		/// Register an <c>IMediator</c> instance with the <c>View</c>
		/// </summary>
		/// <param name="mediator">A reference to the <c>IMediator</c> instance</param>
		/// <remarks>
		///     <para>Registers the <c>IMediator</c> so that it can be retrieved by name, and further interrogates the <c>IMediator</c> for its <c>INotification</c> interests</para>
		///     <para>If the <c>IMediator</c> returns any <c>INotification</c> names to be notified about, an <c>Observer</c> is created encapsulating the <c>IMediator</c> instance's <c>handleNotification</c> method and registering it as an <c>Observer</c> for all <c>INotifications</c> the <c>IMediator</c> is interested in</para>
		/// </remarks>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual void RegisterMediator(IMediator mediator)
		{
			lock (m_syncRoot)
			{
				// do not allow re-registration (you must to removeMediator fist)
				if (m_mediatorMap.ContainsKey(mediator.MediatorName)) return;

				// Register the Mediator for retrieval by name
				m_mediatorMap[mediator.MediatorName] = mediator;

				// Get Notification interests, if any.
				IList<NotifyDefine> interests = mediator.ListNotificationInterests();

				// Register Mediator as an observer for each of its notification interests
				if (interests.Count > 0)
				{
					// Create Observer
					IObserver observer = (Mediator)mediator;
					// Register Mediator as Observer for its list of Notification interests
					for (int i = 0; i < interests.Count; i++)
					{
                        Notifier.Instance.RegisterObserver(interests[i], observer);
					}
				}
			}

			// alert the mediator that it has been registered
			mediator.OnRegister();
		}

		/// <summary>
		/// Retrieve an <c>IMediator</c> from the <c>View</c>
		/// </summary>
		/// <param name="mediatorName">The name of the <c>IMediator</c> instance to retrieve</param>
		/// <returns>The <c>IMediator</c> instance previously registered with the given <c>mediatorName</c></returns>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual IMediator RetrieveMediator(string mediatorName)
		{
			lock (m_syncRoot)
			{
				if (!m_mediatorMap.ContainsKey(mediatorName)) return null;
				return m_mediatorMap[mediatorName];
			}
		}

		/// <summary>
		/// Remove an <c>IMediator</c> from the <c>View</c>
		/// </summary>
		/// <param name="mediatorName">The name of the <c>IMediator</c> instance to be removed</param>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual IMediator RemoveMediator(string mediatorName)
		{
			Mediator mediator = null;
			lock (m_syncRoot)
			{
				// Retrieve the named mediator
				if (!m_mediatorMap.ContainsKey(mediatorName)) return null;
				mediator = (Mediator) m_mediatorMap[mediatorName];

				// for every notification this mediator is interested in...
				IList<NotifyDefine> interests = mediator.ListNotificationInterests();

				for (int i = 0; i < interests.Count; i++)
				{
					// remove the observer linking the mediator 
					// to the notification interest
				   Facade.Instance.RemoveObserver(interests[i], mediator);
				}

				// remove the mediator from the map		
				m_mediatorMap.Remove(mediatorName);
			}

			// alert the mediator that it has been removed
			if (mediator != null) mediator.OnRemove();
			return mediator;
		}

		/// <summary>
		/// Check if a Mediator is registered or not
		/// </summary>
		/// <param name="mediatorName"></param>
		/// <returns>whether a Mediator is registered with the given <code>mediatorName</code>.</returns>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual bool HasMediator(string mediatorName)
		{
			lock (m_syncRoot)
			{
				return m_mediatorMap.ContainsKey(mediatorName);
			}
		}


		/// <summary>
		/// View Singleton Factory method.  This method is thread safe.
		/// </summary>
		public static View Instance
		{
			get
			{
                return m_instance;
			}
		}


		#region Protected & Internal Methods

		/// <summary>
        /// Explicit static constructor to tell C# compiler 
        /// not to mark type as beforefieldinit
        /// </summary>
        static View()
        {
            if (m_instance == null)
            {
                if (m_instance == null) m_instance = new View();
            }
        }

        /// <summary>
        /// Initialize the Singleton View instance
        /// </summary>
        /// <remarks>
        /// <para>Called automatically by the constructor, this is your opportunity to initialize the Singleton instance in your subclass without overriding the constructor</para>
        /// </remarks>
        protected virtual void InitializeView()
		{
		}

        public virtual void InitMe()
        {
            InitializeView();
        }

        public void OverLife()
        {
            m_mediatorMap.Clear();
        }

        #endregion


        /// <summary>
        /// Mapping of Mediator names to Mediator instances
        /// </summary>
        protected IDictionary<string, IMediator> m_mediatorMap;
	
        /// <summary>
        /// Singleton instance
        /// </summary>
		protected static volatile View m_instance;

		/// <summary>
		/// Used for locking
		/// </summary>
		protected readonly object m_syncRoot = new object();

		/// <summary>
		/// Used for locking the instance calls
		/// </summary>
		protected static readonly object m_staticSyncRoot = new object();
	}
}