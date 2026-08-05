using System.Collections.Generic;

public class Tile
{
    private int m_id;
    private TileSettings m_settings;

    private static List<Tile> s_instances = new();
    private static int s_instanceCount;

    public int id => m_id;
    public TileSettings Settings { get => m_settings; }

    public Tile(TileSettings settings)
    {
        m_settings = settings;

        m_id = s_instanceCount++;
        s_instances.Add(this);
    }

    public Tile GetTileById(int id)
    {
        return s_instances.Find(x => x.m_id == id);
    }
}
