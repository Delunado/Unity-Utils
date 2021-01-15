/**
 * A Float Type Scriptable Object. You can assign it from the inspector to your own components.
 * Author: Javier (Delunado).
 * Last Update: 15/1/2021.
*/

using UnityEngine;

[CreateAssetMenu(menuName = "BasicTypes/Float")]
public class FloatSO : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private float floatValue;

    private float runtimeValue;
    public float Value { get => runtimeValue; set => runtimeValue = value; }

    public void OnAfterDeserialize()
    {
        runtimeValue = floatValue;
    }

    public void OnBeforeSerialize()
    {
        
    }
}
