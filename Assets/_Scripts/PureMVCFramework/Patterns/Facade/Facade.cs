/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System;
using System.Collections.Generic;
using PureMVC.Core;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

#endregion

namespace PureMVC.Patterns
{
    /// <summary>
    /// A base Singleton <c>IFacade</c> implementation
    /// </summary>
    /// <remarks>
    ///     <para>In PureMVC, the <c>Facade</c> class assumes these responsibilities:</para>
    ///     <list type="bullet">
    ///         <item>Initializing the <c>Model</c>, <c>View</c> and <c>Controller</c> Singletons</item>
    ///         <item>Providing all the methods defined by the <c>IModel, IView, &amp; IController</c> interfaces</item>
    ///         <item>Providing the ability to override the specific <c>Model</c>, <c>View</c> and <c>Controller</c> Singletons created</item>
    ///         <item>Providing a single point of contact to the application for registering <c>Commands</c> and notifying <c>Observers</c></item>
    ///     </list>
    ///     <example>
    ///         <code>
    ///	using PureMVC.Patterns;
    /// 
    ///	using com.me.myapp.model;
    ///	using com.me.myapp.view;
    ///	using com.me.myapp.controller;
    /// 
    ///	public class MyFacade : Facade
    ///	{
    ///		// Notification constants. The Facade is the ideal
    ///		// location for these constants, since any part
    ///		// of the application participating in PureMVC 
    ///		// Observer Notification will know the Facade.
    ///		public static const string GO_COMMAND = "go";
    /// 
    ///     // we aren't allowed to initialize new instances from outside this class
    ///     protected MyFacade() {}
    /// 
    ///     // we must specify the type of instance
    ///     static MyFacade()
    ///     {
    ///         instance = new MyFacade();
    ///     }
    /// 
    ///		// Override Singleton Factory method 
    ///		public new static MyFacade getInstance() {
    ///			return instance as MyFacade;
    ///		}
    /// 		
    ///		// optional initialization hook for Facade
    ///		public override void initializeFacade() {
    ///			base.initializeFacade();
    ///			// do any special subclass initialization here
    ///		}
    ///	
    ///		// optional initialization hook for Controller
    ///		public override void initializeController() {
    ///			// call base to use the PureMVC Controller Singleton. 
    ///			base.initializeController();
    /// 
    ///			// Otherwise, if you're implmenting your own
    ///			// IController, then instead do:
    ///			// if ( controller != null ) return;
    ///			// controller = MyAppController.getInstance();
    /// 		
    ///			// do any special subclass initialization here
    ///			// such as registering Commands
    ///			registerCommand( GO_COMMAND, com.me.myapp.controller.GoCommand )
    ///		}
    ///	
    ///		// optional initialization hook for Model
    ///		public override void initializeModel() {
    ///			// call base to use the PureMVC Model Singleton. 
    ///			base.initializeModel();
    /// 
    ///			// Otherwise, if you're implmenting your own
    ///			// IModel, then instead do:
    ///			// if ( model != null ) return;
    ///			// model = MyAppModel.getInstance();
    /// 		
    ///			// do any special subclass initialization here
    ///			// such as creating and registering Model proxys
    ///			// that don't require a facade reference at
    ///			// construction time, such as fixed type lists
    ///			// that never need to send Notifications.
    ///			regsiterProxy( new USStateNamesProxy() );
    /// 			
    ///			// CAREFUL: Can't reference Facade instance in constructor 
    ///			// of new Proxys from here, since this step is part of
    ///			// Facade construction!  Usually, Proxys needing to send 
    ///			// notifications are registered elsewhere in the app 
    ///			// for this reason.
    ///		}
    ///	
    ///		// optional initialization hook for View
    ///		public override void initializeView() {
    ///			// call base to use the PureMVC View Singleton. 
    ///			base.initializeView();
    /// 
    ///			// Otherwise, if you're implmenting your own
    ///			// IView, then instead do:
    ///			// if ( view != null ) return;
    ///			// view = MyAppView.Instance;
    /// 		
    ///			// do any special subclass initialization here
    ///			// such as creating and registering Mediators
    ///			// that do not need a Facade reference at construction
    ///			// time.
    ///			registerMediator( new LoginMediator() ); 
    /// 
    ///			// CAREFUL: Can't reference Facade instance in constructor 
    ///			// of new Mediators from here, since this is a step
    ///			// in Facade construction! Usually, all Mediators need 
    ///			// receive notifications, and are registered elsewhere in 
    ///			// the app for this reason.
    ///		}
    ///	}
    ///         </code>
    ///     </example>
    /// </remarks>
	/// <see cref="PureMVC.Core.Model"/>
	/// <see cref="PureMVC.Core.View"/>
	/// <see cref="PureMVC.Core.Controller"/>
	/// <see cref="PureMVC.Patterns.Notification"/>
	/// <see cref="PureMVC.Patterns.Mediator"/>
	/// <see cref="PureMVC.Patterns.Proxy"/>
	/// <see cref="PureMVC.Patterns.SimpleCommand"/>
	/// <see cref="PureMVC.Patterns.MacroCommand"/>
    public class Facade : IFacade,IController,INotifier,ILife
	{
        /// <summary>
        /// Private reference to the Controller
        /// </summary>
        protected Controller m_controller;
        /// <summary>
        /// Private reference to the Model
        /// </summary>
        protected Model m_model;
        /// <summary>
        /// Private reference to the View
        /// </summary>
        protected View m_view;
        /// <summary>
        /// 通知接口
        /// </summary>
        protected Notifier m_notifier;
        /// <summary>
        /// The Singleton Facade Instance
        /// </summary>
        protected static volatile Facade m_instance;
        /// <summary>
        /// Used for locking the instance calls
        /// </summary>
        protected static readonly object m_staticSyncRoot = new object();
        #region Constructors
        /// <summary>
        /// Constructor that initializes the Facade
        /// </summary>
        /// <remarks>
        ///     <para>This <c>IFacade</c> implementation is a Singleton, so you should not call the constructor directly, but instead call the static Singleton Factory method <c>Facade.Instance</c></para>
        /// </remarks>
        protected Facade() 
        {
            InitMe();
		
		}
		#endregion


