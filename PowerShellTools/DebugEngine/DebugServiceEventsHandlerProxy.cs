﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PowerShellTools.Common.ServiceManagement.DebuggingContract;

namespace PowerShellTools.DebugEngine
{
    /// <summary>
    /// Proxy of debugger service event handlers
    /// This works as InstanceContext for debugger service channel
    /// </summary>
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class DebugServiceEventsHandlerProxy : IDebugEngineCallback
    {
        private ScriptDebugger _debugger;

        public DebugServiceEventsHandlerProxy(){}

        public DebugServiceEventsHandlerProxy(ScriptDebugger debugger)
        {
            _debugger = debugger;
        }

        public ScriptDebugger Debugger
        {
            get 
            {
                if (_debugger == null)
                {
                    _debugger = PowerShellToolsPackage.Debugger;
                }
                return _debugger;
            }
        }

        /// <summary>
        /// Debugger stopped
        /// </summary>
        /// <param name="e">DebuggerStoppedEventArgs</param>
        public void DebuggerStopped(DebuggerStoppedEventArgs e)
        {
            Debugger.DebuggerStop(e);
        }

        /// <summary>
        /// Breakpoint has updated
        /// </summary>
        /// <param name="e">DebuggerBreakpointUpdatedEventArgs</param>
        public void BreakpointUpdated(DebuggerBreakpointUpdatedEventArgs e)
        {
            Debugger.UpdateBreakpoint(e);
        }

        /// <summary>
        /// Debugger has string to output
        /// </summary>
        /// <param name="output">string to output</param>
        public void OutputString(string output)
        {
            Debugger.HostUi.VsOutputString(output);
        }

        /// <summary>
        /// Debugger has string to output
        /// </summary>
        /// <param name="output">string to output</param>
        public void OutputStringLine(string output)
        {
            Debugger.HostUi.VsOutputString(output + Environment.NewLine);
        }

        /// <summary>
        /// Debugger finished
        /// </summary>
        public void DebuggerFinished()
        {
            Debugger.DebuggerFinished();
        }

        /// <summary>
        /// Execution engine has terminating exception thrown
        /// </summary>
        /// <param name="ex">DebuggingServiceException</param>
        public void TerminatingException(DebuggingServiceException ex)
        {
            Debugger.TerminateException(ex);
        }

        /// <summary>
        /// Refreshes the prompt in the REPL window to match the current PowerShell prompt value
        /// </summary>
        public void RefreshPrompt()
        {
            Debugger.RefreshPrompt();
        }

        /// <summary>
        /// Ask for user input
        /// </summary>
        /// <returns>Output string</returns>
        public string ReadHostPrompt(string message)
        {
            return Debugger.HostUi.ReadLine(message);
        }

        /// <summary>
        /// Ask for securestring from user
        /// </summary>
        /// <param name="message">Message of dialog window.</param>
        /// <param name="name">Name of the parameter.</param>
        /// <returns>A PSCredential object that contains the credentials for the target.</returns>
        public PSCredential ReadSecureStringPrompt(string message, string name)
        {
            return Debugger.HostUi.ReadSecureStringAsPSCredential(message, name).Result;
        }

        /// <summary>
        /// To open specific file in client
        /// </summary>
        /// <param name="fullName">Full name of remote file(mapped into local)</param>
        public void OpenRemoteFile(string fullName)
        {
            Debugger.OpenRemoteFile(fullName);
        }

        /// <summary>
        /// Set flag to indicate if runspace is hosting remote session
        /// </summary>
        /// <param name="enabled">Boolean indicate remote session is enable or not</param>
        public void SetRemoteRunspace(bool enabled)
        {
            Debugger.RemoteSession = enabled;
        }

        /// <summary>
        /// Outputs the Progress of the operation.
        /// </summary>
        /// <param name="sourceId">The id of the operation.</param>
        /// <param name="record">The record of the operation.</param>
        public void OutputProgress(long sourceId, ProgressRecord record)
        {
            Debugger.HostUi.VSOutputProgress(sourceId, record);
        }

        /// <summary>
        /// Ask for user input PSCredential from VS
        /// </summary>
        /// <param name="caption">The caption for the message window.</param>
        /// <param name="message">The text of the message.</param>
        /// <param name="userName">The user name whose credential is to be prompted for. If this parameter set to null or an empty string, the function will prompt for the user name first.</param>
        /// <param name="targetName">The name of the target for which the credential is collected.</param>
        /// <param name="allowedCredentialTypes">A bitwise combination of the PSCredentialTypes enumeration values that identify the types of credentials that can be returned.</param>
        /// <param name="options">A bitwise combination of the PSCredentialUIOptions enumeration values that identify the UI behavior when it gathers the credentials.</param>
        /// <returns>A PSCredential object that contains the credentials for the target.</returns>
        public PSCredential GetPSCredentialPrompt(string caption, string message, string userName,
            string targetName, PSCredentialTypes allowedCredentialTypes, PSCredentialUIOptions options)
        {
            return Debugger.HostUi.GetPSCredential(caption, message, userName,
                targetName, allowedCredentialTypes, options);
        }
    }
}
