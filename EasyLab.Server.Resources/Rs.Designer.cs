﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EasyLab.Server.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Rs {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Rs() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EasyLab.Server.Resources.Rs", typeof(Rs).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot begin a new transaction while an existing transaction is still running.
        ///Please commit or rollback the existing transaction before starting a new one..
        /// </summary>
        public static string CANNOT_BEGIN_NEW_TRANSACTION_WHILE_A_TRANSACTION_IS_RUNNING {
            get {
                return ResourceManager.GetString("CANNOT_BEGIN_NEW_TRANSACTION_WHILE_A_TRANSACTION_IS_RUNNING", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot commit a transaction while there is no transaction running..
        /// </summary>
        public static string CANNOT_COMMIT_TRANSACTION_WHILE_NO_TRANSACTION_IS_RUNNING {
            get {
                return ResourceManager.GetString("CANNOT_COMMIT_TRANSACTION_WHILE_NO_TRANSACTION_IS_RUNNING", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot roll back a transaction while there is no transaction running..
        /// </summary>
        public static string CANNOT_ROLLBACK_TRANSACTION_WHILE_NO_TRANSACTION_IS_RUNNING {
            get {
                return ResourceManager.GetString("CANNOT_ROLLBACK_TRANSACTION_WHILE_NO_TRANSACTION_IS_RUNNING", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A transaction is running. Call CommitTransaction instead..
        /// </summary>
        public static string CANNOT_SAVE_WHILE_A_TRANSACTION_IS_RUNNING {
            get {
                return ResourceManager.GetString("CANNOT_SAVE_WHILE_A_TRANSACTION_IS_RUNNING", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Get init labs and instruments list error.
        /// </summary>
        public static string GETLABINSTRUMENTLISTINITERROR {
            get {
                return ResourceManager.GetString("GETLABINSTRUMENTLISTINITERROR", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Get init labs and instruments list success.
        /// </summary>
        public static string GETLABINSTRUMENTLISTINITSUCCESS {
            get {
                return ResourceManager.GetString("GETLABINSTRUMENTLISTINITSUCCESS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Server return user list error.
        /// </summary>
        public static string GETUSERLISTERROR {
            get {
                return ResourceManager.GetString("GETUSERLISTERROR", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Get user list success.
        /// </summary>
        public static string GETUSERLISTSUCCESS {
            get {
                return ResourceManager.GetString("GETUSERLISTSUCCESS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An internal service error has occurred.
        /// </summary>
        public static string INTERNAL_SERVICE_ERROR {
            get {
                return ResourceManager.GetString("INTERNAL_SERVICE_ERROR", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid IP v4 address.
        /// </summary>
        public static string INVALID_IP_V4 {
            get {
                return ResourceManager.GetString("INVALID_IP_V4", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid password.
        /// </summary>
        public static string INVALID_PASSWORD {
            get {
                return ResourceManager.GetString("INVALID_PASSWORD", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalida resources id.
        /// </summary>
        public static string INVALID_RESOURCE_ID {
            get {
                return ResourceManager.GetString("INVALID_RESOURCE_ID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Request content: auditlog id: {0}, ResourceType: {1}, ResourceAction: {2}, ResourceValue: {3}.
        /// </summary>
        public static string LOG_MESSAGE_AUDIT_LOG_CONTENT {
            get {
                return ResourceManager.GetString("LOG_MESSAGE_AUDIT_LOG_CONTENT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Get {0} message(s)..
        /// </summary>
        public static string LOG_MESSAGE_COUNT {
            get {
                return ResourceManager.GetString("LOG_MESSAGE_COUNT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error while processing message id={0} : {1}.
        /// </summary>
        public static string LOG_MESSAGE_FAILED_WITH_EXCEPTION {
            get {
                return ResourceManager.GetString("LOG_MESSAGE_FAILED_WITH_EXCEPTION", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Global Setting is not ready, stop processing message..
        /// </summary>
        public static string LOG_MESSAGE_GLOBAL_SETTING_NOT_READY {
            get {
                return ResourceManager.GetString("LOG_MESSAGE_GLOBAL_SETTING_NOT_READY", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown message type {0} in message {1}..
        /// </summary>
        public static string LOG_MESSAGE_INVALID_TYPE {
            get {
                return ResourceManager.GetString("LOG_MESSAGE_INVALID_TYPE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Network connection is not available, stop processing message..
        /// </summary>
        public static string LOG_MESSAGE_NETWORK_NOT_AVAILABLE {
            get {
                return ResourceManager.GetString("LOG_MESSAGE_NETWORK_NOT_AVAILABLE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start processing messages..
        /// </summary>
        public static string LOG_MESSAGE_START_PROCESS {
            get {
                return ResourceManager.GetString("LOG_MESSAGE_START_PROCESS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Network connection is not available, stop processing message..
        /// </summary>
        public static string LOG_NETWORK_NOT_AVAILABLE {
            get {
                return ResourceManager.GetString("LOG_NETWORK_NOT_AVAILABLE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Requesting {0} ...
        /// </summary>
        public static string LOG_SERVICE_CLIENT_REQUEST_CONTENT {
            get {
                return ResourceManager.GetString("LOG_SERVICE_CLIENT_REQUEST_CONTENT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Response is: {0}.
        /// </summary>
        public static string LOG_SERVICE_CLIENT_RESPONSE_CONTENT {
            get {
                return ResourceManager.GetString("LOG_SERVICE_CLIENT_RESPONSE_CONTENT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid auditLog id [{0}] in message [{1}], message type is [{2}].
        /// </summary>
        public static string MESSAGE_INVALID_AUDITLOG_ID {
            get {
                return ResourceManager.GetString("MESSAGE_INVALID_AUDITLOG_ID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do not authority process message.
        /// </summary>
        public static string MESSAGE_LOGIN_FALSE {
            get {
                return ResourceManager.GetString("MESSAGE_LOGIN_FALSE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Must not be blank.
        /// </summary>
        public static string MUST_NOT_BE_BLANK {
            get {
                return ResourceManager.GetString("MUST_NOT_BE_BLANK", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Must not be empty.
        /// </summary>
        public static string MUST_NOT_BE_EMPTY {
            get {
                return ResourceManager.GetString("MUST_NOT_BE_EMPTY", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The request is forbidden on server..
        /// </summary>
        public static string REQUEST_FORBIDDEN {
            get {
                return ResourceManager.GetString("REQUEST_FORBIDDEN", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Required field empty.
        /// </summary>
        public static string REQUIRED_FIELD_EMPTY {
            get {
                return ResourceManager.GetString("REQUIRED_FIELD_EMPTY", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Required field missing.
        /// </summary>
        public static string REQUIRED_FIELD_MISSING {
            get {
                return ResourceManager.GetString("REQUIRED_FIELD_MISSING", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Required field missing or empty.
        /// </summary>
        public static string REQUIRED_FIELD_MISSING_OR_EMPTY {
            get {
                return ResourceManager.GetString("REQUIRED_FIELD_MISSING_OR_EMPTY", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value too long (max length {0}).
        /// </summary>
        public static string VALUE_TOO_LONG {
            get {
                return ResourceManager.GetString("VALUE_TOO_LONG", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to lab manager login error when system verity and error is {0}.
        /// </summary>
        public static string VERITY_LAB_MANAGER_ERROR {
            get {
                return ResourceManager.GetString("VERITY_LAB_MANAGER_ERROR", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reserve&apos;s queue display error.
        /// </summary>
        public static string VERITY_RESERVE_QUEUE_ERROR {
            get {
                return ResourceManager.GetString("VERITY_RESERVE_QUEUE_ERROR", resourceCulture);
            }
        }
    }
}
