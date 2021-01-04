using System;

namespace Utilities.Constants
{
    /// <summary>
    /// Defines the list of layers in the editor, they must have the same values as in the editor.
    /// </summary>
    public enum Layers
    {
        Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2,
        Water = 4,
        UI = 5,

        PlayerBullets = 8,
        Player = 9,
        Enemies = 10,
        EnemyBullets = 11,
        Medals = 12,
        PowerUps = 13,
        PlayerGhostmode = 14
    }

    /// <summary>
    /// Defines the list of layer masks to use for the physics API.
    /// </summary>
    [Flags]
    public enum LayerMasks
    {
        Everything = -1,
        Nothing = 0,
        Default = 1 << Layers.Default,
        TransparentFX = 1 << Layers.TransparentFX,
        IgnoreRaycast = 1 << Layers.IgnoreRaycast,
        Water = 1 << Layers.Water,
        UI = 1 << Layers.UI,

        PlayerBullets = 1 << Layers.PlayerBullets,
        Player = 1 << Layers.Player,
        Enemies = 1 << Layers.Enemies,
        EnemyBullets = 1 << Layers.EnemyBullets,
        Medals = 1 << Layers.Medals,
        PowerUps = 1 << Layers.PowerUps,
        PlayerGhostmode = 1 << Layers.PlayerGhostmode
    }
}