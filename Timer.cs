/**
 * An easy to use Timer. Usable with Update & Fixed Update
 * Author: Javier (Delunado).
 * 
 * Last Update: 11/6/2021.
 * - Fixed a little typo
*/

using UnityEngine;

namespace Delu
{
    public class Timer
    {
        public enum TimerType
        {
            TIMER_UPDATE,
            TIMER_FIXED_UPDATE
        }

        private readonly TimerType timerType;

        private float timeToWait;
        private float timer;

        public bool Started { get; private set; }
        public bool Finished { get; private set; }

        /// <summary>
        /// Creates a Timer with the indicated time and type.
        /// </summary>
        /// <param name="timeToWait">The time to wait for the timer</param>
        public Timer(float timeToWait, TimerType timerType = TimerType.TIMER_UPDATE)
        {
            this.timerType = timerType;
            this.timeToWait = timeToWait;
            timer = 0f;
            Started = false;
            Finished = false;
        }

        public void Update()
        {
            if (Started)
            {
                if (timer < timeToWait)
                {
                    if (timerType == TimerType.TIMER_UPDATE)
                        timer += Time.deltaTime;
                    else
                        timer += Time.fixedDeltaTime;
                }
                else
                {
                    Finished = true;
                }
            }
        }

        /// <summary>
        /// Start the timer
        /// </summary>
        public void Start()
        {
            Started = true;
        }

        /// <summary>
        /// Stop the timer
        /// </summary>
        public void Stop()
        {
            Started = false;
        }

        /// <summary>
        /// Reset the timer. You can use it again with Start.
        /// </summary>
        public void Reset()
        {
            timer = 0f;
            Finished = false;
        }

        /// <summary>
        /// Reset the timer, setting a new Time to Wait. You can use it again with Start.
        /// </summary>
        /// <param name="newTimeToWait"></param>
        public void Reset(float newTimeToWait)
        {
            timeToWait = newTimeToWait;
            Reset();
        }

        /// <summary>
        /// Restart the timer. Start again from the beginning. Same as using Reset + Start.
        /// </summary>
        public void Restart()
        {
            timer = 0f;
            Finished = false;
            Started = true;
        }

        /// <summary>
        /// Restart the timer, setting a new Time to Wait. Start again from the beginning. Same as using Reset + Start.
        /// </summary>
        /// <param name="newTimeToWait"></param>
        public void Restart(float newTimeToWait)
        {
            timeToWait = newTimeToWait;
            timer = 0f;
            Finished = false;
            Started = true;
        }

        public float RemainingTimeValue()
        {
            return timeToWait - timer;
        }

        public float ElapsedTimeValue()
        {
            return timeToWait - (RemainingTimeValue());
        }

    }
}
