﻿//******************************************************************************************************
//  CalulatedMeasurment.cs - Gbtc
//
//  Copyright © 2010, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  04/11/2011 - Aniket Salver
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using TVA.Data;
using System.Data;
using System.Collections.Generic;

namespace TimeSeriesFramework.UI.DataModels
{
    /// <summary>
    /// Represents a record of CalculatedMeasurment information as defined in the database.
    /// </summary>
    public class CalculatedMeasurement : DataModelBase
    {
        #region [ Members ]

        // Fields
        private string m_nodeID;
        private int m_ID;
        private string m_acronym;
        private string m_name;
        private string m_typeName;
        private string m_assemblyName;
        private string m_connectionString;
        private string m_configSection;
        private string m_outputMeasurements;
        private string m_inputMeasurements;
        private int m_minimumMeasurementsToUse;
        private int m_framesPerSecond;
        private double m_lagTime;
        private double m_leadTime;
        private bool m_useLocalClockAsRealTime;
        private bool m_allowSortsByArrival;
        private int m_loadOrder;
        private bool m_enabled;
        private bool m_ignoreBadTimeStamps;
        private int m_timeResolution;
        private bool m_allowPreemptivePublishing;
        private string m_downsamplingMethod;
        private string m_nodeName;
        private bool m_performTimestampReasonabilityCheck;
        private DateTime m_createdOn;
        private string m_createdBy;
        private DateTime m_updatedOn;
        private string m_updatedBy;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets <see cref="CalculatedMeasurement"/> NodeId.
        /// </summary>
        [Required(ErrorMessage = "Please provide NodeId value.")]
        public string NodeID
        {
            get
            {
                return m_nodeID;
            }
            set
            {
                m_nodeID = value;
                OnPropertyChanged("NodeID");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> ID.
        /// </summary>
        // Field is populated by database via auto-increment and has no screen interaction, so no validation attributes are applied
        public int ID
        {
            get
            {
                return m_ID;
            }
            set
            {
                m_ID = value;
                OnPropertyChanged("ID");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> Acronym.
        /// </summary>
        // Field is populated by database via auto-increment and has no screen interaction, so no validation attributes are applied
        [Required(ErrorMessage = "CalulatedMeasurment acronym is a required field, please provide value.")]
        public string Acronym
        {
            get
            {
                return m_acronym;
            }
            set
            {
                m_acronym = value;
                OnPropertyChanged("Acronym");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> Name.
        /// </summary>
        // Field is populated by database via auto-increment and has no screen interaction, so no validation attributes are applied
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> Acronym.
        /// </summary>
        [Required(ErrorMessage = "CalulatedMeasurment TypeName is a required field, please provide value.")]
        public string TypeName
        {
            get
            {
                return m_typeName;
            }
            set
            {
                m_typeName = value;
                OnPropertyChanged("TypeName");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> AssemblyName.
        /// </summary>
        [Required(ErrorMessage = "CalulatedMeasurment AssemblyName is a required field, please provide value.")]
        public string AssemblyName
        {
            get
            {
                return m_assemblyName;
            }
            set
            {
                m_assemblyName = value;
                OnPropertyChanged("AssemblyName");
            }
        }

        /// <summary>
        /// Gets or sets <see cref="CalculatedMeasurement"/> ConnectionString.
        /// </summary>
        // Field is populated by database via auto-increment and has no screen interaction, so no validation attributes are applied
        public string ConnectionString
        {
            get
            {
                return m_connectionString;
            }
            set
            {
                m_connectionString = value;
                OnPropertyChanged("ConnectionString");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> ConfigSection.
        /// </summary>
        // Field is populated by database via auto-increment and has no screen interaction, so no validation attributes are applied
        public string ConfigSection
        {
            get
            {
                return m_configSection;
            }
            set
            {
                m_configSection = value;
                OnPropertyChanged("ConfigSection");
            }
        }

        /// <summary>
        /// Gets or sets <see cref="CalculatedMeasurement"/> OutputMeasurements.
        /// </summary>
        // Field is populated by database via auto-increment and has no screen interaction, so no validation attributes are applied
        public string OutputMeasurements
        {
            get
            {
                return m_outputMeasurements;
            }
            set
            {
                m_outputMeasurements = value;
                OnPropertyChanged("OutputMeasurements");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> InputMeasurements.
        /// </summary>
        // Field is populated by database via auto-increment and has no screen interaction, so no validation attributes are applied
        public string InputMeasurements
        {
            get
            {
                return m_inputMeasurements;
            }
            set
            {
                m_inputMeasurements = value;
                OnPropertyChanged("InputMeasurements");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> MinimumMeasurementsToUse.
        /// </summary>
        [Required(ErrorMessage = " CalulatedMeasurment MinimumMeasurementsToUse is a required field, please provide value..")]
        [DefaultValue(typeof(int), "-1")]
        public int MinimumMeasurementsToUse
        {
            get
            {
                return m_minimumMeasurementsToUse;
            }
            set
            {
                m_minimumMeasurementsToUse = value;
                OnPropertyChanged("MinimumMeasurmentsToUse");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> FramesPerSecond.
        /// </summary>
        [Required(ErrorMessage = " CalulatedMeasurment FramesPerSecond is a required field, please provide value..")]
        [DefaultValue(typeof(int), "30")]
        public int FramesPerSecond
        {
            get
            {
                return m_framesPerSecond;
            }
            set
            {
                m_framesPerSecond = value;
                OnPropertyChanged("FramesPerSecond");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> LagTime.
        /// </summary>
        [Required(ErrorMessage = "CalulatedMeasurment LagTime is a required field, please provide value.")]
        [DefaultValue(typeof(double), "3.0")]
        public double LagTime
        {
            get
            {
                return m_lagTime;
            }
            set
            {
                m_lagTime = value;
                OnPropertyChanged("LagTime");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> LeadTime.
        /// </summary>
        [Required(ErrorMessage = "LeadTime is required field.")]
        [DefaultValue(typeof(double), "1.0")]
        public double LeadTime
        {
            get
            {
                return m_leadTime;
            }
            set
            {
                m_leadTime = value;
                OnPropertyChanged("LeadTime");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> UseLocalClockAsRealTime.
        /// </summary>
        [Required(ErrorMessage = " CalulatedMeasurment LagTime is a required field, please provide value.")]
        [DefaultValue(typeof(bool), "0")]
        public bool UseLocalClockAsRealTime
        {
            get
            {
                return m_useLocalClockAsRealTime;
            }
            set
            {
                m_useLocalClockAsRealTime = value;
                OnPropertyChanged("UseLocalAsRealTime");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> AllowSortsByArrival.
        /// </summary>
        [Required(ErrorMessage = "CalulatedMeasurment AllowSortsByArrival is a required field, please provide value.")]
        [DefaultValue(typeof(bool), "1")]
        public bool AllowSortsByArrival
        {
            get
            {
                return m_allowSortsByArrival;
            }
            set
            {
                m_allowSortsByArrival = value;
                OnPropertyChanged("AllowSortsByArrival");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> LoadOrder.
        /// </summary>
        [Required(ErrorMessage = "CalulatedMeasurment LoadOrder is a required field, please provide value.")]
        [DefaultValue(typeof(int), "0")]
        public int LoadOrder
        {
            get
            {
                return m_loadOrder;
            }
            set
            {
                m_loadOrder = value;
                OnPropertyChanged("LoadOrder");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> Enabled
        /// </summary>
        [Required(ErrorMessage = "CalulatedMeasurment Enabled is a required field, please provide value.")]
        [DefaultValue(typeof(bool), "0")]
        public bool Enabled
        {
            get
            {
                return m_enabled;
            }
            set
            {
                m_enabled = value;
                OnPropertyChanged("Enabled");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> IgnoreBadTimeStamps
        /// </summary>
        [Required(ErrorMessage = "CalulatedMeasurment is a required field, please provide value.")]
        [DefaultValue(typeof(bool), "0")]
        public bool IgnoreBadTimeStamps
        {
            get
            {
                return m_ignoreBadTimeStamps;
            }
            set
            {
                m_ignoreBadTimeStamps = value;
                OnPropertyChanged("IgnoreBadtimeStamps");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> TimeResolution
        /// </summary>
        [Required(ErrorMessage = "CalulatedMeasurment TimeResoulution is a required field, please provide value.")]
        [DefaultValue(typeof(int), "10000")]
        public int TimeResolution
        {
            get
            {
                return m_timeResolution;
            }
            set
            {
                m_timeResolution = value;
                OnPropertyChanged("TimeResolution");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> AllowPreemptivePublishing
        /// </summary>
        [Required(ErrorMessage = "CalulatedMeasurment AllowPreemptivePublishing is a required field, please provide value.")]
        [DefaultValue(typeof(bool), "1")]
        public bool AllowPreemptivePublishing
        {
            get
            {
                return m_allowPreemptivePublishing;
            }
            set
            {
                m_allowPreemptivePublishing = value;
                OnPropertyChanged("AllowPreemptivePublishing");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> DownsamplingMethod
        /// </summary>
        [Required(ErrorMessage = "CalculatedMeasurment DownsamplingMethod is a required field, please provide value..")]
        [DefaultValue(typeof(string), "LastReceived")]
        public string DownsamplingMethod
        {
            get
            {
                return m_downsamplingMethod;
            }
            set
            {
                m_downsamplingMethod = value;
                OnPropertyChanged("DownsamplingMethod");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> NodeName
        /// </summary>
        [Required(ErrorMessage = "CalulatedMeasurment NodeName is a required field, please provide value..")]
        public string NodeName
        {
            get
            {
                return m_nodeName;
            }
            set
            {
                m_nodeName = value;
                OnPropertyChanged("NodeName");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> PerformTimestampReasonabilityCheck
        /// </summary>
        [Required(ErrorMessage = "CalulatedMeasurment PerformTimestampReasonabilityCheck is a required field, please provide value.")]
        [DefaultValue(typeof(bool), "1")]
        public bool PerformTimestampReasonabilityCheck
        {
            get
            {
                return m_performTimestampReasonabilityCheck;
            }
            set
            {
                m_performTimestampReasonabilityCheck = value;
                OnPropertyChanged("PerformTimestampReasonabilityCheck");
            }
        }

        /// <summary>
        /// Gets or sets <see cref="CalculatedMeasurement"/> CreatedOn
        /// </summary>
        // Field is populated by database via auto-increment and has no screen interaction, so no validation attributes are applied
        public DateTime CreatedOn
        {
            get
            {
                return m_createdOn;
            }
            set
            {
                m_createdOn = value;
                OnPropertyChanged("CreatedOn");
            }
        }

        /// <summary>
        /// Gets or sets <see cref="CalculatedMeasurement"/> CreatedBy
        /// </summary>
        // Field is populated by database via auto-increment and has no screen interaction, so no validation attributes are applied
        public string CreatedBy
        {
            get
            {
                return m_createdBy;
            }
            set
            {
                m_createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> UpdatedOn
        /// </summary>
        // Field is populated by database via auto-increment and has no screen interaction, so no validation attributes are applied
        public DateTime UpdatedOn
        {
            get
            {
                return m_updatedOn;
            }
            set
            {
                m_updatedOn = value;
                OnPropertyChanged("UpdatedOn");
            }
        }

        /// <summary>
        ///  Gets or sets <see cref="CalculatedMeasurement"/> UpdatedBy
        /// </summary>
        // Field is populated by database via auto-increment and has no screen interaction, so no validation attributes are applied
        public string UpdatedBy
        {
            get
            {
                return m_updatedBy;
            }
            set
            {
                m_updatedBy = value;
                OnPropertyChanged("UpdatedBy");
            }
        }

        #endregion

        #region [ Static ]

        // Static Methods

        /// <summary>
        /// Loads <see cref="Company"/> information as an <see cref="ObservableCollection{T}"/> style list.
        /// </summary>
        /// <param name="database"><see cref="AdoDataConnection"/> to connection to database.</param>
        /// <returns>Collection of <see cref="CalculatedMeasurement"/>.</returns>
        public static ObservableCollection<CalculatedMeasurement> Load(AdoDataConnection database, string nodeID)
        {
            bool createdConnection = false;

            try
            {
                createdConnection = CreateConnection(ref database);

                ObservableCollection<CalculatedMeasurement> calculatedMeasurementList = new ObservableCollection<CalculatedMeasurement>();
                DataTable calculatedMeasurementTable = database.Connection.RetrieveData(database.AdapterType, "SELECT NodeID, ID, Acronym, Name, AssemblyName, " +
                    "TypeName, ConnectionString, ConfigSection, InputMeasurements, OutputMeasurements, MinimumMeasurementsToUse, FramesPerSecond, LagTime, " +
                    "LeadTime, UseLocalClockAsRealTime, AllowSortsByArrival, LoadOrder, Enabled, IgnoreBadTimeStamps, TimeResolution, AllowPreemptivePublishing, " +
                    "DownSamplingMethod, NodeName, PerformTimestampReasonabilityCheck From CalculatedMeasurementDetail Where NodeID = @nodeID Order By LoadOrder",
                    DefaultTimeout, database.IsJetEngine()? "{" + nodeID + "}" : nodeID);

                foreach (DataRow row in calculatedMeasurementTable.Rows)
                {
                    calculatedMeasurementList.Add(new CalculatedMeasurement()
                    {
                        NodeID = row.Field<object>("NodeID").ToString(),
                        ID = row.Field<int>("ID"),
                        Acronym = row.Field<string>("Acronym"),
                        Name = row.Field<string>("Name"),
                        AssemblyName = row.Field<string>("AssemblyName"),
                        TypeName = row.Field<string>("TypeName"),
                        ConnectionString = row.Field<string>("ConnectionString"),
                        ConfigSection = row.Field<string>("ConfigSection"),
                        InputMeasurements = row.Field<string>("InputMeasurements"),
                        OutputMeasurements = row.Field<string>("OutputMeasurements"),
                        MinimumMeasurementsToUse = row.Field<int>("MinimumMeasurementsToUse"),
                        FramesPerSecond = Convert.ToInt32(row.Field<object>("FramesPerSecond") ?? 30),
                        LagTime = row.Field<double>("LagTime"),
                        LeadTime = row.Field<double>("LeadTime"),
                        UseLocalClockAsRealTime = Convert.ToBoolean(row.Field<object>("UseLocalClockAsRealTime")),
                        AllowSortsByArrival = Convert.ToBoolean(row.Field<object>("AllowSortsByArrival")),
                        LoadOrder = row.Field<int>("LoadOrder"),
                        Enabled = Convert.ToBoolean(row.Field<object>("Enabled")),
                        IgnoreBadTimeStamps = Convert.ToBoolean(row.Field<object>("IgnoreBadTimeStamps")),
                        TimeResolution = Convert.ToInt32(row.Field<object>("TimeResolution")),
                        AllowPreemptivePublishing = Convert.ToBoolean(row.Field<object>("AllowPreemptivePublishing")),
                        DownsamplingMethod = row.Field<string>("DownSamplingMethod"),
                        NodeName = row.Field<string>("NodeName"),
                        PerformTimestampReasonabilityCheck = Convert.ToBoolean(row.Field<object>("PerformTimestampReasonabilityCheck"))
                    });
                }

                return calculatedMeasurementList;
            }
            finally
            {
                if (createdConnection && database != null)
                    database.Dispose();
            }
        }

        /// <summary>
        /// Gets a <see cref="Dictionary{T1,T2}"/> style list of <see cref="Company"/> information.
        /// </summary>
        /// <param name="database"><see cref="AdoDataConnection"/> to connection to database.</param>
        /// <param name="isOptional">Indicates if selection on UI is optional for this collection.</param>
        /// <returns>Dictionary<int, string> containing ID and Name of companies defined in the database.</returns>
        public static Dictionary<int, string> GetLookupList(AdoDataConnection database, bool isOptional)
        {
            bool createdConnection = false;
            try
            {
                createdConnection = CreateConnection(ref database);

                Dictionary<int, string> calculatedMeasurementList = new Dictionary<int, string>();
                if (isOptional)
                    calculatedMeasurementList.Add(0, "Select CalculatedMeasurement");

                DataTable calculatedMeasurementTable = database.Connection.RetrieveData(database.AdapterType, "SELECT ID, Name FROM CalculatedMeasurement ORDER BY LoadOrder");

                foreach (DataRow row in calculatedMeasurementTable.Rows)
                    calculatedMeasurementList[row.Field<int>("ID")] = row.Field<string>("Name");

                return calculatedMeasurementList;
            }
            finally
            {
                if (createdConnection && database != null)
                    database.Dispose();
            }
        }

        /// <summary>
        /// Saves <see cref="Company"/> information to database.
        /// </summary>
        /// <param name="database"><see cref="AdoDataConnection"/> to connection to database.</param>
        /// <param name="calculatedMeasurement">Information about <see cref="CalculatedMeasurement"/>.</param>
        /// <param name="isNew">Indicates if save is a new addition or an update to an existing record.</param>
        /// <returns>String, for display use, indicating success.</returns>
        public static string Save(AdoDataConnection database, CalculatedMeasurement calculatedMeasurement, bool isNew)
        {
            bool createdConnection = false;
            try
            {
                createdConnection = CreateConnection(ref database);

                if (isNew)
                    database.Connection.ExecuteNonQuery("Insert Into CalculatedMeasurement (NodeID, Acronym, Name, AssemblyName, TypeName, ConnectionString, " +
                    "ConfigSection, InputMeasurements, OutputMeasurements, MinimumMeasurementsToUse, FramesPerSecond, LagTime, LeadTime, UseLocalClockAsRealTime, " +
                    "AllowSortsByArrival, LoadOrder, Enabled, IgnoreBadTimeStamps, TimeResolution, AllowPreemptivePublishing, DownsamplingMethod, " +
                    "PerformTimestampReasonabilityCheck, UpdatedBy, UpdatedOn, CreatedBy, CreatedOn) Values (@nodeID, @acronym, @name, @assemblyName, " +
                    "@typeName, @connectionString, @configSection, @inputMeasurements, @outputMeasurements, @minimumMeasurementsToUse, @framesPerSecond, " +
                    "@lagTime, @leadTime, @useLocalClockAsRealTime, @allowSortsByArrival, @loadOrder, @enabled, @ignoreBadTimeStamps, @timeResolution, " +
                    "@allowPreemptivePublishing, @downsamplingMethod, @performTimestampReasonabilityCheck, @updatedBy, @updatedOn, @createdBy, @createdOn)",
                    DefaultTimeout, calculatedMeasurement.NodeID, calculatedMeasurement.Acronym.Replace(" ", "").ToUpper(), calculatedMeasurement.Name,
                    calculatedMeasurement.AssemblyName, calculatedMeasurement.TypeName, calculatedMeasurement.ConnectionString, calculatedMeasurement.ConfigSection,
                    calculatedMeasurement.InputMeasurements, calculatedMeasurement.OutputMeasurements, calculatedMeasurement.MinimumMeasurementsToUse,
                    calculatedMeasurement.FramesPerSecond, calculatedMeasurement.LagTime, calculatedMeasurement.LeadTime, calculatedMeasurement.UseLocalClockAsRealTime,
                    calculatedMeasurement.AllowSortsByArrival, calculatedMeasurement.LoadOrder, calculatedMeasurement.Enabled, calculatedMeasurement.IgnoreBadTimeStamps,
                    calculatedMeasurement.TimeResolution, calculatedMeasurement.AllowPreemptivePublishing, calculatedMeasurement.DownsamplingMethod, 
                    calculatedMeasurement.PerformTimestampReasonabilityCheck, CommonFunctions.CurrentUser, database.IsJetEngine() ? DateTime.UtcNow.Date : DateTime.UtcNow,
                    CommonFunctions.CurrentUser, database.IsJetEngine() ? DateTime.UtcNow.Date : DateTime.UtcNow);
                else
                    database.Connection.ExecuteNonQuery("Update CalculatedMeasurement Set NodeID = @nodeID, Acronym = @acronym, Name = @name, AssemblyName = @assemblyName, " +
                    "TypeName = @typeName, ConnectionString = @connectionString, ConfigSection = @configSection, InputMeasurements = @inputMeasurements, " +
                    "OutputMeasurements = @outputMeasurements, MinimumMeasurementsToUse = @minimumMeasurementsToUse, FramesPerSecond = @framesPerSecond, " +
                    "LagTime = @lagTime, LeadTime = @leadTime, UseLocalClockAsRealTime = @useLocalClockAsRealTime, AllowSortsByArrival = @allowSortsByArrival, " +
                    "LoadOrder = @loadOrder, Enabled = @enabled, IgnoreBadTimeStamps = @ignoreBadTimeStamps, TimeResolution = @timeResolution, AllowPreemptivePublishing " +
                    "= @allowPreemptivePublishing, DownsamplingMethod = @downsamplingMethod, PerformTimestampReasonabilityCheck = @performTimestampReasonabilityCheck, " +
                    "UpdatedBy = @updatedBy, UpdatedOn = @updatedOn Where ID = @id", DefaultTimeout, calculatedMeasurement.NodeID, 
                    calculatedMeasurement.Acronym.Replace(" ", "").ToUpper(), calculatedMeasurement.Name, calculatedMeasurement.AssemblyName, 
                    calculatedMeasurement.TypeName, calculatedMeasurement.ConnectionString, calculatedMeasurement.ConfigSection, calculatedMeasurement.InputMeasurements, 
                    calculatedMeasurement.OutputMeasurements, calculatedMeasurement.MinimumMeasurementsToUse, calculatedMeasurement.FramesPerSecond, 
                    calculatedMeasurement.LagTime, calculatedMeasurement.LeadTime, calculatedMeasurement.UseLocalClockAsRealTime, calculatedMeasurement.AllowSortsByArrival, 
                    calculatedMeasurement.LoadOrder, calculatedMeasurement.Enabled, calculatedMeasurement.IgnoreBadTimeStamps, calculatedMeasurement.TimeResolution, 
                    calculatedMeasurement.AllowPreemptivePublishing, calculatedMeasurement.DownsamplingMethod, calculatedMeasurement.PerformTimestampReasonabilityCheck, 
                    CommonFunctions.CurrentUser, database.IsJetEngine() ? DateTime.UtcNow.Date : DateTime.UtcNow, calculatedMeasurement.ID);

                return "Calculated measurement information saved successfully";
            }
            finally
            {
                if (createdConnection && database != null)
                    database.Dispose();
            }
        }

        /// <summary>
        /// Deletes specified <see cref="Company"/> record from database.
        /// </summary>
        /// <param name="database"><see cref="AdoDataConnection"/> to connection to database.</param>
        /// <param name="calculatedMeasurementID">ID of the record to be deleted.</param>
        /// <returns>String, for display use, indicating success.</returns>
        public static string Delete(AdoDataConnection database, int calculatedMeasurementID)
        {
            bool createdConnection = false;

            try
            {
                createdConnection = CreateConnection(ref database);

                // Setup current user context for any delete triggers
                CommonFunctions.SetCurrentUserContext(database);

                database.Connection.ExecuteNonQuery("DELETE FROM CalculatedMeasurement WHERE ID = @calculatedMeasurementID", DefaultTimeout, calculatedMeasurementID);

                return "Calculated measurement deleted successfully";
            }
            finally
            {
                if (createdConnection && database != null)
                    database.Dispose();
            }
        }

        #endregion

    }
}
