using System.Collections;

namespace DesertImage
{
    public interface IAsyncAwake : IAsync
    {
        IEnumerator OnAsyncAwake();
    }
}