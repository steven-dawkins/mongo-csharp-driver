﻿/* Copyright 2010-2013 10gen Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Core.Support;

namespace MongoDB.Driver.Core.Protocol
{
    /// <summary>
    /// Builds a <see cref="BsonBufferedRequestMessage"/> for a query.
    /// </summary>
    public sealed class QueryMessageBuilder : BsonBufferedRequestMessageBuilder
    {
        // private fields
        private readonly QueryFlags _flags;
        private readonly MongoNamespace _namespace;
        private readonly int _numberToReturn;
        private readonly int _numberToSkip;
        private readonly object _query;
        private readonly object _returnFieldSelector;
        private readonly BsonBinaryWriterSettings _writerSettings;

        // constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryMessageBuilder" /> class.
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="numberToSkip">The number to skip.</param>
        /// <param name="numberToReturn">The number to return.</param>
        /// <param name="query">The query.</param>
        /// <param name="returnFieldSelector">The return field selector.</param>
        /// <param name="writerSettings">The writer settings.</param>
        public QueryMessageBuilder(MongoNamespace @namespace, QueryFlags flags, int numberToSkip, int numberToReturn, object query, object returnFieldSelector, BsonBinaryWriterSettings writerSettings)
            : base(OpCode.Query)
        {
            Ensure.IsNotNull("@namespace", @namespace);
            Ensure.IsNotNull("query", query);
            Ensure.IsNotNull("writerSettings", writerSettings);
            // NOTE: returnFieldSelector is allowed to be null as it is not required by the protocol

            _namespace = @namespace;
            _flags = flags;
            _numberToSkip = numberToSkip;
            _numberToReturn = numberToReturn;
            _returnFieldSelector = returnFieldSelector;
            _query = query;
            _writerSettings = writerSettings;
        }

        // protected methods
        /// <summary>
        /// Writes the message to the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        protected override void Write(BsonBuffer buffer)
        {
            buffer.WriteInt32((int)_flags); // flags
            buffer.WriteCString(__encoding, _namespace.FullName); // fullCollectionName
            buffer.WriteInt32(_numberToSkip); // numberToSkip
            buffer.WriteInt32(_numberToReturn); // numberToReturn

            using (var writer = new BsonBinaryWriter(buffer, false, _writerSettings))
            {
                // TODO: pass in serializers for these guys?
                BsonSerializer.Serialize(writer, _query.GetType(), _query, null); // query
                if (_returnFieldSelector != null)
                {
                    BsonSerializer.Serialize(writer, _returnFieldSelector.GetType(), _returnFieldSelector, null); // returnFieldSelector
                }
            }
        }
    }
}