namespace DesertImage.UI
{
    public interface IScreenSlot : IScreen<ushort>
    {
        ushort Id { get; }

        IScreen Screen { get; }

        bool IsIndividualInstance { get; }

        void SetScreenInstance(IScreen instance, bool dontSubscribe = false);

        void Show();
        void Show(IScreenSettings settings);
        void Hide(bool animate);
    }
}