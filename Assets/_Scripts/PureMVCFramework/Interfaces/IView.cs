/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/
using PureMVC.Patterns;
using System;

namespace PureMVC.Interfaces
{
    /// <summary>
    /// The interface definition for a PureMVC View
    /// </summary>
    /// <remarks>
    ///     <para>In PureMVC, <c>IView</c> implementors assume these responsibilities:</para>
    ///     <list type="bullet">
    ///         <item>Maintain a cache of <c>IMediator</c> instances</item>
    ///         <item>Provide methods for registering, retrieving, and removing <c>IMediators</c></item>
    ///         <item>Managing the observer lists for each <c>INotification</c> in the application</item>
    ///         <item>Providing a method for attaching <c>IObservers</c> to an <c>INotification</c>'s observer list</item>
    ///         <item>Providing a method for broadcasting an <c>INotification</c></item>
    ///         <item>Notifying the <c>IObservers</c> of a given <c>INotification</c> when it broadcast</item>
    ///     </list>
    /// </remarks>
	/// <see cref="PureMVC.Interfaces.IMediator"/>
	/// <see cref="PureMVC.Interfaces.IObserver"/>
	/// <see cref="PureMVC.Interfaces.INotification"/>
    public interface IView
	{
		#region Observer

		#endregion

		#region Mediator

		/// <summary>
        /// Register an <c>IMediator</c> instance with the <c>View</c>
        /// </summary>
        /// <param name="mediator">A a reference to the <c>IMediator</c> instance</param>
        /// <remarks>
        ///     <para>Registers the <c>IMediator</c> so that it can be retrieved by name, and further interrogates the <c>IMediator</c> for its <c>INotification</c> interests</para>
        ///     <para>If the <c>IMediator</c> returns any <c>INotification</c> names to be notified about, an <c>Observer</c> is created encapsulating  the <c>IMediator</c> instance's <c>handleNotification</c> method and registering it as an <c>Observer</c> for all <c>INotifications</c> the <c>IMediator</c> is interested in</para>
        /// </remarks>
		void RegisterMediator(IMediator mediator);

        /// <summary>
        /// Retrieve an <c>IMediator</c> from the <c>View</c>
        /// </summary>
        /// <param name="mediatorName">The name of the <c>IMediator</c> instance to retrieve</param>
        /// <returns>The <c>IMediator</c> instance previously registered with the given <c>mediatorName</c></returns>
		IMediator RetrieveMediator(string mediatorName);

        /// <summary>
        /// Remove an <c>IMediator</c> from the <c>View</c>
        /// </summary>
        /// <param name="mediatorName">The name of the <c>IMediator</c> instance to be removed</param>
        IMediator RemoveMediator(string mediatorName);
		
		/// <summary>
		/// Check if a Mediator is registered or not
		/// </summary>
		/// <param name="mediatorName">The name of the <c>IMediator</c> instance to check for</param>
		/// <returns>whether a Mediator is registered with the given <c>mediatorName</c>.</returns>
		bool HasMediator(string mediatorName);

		#endregion
	}
}
