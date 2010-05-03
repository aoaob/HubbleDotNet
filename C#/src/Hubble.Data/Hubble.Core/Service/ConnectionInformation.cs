﻿/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Contexts;

namespace Hubble.Core.Service
{
    public class ConnectionInformation 
    {
        private string _DatabaseName;

        internal string DatabaseName
        {
            get
            {
                return _DatabaseName;
            }
        }

        private CommandContent _CurrentCommandContent;

        internal CommandContent CurrentCommandContent
        {
            get
            {
                return _CurrentCommandContent;
            }
        }

        internal void StartCommand()
        {
            _CurrentCommandContent = new CommandContent();
        }

        public ConnectionInformation(string connectionString)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder sqlConnString = null;

            try
            {
                sqlConnString = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
            }
            catch 
            {
                throw new Data.DataException(string.Format("Invalid connection string: {0} , the version of sqlclient is less then 0.8.2.9!",
                    connectionString));
            }

            if (string.IsNullOrEmpty(sqlConnString.InitialCatalog))
            {
                throw new Data.DataException("Database name is empty");
            }

            string databaseName = sqlConnString.InitialCatalog;

            string userId = Hubble.Framework.Security.DesEncryption.Decrypt(Global.Setting.Config.DesKey,
                sqlConnString.UserID);

            string password = Hubble.Framework.Security.DesEncryption.Decrypt(Global.Setting.Config.DesKey,
                sqlConnString.Password);


            if (!Global.Setting.DatabaseExists(databaseName))
            {
                throw new Data.DataException(string.Format("Database name: {0} does not exist",
                    databaseName));
            }

            _DatabaseName = databaseName;

        }
    }
}
