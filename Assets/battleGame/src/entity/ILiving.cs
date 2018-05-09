namespace src.entity {

    /// <summary>
    /// Interface for living objects that contain healthbars.
    /// </summary>
    public interface ILiving {

        int getMaxHealth();

        float getHealthBarHeight();
    }
}
