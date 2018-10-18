namespace codeshaper.entity.unit.task {

    public interface ITask {
        
        /// <summary>
        /// Preforms the ai task.  Return true to keep running this task on the next tick.
        /// </summary>
        bool preform();
    }
}
