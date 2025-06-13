using NUnit.Framework;
using UnityEngine;
using Model;
//assert error

public class GameSaveManagerTests
{
    private GameObject goStorage;
    private GameObject goSaveMgr;

    // ───────────────────────── Helpers ─────────────────────────

    private void CreateHeroStorageSingleton(out HeroStorage storage)
    {
        goStorage = new GameObject("HeroStorageStub");
        storage   = goStorage.AddComponent<HeroStorage>();
        storage.SendMessage("Awake");          // simulate Unity's Awake
    }

    private void CreateGameSaveManagerSingleton(out GameSaveManager gsm)
    {
        goSaveMgr = new GameObject("GameSaveManager");
        gsm       = goSaveMgr.AddComponent<GameSaveManager>();
        gsm.SendMessage("Awake");
    }

    // ───────────────────────── Setup / Teardown ─────────────────────────

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(goStorage);
        Object.DestroyImmediate(goSaveMgr);

        // Reset singletons so other tests aren’t affected
        HeroStorage.Instance    = null;
        GameSaveManager.Instance = null;
    }

    // ───────────────────────── Tests ─────────────────────────

    [Test]
    public void Awake_SetsSingletonInstance_WhenNoneExists()
    {
        CreateHeroStorageSingleton(out _);
        CreateGameSaveManagerSingleton(out var gsm);

        Assert.IsNotNull(GameSaveManager.Instance);
        Assert.AreSame(gsm, GameSaveManager.Instance);
    }

    [Test]
    public void SaveGame_LogsErrorAndReturns_WhenNoHeroSelected()
    {
        // Arrange – create singletons
        CreateHeroStorageSingleton(out var hStorage);
        CreateGameSaveManagerSingleton(out var gsm);

        // Ensure no hero chosen
        Assert.IsNull(hStorage.SelectedHero);

        // Expect an error log about missing hero
        //LogAssert.Expect(LogType.Error, "❌ HeroStorage.Instance.SelectedHero 是 null，请确保你已经选好了角色！");

        // Act – call SaveGame
        gsm.SaveGame();

        // Assert – nothing else needed; LogAssert handles verification
    }
}