/**
 * A Vector2 Type Scriptable Object. You can assign it from the inspector to your own components.
 * Author: Javier (Delunado).
 * Last Update: 15/1/2021.
*/

using UnityEngine;

[CreateAssetMenu(menuName = "BasicTypes/Vector2")]
public class Vector2SO : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private Vector2 vector2Value;

    private Vector2 runtimeValue;
    public Vector2 Value { get => runtimeValue; set => runtimeValue = value; }

    public void OnAfterDeserialize()
    {
        runtimeValue = vector2Value;
    }

    public void OnBeforeSerialize()
    {

    }
}
