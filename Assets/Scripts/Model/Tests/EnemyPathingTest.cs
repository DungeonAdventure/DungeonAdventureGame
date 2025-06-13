using NUnit.Framework;
using UnityEngine;

public class EnemyPathingTests
{
    private GameObject leftEdge, rightEdge, enemyObj, container;
    private EnemyPathing pathing;

    [SetUp]
    public void SetUp()
    {
        container = new GameObject("EnemyContainer");
        leftEdge = new GameObject("LeftEdge");
        rightEdge = new GameObject("RightEdge");
        enemyObj = new GameObject("Enemy");

        leftEdge.transform.position = new Vector3(-5, 0, 0);
        rightEdge.transform.position = new Vector3(5, 0, 0);
        enemyObj.transform.position = new Vector3(0, 0, 0);

        pathing = container.AddComponent<EnemyPathing>();

        // Assign required references
        var anim = container.AddComponent<Animator>();

        typeof(EnemyPathing).GetField("leftEdge",     System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(pathing, leftEdge.transform);
        typeof(EnemyPathing).GetField("rightEdge",    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(pathing, rightEdge.transform);
        typeof(EnemyPathing).GetField("enemy",        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(pathing, enemyObj.transform);
        typeof(EnemyPathing).GetField("anim",         System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(pathing, anim);
        typeof(EnemyPathing).GetField("speed",        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(pathing, 2f);
        typeof(EnemyPathing).GetField("idleDuration", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(pathing, 0.5f);

        pathing.SendMessage("Start");
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(leftEdge);
        Object.DestroyImmediate(rightEdge);
        Object.DestroyImmediate(enemyObj);
        Object.DestroyImmediate(container);
    }

    [Test]
    public void Start_SetsInitialScale()
    {
        Vector3 expected = enemyObj.transform.localScale;
        Vector3 actual = (Vector3) typeof(EnemyPathing)
            .GetField("initScale", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(pathing);

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void MoveInDirection_Left_MovesEnemyLeft()
    {
        Vector3 startPos = enemyObj.transform.position;

        typeof(EnemyPathing).GetMethod("MoveInDirection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(pathing, new object[] { -1 });

        Vector3 endPos = enemyObj.transform.position;

        Assert.Less(endPos.x, startPos.x);
    }

    [Test]
    public void MoveInDirection_Right_MovesEnemyRight()
    {
        Vector3 startPos = enemyObj.transform.position;

        typeof(EnemyPathing).GetMethod("MoveInDirection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(pathing, new object[] { 1 });

        Vector3 endPos = enemyObj.transform.position;

        Assert.Greater(endPos.x, startPos.x);
    }

    [Test]
    public void DirectionChange_SwitchesDirection_AfterIdleDuration()
    {
        // Set movingLeft to true initially
        typeof(EnemyPathing).GetField("movingLeft", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(pathing, true);

        // simulate idle time greater than idleDuration
        typeof(EnemyPathing).GetField("idleTimer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(pathing, 0.6f); // > idleDuration

        typeof(EnemyPathing).GetMethod("DirectionChange", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(pathing, null);

        bool newDirection = (bool) typeof(EnemyPathing).GetField("movingLeft", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.GetValue(pathing);

        Assert.IsFalse(newDirection);
    }
}