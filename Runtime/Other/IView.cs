namespace DesertImage.UI
{
    public interface IView
    {
        
    }

    public interface IView<T>
    {
        T Model { get; }

        void Bind(T model);
    }
}