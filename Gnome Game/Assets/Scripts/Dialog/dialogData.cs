using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog/Conversation")]
public class dialogData : ScriptableObject
{
    [TextArea(3, 5)]
    public string[] lines;
}
