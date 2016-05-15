//******************************************************************************************************
//  ITableOperations.cs - Gbtc
//
//  Copyright � 2016, Grid Protection Alliance.  All Rights Reserved.
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
//  05/12/2016 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections;
using System.Data;

namespace GSF.Data.Model
{
    /// <summary>
    /// Defines a common interface for implementations of <see cref="TableOperations{T}"/>.
    /// </summary>
    public interface ITableOperations
    {
        /// <summary>
        /// Gets the table name defined for the modeled table, includes any escaping as defined in model.
        /// </summary>
        string TableName
        {
            get;
        }

        /// <summary>
        /// Gets flag that determines if modeled table has a primary key that is an identity field.
        /// </summary>
        bool HasPrimaryKeyIdentityField
        {
            get;
        }

        /// <summary>
        /// Gets or sets delegate used to handle table operation exceptions.
        /// </summary>
        /// <remarks>
        /// When exception handler is provided, table operations will not throw exceptions for database calls, any
        /// encountered exceptions will be passed to handler for processing. Otherwise, exceptions will be thrown
        /// on the call stack.
        /// </remarks>
        Action<Exception> ExceptionHandler
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets flag that determines if field names should be treated as case sensitive. Defaults to <c>false</c>.
        /// </summary>
        /// <remarks>
        /// In cases where modeled table fields have applied <see cref="UseEscapedNameAttribute"/>, this flag will be used
        /// to properly update escaped field names that may be case sensitive.
        /// </remarks>
        bool UseCaseSensitiveFieldNames
        {
            get;
            set;
        }

        /// <summary>
        /// Queries database and returns modeled table records for the specified parameters.
        /// </summary>
        /// <param name="orderByExpression">Field name expression used for sort order, include ASC or DESC as needed - does not include ORDER BY; defaults to primary keys.</param>
        /// <param name="restriction">Record restriction to apply, if any.</param>
        /// <param name="limit">Limit of number of record to return.</param>
        /// <returns>An enumerable of modeled table row instances for queried records.</returns>
        /// <remarks>
        /// If no record <paramref name="restriction"/> or <paramref name="limit"/> is provided, all rows will be returned.
        /// </remarks>
        IEnumerable QueryRecords(string orderByExpression = null, RecordRestriction restriction = null, int limit = -1);

        /// <summary>
        /// Queries database and returns modeled table records for the specified sorting and paging parameters.
        /// </summary>
        /// <param name="sortField">Field name to order-by.</param>
        /// <param name="ascending">Sort ascending flag; set to <c>false</c> for descending.</param>
        /// <param name="page">Page number of records to return (1-based).</param>
        /// <param name="pageSize">Current page size.</param>
        /// <param name="restriction">Record restriction to apply, if any.</param>
        /// <returns>An enumerable of modeled table row instances for queried records.</returns>
        /// <remarks>
        /// This function is used for record paging. Primary keys are cached server-side, typically per user session, to maintain desired per-page sort order.
        /// </remarks>
        IEnumerable QueryRecords(string sortField, bool ascending, int page, int pageSize, RecordRestriction restriction = null);

        /// <summary>
        /// Gets the total record count for the modeled table.
        /// </summary>
        /// <param name="restriction">Record restriction to apply, if any.</param>
        /// <returns>Total record count for the modeled table.</returns>
        int QueryRecordCount(RecordRestriction restriction = null);

        /// <summary>
        /// Creates a new modeled table record queried from the specified <paramref name="primaryKeys"/>.
        /// </summary>
        /// <param name="primaryKeys">Primary keys values of the record to load.</param>
        /// <returns>New modeled table record queried from the specified <paramref name="primaryKeys"/>.</returns>
        object LoadRecord(params object[] primaryKeys);

        /// <summary>
        /// Deletes the specified modeled table <paramref name="record"/> from the database.
        /// </summary>
        /// <param name="record">Record to delete.</param>
        /// <returns>Number of rows affected.</returns>
        int DeleteRecord(object record);