		/// <summary>
		/// Register an <c>IProxy</c> with the <c>Model</c> by name
		/// </summary>
		/// <param name="proxy">The <c>IProxy</c> to be registered with the <c>Model</c></param>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual void RegisterProxy(IProxy proxy)
		{
			// The model is initialized in the constructor of the singleton, so this call should be thread safe.
			// This method is thread safe on the model.
			m_model.RegisterProxy(proxy);
		}

		/// <summary>
		/// Retrieve a <c>IProxy</c> from the <c>Model</c> by name
		/// </summary>
		/// <param name="proxyName">The name of the <c>IProxy</c> instance to be retrieved</param>
		/// <returns>The <c>IProxy</c> previously regisetered by <c>proxyName</c> with the <c>Model</c></returns>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual IProxy RetrieveProxy(string proxyName)
		{
			// The model is initialized in the constructor of the singleton, so this call should be thread safe.
			// This method is thread safe on the model.
			return m_model.RetrieveProxy(proxyName);
		}

		/// <summary>
		/// Remove an <c>IProxy</c> instance from the <c>Model</c> by name
		/// </summary>
		/// <param name="proxyName">The <c>IProxy</c> to remove from the <c>Model</c></param>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual IProxy RemoveProxy(string proxyName)
		{
			// The model is initialized in the constructor of the singleton, so this call should be thread safe.
			// This method is thread safe on the model.
			return m_model.RemoveProxy(proxyName);
		}

		/// <summary>
		/// Check if a Proxy is registered
		/// </summary>
		/// <param name="proxyName">The name of the <c>IProxy</c> instance to check for</param>
		/// <returns>whether a Proxy is currently registered with the given <c>proxyName</c>.</returns>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual bool HasProxy(string proxyName)
		{
			// The model is initialized in the constructor of the singleton, so this call should be thread safe.
			// This method is thread safe on the model.
			return m_model.HasProxy(proxyName);
		}

        /// <summary>
        /// mediator也作为一个观察者使用,监听的命令由mediator ListNotificationInterests()控制
        /// </summary>
        /// <param name="mediator"></param>
        public virtual void RegisterMediator(IMediator mediator)
		{
			m_view.RegisterMediator(mediator);
		}

        /// <summary>
        /// 获取mediator
        /// </summary>
        /// <param name="mediatorName"></param>
        /// <returns></returns>
        public virtual IMediator RetrieveMediator(string mediatorName)
		{
			// The view is initialized in the constructor of the singleton, so this call should be thread safe.
			// This method is thread safe on the view.
			return m_view.RetrieveMediator(mediatorName);
		}

		/// <summary>
		/// Remove a <c>IMediator</c> instance from the <c>View</c>
		/// </summary>
		/// <param name="mediatorName">The name of the <c>IMediator</c> instance to be removed</param>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual IMediator RemoveMediator(string mediatorName)
		{
			// The view is initialized in the constructor of the singleton, so this call should be thread safe.
			// This method is thread safe on the view.
			return m_view.RemoveMediator(mediatorName);
		}

		/// <summary>
		/// Check if a Mediator is registered or not
		/// </summary>
		/// <param name="mediatorName">The name of the <c>IMediator</c> instance to check for</param>
		/// <returns>whether a Mediator is registered with the given <code>mediatorName</code>.</returns>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual bool HasMediator(string mediatorName)
		{
			// The view is initialized in the constructor of the singleton, so this call should be thread safe.
			// This method is thread safe on the view.
			return m_view.HasMediator(mediatorName);
		}


		/// <summary>
		/// Facade Singleton Factory method.  This method is thread safe.
		/// </summary>
		public static Facade Instance
		{
			get
			{
                if (m_instance == null)
                {
                    lock (m_staticSyncRoot)
                    {
                        if (m_instance == null) m_instance = new Facade();
                    }
                }
                return m_instance;
			}
		}



		/// <summary>
        /// Explicit static constructor to tell C# compiler 
        /// not to mark type as beforefieldinit
        ///</summary>
        static Facade()
        {
        }


