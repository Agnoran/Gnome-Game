using UnityEngine;


[CreateAssetMenu(menuName = "Progress/Achievements")]
public class achievementItem : ScriptableObject
{
    public string achievementID;
    public string title;
    public string description;
    public string achievementType;
    bool completed;
}
