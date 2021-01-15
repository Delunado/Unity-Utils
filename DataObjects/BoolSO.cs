/**
 * A Bool Type Scriptable Object. You can assign it from the inspector to your own components.
 * Author: Javier (Delunado).
 * Last Update: 15/1/2021.
*/

using UnityEngine;

[CreateAssetMenu(menuName = "Basic Data Types/Bool")]
public class BoolSO : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] bool boolValue;

    [HideInInspector] [SerializeField] private bool runtimeValue;

    public bool Value
    {
        get
        {
            return runtimeValue;
        }
        set
        {
            runtimeValue = value; 
        }
    }

    public void OnAfterDeserialize()
    {
        runtimeValue = boolValue;
    }

    public void OnBeforeSerialize()
    {

    }
}