//******************************************************************************************************
//  PublisherInstance.h - Gbtc
//
//  Copyright � 2018, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  12/05/2018 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

#ifndef __DATA_PUBLISHER_INSTANCE_H
#define __DATA_PUBLISHER_INSTANCE_H

#include "DataPublisher.h"

namespace GSF {
namespace TimeSeries {
namespace Transport
{
    class PublisherInstance
    {
    private:
        // Publication members
        uint16_t m_port;
        bool m_isIPV6;
        DataPublisher m_publisher;
        bool m_initialized;

        // Internal subscription event handlers
        static void HandleStatusMessage(DataPublisher* source, const std::string& message);
        static void HandleErrorMessage(DataPublisher* source, const std::string& message);
        static void HandleClientConnected(DataPublisher* source, const Guid& clientID, const std::string& connectionInfo, const std::string& subscriberInfo);

    protected:
        virtual void StatusMessage(const std::string& message);	// Defaults output to cout
        virtual void ErrorMessage(const std::string& message);	// Defaults output to cerr
        virtual void ClientConnected(const Guid& clientID, const std::string& connectionInfo, const std::string& subscriberInfo);

    public:
        PublisherInstance(uint16_t port, bool ipV6);
        virtual ~PublisherInstance();

        // Publisher functions

        // Initialize connection, i.e., indicate readiness for clients
        void Initialize();

        // Define metadata from existing metadata tables
        void DefineMetadata(const std::vector<DeviceMetadataPtr>& deviceMetadata, const std::vector<MeasurementMetadataPtr>& measurementMetadata, const std::vector<PhasorMetadataPtr>& phasorMetadata);

        // Define metadata from existing configuration frames
        void DefineMetadata(const std::vector<ConfigurationFramePtr>& devices, const MeasurementMetadataPtr& qualityFlags = nullptr);

        // Define metadata from existing XML document
        void DefineMetadata(const pugi::xml_document& metadata);

        void PublishMeasurements(const std::vector<Measurement>& measurements);
        void PublishMeasurements(const std::vector<MeasurementPtr>& measurements);

        bool IsMetadataRefreshAllowed() const;
        void SetMetadataRefreshAllowed(bool allowed);

        bool IsNaNValueFilterAllowed() const;
        void SetNaNValueFilterAllowed(bool allowed);

        bool IsNaNValueFilterForced() const;
        void SetNaNValueFilterForced(bool forced);

        uint32_t GetCipherKeyRotationPeriod() const;
        void SetCipherKeyRotationPeriod(uint32_t period);

        uint16_t GetPort() const;
        bool IsIPv6() const;

        // Statistical functions
        uint64_t GetTotalCommandChannelBytesSent() const;
        uint64_t GetTotalDataChannelBytesSent() const;
        uint64_t GetTotalMeasurementsSent() const;
        bool IsConnected() const;
        bool IsInitialized() const;
    };
}}}

#endif