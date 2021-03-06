//******************************************************************************************************
//  CommonTypes.h - Gbtc
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
//  03/09/2012 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

#ifndef __COMMON_TYPES_H
#define __COMMON_TYPES_H

#include <cstddef>
#include <boost/any.hpp>
#include <boost/uuid/uuid.hpp>
#include <boost/exception/exception.hpp>
#include <boost/locale/collator.hpp>
#include <boost/multiprecision/cpp_dec_float.hpp>
#include <boost/thread.hpp>
#include <boost/thread/condition_variable.hpp>
#include <boost/thread/locks.hpp>
#include <boost/asio.hpp>
#include <boost/asio/ip/tcp.hpp>
#include <boost/asio/ip/udp.hpp>
#include <boost/iostreams/filtering_stream.hpp>
#include <boost/iostreams/filter/gzip.hpp>
#include <boost/iostreams/device/array.hpp>

namespace GSF {
namespace TimeSeries
{
    struct Int8
    {
        static const int8_t MaxValue = static_cast<int8_t>(127);
        static const int8_t MinValue = static_cast<int8_t>(-128);
    };

    struct UInt8
    {
        static const uint8_t MaxValue = static_cast<uint8_t>(255);
        static const uint8_t MinValue = static_cast<uint8_t>(0);
    };

    struct Int16
    {
        static const int16_t MaxValue = static_cast<int16_t>(32767);
        static const int16_t MinValue = static_cast<int16_t>(-32768);
    };

    struct UInt16
    {
        static const uint16_t MaxValue = static_cast<uint16_t>(65535);
        static const uint16_t MinValue = static_cast<uint16_t>(0);
    };

    struct Int32
    {
        static const int32_t MaxValue = static_cast<int32_t>(2147483647);
        static const int32_t MinValue = static_cast<int32_t>(-2147483647) - 1;
    };

    struct UInt32
    {
        static const uint32_t MaxValue = static_cast<uint32_t>(4294967295U);
        static const uint32_t MinValue = static_cast<uint32_t>(0);
    };

    struct Int64
    {
        static const int64_t MaxValue = static_cast<int64_t>(9223372036854775807L);
        static const int64_t MinValue = static_cast<int64_t>(-9223372036854775807L) - 1L;
    };

    struct UInt64
    {
        static const uint64_t MaxValue = static_cast<uint64_t>(18446744073709551615UL);
        static const uint64_t MinValue = static_cast<uint64_t>(0UL);
    };

    struct Decimal
    {
        static const boost::multiprecision::cpp_dec_float_100 MaxValue;
        static const boost::multiprecision::cpp_dec_float_100 MinValue;

        static const boost::multiprecision::cpp_dec_float_100 DotNetMaxValue;
        static const boost::multiprecision::cpp_dec_float_100 DotNetMinValue;
    };

    template<class T>
    using SharedPtr = boost::shared_ptr<T>;

    template<class T>
    using EnableSharedThisPtr = boost::enable_shared_from_this<T>;

    template<class T>
    SharedPtr<T> NewSharedPtr()
    {
        return boost::make_shared<T>();
    }

    template<class T, typename P1>
    SharedPtr<T> NewSharedPtr(P1 p1)
    {
        return boost::make_shared<T>(p1);
    }

    template<class T, typename P1, typename P2>
    SharedPtr<T> NewSharedPtr(P1 p1, P2 p2)
    {
        return boost::make_shared<T>(p1, p2);
    }

    template<class T, typename P1, typename P2, typename P3>
    SharedPtr<T> NewSharedPtr(P1 p1, P2 p2, P3 p3)
    {
        return boost::make_shared<T>(p1, p2, p3);
    }

    template<class T, class S>
    SharedPtr<T> CastSharedPtr(SharedPtr<S> const& source)
    {
        return boost::static_pointer_cast<T>(source);
    }

    template<typename T>
    using Action = std::function<void(T)>;

    template<typename T>
    using Func = std::function<T(void)>;

    template<typename TKey, typename TValue>
    bool TryGetValue(const std::map<TKey, TValue>& dictionary, const TKey& key, TValue& value, const TValue& defaultValue)
    {
        auto iterator = dictionary.find(key);

        if (iterator != dictionary.end())
        {
            value = iterator->second;
            return true;
        }

        value = defaultValue;
        return false;
    }

    // std::map string comparer options
    static boost::locale::comparator<char, boost::locale::collator_base::primary> StringComparerIgnoreCaseAndAccents;
    static boost::locale::comparator<char, boost::locale::collator_base::secondary> StringComparerIgnoreCase;

    typedef boost::any Object;
    typedef boost::uuids::uuid Guid;
    typedef boost::system::error_code ErrorCode;
    typedef boost::system::system_error SystemError;
    typedef boost::exception Exception;
    typedef boost::thread Thread;
    typedef boost::mutex Mutex;
    typedef boost::condition_variable WaitHandle;
    typedef boost::lock_guard<Mutex> ScopeLock;
    typedef boost::unique_lock<Mutex> UniqueLock;
    typedef boost::asio::ip::address IPAddress;
    typedef boost::asio::ip::tcp::socket TcpSocket;
    typedef boost::asio::ip::udp::socket UdpSocket;
    typedef boost::asio::ip::tcp::resolver DnsResolver;
    typedef boost::iostreams::filtering_streambuf<boost::iostreams::input> Decompressor;
    typedef boost::iostreams::gzip_decompressor GZipDecompressor;

    // Floating-point types
    typedef float float32_t;
    typedef double float64_t;
    typedef boost::multiprecision::cpp_dec_float_100 decimal_t;

    template<class T>
    T Cast(const Object& source)
    {
        return boost::any_cast<T>(source);
    }

    struct MemoryStream : boost::iostreams::array_source
    {
        MemoryStream(const std::vector<uint8_t>& buffer) : boost::iostreams::array_source(reinterpret_cast<const char*>(buffer.data()), buffer.size())
        {
        }

        MemoryStream(const uint8_t* buffer, uint32_t offset, uint32_t length) : boost::iostreams::array_source(reinterpret_cast<const char*>(buffer + offset), length)
        {
        }
    };

    inline void CopyStream(Decompressor& source, std::vector<uint8_t>& sink)
    {
        sink.assign(std::istreambuf_iterator<char>{ &source }, {});
    }

    // Handy string functions (boost wrappers)
    bool IsEqual(const std::string& left, const std::string& right, bool ignoreCase = true);
    bool StartsWith(const std::string& value, const std::string& testValue, bool ignoreCase = true);
    bool EndsWith(const std::string& value, const std::string& testValue, bool ignoreCase = true);
    bool Contains(const std::string& value, const std::string& testValue, bool ignoreCase = true);
    std::string ToUpper(const std::string& value);
    std::string ToLower(const std::string& value);
    std::string Trim(const std::string& value);
    std::string TrimRight(const std::string& value);
    std::string TrimLeft(const std::string& value);

    // Empty types
    struct Empty
    {
        static const std::string String;
        static const GSF::TimeSeries::Guid Guid;
        static const GSF::TimeSeries::Object Object;
        static const GSF::TimeSeries::IPAddress IPAddress;
        static const uint8_t* ZeroLengthBytes;
    };
}}

// Setup standard hash code for Guid
namespace std  // NOLINT
{
    template<>
    struct hash<GSF::TimeSeries::Guid>
    {
        size_t operator () (const GSF::TimeSeries::Guid& uid) const
        {
            return boost::hash<GSF::TimeSeries::Guid>()(uid);
        }
    };
}

#endif