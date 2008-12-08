//*******************************************************************************************************
//  ServerBase.cs
//  Copyright © 2008 - TVA, all rights reserved - Gbtc
//
//  Build Environment: C#, Visual Studio 2008
//  Primary Developer: Pinal C. Patel, Operations Data Architecture [PCS]
//      Office: PSO TRAN & REL, CHATTANOOGA - MR BK-C
//       Phone: 423/751-3024
//       Email: pcpatel@tva.gov
//
//  Code Modification History:
//  -----------------------------------------------------------------------------------------------------
//  06/01/2006 - Pinal C. Patel
//       Original version of source code generated
//  09/06/2006 - J. Ritchie Carroll
//       Added bypass optimizations for high-speed server data access
//  11/30/2007 - Pinal C. Patel
//       Modified the "design time" check in EndInit() method to use LicenseManager.UsageMode property
//       instead of DesignMode property as the former is more accurate than the latter
//  02/19/2008 - Pinal C. Patel
//       Added code to detect and avoid redundant calls to Dispose().
//  09/29/2008 - James R Carroll
//       Converted to C#.
//
//*******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading;
using PCS.Configuration;
using PCS.IO.Compression;
using PCS.Security.Cryptography;

namespace PCS.Communication
{
    /// <summary>
    /// Represents a server involved in server-client communication.
    /// </summary>
    [ToolboxBitmap(typeof(ServerBase))]
    public abstract partial class ServerBase : Component, IServer, ISupportInitialize, IProvideStatus, IPersistSettings
    {
        #region [ Members ]

        // Constants

        /// <summary>
        /// Specifies the default value for the <see cref="MaxClientConnections"/> property.
        /// </summary>
        public const int DefaultMaxClientConnections = -1;

        /// <summary>
        /// Specifies the default value for the <see cref="Handshake"/> property.
        /// </summary>
        public const bool DefaultHandshake = false;

        /// <summary>
        /// Specifies the default value for the <see cref="HandshakeTimeout"/> property.
        /// </summary>
        public const int DefaultHandshakeTimeout = 3000;

        /// <summary>
        /// Specifies the default value for the <see cref="HandshakePassphrase"/> property.
        /// </summary>
        public const string DefaultHandshakePassphrase = "6572a33d-826f-4d96-8c28-8be66bbc700e";

        /// <summary>
        /// Specifies the default value for the <see cref="SecureSession"/> property.
        /// </summary>
        public const bool DefaultSecureSession = false;

        /// <summary>
        /// Specifies the default value for the <see cref="ReceiveTimeout"/> property.
        /// </summary>
        public const int DefaultReceiveTimeout = -1;

        /// <summary>
        /// Specifies the default value for the <see cref="ReceiveBufferSize"/> property.
        /// </summary>
        public const int DefaultReceiveBufferSize = 8192;

        /// <summary>
        /// Specifies the default value for the <see cref="Encryption"/> property.
        /// </summary>
        public const CipherStrength DefaultEncryption = CipherStrength.None;

        /// <summary>
        /// Specifies the default value for the <see cref="Compression"/> property.
        /// </summary>
        public const CompressionStrength DefaultCompression = CompressionStrength.NoCompression;

        /// <summary>
        /// Specifies the default value for the <see cref="PersistSettings"/> property.
        /// </summary>
        public const bool DefaultPersistSettings = false;

        /// <summary>
        /// Specifies the default value for the <see cref="SettingsCategory"/> property.
        /// </summary>
        public const string DefaultSettingsCategory = "CommunicationServer";

        // Events

        /// <summary>
        /// Occurs when the server is started.
        /// </summary>
        [Category("Server"),
        Description("Occurs when the server is started.")]
        public event EventHandler ServerStarted;

        /// <summary>
        /// Occurs when the server is stopped.
        /// </summary>
        [Category("Server"),
        Description("Occurs when the server is stopped.")]
        public event EventHandler ServerStopped;

        /// <summary>
        /// Occurs when server-client handshake, when enabled, cannot be performed within the specified <see cref="HandshakeTimeout"/> time.
        /// </summary>
        [Category("Server"),
        Description("Occurs when server-client handshake, when enabled, cannot be performed within the specified HandshakeTimeout time.")]
        public event EventHandler HandshakeTimedout;

        /// <summary>
        /// Occurs when server-client handshake, when enabled, cannot be performed successfully due to information mismatch.
        /// </summary>
        [Category("Server"),
        Description("Occurs when server-client handshake, when enabled, cannot be performed successfully due to information mismatch.")]
        public event EventHandler HandshakeUnsuccessful;

        /// <summary>
        /// Occurs when a client connects to the server.
        /// </summary>
        /// <remarks>
        /// <see cref="EventArgs{T}.Argument"/> is the ID of the client that connected to the server.
        /// </remarks>
        [Category("Client"),
        Description("Occurs when a client connects to the server.")]
        public event EventHandler<EventArgs<Guid>> ClientConnected;

        /// <summary>
        /// Occurs when a client disconnects from the server.
        /// </summary>
        /// <remarks>
        /// <see cref="EventArgs{T}.Argument"/> is the ID of the client that disconnected from the server.
        /// </remarks>
        [Category("Client"),
        Description("Occurs when a client disconnects from the server.")]
        public event EventHandler<EventArgs<Guid>> ClientDisconnected;

        /// <summary>
        /// Occurs when data is being sent to a client.
        /// </summary>
        /// <remarks>
        /// <see cref="EventArgs{T}.Argument"/> is the ID of the client to which the data is being sent.
        /// </remarks>
        [Category("Data"),
        Description("Occurs when data is being sent to a client.")]
        public event EventHandler<EventArgs<Guid>> SendClientDataStarted;

        /// <summary>
        /// Occurs when data has been sent to a client.
        /// </summary>
        /// <remarks>
        /// <see cref="EventArgs{T}.Argument"/> is the ID of the client to which the data has been sent.
        /// </remarks>
        [Category("Data"),
        Description("Occurs when data has been sent to a client.")]
        public event EventHandler<EventArgs<Guid>> SendClientDataComplete;

