using System;

namespace codeshaper.entity.unit.task {

    public class TaskIdle : TaskBase<UnitBase> {

        public TaskIdle(UnitBase unit) : base(unit) {
        }

        public override bool preform() {
            throw new NotImplementedException();
        }
    }
}
