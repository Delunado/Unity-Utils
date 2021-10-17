/**
 * Some General Utilities:
 *  - Get Mouse Position In World.
 *  
 * Author: Javier (Delunado).
 * Last Update: 7/1/2021.
*/

using UnityEngine;

namespace Delu
{
    public static class Utils
    {
        /// <summary>
        /// Returns the mouse's world position.
        /// </summary>
        /// <returns></returns>
        static public Vector2 GetMousePos()
        {
            Camera mainCamera = Camera.main;

            if (mainCamera)
            {
                return Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                Debug.LogError("There isn't any active Main Camera!");
                return Vector2.zero;
            }
        }

        /// <summary>
        /// Converts seconds (X) to minutes and seconds (XX:XX).
        /// </summary>
        /// <param name="totalSeconds">The number of seconds you want to convert into minutes and seconds</param>
        /// <returns>The minutes and seconds as string. Format: (XX:XX)</returns>
        public static string SecondsToMinutesString(float totalSeconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);

            int minutes = timeSpan.Minutes;
            int seconds = timeSpan.Seconds;

            string minutesString = (minutes < 10) ? ("0" + minutes) : minutes.ToString();
            string secondsString = (seconds < 10) ? ("0" + seconds) : seconds.ToString();

            return (minutesString + ":" + secondsString);
        }
    }
}
}