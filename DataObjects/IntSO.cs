/**
 * An Int Type Scriptable Object. You can assign it from the inspector to your own components.
 * Author: Javier (Delunado).
 * Last Update: 15/1/2021.
*/

using UnityEngine;

[CreateAssetMenu(menuName = "BasicTypes/Int")]
public class IntSO : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private int intValue;

    private int runtimeValue;
    public int Value { get => runtimeValue; set => runtimeValue = value; }

    public void OnAfterDeserialize()
    {
        runtimeValue = intValue;
    }

    public void OnBeforeSerialize()
    {
        
    }
}