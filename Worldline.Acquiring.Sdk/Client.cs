/*
 * This file was automatically generated.
 */
using System;
using Worldline.Acquiring.Sdk.Logging;
using Worldline.Acquiring.Sdk.V1;

namespace Worldline.Acquiring.Sdk
{
    /// <summary>
    /// Worldline Acquiring platform client. Thread-safe.
    /// </summary>
    public class Client : ApiResource, IDisposable, ILoggingCapable, IObfuscationCapable
    {
        public Client(Communicator communicator) :
            base(communicator, null)
        {
        }

        /// <summary>
        /// Utility method that delegates the call to this client's communicator.
        /// </summary>
        public void CloseExpiredConnections()
        {
            _communicator.CloseExpiredConnections();
        }

        /// <summary>
        /// Utility method that delegates the call to this client's communicator.
        /// </summary>
        /// <param name="timespan">Idle time.</param>
        public void CloseIdleConnections(TimeSpan timespan)
        {
            _communicator.CloseIdleConnections(timespan);
        }

        #region IObfuscationCapable support
        public BodyObfuscator BodyObfuscator
        {
            set => _communicator.BodyObfuscator = value;
        }

        public HeaderObfuscator HeaderObfuscator
        {
            set => _communicator.HeaderObfuscator = value;
        }
        #endregion

        #region ILoggingCapable support
        public void EnableLogging(ICommunicatorLogger communicatorLogger)
        {
            // delegate to the communicator
            _communicator.EnableLogging(communicatorLogger);
        }

        public void DisableLogging()
        {
            // delegate to the communicator
            _communicator.DisableLogging();
        }
        #endregion

        #region IDisposable support
        /// <summary>
        /// Releases any system resources associated with this object.
        /// </summary>
        public void Dispose()
        {
            _communicator.Dispose();
        }
        #endregion

        public V1Client V1 => new V1Client(this, null);
    }
}
