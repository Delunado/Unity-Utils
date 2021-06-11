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

    }
}