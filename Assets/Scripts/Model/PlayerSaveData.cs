using System.Collections.Generic;

/// <summary>
/// Serializable data structure used to store the player's save state,
/// including position and collected pillars.
/// </summary>
[System.Serializable]
public class PlayerSaveData
{
    /// <summary>
    /// The player's position in the world, stored as a float array [x, y, z].
    /// </summary>
    public float[] position;

    /// <summary>
    /// The list of pillar names the player has collected.
    /// </summary>
    public List<string> collectedPillars;
}