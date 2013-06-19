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

using System.IO;
using MongoDB.Bson.IO;

namespace MongoDB.Driver.Core.Protocol
{
    /// <summary>
    /// Represents a GetMore message.
    /// </summary>
    public sealed class GetMoreMessage : RequestMessage
    {
        // private fields
        private readonly long _cursorId;
        private readonly MongoNamespace _namespace;
        private readonly int _numberToReturn;

        // constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GetMoreMessage" /> class.
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        /// <param name="cursorId">The cursor id.</param>
        /// <param name="numberToReturn">The number to return.</param>
        public GetMoreMessage(MongoNamespace @namespace, long cursorId, int numberToReturn)
            : base(OpCode.GetMore)
        {
            _namespace = @namespace;
            _numberToReturn = numberToReturn;
            _cursorId = cursorId;
        }

        // protected methods
        /// <summary>
        /// Writes the body of the message a stream.
        /// </summary>
        /// <param name="streamWriter">The stream.</param>
        protected override void WriteBodyTo(BsonStreamWriter streamWriter)
        {
            streamWriter.WriteBsonInt32(0); // reserved
            streamWriter.WriteBsonCString(_namespace.FullName);
            streamWriter.WriteBsonInt32(_numberToReturn);
            streamWriter.WriteBsonInt64(_cursorId);
        }
    }
}