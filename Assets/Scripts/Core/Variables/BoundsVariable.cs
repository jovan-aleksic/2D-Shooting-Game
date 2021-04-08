using UnityEngine;

[CreateAssetMenu(fileName = "BoundsVariable", menuName = "Variable/Bounds")]
public class BoundsVariable : VariableBase<Bounds>
{
    public Vector3 Max => Value.max;

    public Vector3 Min => Value.min;

    #region Overrides of VariableBase<Bounds>

    /// <inheritdoc />
    public override void Add(Bounds amount) =>
        throw new System.NotSupportedException("Adding of Bonds is not supported.");

    /// <inheritdoc />
    public override void Add(VariableBase<Bounds> amount) =>
        throw new System.NotSupportedException("Adding of Bonds is not supported.");

    #endregion
}