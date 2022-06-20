namespace DesertImage.Audio
{
    public interface ISoundLayerManager
    {
        ISoundLayer GetLayer(ushort id);

        void Register(ushort id, int simultaneousSoundsCount = 3, float volume = 1f);
    }
}