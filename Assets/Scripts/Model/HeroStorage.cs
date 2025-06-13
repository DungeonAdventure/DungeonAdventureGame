using Model;
using UnityEngine;

/// <summary>
/// 用于管理当前选择的英雄，包括跨场景持久化，以及记录英雄类名（HeroClass）。
/// </summary>
public class HeroStorage : MonoBehaviour
{
    public static HeroStorage Instance;

    /// <summary>当前已选英雄实例</summary>
    public Hero SelectedHero { get; private set; }

    /// <summary>保存的英雄类名（如 "Warrior", "Thief", "Priestess"）用于重新加载</summary>
    public string SavedHeroClass { get; internal set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 设置当前英雄，并保存对应的英雄类名（用于存档重建）
    /// </summary>
    public void SetHero(Hero hero)
    {
        SelectedHero = hero;
        SavedHeroClass = hero.GetType().Name;  // 例如：Warrior、Thief、Priestess
        
        Debug.Log($"✅ 已保存英雄：{hero.Name}，类型：{SavedHeroClass}");
    }
    

}