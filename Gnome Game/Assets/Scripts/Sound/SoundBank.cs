using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "SoundBank", menuName = "Scriptable Objects/SoundBank")]
public class SoundBank : ScriptableObject
{
    public List<Sound> sounds;
}
