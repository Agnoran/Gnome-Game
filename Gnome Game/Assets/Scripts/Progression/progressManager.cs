using UnityEngine;
using System.Collections.Generic;

public class progressManager : MonoBehaviour
{

    public static progressManager instance;

    public HashSet<string> collectedItems = new HashSet<string>();
    public HashSet<string> achievementsUnlocked = new HashSet<string>();

    public int playerLevel = 1;
    public int xp = 0;
    public int floor = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UnlockAchievements(achievementItem achievement)
    {
        if (!achievementsUnlocked.Contains(achievement.achievementID))
        {
            achievementsUnlocked.Add(achievement.achievementID);
            Debug.Log("Achievement Unlocked: " + achievement.title);
        }
    }

    public void CollectItem(collectables item)
    {
        if (!collectedItems.Contains(item.collectableID))
        {
            collectedItems.Add(item.collectableID);
            Debug.Log("Collected: " + item.displayName);
        }
    }

    public bool HasCollected(string id)
    {
        return collectedItems.Contains(id);
    }

    public void AddXp(int amount)
    {
        xp += amount;

        if (xp >= GetXpRequired())
        {
            LevelUp();
        }
    }

    int GetXpRequired()
    {
        return playerLevel * 100;
    }

    void LevelUp()
    {
        xp = 0;
        playerLevel++;

        Debug.Log("Level Up! Level: " + playerLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

// must add code to enemies on death to make this work
// ( progressManager.instance.AddXp(25) ) // the 25 is interchangable to whatever amount you want
// We can also use this to progress through dungeon levels using 
// ( if(progressManager.instance.floor >=  [enter number here]) { unlockNextRomm();}
/* 
 enemyKillCount++;

if(enemyKillCount == 1)
{
    ProgressionManager.Instance.UnlockAchievement(firstKillAchievement);
}
*/
// And then we can just add code to anything else we want achievements on
// like Gnomes or secret doors found
/*
 if(ProgressionManager.Instance.collectedItems.Count >= 10)
{
    UnlockAchievement(gnomeHunterAchievement);
}
*/
// and so on.