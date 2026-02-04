namespace MonoGameLibrary.Graphics;

public class Tileset
{
  private readonly TextureRegion[] _tiles;

  // get the width of each tile in this tileset
  public int TileWidth { get; }

  // get the height of each tile in this tileset
  public int TileHeight { get; }

  public int Columns { get; }

  public int Rows { get; }

  public int Count { get; }

  // constructor
  public Tileset(TextureRegion textureRegion, int tileWidth, int tileHeight)
  {
    TileWidth = tileWidth;
    TileHeight = tileHeight;
    Columns = textureRegion.Width / tileWidth;
    Rows = textureRegion.Height / tileHeight;
    Count = Columns * Rows;

    // Create the texture regions that make up each individual tile
    _tiles = new TextureRegion[Count];

    for (int i = 0; i < Count; i++)
    {
      int x = i % Columns * tileWidth;
      int y = i / Columns * tileHeight;
      _tiles[i] = new TextureRegion(textureRegion.Texture, textureRegion.SourceRectangle.X + x, textureRegion.SourceRectangle.Y + y, tileWidth, tileHeight);
    }
  }

  public TextureRegion GetTile(int index) => _tiles[index];

  public TextureRegion GetTile(int column, int row)
  {
    int index = row * Columns + column;
    return GetTile(index);
  }
}
