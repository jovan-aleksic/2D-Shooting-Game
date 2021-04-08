#region Description

// 03-05-2021
// James LaFritz
// ----------------------------------------------------------------------------
// Based on
//
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Game Move Direction", menuName = "Variable/Game Move Direction")]
public class GameMoveDirectionVariable : VariableBase<GameMoveDirectionEnum>
{
    #region Overrides of VariableBase<GameMoveDirectionEnum>

    /// <inheritdoc />
    public override void Add(GameMoveDirectionEnum amount) =>
        throw new System.NotSupportedException("Adding of Enum is not supported.");

    /// <inheritdoc />
    public override void Add(VariableBase<GameMoveDirectionEnum> amount) =>
        throw new System.NotSupportedException("Adding of Enum is not supported.");

    #endregion
}