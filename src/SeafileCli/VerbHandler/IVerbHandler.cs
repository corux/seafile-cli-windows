namespace SeafileCli.VerbHandler
{
    /// <summary>
    /// Handler for different command implementation.
    /// </summary>
    public interface IVerbHandler
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        void Run();
    }
}