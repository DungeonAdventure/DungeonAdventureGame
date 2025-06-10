using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player's animation state based on movement.
/// This script should be attached to the player sprite or image GameObject.
/// </summary>
public class PlayerImage : MonoBehaviour
{
    // Reference to the Animator component on the player image
    private Animator animator;
    
    // Animator parameter name for the "running" state
    private const string IS_RUNNING = "IsRunning";

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Caches the Animator component.
    /// </summary>
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Called once per frame.
    /// Updates the "IsRunning" animator parameter based on the player's movement state.
    /// </summary>
    private void Update()
    {
        // Query the singleton Player instance to check if the player is running
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
    }
}