        /// <summary>
        /// Deletes the record referenced by the specified <paramref name="primaryKeys"/>.
        /// </summary>
        /// <param name="primaryKeys">Primary keys values of the record to load.</param>
        /// <returns>Number of rows affected.</returns>
        int DeleteRecord(params object[] primaryKeys);

        /// <summary>
        /// Deletes the records referenced by the specified <paramref name="restriction"/>.
        /// </summary>
        /// <param name="restriction">Record restriction to apply</param>
        /// <returns>Number of rows affected.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="restriction"/> cannot be <c>null</c>.</exception>
        int DeleteRecord(RecordRestriction restriction);

        /// <summary>
        /// Updates the database with the specified modeled table <paramref name="record"/>.
        /// </summary>
        /// <param name="record">Record to update.</param>
        /// <param name="restriction">Record restriction to apply, if any.</param>
        /// <returns>Number of rows affected.</returns>
        /// <remarks>
        /// Record restriction is only used for custom update expressions or in cases where modeled
        /// table has no defined primary keys.
        /// </remarks>
        int UpdateRecord(object record, RecordRestriction restriction = null);

        /// <summary>
        /// Adds the specified modeled table <paramref name="record"/> to the database.
        /// </summary>
        /// <param name="record">Record to add.</param>
        /// <returns>Number of rows affected.</returns>
        int AddNewRecord(object record);

        /// <summary>
        /// Gets the primary key values from the specified <paramref name="record"/>.
        /// </summary>
        /// <param name="record">Record of data to retrieve primary keys from.</param>
        /// <returns>Primary key values from the specified <paramref name="record"/>.</returns>
        object[] GetPrimaryKeys(object record);

        /// <summary>
        /// Gets the primary key values from the specified <paramref name="row"/>.
        /// </summary>
        /// <param name="row"><see cref="DataRow"/> of queried data.</param>
        /// <returns>Primary key values from the specified <paramref name="row"/>.</returns>
        object[] GetPrimaryKeys(DataRow row);

        /// <summary>
        /// Gets the field names for the table, includes any escaping as defined in model.
        /// </summary>
        /// <returns>Array of field names.</returns>
        string[] GetFieldNames();

        /// <summary>
        /// Get the primary key field names for the table, includes any escaping as defined in model.
        /// </summary>
        /// <returns>Array of primary key field names.</returns>
        string[] GetPrimaryKeyFieldNames();

        /// <summary>
        /// Attempts to get the specified <paramref name="attribute"/> for a field.
        /// </summary>
        /// <typeparam name="TAttribute">Type of attribute to attempt to get.</typeparam>
        /// <param name="fieldName">Name of field to use for attribute lookup.</param>
        /// <param name="attribute">Type of attribute to lookup.</param>
        /// <returns><c>true</c> if attribute was found; otherwise, <c>false</c>.</returns>
        bool TryGetFieldAttribute<TAttribute>(string fieldName, out TAttribute attribute) where TAttribute : Attribute;

        /// <summary>
        /// Determines if the specified field has an associated attribute.
        /// </summary>
        /// <typeparam name="TAttribute">Type of attribute to search for.</typeparam>
        /// <param name="fieldName">Name of field to use for attribute lookup.</param>
        /// <returns><c>true</c> if field has attribute; otherwise, <c>false</c>.</returns>
        bool FieldHasAttribute<TAttribute>(string fieldName) where TAttribute : Attribute;

        /// <summary>
        /// Gets the value for the specified field.
        /// </summary>
        /// <param name="record">Modeled table record.</param>
        /// <param name="fieldName">Field name to retrieve.</param>
        /// <returns>Field value or <c>null</c> if field is not found.</returns>
        object GetFieldValue(object record, string fieldName);

        /// <summary>
        /// Gets the <see cref="Type"/> for the specified field.
        /// </summary>
        /// <param name="fieldName">Field name to retrieve.</param>
        /// <returns>Field <see cref="Type"/> or <c>null</c> if field is not found.</returns>
        Type GetFieldType(string fieldName);
    }
}