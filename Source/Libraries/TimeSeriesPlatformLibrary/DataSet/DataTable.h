//******************************************************************************************************
//  DataTable.h - Gbtc
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
//  11/03/2018 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

#ifndef __DATA_TABLE_H
#define __DATA_TABLE_H

#include <unordered_map>
#include <vector>

#include "../Common/CommonTypes.h"
#include "DataColumn.h"
#include "DataRow.h"

namespace GSF {
namespace DataSet
{

class DataSet;
typedef TimeSeries::SharedPtr<DataSet> DataSetPtr;

class DataTable;
typedef TimeSeries::SharedPtr<DataTable> DataTablePtr;

enum class DataType;

class DataTable : public TimeSeries::EnableSharedThisPtr<DataTable> // NOLINT
{
private:
    DataSetPtr m_parent;
    std::string m_name;
    std::unordered_map<std::string, int32_t> m_columnIndexes;
    std::vector<DataColumnPtr> m_columns;
    std::vector<DataRowPtr> m_rows;

public:
    DataTable(const DataSetPtr& parent, std::string name);
    ~DataTable();

    const DataSetPtr& Parent() const;

    const std::string& Name() const;

    void AddColumn(DataColumnPtr column);

    const DataColumnPtr& Column(const std::string& columnName) const;

    const DataColumnPtr& Column(int32_t index) const;

    const DataColumnPtr& operator[](const std::string& columnName) const;

    const DataColumnPtr& operator[](int32_t index) const;

    DataColumnPtr CreateColumn(const std::string& name, DataType type);

    int32_t ColumnCount() const;

    const DataRowPtr& Row(int32_t index);

    void AddRow(DataRowPtr row);

    DataRowPtr CreateRow();

    int32_t RowCount() const;

    static const DataTablePtr NullPtr;
};

typedef TimeSeries::SharedPtr<DataTable> DataTablePtr;

}}

#endif