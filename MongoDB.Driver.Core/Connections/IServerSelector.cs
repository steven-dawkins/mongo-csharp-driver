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

using System.Collections.Generic;

namespace MongoDB.Driver.Core.Connections
{
    /// <summary>
    /// Provides logic for selecting a <see cref="IServer"/> from a <see cref="ICluster"/>.
    /// </summary>
    public interface IServerSelector
    {
        /// <summary>
        /// Gets the description of the server selector.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Selects a server from the provided servers.
        /// </summary>
        /// <param name="servers">The servers.</param>
        /// <returns>The selected server or <c>null</c> if none match.</returns>
        ServerDescription SelectServer(IEnumerable<ServerDescription> servers);
    }
}