        /// <summary>
        /// Occurs when an <see cref="Exception"/> is encountered when sending data to a client.
        /// </summary>
        /// <remarks>
        /// <see cref="EventArgs{T1,T2}.Argument1"/> is the ID of the client to which the data was being sent.<br/>
        /// <see cref="EventArgs{T1,T2}.Argument2"/> is the <see cref="Exception"/> encountered when sending data to a client.
        /// </remarks>
        [Category("Data"),
        Description("Occurs when an Exception is encountered when sending data to a client.")]
        public event EventHandler<EventArgs<Guid, Exception>> SendClientDataException;

        /// <summary>
        /// Occurs when no data is received from a client for the <see cref="ReceiveTimeout"/> time.
        /// </summary>
        [Category("Data"), 
        Description("Occurs when no data is received from a client for the ReceiveTimeout time.")]
        public event EventHandler<EventArgs<Guid>> ReceiveClientDataTimedout;

        /// <summary>
        /// Occurs when data is received from a client.
        /// </summary>
        /// <remarks>
        /// <see cref="EventArgs{T1,T2,T3}.Argument1"/> is the ID of the client from which data is received.<br/>
        /// <see cref="EventArgs{T1,T2,T3}.Argument2"/> is the buffer containing data received from the client starting at index zero.<br/>
        /// <see cref="EventArgs{T1,T2,T3}.Argument3"/> is the number of bytes received in the buffer from the client.
        /// </remarks>
        [Category("Data"),
        Description("Occurs when data is received from a client.")]
        public event EventHandler<EventArgs<Guid, byte[], int>> ReceiveClientDataComplete;

        // Fields
        private string m_configurationString;
        private int m_maxClientConnections;
        private bool m_handshake;
        private int m_handshakeTimeout;
        private string m_handshakePassphrase;
        private bool m_secureSession;
        private int m_receiveTimeout;
        private int m_receiveBufferSize;
        private CipherStrength m_encryption;
        private CompressionStrength m_compression;
        private bool m_persistSettings;
        private string m_settingsCategory;
        private Encoding m_textEncoding;
        private Action<Guid, byte[], int> m_receiveClientDataHandler;
        private TransportProtocol m_transportProtocol;
        private Guid m_serverID;
        private List<Guid> m_clientIDs;
        private bool m_isRunning;
        private long m_stopTime;
        private long m_startTime;
        private bool m_disposed;
        private bool m_initialized;             

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the server.
        /// </summary>
        protected ServerBase()
        {
            m_maxClientConnections = DefaultMaxClientConnections;
            m_handshake = DefaultHandshake;
            m_handshakeTimeout = DefaultHandshakeTimeout;
            m_handshakePassphrase = DefaultHandshakePassphrase;
            m_secureSession = DefaultSecureSession;
            m_receiveTimeout = DefaultReceiveTimeout;
            m_receiveBufferSize = DefaultReceiveBufferSize;
            m_encryption = DefaultEncryption;
            m_compression = DefaultCompression;
            m_persistSettings = DefaultPersistSettings;
            m_settingsCategory = DefaultSettingsCategory;
            m_textEncoding = Encoding.ASCII;
            m_serverID = Guid.NewGuid();
            m_clientIDs = new List<Guid>();
        }

