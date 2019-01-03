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

namespace PureMVC.Patterns
{
    /// <summary>
    /// A base <c>IMediator</c> implementation
    /// </summary>
    /// <see cref="PureMVC.Core.View"/>
    public class Mediator :GetFacade, IMediator,IObserver
	{
		#region Constants

		/// <summary>
        /// The name of the <c>Mediator</c>
        /// </summary>
        /// <remarks>
        ///     <para>Typically, a <c>Mediator</c> will be written to serve one specific control or group controls and so, will not have a need to be dynamically named</para>
        /// </remarks>
		public const string NAME = "Mediator";

		#endregion

		#region Constructors

		/// <summary>
        /// Constructs a new mediator with the default name and no view component
        /// </summary>
        public Mediator()
            : this(NAME, null)
        {
		}

        /// <summary>
        /// Constructs a new mediator with the specified name and no view component
        /// </summary>
        /// <param name="mediatorName">The name of the mediator</param>
        public Mediator(string mediatorName)
            : this(mediatorName, null)
        {
		}

        /// <summary>
        /// Constructs a new mediator with the specified name and view component
        /// </summary>
        /// <param name="mediatorName">The name of the mediator</param>
        /// <param name="viewComponent">The view component to be mediated</param>
		public Mediator(string mediatorName, object viewComponent)
		{
			m_mediatorName = (mediatorName != null) ? mediatorName : NAME;
			m_viewComponent = viewComponent;
		}

		#endregion

		#region Public Methods

		#region IMediator Members

		/// <summary>
		/// List the <c>INotification</c> names this <c>Mediator</c> is interested in being notified of
		/// </summary>
		/// <returns>The list of <c>INotification</c> names </returns>
		public virtual IList<NotifyDefine> ListNotificationInterests()
		{
			return new List<NotifyDefine>();
		}


		/// <summary>
		/// Called by the View when the Mediator is registered
		/// </summary>
		public virtual void OnRegister()
		{
		}

        
		/// <summary>
		/// Called by the View when the Mediator is removed
		/// </summary>
		public virtual void OnRemove()
		{
		}


        public virtual void HandleNotify<SendEntity, Param>(INotification<SendEntity, Param> notification) {
        }

        public void OnNotify<SendEntity, Param>( INotification<SendEntity, Param> notification)
        {
            HandleNotify(notification);
        }

        #endregion

        #endregion

        #region Accessors

        /// <summary>
        /// The name of the <c>Mediator</c>
        /// </summary>
        /// <remarks><para>You should override this in your subclass</para></remarks>
        public virtual string MediatorName
		{
			get { return m_mediatorName; }
		}

		/// <summary>
		/// The <code>IMediator</code>'s view component.
		/// </summary>
		/// <remarks>
		///     <para>Additionally, an implicit getter will usually be defined in the subclass that casts the view object to a type, like this:</para>
		///     <example>
		///         <code>
		///             private System.Windows.Form.ComboBox comboBox {
		///                 get { return viewComponent as ComboBox; }
		///             }
		///         </code>
		///     </example>
		/// </remarks>
		public virtual object ViewComponent
		{
			get { return m_viewComponent; }
			set { m_viewComponent = value; }
		}

		#endregion

		#region Members

		/// <summary>
        /// The mediator name
        /// </summary>
        protected string m_mediatorName;

        /// <summary>
        /// The view component being mediated
        /// </summary>
        protected object m_viewComponent;

        #endregion
    }
}
