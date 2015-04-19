using System;

namespace algs4.algs4
{
    public class Stopwatch
    {

        private readonly long _start;

        /// <summary>
        /// Initialize a stopwatch object.
        /// </summary>
        public Stopwatch()
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            _start = Convert.ToInt64((DateTime.UtcNow - epoch).TotalMilliseconds);
        }


        /// <summary>
        /// Returns the elapsed time (in seconds) since this object was created.
        /// </summary>
        /// <returns></returns>
        public double ElapsedTime()
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long now = Convert.ToInt64((DateTime.UtcNow - epoch).TotalMilliseconds);
            return (now - _start) / 1000.0;
        }

    }

}
