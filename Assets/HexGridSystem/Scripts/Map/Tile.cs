using UnityEngine.Rendering.Universal;

public class Tile
{
    private int xPos;
    private int yPos;
    private TileSettings settings;

    public int x { get => xPos; }
    public int y { get => yPos; }
    public TileSettings Settings { get => settings; }

    public Tile(int xPos, int yPos, TileSettings settings)
    {
        this.xPos = xPos;
        this.yPos = yPos;
        this.settings = settings;
    }
}
