﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Description;
using System.Text;
using SuperSocket.Common;
using SuperSocket.Common.Logging;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;

namespace SuperSocket.SocketBase
{
    public interface ILoggerProvider
    {
        ILog Logger { get; }
    }

    public interface IPerformanceDataSource
    {
        /// <summary>
        /// Collects the performance data.
        /// </summary>
        /// <param name="globalPerfData">The global perf data.</param>
        /// <returns></returns>
        PerformanceData CollectPerformanceData(GlobalPerformanceData globalPerfData);
    }

    public interface IAppServer : ILoggerProvider
    {
        /// <summary>
        /// Gets the name of the server instance.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the server credentials for client console
        /// </summary>
        /// <value>
        /// The server credentials.
        /// </value>
        ServiceCredentials ServerCredentials { get; set; }
        
        /// <summary>
        /// Gets or sets the server's connection filter
        /// </summary>
        /// <value>
        /// The server's connection filters
        /// </value>
        IEnumerable<IConnectionFilter> ConnectionFilters{ get; set; }

        /// <summary>
        /// Setups the specified root config.
        /// </summary>
        /// <param name="rootConfig">The SuperSocket root config.</param>
        /// <param name="config">The socket server instance config.</param>
        /// <param name="socketServerFactory">The socket server factory.</param>
        /// <returns></returns>
        bool Setup(IRootConfig rootConfig, IServerConfig config, ISocketServerFactory socketServerFactory);

        /// <summary>
        /// Starts this server instance.
        /// </summary>
        /// <returns>return true if start successfull, else false</returns>
        bool Start();

        /// <summary>
        /// Stops this server instance.
        /// </summary>
        void Stop();

        /// <summary>
        /// Gets the server's config.
        /// </summary>
        IServerConfig Config { get; }

        /// <summary>
        /// Gets the certificate of current server.
        /// </summary>
        X509Certificate Certificate { get; }

        /// <summary>
        /// Gets the transfer layer security protocol.
        /// </summary>
        SslProtocols BasicSecurity { get; }

        /// <summary>
        /// Gets the total session count.
        /// </summary>
        int SessionCount { get; }


        /// <summary>
        /// Creates the app session.
        /// </summary>
        /// <param name="socketSession">The socket session.</param>
        /// <returns></returns>
        IAppSession CreateAppSession(ISocketSession socketSession);

        /// <summary>
        /// Gets the app session by ID.
        /// </summary>
        /// <param name="identityKey">The session ID.</param>
        /// <returns></returns>
        IAppSession GetAppSessionByID(string sessionID);
    }

    public interface IAppServer<TAppSession> : IAppServer
        where TAppSession : IAppSession
    {
        /// <summary>
        /// Gets the matched sessions from sessions snapshot.
        /// </summary>
        /// <param name="critera">The prediction critera.</param>
        /// <returns></returns>
        IEnumerable<TAppSession> GetSessions(Func<TAppSession, bool> critera);

        /// <summary>
        /// Gets all sessions in sessions snapshot.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TAppSession> GetAllSessions();
    }

    public interface IAppServer<TAppSession, TRequestInfo> : IAppServer<TAppSession>
        where TRequestInfo : IRequestInfo
        where TAppSession : IAppSession<TRequestInfo>
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="requestInfo">The request info.</param>
        void ExecuteCommand(IAppSession<TRequestInfo> session, TRequestInfo requestInfo);
    }
}
