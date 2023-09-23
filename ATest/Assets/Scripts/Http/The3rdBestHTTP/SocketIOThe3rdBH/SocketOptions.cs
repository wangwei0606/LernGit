using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BestHTTP.SocketIO
{
    public sealed class SocketOptions
    {
        /// <summary>
        /// Whether to reconnect automatically after a disconnect (default true)
        /// </summary>
        public bool Reconnection { get; set; }

        /// <summary>
        /// Number of attempts before giving up (default Int.MaxValue)
        /// </summary>
        public int ReconnectionAttempts { get; set; }

        /// <summary>
        /// How long to initially wait before attempting a new reconnection (default 1000ms).
        /// Affected by +/- RandomizationFactor, for example the default initial delay will be between 500ms to 1500ms.
        /// </summary>
        public TimeSpan ReconnectionDelay { get; set; }

        /// <summary>
        /// Maximum amount of time to wait between reconnections (default 5000ms).
        /// Each attempt increases the reconnection delay along with a randomization as above.
        /// </summary>
        public TimeSpan ReconnectionDelayMax { get; set; }

        /// <summary>
        /// (default 0.5`), [0..1]
        /// </summary>
        public float RandomizationFactor { get { return randomizationFactor; } set { randomizationFactor = Math.Min(1.0f, Math.Max(0.0f, value)); } }
        private float randomizationFactor;

        /// <summary>
        /// Connection timeout before a connect_error and connect_timeout events are emitted (default 20000ms)
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// By setting this false, you have to call SocketManager's Open() whenever you decide it's appropriate.
        /// </summary>
        public bool AutoConnect { get; set; }

        /// <summary>
        /// Constructor, setting the default option values.
        /// </summary>
        public SocketOptions()
        {
            Reconnection = true;
            ReconnectionAttempts = int.MaxValue;
            ReconnectionDelay = TimeSpan.FromMilliseconds(1000);
            ReconnectionDelayMax = TimeSpan.FromMilliseconds(5000);
            RandomizationFactor = 0.5f;
            Timeout = TimeSpan.FromMilliseconds(20000);
            AutoConnect = true;
        }
    }
}