namespace Shared.Code.Block_System;

/// <summary>
///     Each block is a different type of tile which can be placed. The behaviours specified in each block class subtype
///     will be used for every tile of that type.
/// </summary>
public sealed class BlockShared {
    /// <summary> The block's name. </summary>
    public string BlockName = "Error";

    /// <summary> Whether or not the block supports icon smoothing. </summary>
    public bool BlockSmoothing = false;

    /// <summary>
    ///     The block's unique string ID, generated from the name without spaces and turned to lowercase.
    /// </summary>
    public string BlockUid = "error";

    /// <summary> Whether or not a block can be placed in the background. </summary>
    public bool CanPlaceBackground = true;

    /// <summary> Whether or not a block smooths only with itself </summary>
    /// <remarks> (Use normal 8x8 sprites to prevent overlap) </remarks>
    public bool SmoothSelf = false;

    /* METHODS */

    /// <summary>
    ///     Called whenever a block is placed.
    /// </summary>
    /// <param name="position">The position of the block being placed.</param>
    /// <param name="foreground">Whether the block being placed is in the foreground or not.</param>
    public void OnPlace(Vector2Int position, bool foreground) { }

    /// <summary>
    ///     Called whenever a block is broken.
    /// </summary>
    /// <param name="position">The position of the block being broken.</param>
    /// <param name="foreground">Whether the block being broken is in the foreground or not.</param>
    public void OnBreak(Vector2Int position, bool foreground) { }

    /// <inheritdoc />
    public override string ToString() => BlockName;
}