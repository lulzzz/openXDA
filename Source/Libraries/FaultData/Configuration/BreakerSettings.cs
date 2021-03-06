﻿//******************************************************************************************************
//  BreakerSettings.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  08/02/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using System.Configuration;

namespace FaultData.Configuration
{
    public class BreakerSettings
    {
        #region [ Members ]

        // Fields
        private bool m_applyDCOffsetLogic;
        private double m_dcOffsetWindowSize;
        private double m_lateBreakerThreshold;
        private double m_minCyclesBeforeOpen;
        private double m_minWaitBeforeReclose;
        private double m_openBreakerThreshold;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the flag that determines whether to apply additional logic
        /// to help obtain more accurate breaker timing results in cases where DC
        /// current gradually drains from the line after the breaker is open.
        /// </summary>
        [Setting]
        [DefaultValue(false)]
        public bool ApplyDCOffsetLogic
        {
            get
            {
                return m_applyDCOffsetLogic;
            }
            set
            {
                m_applyDCOffsetLogic = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the window, in cycles,
        /// to use when applying the DC offset logic.
        /// </summary>
        [Setting]
        [DefaultValue(9.0D / 8.0D)]
        public double DCOffsetWindowSize
        {
            get
            {
                return m_dcOffsetWindowSize;
            }
            set
            {
                m_dcOffsetWindowSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of cycles that a breaker
        /// operation's timing can exceed the configured breaker speed.
        /// </summary>
        [Setting]
        [DefaultValue(0.0D)]
        public double LateBreakerThreshold
        {
            get
            {
                return m_lateBreakerThreshold;
            }
            set
            {
                m_lateBreakerThreshold = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum number of cycles that the breaker is expected
        /// to remain closed after receiving the trip coil energized signal.
        /// </summary>
        /// <remarks>
        /// This value helps to prevent phase timing calculations when the current
        /// signal is not large enough to detect the point at which the breaker opened.
        /// </remarks>
        [Setting]
        [DefaultValue(0.0D)]
        public double MinCyclesBeforeOpen
        {
            get
            {
                return m_minCyclesBeforeOpen;
            }
            set
            {
                m_minCyclesBeforeOpen = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum amount of time, in cycles, the system must wait
        /// before automatically reclosing after a breaker operation has occurred.
        /// </summary>
        [Setting]
        [DefaultValue(15.0D)]
        public double MinWaitBeforeReclose
        {
            get
            {
                return m_minWaitBeforeReclose;
            }
            set
            {
                m_minWaitBeforeReclose = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum RMS current, in amps,
        /// at which the breaker can be considered open.
        /// </summary>
        [Setting]
        [DefaultValue(20.0D)]
        public double OpenBreakerThreshold
        {
            get
            {
                return m_openBreakerThreshold;
            }
            set
            {
                m_openBreakerThreshold = value;
            }
        }

        #endregion
    }
}
