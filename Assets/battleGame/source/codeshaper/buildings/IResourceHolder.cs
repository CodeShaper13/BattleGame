namespace codeshaper.buildings {

    /// <summary>
    /// Interface for buildings that hold resources.
    /// </summary>
    public interface IResourceHolder {

        /// <summary>
        /// Returns the number of resources that this building holds.
        /// </summary>
        int getHeldResources();

        /// <summary>
        /// Returns the maximum number of resources that this building can hold.
        /// </summary>
        int getHoldLimit();
    }
}
