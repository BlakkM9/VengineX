using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.ECS
{
    public abstract class BehaviorComponent : Component {

        protected BehaviorComponent() : base(typeof(BehaviorComponent)) { }

        /// <summary>
        /// Called when the frame is updated.
        /// </summary>
        public virtual void Update(double delta) { }
    }
}
