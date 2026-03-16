using UnityEngine;

[CreateAssetMenu(menuName = "Dialog/Choices")]
public class dialogCoreNodes : ScriptableObject
{
    [TextArea(3, 5)]
    public string dialogText;

    public dialogChoice[] choices;
}
