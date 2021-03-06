﻿//******************************************************************************************************
//  Program.cs - Gbtc
//
//  Copyright © 2013, Grid Protection Alliance.  All Rights Reserved.
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
//  05/02/2013 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

#if !DEBUG
    #define RunAsService
#endif

#if RunAsService
    using System.ServiceProcess;
#else
    using System.Windows.Forms;
#endif

using System;
using System.IO;
using System.Text;

namespace XDAFileWatcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if RunAsService
            ServiceHost host = new ServiceHost();
            RedirectConsoleOutput(host);
            ServiceBase.Run(host);
#else
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DebugHost host = new DebugHost();
            RedirectConsoleOutput(host.ServiceHost);
            Application.Run(host);
#endif
        }

        static void RedirectConsoleOutput(ServiceHost host)
        {
            Console.SetOut(new RemoteConsoleWriter(host));
        }

        private class RemoteConsoleWriter : TextWriter
        {
            private ServiceHost m_host;
            private StringBuilder m_message;

            public RemoteConsoleWriter(ServiceHost host)
            {
                m_host = host;
                m_message = new StringBuilder();
            }

            public override void Write(char value)
            {
                const string ErrorPrefix = "ERROR: ";
                string message;

                m_message.Append(value);
                message = m_message.ToString();

                if (message.EndsWith(Environment.NewLine))
                {
                    if (message.StartsWith(ErrorPrefix))
                        m_host.BroadcastError(message.Substring(ErrorPrefix.Length));
                    else
                        m_host.BroadcastMessage(message);

                    m_message.Clear();
                }
            }

            public override Encoding Encoding
            {
                get
                {
                    return Console.OutputEncoding;
                }
            }
        }
    }
}
