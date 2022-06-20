namespace DesertImage
{
    public interface ISwitchable
    {
        bool IsActive { get; }

        /// <summary>
        /// Turn on
        /// </summary>
        void Activate();

        /// <summary>
        /// Turn off
        /// </summary>
        void Deactivate();
    }
}