namespace Blocktest.Rendering;

public sealed class SpriteSheet {
    public readonly Drawable[,] MappedSprites;
    public readonly Drawable[] OrderedSprites;
    public readonly Texture2D SourceTexture;

    public SpriteSheet(string filename, int frameColumns, int frameRows, int padding = 0) {
        MappedSprites = new Drawable[frameColumns, frameRows];
        OrderedSprites = new Drawable[frameColumns * frameRows];
        SourceTexture = new Drawable(filename).Texture;

        int frameWidth = SourceTexture.Width / frameColumns;
        int frameHeight = SourceTexture.Height / frameRows;

        for (int frameRow = 0; frameRow < frameRows; frameRow++)
        for (int frameColumn = 0; frameColumn < frameColumns; frameColumn++) {
            Rectangle sourceRectangle = new(frameColumn * frameWidth + padding, frameRow * frameHeight + padding,
                frameWidth - padding * 2, frameHeight - padding * 2);
            Drawable newDrawable = new(filename, sourceRectangle);
            MappedSprites[frameColumn, frameRow] = newDrawable;
            OrderedSprites[frameColumn + frameRow * frameColumns] = newDrawable;
        }
    }
}