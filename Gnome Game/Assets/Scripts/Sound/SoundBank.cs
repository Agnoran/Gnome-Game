using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Sound Bank", menuName = "Scriptable Objects/Sound Bank")]
public class SoundBank : ScriptableObject
{
    public List<Sound> sounds;
}