        /// <summary>
        /// Initializes a new instance of the server.
        /// </summary>
        /// <param name="transportProtocol">One of the <see cref="TransportProtocol"/> values.</param>
        /// <param name="configurationString">The data used by the server for initialization.</param>
        protected ServerBase(TransportProtocol transportProtocol, string configurationString)
            : this()
        {
            m_transportProtocol = transportProtocol;
            this.ConfigurationString = configurationString;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the data required by the server to initialize.
        /// </summary>
        [Category("Settings"),
        Description("The data that is required by the server to initialize.")]
        public virtual string ConfigurationString
        {
            get
            {
                return m_configurationString;
            }
            set
            {
                ValidateConfigurationString(value);

                m_configurationString = value;
                if (m_isRunning)
                {
                    // Restart the server when configuration data is changed.
                    Stop();
                    Start();
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of clients that can connect to the server.
        /// </summary>
        /// <remarks>
        /// Set <see cref="MaxClientConnections"/> to -1 to allow infinite client connections.
        /// </remarks>
        [Category("Settings"),
        DefaultValue(DefaultMaxClientConnections),
        Description("The maximum number of clients that can connect to the server. Set MaxClientConnections to -1 to allow infinite client connections.")]
        public virtual int MaxClientConnections
        {
            get
            {
                return m_maxClientConnections;
            }
            set
            {
                if (value < 1)
                    m_maxClientConnections = -1;
                else
                    m_maxClientConnections = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates whether the server will do a handshake with the clients after the connection has been established.
        /// </summary>
        /// <remarks>
        /// <see cref="Handshake"/> is required when <see cref="SecureSession"/> is enabled.
        /// </remarks>
        /// <exception cref="InvalidOperationException"><see cref="Handshake"/> is being disabled while <see cref="SecureSession"/> is enabled.</exception>
        [Category("Security"),
        DefaultValue(DefaultHandshake),
        Description("Indicates whether the server will do a handshake with the client after accepting its connection.")]
        public virtual bool Handshake
        {
            get
            {
                return m_handshake;
            }
            set
            {
                // Can't disable handshake when secure session is enabled.
                if (!value && m_secureSession)
                    throw new InvalidOperationException("Handshake is required when SecureSession is enabled.");

                m_handshake = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of milliseconds that the server will wait for the clients to initiate the <see cref="Handshake"/> process.
        /// </summary>
        /// <exception cref="ArgumentException">The value specified is either zero or negative.</exception>
        [Category("Security"), 
        DefaultValue(DefaultHandshakeTimeout),
        Description("The number of milliseconds that the server will wait for the clients to initiate the handshake process.")]
        public virtual int HandshakeTimeout
        {
            get
            {
                return m_handshakeTimeout;
            }
            set 
            {
                if (value < 1)
                    throw new ArgumentException("Value cannot be zero or negative."); 
                    
                m_handshakeTimeout = value;
            }
        }

        /// <summary>
        /// Gets or sets the passpharse that the clients must provide for authentication during the <see cref="Handshake"/> process.
        /// </summary>
        [Category("Security"),
        DefaultValue(DefaultHandshakePassphrase),
        Description("The passpharse that the clients must provide for authentication during the handshake process.")]
        public virtual string HandshakePassphrase
        {
            get
            {
                return m_handshakePassphrase;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    m_handshakePassphrase = value;
                else
                    m_handshakePassphrase = DefaultHandshakePassphrase;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates whether the data exchanged between the server and clients will be encrypted using a private session passphrase.
        /// </summary>
        ///<remarks>
        ///<see cref="Handshake"/> and <see cref="Encryption"/> must be enabled in order to use <see cref="SecureSession"/>.
        ///</remarks>
        ///<exception cref="InvalidOperationException"><see cref="SecureSession"/> is being enabled before enabling <see cref="Handshake"/>.</exception>
        ///<exception cref="InvalidOperationException"><see cref="SecureSession"/> is being enabled before enabling <see cref="Encryption"/>.</exception>
        [Category("Security"),
        DefaultValue(DefaultSecureSession),
        Description("Indicates whether the data exchanged between the server and clients will be encrypted using a private session passphrase.")]
        public virtual bool SecureSession
        {
            get
            {
                return m_secureSession;
            }
            set
            {
                // Handshake is required for SecureSession.
                if (value && !m_handshake)
                    throw new InvalidOperationException("Handshake must be enabled in order to use SecureSession.");

                // Encryption is required for SecureSession.
                if (value && m_encryption == CipherStrength.None)
                    throw new InvalidOperationException("Encryption must be enabled in order to use SecureSession.");

                m_secureSession = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of milliseconds after which the server will raise the <see cref="ReceiveClientDataTimedout"/> event if no data is received from a client.
        /// </summary>
        /// <remarks>Set <see cref="ReceiveTimeout"/> to -1 to disable this feature.</remarks>
        [Category("Data"), 
        DefaultValue(DefaultReceiveTimeout), 
        Description("The number of milliseconds after which the server will raise the ReceiveClientDataTimedout event if no data is received from a client.")]
        public virtual int ReceiveTimeout
        {
            get
            {
                return m_receiveTimeout;
            }
            set 
            {
                if (value < 1)
                    m_receiveTimeout = -1;
                else
                    m_receiveTimeout = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the buffer used by the server for receiving data from the clients.
        /// </summary>
        /// <exception cref="ArgumentException">The value specified is either zero or negative.</exception>
        [Category("Data"),
        DefaultValue(DefaultReceiveBufferSize),
        Description("The size of the buffer used by the server for receiving data from the clients.")]
        public virtual int ReceiveBufferSize
        {
            get
            {
                return m_receiveBufferSize;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentException("Value cannot be zero or negative.");

                m_receiveBufferSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="CipherStrength"/> to be used for ciphering the data exchanged between the server and clients.
        /// </summary>
        /// <remarks>
        /// <list type="table">
        ///     <listheader>
        ///         <term><see cref="SecureSession"/></term>
        ///         <description>Key used for <see cref="Encryption"/></description>
        ///     </listheader>
        ///     <item>
        ///         <term>Disabled</term>
        ///         <description><see cref="HandshakePassphrase"/> is used.</description>
        ///     </item>
        ///     <item>
        ///         <term>Enabled</term>
        ///         <description>A private key exchanged between the server and client during the <see cref="Handshake"/> process is used.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Category("Data"),
        DefaultValue(DefaultEncryption),
        Description("The CipherStrength to be used for ciphering the data exchanged between the server and clients.")]
        public virtual CipherStrength Encryption
        {
            get
            {
                return m_encryption;
            }
            set
            {
                // Can't disable encryption when secure session is enabled.
                if (value == CipherStrength.None && m_secureSession)
                    throw new InvalidOperationException("Encryption is required when SecureSession is enabled.");

                m_encryption = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="CompressionStrength"/> to be used for compressing the data exchanged between the server and clients.
        /// </summary>
        [Category("Data"),
        DefaultValue(DefaultCompression),
        Description("The CompressionStrength to be used for compressing the data exchanged between the server and clients.")]
        public virtual CompressionStrength Compression
        {
            get
            {
                return m_compression;
            }
            set
            {
                m_compression = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates whether the server settings are to be saved to the config file.
        /// </summary>
        [Category("Persistance"),
        DefaultValue(DefaultPersistSettings),
        Description("Indicates whether the server settings are to be saved to the config file.")]
        public bool PersistSettings
        {
            get
            {
                return m_persistSettings;
            }
            set
            {
                m_persistSettings = value;
            }
        }

        /// <summary>
        /// Gets or sets the category under which the server settings are to be saved to the config file if the <see cref="PersistSettings"/> property is set to true.
        /// </summary>
        /// <exception cref="ArgumentNullException">The value specified is a null or empty string.</exception>
        [Category("Persistance"),
        DefaultValue(DefaultSettingsCategory),
        Description("Category under which the server settings are to be saved to the config file if the PersistSettings property is set to true.")]
        public string SettingsCategory
        {
            get
            {
                return m_settingsCategory;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw (new ArgumentNullException());

                m_settingsCategory = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates whether the server is currently enabled.
        /// </summary>
        /// <remarks>
        /// Setting <see cref="Enabled"/> to true will start the server if it is not running, setting
        /// to false will stop the server if it is running.
        /// </remarks>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Enabled
        {
            get
            {
                return IsRunning;
            }
            set
            {
                if (value && !IsRunning)
                    Start();
                else if (!value && IsRunning)
                    Stop();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Encoding"/> to be used for the text sent to the connected clients.
        /// </summary>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Encoding TextEncoding
        {
            get
            {
                return m_textEncoding;
            }
            set
            {
                m_textEncoding = value;
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="Delegate"/> to be invoked instead of the <see cref="ReceiveClientDataComplete"/> event when data is received from clients.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property only needs to be implemented if you need data from the clients absolutelty as fast as possible, for most uses this will not be necessary.  
        /// Setting this property gives the consumer access to the data stream as soon as it's available, but this also bypasses <see cref="Encryption"/> and 
        /// <see cref="Compression"/> on received data.
        /// </para>
        /// <para>
        /// arg1 in <see cref="ReceiveClientDataHandler"/> is the ID or the client from which data is received.<br/>
        /// arg2 in <see cref="ReceiveClientDataHandler"/> is the buffer containing data received from the client starting at index zero.<br/>
        /// arg3 in <see cref="ReceiveClientDataHandler"/> is the number of bytes received from the client that is stored in the buffer (arg2).
        /// </para>
        /// </remarks>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Action<Guid, byte[], int> ReceiveClientDataHandler
        {
            get
            {
                return m_receiveClientDataHandler;
            }
            set
            {
                m_receiveClientDataHandler = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="TransportProtocol"/> used by the server for the transportation of data with the clients.
        /// </summary>
        [Browsable(false)]
        public virtual TransportProtocol TransportProtocol
        {
            get
            {
                return m_transportProtocol;
            }
        }

        /// <summary>
        /// Gets the server's ID.
        /// </summary>
        [Browsable(false)]
        public virtual Guid ServerID
        {
            get
            {
                return m_serverID;
            }
        }

        /// <summary>
        /// Gets the IDs of clients connected to the server.
        /// </summary>
        [Browsable(false)]
        public virtual Guid[] ClientIDs
        {
            get
            {
                lock (m_clientIDs)
                {
                    return m_clientIDs.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets a boolean value that indicates whether the server is currently running.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsRunning
        {
            get
            {
                return m_isRunning;
            }
        }

        /// <summary>
        /// Gets the time in seconds for which the server has been running.
        /// </summary>
        [Browsable(false)]
        public virtual double RunTime
        {
            get
            {
                double serverRunTime = 0.0D;

                if (m_startTime > 0)
                {
                    if (m_isRunning)
                        // Server is running.
                        serverRunTime = Ticks.ToSeconds(DateTime.Now.Ticks - m_startTime);
                    else
                        // Server is not running.
                        serverRunTime = Ticks.ToSeconds(m_stopTime - m_startTime);
                }
                return serverRunTime;
            }
        }

        /// <summary>
        /// Gets the unique identifier of the server.
        /// </summary>
        [Browsable(false)]
        public string Name
        {
            get
            {
                return m_settingsCategory;
            }
        }

        /// <summary>
        /// Gets the descriptive status of the server.
        /// </summary>
        [Browsable(false)]
        public string Status
        {
            get
            {
                StringBuilder status = new StringBuilder();
                if (m_handshake)
                {
                    // Display ID only if handshaking is enabled.
                    status.Append("                 Server ID: ");
                    status.Append(m_serverID.ToString());
                    status.AppendLine();
                }
                status.Append("              Server state: ");
                status.Append(m_isRunning ? "Running" : "Not Running");
                status.AppendLine();
                status.Append("            Server runtime: ");
                status.Append(Seconds.ToText(RunTime));
                status.AppendLine();
                status.Append("      Configuration string: ");
                status.Append(m_configurationString);
                status.AppendLine();
                status.Append("         Connected clients: ");
                lock (m_clientIDs)
                {
                    status.Append(m_clientIDs.Count);
                }
                status.AppendLine();
                status.Append("           Maximum clients: ");
                status.Append(m_maxClientConnections == -1 ? "Infinite" : m_maxClientConnections.ToString());
                status.AppendLine();
                status.Append("            Receive buffer: ");
                status.Append(m_receiveBufferSize.ToString());
                status.AppendLine();
                status.Append("        Transport protocol: ");
                status.Append(m_transportProtocol.ToString());
                status.AppendLine();
                status.Append("        Text encoding used: ");
                status.Append(m_textEncoding.EncodingName);
                status.AppendLine();

                return status.ToString();
            }
        }

        #endregion

        #region [ Methods ]

        #region [ Abstract ]

        /// <summary>
        /// When overridden in a derived class, starts the server.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// When overridden in a derived class, stops the server.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// When overridden in a derived class, disconnects a connected client.
        /// </summary>
        /// <param name="clientID">ID of the client to be disconnected.</param>
        public abstract void DisconnectOne(Guid clientID);

        /// <summary>
        /// When overridden in a derived class, validates the specified <paramref name="configurationString"/>.
        /// </summary>
        /// <param name="configurationString">The configuration string to be validated.</param>
        protected abstract void ValidateConfigurationString(string configurationString);

        /// <summary>
        /// When overridden in a derived class, returns the passphrase used for ciphering client data.
        /// </summary>
        /// <param name="clientID">ID of the client whose passphrase is to be retrieved.</param>
        /// <returns>Passphrase of the specified client.</returns>
        protected abstract string GetSessionPassphrase(Guid clientID);

        /// <summary>
        /// When overridden in a derived class, sends data to the specified client asynchronously.
        /// </summary>
        /// <param name="clientID">ID of the client to which the data is to be sent.</param>
        /// <param name="data">The buffer that contains the binary data to be sent.</param>
        /// <param name="offset">The zero-based position in the <paramref name="data"/> at which to begin sending data.</param>
        /// <param name="length">The number of bytes to be sent from <paramref name="data"/> starting at the <paramref name="offset"/>.</param>
        /// <returns><see cref="WaitHandle"/> for the asynchronous operation.</returns>
        protected abstract WaitHandle SendDataToAsync(Guid clientID, byte[] data, int offset, int length);

        #endregion

        /// <summary>
        /// Initializes the server.
        /// </summary>
        /// <remarks>
        /// <see cref="Initialize()"/> is to be called by user-code directly only if the server is not consumed through the designer surface of the IDE.
        /// </remarks>
        public void Initialize()
        {
            if (!m_initialized)
            {
                LoadSettings();         // Load settings from the config file.
                m_initialized = true;   // Initialize only once.
            }
        }

        /// <summary>
        /// Sends data to the specified client synchronously.
        /// </summary>
        /// <param name="clientID">ID of the client to which the data is to be sent.</param>
        /// <param name="data">The plain-text data that is to be sent.</param>
        public virtual void SendTo(Guid clientID, string data)
        {
            SendTo(clientID, m_textEncoding.GetBytes(data));
        }

        /// <summary>
        /// Sends data to the specified client synchronously.
        /// </summary>
        /// <param name="clientID">ID of the client to which the data is to be sent.</param>
        /// <param name="serializableObject">The serializable object that is to be sent.</param>
        public virtual void SendTo(Guid clientID, object serializableObject)
        {
            SendTo(clientID, Serialization.GetBytes(serializableObject));
        }

        /// <summary>
        /// Sends data to the specified client synchronously.
        /// </summary>
        /// <param name="clientID">ID of the client to which the data is to be sent.</param>
        /// <param name="data">The binary data that is to be sent.</param>
        public virtual void SendTo(Guid clientID, byte[] data)
        {
            SendTo(clientID, data, 0, data.Length);
        }

        /// <summary>
        /// Sends data to the specified client synchronously.
        /// </summary>
        /// <param name="clientID">ID of the client to which the data is to be sent.</param>
        /// <param name="data">The buffer that contains the binary data to be sent.</param>
        /// <param name="offset">The zero-based position in the <paramref name="data"/> at which to begin sending data.</param>
        /// <param name="length">The number of bytes to be sent from <paramref name="data"/> starting at the <paramref name="offset"/>.</param>
        public virtual void SendTo(Guid clientID, byte[] data, int offset, int length)
        {
            SendToAsync(clientID, data, offset, length).WaitOne();
        }

        /// <summary>
        /// Sends data to all of the connected clients synchronously.
        /// </summary>
        /// <param name="data">The plain-text data that is to be sent.</param>
        public virtual void Multicast(string data)
        {
            Multicast(m_textEncoding.GetBytes(data));
        }

        /// <summary>
        /// Sends data to all of the connected clients synchronously.
        /// </summary>
        /// <param name="serializableObject">The serializable object that is to be sent.</param>
        public virtual void Multicast(object serializableObject)
        {
            Multicast(Serialization.GetBytes(serializableObject));
        }

        /// <summary>
        /// Sends data to all of the connected clients synchronously.
        /// </summary>
        /// <param name="data">The binary data that is to be sent.</param>
        public virtual void Multicast(byte[] data)
        {
            Multicast(data, 0, data.Length);
        }

        /// <summary>
        /// Sends data to all of the connected clients synchronously.
        /// </summary>
        /// <param name="data">The buffer that contains the binary data to be sent.</param>
        /// <param name="offset">The zero-based position in the <paramref name="data"/> at which to begin sending data.</param>
        /// <param name="length">The number of bytes to be sent from <paramref name="data"/> starting at the <paramref name="offset"/>.</param>
        public virtual void Multicast(byte[] data, int offset, int length)
        {
            WaitHandle.WaitAll(MulticastAsync(data, offset, length));
        }

        /// <summary>
        /// Disconnects all of the connected clients.
        /// </summary>
        public void DisconnectAll()
        {
            foreach (Guid clientID in ClientIDs)
            {
                DisconnectOne(clientID);
            }
        }

        /// <summary>
        /// Saves server settings to the config file if the <see cref="PersistSettings"/> property is set to true.
        /// </summary>        
        public virtual void SaveSettings()
        {
            if (m_persistSettings)
            {
                // Ensure that settings category is specified.
                if (string.IsNullOrEmpty(m_settingsCategory))
                    throw new InvalidOperationException("SettingsCategory property has not been set.");

                // Save settings under the specified category.
                ConfigurationFile config = ConfigurationFile.Current;
                CategorizedSettingsElementCollection settings = config.Settings[m_settingsCategory];
                settings["ConfigurationString", true].Update(m_configurationString, "Data required by the server for initialization.");
                settings["MaxClientConnections", true].Update(m_maxClientConnections, "Maximum number of clients that can connect to the server.");
                settings["Handshake", true].Update(m_handshake, "True if the server will do a handshake with the client; otherwise False.");
                settings["HandshakePassphrase", true].Update(m_handshakePassphrase, "Passpharse that the clients must provide for authentication during the handshake process.");
                settings["SecureSession", true].Update(m_secureSession, "True if the data exchanged between the server and clients will be encrypted using a private session passphrase; otherwise False.");
                settings["ReceiveBufferSize", true].Update(m_receiveBufferSize, "Maximum number of bytes that can be received at a time by the server from the clients.");
                settings["Encryption", true].Update(m_encryption, "Cipher strength (None; Level1; Level2; Level3; Level4; Level5) to be used for encrypting the data exchanged between the server and clients.");
                settings["Compression", true].Update(m_compression, "Compression strength (NoCompression; DefaultCompression; BestSpeed; BestCompression; MultiPass) to be used for compressing the data exchanged between the server and clients.");
                config.Save();
            }
        }

        /// <summary>
        /// Loads saved server settings from the config file if the <see cref="PersistSettings"/> property is set to true.
        /// </summary>        
        public virtual void LoadSettings()
        {
            if (m_persistSettings)
            {
                // Ensure that settings category is specified.
                if (string.IsNullOrEmpty(m_settingsCategory))
                    throw new InvalidOperationException("SettingsCategory property has not been set.");

                // Load settings from the specified category.
                ConfigurationFile config = ConfigurationFile.Current;
                CategorizedSettingsElementCollection settings = config.Settings[m_settingsCategory];
                ConfigurationString = settings["ConfigurationString", true].ValueAs(m_configurationString);
                MaxClientConnections = settings["MaxClientConnections", true].ValueAs(m_maxClientConnections);
                Handshake = settings["Handshake", true].ValueAs(m_handshake);
                HandshakePassphrase = settings["HandshakePassphrase", true].ValueAs(m_handshakePassphrase);
                ReceiveBufferSize = settings["ReceiveBufferSize", true].ValueAs(m_receiveBufferSize);
                Encryption = settings["Encryption", true].ValueAs(m_encryption);
                Compression = settings["Compression", true].ValueAs(m_compression);
                SecureSession = settings["SecureSession", true].ValueAs(m_secureSession);
            }
        }

        /// <summary>
        /// Performs necessary operations before the server properties are initialized.
        /// </summary>
        /// <remarks>
        /// <see cref="BeginInit()"/> should never be called by user-code directly. This method exists solely for use by the designer if the server is consumed through 
        /// the designer surface of the IDE.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void BeginInit()
        {
            // Nothing needs to be done before component is initialized.
        }

        /// <summary>
        /// Performs necessary operations after the server properties are initialized.
        /// </summary>
        /// <remarks>
        /// <see cref="EndInit()"/> should never be called by user-code directly. This method exists solely for use by the designer if the server is consumed through the 
        /// designer surface of the IDE.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void EndInit()
        {
            if (!DesignMode)
            {
                Initialize();
            }
        }

        /// <summary>
        /// Sends data to the specified client asynchronously.
        /// </summary>
        /// <param name="clientID">ID of the client to which the data is to be sent.</param>
        /// <param name="data">The plain-text data that is to be sent.</param>
        /// <returns><see cref="WaitHandle"/> for the asynchronous operation.</returns>
        public virtual WaitHandle SendToAsync(Guid clientID, string data)
        {
            return SendToAsync(clientID, m_textEncoding.GetBytes(data));
        }

        /// <summary>
        /// Sends data to the specified client asynchronously.
        /// </summary>
        /// <param name="clientID">ID of the client to which the data is to be sent.</param>
        /// <param name="serializableObject">The serializable object that is to be sent.</param>
        /// <returns><see cref="WaitHandle"/> for the asynchronous operation.</returns>
        public virtual WaitHandle SendToAsync(Guid clientID, object serializableObject)
        {
            return SendToAsync(clientID, Serialization.GetBytes(serializableObject));
        }

        /// <summary>
        /// Sends data to the specified client asynchronously.
        /// </summary>
        /// <param name="clientID">ID of the client to which the data is to be sent.</param>
        /// <param name="data">The binary data that is to be sent.</param>
        /// <returns><see cref="WaitHandle"/> for the asynchronous operation.</returns>
        public virtual WaitHandle SendToAsync(Guid clientID, byte[] data)
        {
            return SendToAsync(clientID, data, 0, data.Length);
        }

        /// <summary>
        /// Sends data to the specified client asynchronously.
        /// </summary>
        /// <param name="clientID">ID of the client to which the data is to be sent.</param>
        /// <param name="data">The buffer that contains the binary data to be sent.</param>
        /// <param name="offset">The zero-based position in the <paramref name="data"/> at which to begin sending data.</param>
        /// <param name="length">The number of bytes to be sent from <paramref name="data"/> starting at the <paramref name="offset"/>.</param>
        /// <returns><see cref="WaitHandle"/> for the asynchronous operation.</returns>
        public virtual WaitHandle SendToAsync(Guid clientID, byte[] data, int offset, int length)
        {
            if (m_isRunning)
            {
                // Pre-condition data as needed and then send it.
                Payload.ProcessTransmit(ref data, ref offset, ref length, m_encryption, GetSessionPassphrase(clientID), m_compression);
                return SendDataToAsync(clientID, data, offset, length);
            }
            else
            {
                throw new InvalidOperationException("Server is not running.");
            }
        }

        /// <summary>
        /// Sends data to all of the connected clients asynchronously.
        /// </summary>
        /// <param name="data">The plain-text data that is to be sent.</param>
        /// <returns>Array of <see cref="WaitHandle"/> for the asynchronous operation.</returns>
        public virtual WaitHandle[] MulticastAsync(string data)
        {
            return MulticastAsync(m_textEncoding.GetBytes(data));
        }

        /// <summary>
        /// Sends data to all of the connected clients asynchronously.
        /// </summary>
        /// <param name="serializableObject">The serializable object that is to be sent.</param>
        /// <returns>Array of <see cref="WaitHandle"/> for the asynchronous operation.</returns>
        public virtual WaitHandle[] MulticastAsync(object serializableObject)
        {
            return MulticastAsync(Serialization.GetBytes(serializableObject));
        }

        /// <summary>
        /// Sends data to all of the connected clients asynchronously.
        /// </summary>
        /// <param name="data">The binary data that is to be sent.</param>
        /// <returns>Array of <see cref="WaitHandle"/> for the asynchronous operation.</returns>
        public virtual WaitHandle[] MulticastAsync(byte[] data)
        {
            return MulticastAsync(data, 0, data.Length);
        }

        /// <summary>
        /// Sends data to all of the connected clients asynchronously.
        /// </summary>
        /// <param name="data">The buffer that contains the binary data to be sent.</param>
        /// <param name="offset">The zero-based position in the <paramref name="data"/> at which to begin sending data.</param>
        /// <param name="length">The number of bytes to be sent from <paramref name="data"/> starting at the <paramref name="offset"/>.</param>
        /// <returns>Array of <see cref="WaitHandle"/> for the asynchronous operation.</returns>
        public virtual WaitHandle[] MulticastAsync(byte[] data, int offset, int length)
        {
            List<WaitHandle> handles = new List<WaitHandle>();
            foreach (Guid clientID in ClientIDs)
            {
                handles.Add(SendToAsync(clientID, data, offset, length));
            }

            return handles.ToArray();
        }

        /// <summary>
        /// Releases the unmanaged resources used by the server and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                try
                {
                    // This will be done regardless of whether the object is finalized or disposed.
                    SaveSettings();
                    if (disposing)
                    {
                        // This will be done only when the object is disposed by calling Dispose().
                        Stop();
                    }
                }
                finally
                {
                    base.Dispose(disposing);    // Call base class Dispose().
                    m_disposed = true;          // Prevent duplicate dispose.
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="ServerStarted"/> event.
        /// </summary>
        /// <param name="e"><see cref="ServerStarted"/> event data.</param>
        protected virtual void OnServerStarted(EventArgs e)
        {
            m_isRunning = true;
            m_stopTime = 0;
            m_startTime = DateTime.Now.Ticks;   // Save the time when server is started.

            if (ServerStarted != null)
                ServerStarted(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ServerStopped"/> event.
        /// </summary>
        /// <param name="e"><see cref="ServerStopped"/> event data.</param>
        protected virtual void OnServerStopped(EventArgs e)
        {
            m_isRunning = false;
            m_stopTime = DateTime.Now.Ticks;    // Save the time when server is stopped.

            if (ServerStopped != null)
                ServerStopped(this, e);
        }

        /// <summary>
        /// Raises the <see cref="HandshakeTimedout"/> event.
        /// </summary>
        /// <param name="e"><see cref="HandshakeTimedout"/> event data.</param>
        protected virtual void OnHandshakeTimedout(EventArgs e)
        {
            if (HandshakeTimedout != null)
                HandshakeTimedout(this, e);
        }

        /// <summary>
        /// Raises the <see cref="HandshakeUnsuccessful"/> event.
        /// </summary>
        /// <param name="e"><see cref="HandshakeUnsuccessful"/> event data.</param>
        protected virtual void OnHandshakeUnsuccessful(EventArgs e)
        {
            if (HandshakeUnsuccessful != null)
                HandshakeUnsuccessful(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ClientConnected"/> event.
        /// </summary>
        /// <param name="clientID">ID of client to send to <see cref="ClientConnected"/> event.</param>
        protected virtual void OnClientConnected(Guid clientID)
        {
            lock (m_clientIDs)
            {
                m_clientIDs.Add(clientID);
            }

            if (ClientConnected != null)
                ClientConnected(this, new EventArgs<Guid>(clientID));
        }

        /// <summary>
        /// Raises the <see cref="ClientDisconnected"/> event.
        /// </summary>
        /// <param name="clientID">ID of client to send to <see cref="ClientDisconnected"/> event.</param>
        protected virtual void OnClientDisconnected(Guid clientID)
        {
            lock (m_clientIDs)
            {
                m_clientIDs.Remove(clientID);
            }

            if (ClientDisconnected != null)
                ClientDisconnected(this, new EventArgs<Guid>(clientID));
        }

        /// <summary>
        /// Raises the <see cref="SendClientDataStarted"/> event.
        /// </summary>
        /// <param name="clientID">ID of client to send to <see cref="SendClientDataStarted"/> event.</param>
        protected virtual void OnSendClientDataStarted(Guid clientID)
        {
            if (SendClientDataStarted != null)
                SendClientDataStarted(this, new EventArgs<Guid>(clientID));
        }

        /// <summary>
        /// Raises the <see cref="SendClientDataComplete"/> event.
        /// </summary>
        /// <param name="clientID">ID of client to send to <see cref="SendClientDataComplete"/> event.</param>
        protected virtual void OnSendClientDataComplete(Guid clientID)
        {
            if (SendClientDataComplete != null)
                SendClientDataComplete(this, new EventArgs<Guid>(clientID));
        }

        /// <summary>
        /// Raises the <see cref="SendClientDataException"/> event.
        /// </summary>
        /// <param name="clientID">ID of client to send to <see cref="SendClientDataException"/> event.</param>
        protected virtual void OnSendClientDataException(Guid clientID, Exception ex)
        {
            if (SendClientDataException != null)
                SendClientDataException(this, new EventArgs<Guid,Exception>(clientID, ex));
        }

        /// <summary>
        /// Raises the <see cref="ReceiveClientDataTimedout"/> event.
        /// </summary>
        /// <param name="clientID">ID of client to send to <see cref="ReceiveClientDataTimedout"/> event.</param>
        protected virtual void OnReceiveClientDataTimedout(Guid clientID)
        {
            if (ReceiveClientDataTimedout != null)
                ReceiveClientDataTimedout(this, new EventArgs<Guid>(clientID));
        }

        /// <summary>
        /// Raises the <see cref="ReceiveClientDataComplete"/> event.
        /// </summary>
        /// <param name="clientID">ID of the client from which data is received.</param>
        /// <param name="data">Data received from the client.</param>
        /// <param name="size">Number of bytes received from the client.</param>
        protected virtual void OnReceiveClientDataComplete(Guid clientID, byte[] data, int size)
        {
            if (m_receiveClientDataHandler != null)
            {
                m_receiveClientDataHandler(clientID, data, size);
            }
            else
            {
                if (ReceiveClientDataComplete != null)
                {
                    try
                    {
                        int offset = 0; // Received buffer will always have valid data starting at offset zero.
                        Payload.ProcessReceived(ref data, ref offset, ref size, m_encryption, GetSessionPassphrase(clientID), m_compression);
                    }
                    catch
                    {
                        // Ignore encountered exception and pass-on the raw data.
                    }
                    finally
                    {
                        ReceiveClientDataComplete(this, new EventArgs<Guid, byte[], int>(clientID, data, size));
                    }
                }
            }
        }

        #endregion

        #region [ Static ]

        ///// <summary>
        ///// Create a communications server
        ///// </summary>
        ///// <remarks>
        ///// Note that typical configuration string should be prefixed with a "protocol=tcp" or a "protocol=udp"
        ///// </remarks>
        //public static IServer Create(string configurationString)
        //{
        //    Dictionary<string, string> configurationData = configurationString.ParseKeyValuePairs();
        //    IServer server = null;
        //    string protocol;

        //    if (configurationData.TryGetValue("protocol", out protocol))
        //    {
        //        configurationData.Remove("protocol");
        //        StringBuilder settings = new StringBuilder();

        //        foreach (string key in configurationData.Keys)
        //        {
        //            settings.Append(key);
        //            settings.Append("=");
        //            settings.Append(configurationData[key]);
        //            settings.Append(";");
        //        }

        //        switch (protocol.ToLower())
        //        {
        //            case "tcp":
        //                server = new TcpServer(settings.ToString());
        //                break;
        //            case "udp":
        //                server = new UdpServer(settings.ToString());
        //                break;
        //            default:
        //                throw new ArgumentException("Transport protocol \'" + protocol + "\' is not valid.");
        //        }
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Transport protocol must be specified.");
        //    }

        //    return server;
        //}

        #endregion

    }
}

///// <summary>
///// Performs the necessary uncompression and decryption on the specified data and returns it.
///// </summary>
///// <param name="data">The data on which uncompression and decryption is to be performed.</param>
///// <param name="offset"></param>
///// <param name="length"></param>
///// <param name="cryptoKey"></param>
///// <returns>Uncompressed and decrypted data.</returns>
//protected virtual byte[] GetActualData(byte[] data, int offset, int length, string cryptoKey)
//{
//    if (m_encryption == CipherStrength.None && m_compression == CompressionStrength.NoCompression)
//    {
//        if (length - offset == data.Length)
//            return data;
//        else
//            return data.BlockCopy(offset, length);
//    }
//    else
//    {
//        if (m_encryption != CipherStrength.None)
//        {
//            data = Payload.DecryptData(data, offset, length, cryptoKey, m_encryption);
//        }

//        if (m_compression != CompressionStrength.NoCompression)
//        {
//            data = Payload.DecompressData(data, m_compression);
//        }

//        return data;
//    }
//}

///// <summary>
///// Performs the necessary compression and encryption on the specified data and returns it.
///// </summary>
///// <param name="data">The data on which compression and encryption is to be performed.</param>
///// <param name="offset"></param>
///// <param name="length"></param>
///// <param name="cryptoKey"></param>
///// <returns>Compressed and encrypted data.</returns>
//protected virtual byte[] GetPreparedData(byte[] data, int offset, int length, string cryptoKey)
//{
//    if (m_encryption == CipherStrength.None && m_compression == CompressionStrength.NoCompression)
//    {

//        if (length - offset == data.Length)
//            return data;
//        else
//            return data.BlockCopy(offset, length);
//    }
//    else
//    {
//        if (m_compression != CompressionStrength.NoCompression)
//        {
//            // Compress the data.
//            data = Payload.CompressData(data, offset, length, m_compression);
//            offset = 0;
//            length = data.Length;
//        }

//        if (m_encryption != CipherStrength.None)
//        {
//            // Encrypt the data.
//            data = Payload.EncryptData(data, offset, length, cryptoKey, m_encryption);
//        }

//        return data;
//    }
//}

//if (m_isRunning)
//{
//    if (data == null)
//        throw new ArgumentNullException("data");

//    if (length > 0)
//    {
//        // Pre-condition data as needed (compression, encryption, etc.)
//        data = GetPreparedData(data, offset, length);
//        lock (m_clientIDs)
//        {
//            foreach (Guid clientID in m_clientIDs)
//            {
//                try
//                {
//                    // PCP - 05/24/2007: Reverting to synchronous send to avoid out-of-sequence transmissions.
//                    SendPreparedDataTo(clientID, data);
//                }
//                catch
//                {
//                    // In rare cases, we might encounter an exception here when the inheriting server
//                    // class doesn't think that the ID of the client to which it has to send the message is
//                    // valid (even though it is). This might happen when connection with a new client is
//                    // not yet complete and we're trying to send a message to the client.
//                }
//            }
//        }
//    }
//}

///// <summary>
///// The key used for encryption and decryption when Encryption is enabled but HandshakePassphrase is not set.
///// </summary>
//private const string DefaultCryptoKey = "6572a33d-826f-4d96-8c28-8be66bbc700e";

///// <summary>
///// The maximum number of bytes that can be sent from the server to clients in a single send operation.
///// </summary>
//public const int MaximumDataSize = 524288000; // 500 MB

///// <summary>
///// Occurs when the server is started.
///// </summary>
//[Description("Occurs when the server is started."), Category("Server")]
//public event EventHandler ServerStarted;

///// <summary>
///// Occurs when the server is stopped.
///// </summary>
//[Description("Occurs when the server is stopped."), Category("Server")]
//public event EventHandler ServerStopped;

///// <summary>
///// Occurs when an exception is encountered while starting up the server.
///// </summary>
//[Description("Occurs when an exception is encountered while starting up the server."), Category("Server")]
//public event EventHandler<EventArgs<Exception>> ServerStartupException;

///// <summary>
///// Raises the PCS.Communication.ServerBase.ServerStarted event.
///// </summary>
///// <param name="e">A System.EventArgs that contains the event data.</param>
///// <remarks>This method is to be called after the server has been started.</remarks>
//protected virtual void OnServerStarted(EventArgs e)
//{
//    m_isRunning = true;
//    m_startTime = DateTime.Now.Ticks; // Save the time when server is started.
//    m_stopTime = 0;
//    if (ServerStarted != null) ServerStarted(this, e);
//}

///// <summary>
///// Raises the PCS.Communication.ServerBase.ServerStopped event.
///// </summary>
///// <param name="e">A System.EventArgs that contains the event data.</param>
///// <remarks>This method is to be called after the server has been stopped.</remarks>
//protected virtual void OnServerStopped(EventArgs e)
//{
//    m_isRunning = false;
//    m_stopTime = DateTime.Now.Ticks; // Save the time when server is stopped.
//    if (ServerStopped != null) ServerStopped(this, e);
//}

///// <summary>
///// Raises the PCS.Communication.ServerBase.ServerStartupException event.
///// </summary>
///// <param name="e">A PCS.ExceptionEventArgs that contains the event data.</param>
///// <remarks>This method is to be called if the server throws an exception during startup.</remarks>
//protected virtual void OnServerStartupException(Exception e)
//{
//    if (ServerStartupException != null) ServerStartupException(this, new EventArgs<Exception>(e));
//}

///// <summary>
///// Performs the necessary compression and encryption on the specified data and returns it.
///// </summary>
///// <param name="data">The data on which compression and encryption is to be performed.</param>
///// <returns>Compressed and encrypted data.</returns>
///// <remarks>No encryption is performed if SecureSession is enabled, even if Encryption is enabled.</remarks>
//protected virtual byte[] GetPreparedData(byte[] data)
//{
//    return GetPreparedData(data, 0, data.Length);
//}