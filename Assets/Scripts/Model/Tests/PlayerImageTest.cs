/*
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerImageTests
{
    private GameObject playerImageObject;
    private Animator animator;
    private PlayerImage playerImageScript;

    private class TestPlayer : Player
    {
        private static bool runningState;
        public static void SetRunningState(bool state) => runningState = state;

        public override bool IsRunning()
        {
            return runningState;
        }
    }

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        GameObject playerGO = new GameObject("Player");
        Player.Instance = playerGO.AddComponent<TestPlayer>();

        playerImageObject = new GameObject("PlayerImage");
        animator = playerImageObject.AddComponent<Animator>();
        playerImageScript = playerImageObject.AddComponent<PlayerImage>();

        var controller = Resources.Load<RuntimeAnimatorController>("TestAnimatorController");
        Assert.IsNotNull(controller, "TestAnimatorController not found in Resources folder.");
        animator.runtimeAnimatorController = controller;

        yield return null; 
    }

    [UnityTest]
    public IEnumerator SetsIsRunningTrue_WhenPlayerIsRunning()
    {
        TestPlayer.SetRunningState(true);
        yield return null; 

        Assert.IsTrue(animator.GetBool("IsRunning"), "Animator should be set to running");
    }

    [UnityTest]
    public IEnumerator SetsIsRunningFalse_WhenPlayerIsNotRunning()
    {
        TestPlayer.SetRunningState(false);
        yield return null;

        Assert.IsFalse(animator.GetBool("IsRunning"), "Animator should be set to not running");
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(playerImageObject);
        Object.DestroyImmediate(Player.Instance.gameObject);
    }
}
*/