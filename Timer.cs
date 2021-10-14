/**
 * An easy to use Timer. Usable with Update & Fixed Update.
 * Author: Javier (Delunado).
 * Last Update: 14/10/2021.
*/

using System;
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
        public float TimeToWait => timeToWait;

        private float timer;

        public bool Started { get; private set; }
        public bool Finished { get; private set; }
        public bool AutoRestart { get; private set; }
        public bool UnscaledTime { get; private set; }

        public delegate void TimerDelegate();

        private event TimerDelegate OnFinish;

        /// <summary>
        /// Creates a Timer which the indicated time and type.
        /// </summary>
        /// <param name="timeToWait">The time to wait for the timer</param>
        public Timer(float timeToWait, TimerType timerType = TimerType.TIMER_UPDATE)
        {
            this.timerType = timerType;
            this.timeToWait = timeToWait;
            timer = 0f;
            Started = false;
            Finished = false;
            AutoRestart = false;
            UnscaledTime = false;
        }

        public Timer(TimerType timerType = TimerType.TIMER_UPDATE)
        {
            this.timerType = timerType;
            this.timeToWait = 0.0f;
            timer = 0f;
            Started = false;
            Finished = false;
            AutoRestart = false;
            UnscaledTime = false;
        }

        public void Update()
        {
            if (!Started) return;

            if (Finished && AutoRestart)
            {
                Restart();
            }

            if (timer < timeToWait)
            {
                if (timerType == TimerType.TIMER_UPDATE)
                    timer += UnscaledTime ? Time.unscaledTime : Time.deltaTime;
                else
                    timer += UnscaledTime ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime;
            }
            else
            {
                if (!Finished)
                {
                    OnFinish?.Invoke();
                }

                Finished = true;
            }
        }

        /// <summary>
        /// Start the timer. Sets Started to true.
        /// </summary>
        public Timer Start()
        {
            Started = true;

            return this;
        }

        public Timer AddOnFinishCallback(TimerDelegate function)
        {
            OnFinish += function;
            return this;
        }

        /// <summary>
        /// Stop the timer. Sets Started to false.
        /// </summary>
        public Timer Stop()
        {
            Started = false;

            return this;
        }

        /// <summary>
        /// Reset the timer. Sets Finished to false. You can use it again with Start.
        /// </summary>
        public Timer Reset()
        {
            timer = 0f;
            Finished = false;

            return this;
        }

        /// <summary>
        /// Reset the timer, setting a new Time to Wait. You can use it again with Start.
        /// </summary>
        /// <param name="newTimeToWait"></param>
        public Timer Reset(float newTimeToWait)
        {
            timeToWait = newTimeToWait;
            return Reset();
        }

        /// <summary>
        /// Restart the timer. Start again from the beginning. Same as using Reset + Start.
        /// </summary>
        public Timer Restart()
        {
            timer = 0f;
            Finished = false;
            Started = true;

            return this;
        }

        /// <summary>
        /// Restart the timer, setting a new Time to Wait. Start again from the beginning. Same as using Reset + Start.
        /// </summary>
        /// <param name="newTimeToWait"></param>
        public Timer Restart(float newTimeToWait)
        {
            timeToWait = newTimeToWait;
            timer = 0f;
            Finished = false;
            Started = true;

            return this;
        }

        public float RemainingTimeValue()
        {
            return timeToWait - timer;
        }

        public float ElapsedTimeValue()
        {
            return timeToWait - (RemainingTimeValue());
        }

        public Timer SetAutoRestart(bool value)
        {
            AutoRestart = value;

            return this;
        }

        public Timer SetUnscaledTime(bool value)
        {
            UnscaledTime = value;

            return this;
        }
    }
}