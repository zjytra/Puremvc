/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System;
using UnityEngine;

#endregion

namespace PureMVC.Interfaces
{
    /// <summary>
    /// The interface definition for a PureMVC Facade
    /// </summary>
    /// <remarks>
    ///     <para>The Facade Pattern suggests providing a single class to act as a certal point of communication for subsystems</para>
    ///     <para>In PureMVC, the Facade acts as an interface between the core MVC actors (Model, View, Controller) and the rest of your application</para>
    /// </remarks>
	/// <see cref="PureMVC.Interfaces.IModel"/>
	/// <see cref="PureMVC.Interfaces.IView"/>
	/// <see cref="PureMVC.Interfaces.IController"/>
	/// <see cref="PureMVC.Interfaces.ICommand"/>
	/// <see cref="PureMVC.Interfaces.INotification"/>
    public interface IFacade 
	{
		void RegisterProxy(IProxy proxy);
		IProxy RetrieveProxy(string proxyName);
        IProxy RemoveProxy(string proxyName);
		bool HasProxy(string proxyName);

		void RegisterMediator(IMediator mediator);
		IMediator RetrieveMediator(string mediatorName);
        IMediator RemoveMediator(string mediatorName);

		bool HasMediator(string mediatorName);

        //SimpleFramework Code By Jarjin lee
        void AddManager(string typeName, object obj);

        T AddManager<T>(string typeName) where T : Component;

        T GetManager<T>(string typeName) where T : class;

        void RemoveManager(string typeName);

    }
}