        /// <summary>
        /// Initialize the Singleton <c>Facade</c> instance
        /// </summary>
        /// <remarks>
        /// <para>Called automatically by the constructor. Override in your subclass to do any subclass specific initializations. Be sure to call <c>base.initializeFacade()</c>, though</para>
        /// </remarks>
        protected virtual void InitializeFacade()
        {
            
            if (m_model == null)
                m_model = Model.Instance;
            if (m_notifier == null)
                m_notifier = Notifier.Instance;
            if (m_controller == null)
                m_controller = Controller.Instance;
            if (m_view == null) 
                m_view = View.Instance;
          


            InitializeModel();
			InitializeController();
			InitializeView();
		}

        /// <summary>
        /// Initialize the <c>Controller</c>
        /// </summary>
        /// <remarks>
        ///     <para>Called by the <c>initializeFacade</c> method. Override this method in your subclass of <c>Facade</c> if one or both of the following are true:</para>
        ///     <list type="bullet">
        ///         <item>You wish to initialize a different <c>IController</c></item>
        ///         <item>You have <c>Commands</c> to register with the <c>Controller</c> at startup</item>
        ///     </list>
        ///     <para>If you don't want to initialize a different <c>IController</c>, call <c>base.initializeController()</c> at the beginning of your method, then register <c>Command</c>s</para>
        /// </remarks>
		protected virtual void InitializeController()
        {
		
		}
        protected virtual void InitializeModel()
        {
		
			
		}
        
        protected virtual void InitializeView()
        {
		
		}




        //SimpleFramework Code By Jarjin lee
        static GameObject m_GameManager;
        static Dictionary<string, object> m_Managers = new Dictionary<string, object>();

        GameObject AppGameManager {
            get {
                if (m_GameManager == null) {
                    m_GameManager = GameObject.Find("GameManager");
                }
                return m_GameManager;
            }
        }

        /// <summary>
        /// 添加管理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public void AddManager(string typeName, object obj) {
            if (!m_Managers.ContainsKey(typeName)) {
                m_Managers.Add(typeName, obj);
            }
        }

        /// <summary>
        /// 添加Unity对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public T AddManager<T>(string typeName) where T : Component {
            object result = null;
            m_Managers.TryGetValue(typeName, out result);
            if (result != null) {
                return (T)result;
            }
            Component c = AppGameManager.AddComponent<T>();
            m_Managers.Add(typeName, c);
            return default(T);
        }

        /// <summary>
        /// 获取系统管理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public T GetManager<T>(string typeName) where T : class {
            if (!m_Managers.ContainsKey(typeName)) {
                return default(T);
            }
            object manager = null;
            m_Managers.TryGetValue(typeName, out manager);
            return (T)manager;
        }

        /// <summary>
        /// 删除管理器
        /// </summary>
        /// <param name="typeName"></param>
        public void RemoveManager(string typeName) {
            if (!m_Managers.ContainsKey(typeName)) {
                return;
            }
            object manager = null;
            m_Managers.TryGetValue(typeName, out manager);
            Type type = manager.GetType();
            if (type.IsSubclassOf(typeof(MonoBehaviour))) {
                GameObject.Destroy((Component)manager);
            }
            m_Managers.Remove(typeName);
        }
        
        public void RegisterObserver(NotifyDefine notifi, IObserver observer)
        {
            m_notifier.RegisterObserver(notifi, observer);
        }

        public void RemoveObserver(NotifyDefine notifi, IObserver observer)
        {
            m_notifier.RemoveObserver(notifi, observer);
        }

        public void NotifyObservers<SendEntity, Param>(INotification<SendEntity, Param> note)
        {
            m_notifier.NotifyObservers(note);
        }

        public void SendNotification(NotifyDefine notifi)
        {
            m_notifier.SendNotification(notifi);
        }

        public void SendNotification<Param>(NotifyDefine notifiid, Param body)
        {
            m_notifier.SendNotification(notifiid, body);
        }

        public void SendNotification<SendEntity, Param>(NotifyDefine notifiid, SendEntity send, Param body)
        {
            m_notifier.SendNotification(notifiid, send, body);
        }
        public void RemoveCommand(NotifyDefine notiid)
        {
            m_controller.RemoveCommand(notiid);
        }

        public bool HasCommand(NotifyDefine notiid)
        {
            return m_controller.HasCommand(notiid);
        }

        public void ExcuteCmd<SendEntity, Param>(INotification<SendEntity, Param> note)
        {
            m_controller.ExcuteCmd(note);
        }

        public ICommand GetCommand(NotifyDefine notifyid)
        {
            return m_controller.GetCommand(notifyid);
        }

        public void RegisterCommand(NotifyDefine notiid, ICommand command)
        {
            m_controller.RegisterCommand(notiid, command);
        }

        public virtual void InitMe()
        {
            InitializeFacade();
        }

        public void OverLife()
        {
            m_model.OverLife();
            m_view.OverLife();
            m_controller.OverLife();
            m_notifier.OverLife();
        }
    }
}
