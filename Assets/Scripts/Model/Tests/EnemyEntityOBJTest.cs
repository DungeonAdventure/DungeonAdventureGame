using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class EnemyEntityOBJTests
{
    // ───────────────────────── Helpers ─────────────────────────

    /// <summary>
    /// Create an EnemyEntityOBJ on a fresh GameObject and return it.
    /// Private serialized fields are then reachable via reflection.
    /// </summary>
    private EnemyEntityOBJ CreateEnemy()
    {
        var go    = new GameObject("Enemy");
        var enemy = go.AddComponent<EnemyEntityOBJ>();

        // Add the components EnemyEntityOBJ expects (to avoid null refs
        // during Start or when we flip). We don’t need a full Animator
        // controller for unit tests; an empty one is enough.
        go.AddComponent<BoxCollider2D>();
        go.AddComponent<Rigidbody2D>();
        go.AddComponent<Animator>();

        // Initialise Random to deterministic seed so heal/damage ranges
        // are repeat-able across test runs.
        Random.InitState(12345);

        return enemy;
    }

    /// <summary>
    /// Reflection utility for reading/writing privates quickly.
    /// </summary>
    private static T GetPrivate<T>(object obj, string field)
        => (T) obj.GetType()
                  .GetField(field, BindingFlags.Instance | BindingFlags.NonPublic)!
                  .GetValue(obj);

    private static void SetPrivate(object obj, string field, object value)
        => obj.GetType()
              .GetField(field, BindingFlags.Instance | BindingFlags.NonPublic)!
              .SetValue(obj, value);

    // ───────────────────────── Tests ─────────────────────────

    [Test]
    public void TakeDamage_ReducesHealth_WhenHealChanceZero()
    {
        var enemy = CreateEnemy();

        SetPrivate(enemy, "healChance", 0f);          // guarantee no heal
        SetPrivate(enemy, "maxHealth", 100);          // ensure baseline
        SetPrivate(enemy, "currentHealth", 100);

        enemy.TakeDamage(25);

        int hp = GetPrivate<int>(enemy, "currentHealth");
        Assert.That(hp, Is.EqualTo(75));
    }

    [Test]
    public void TakeDamage_Heals_WhenHealChanceOne()
    {
        var enemy = CreateEnemy();

        SetPrivate(enemy, "healChance", 1f);          // guarantee heal
        SetPrivate(enemy, "minHeal", 20);
        SetPrivate(enemy, "maxHeal", 40);
        SetPrivate(enemy, "currentHealth", 50);

        enemy.TakeDamage(10); // 50 −10 = 40, then heal 20-40 ⇒ 60-80

        int hp = GetPrivate<int>(enemy, "currentHealth");
        Assert.That(hp, Is.InRange(60, 80));
    }

    [Test]
    public void TakeDamage_KillsEnemy_WhenHealthDropsToZero()
    {
        var enemy = CreateEnemy();

        SetPrivate(enemy, "healChance", 0f);
        SetPrivate(enemy, "currentHealth", 10);

        enemy.TakeDamage(999); // massive overkill

        bool isDead = GetPrivate<bool>(enemy, "isDead");
        int  hp     = GetPrivate<int>(enemy, "currentHealth");

        Assert.IsTrue(isDead);
        Assert.That(hp, Is.EqualTo(0));
    }

    [Test]
    public void Flip_TogglesFacingDirection_AndReversesScaleX()
    {
        var enemy = CreateEnemy();

        // Make sure initial state is FacingRight = true and scale.x = +1
        SetPrivate(enemy, "FacingRight", true);
        enemy.transform.localScale = Vector3.one;

        // Invoke the private Flip() method via reflection
        enemy.GetType()
             .GetMethod("Flip", BindingFlags.Instance | BindingFlags.NonPublic)!
             .Invoke(enemy, null);

        bool facingRight = GetPrivate<bool>(enemy, "FacingRight");
        float scaleX     = enemy.transform.localScale.x;

        Assert.IsFalse(facingRight, "FacingRight should toggle to false.");
        Assert.That(scaleX, Is.EqualTo(-1f));
    }
}