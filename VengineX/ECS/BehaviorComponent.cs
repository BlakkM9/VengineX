using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.ECS
{
    public abstract class BehaviorComponent : Component {

        /// <summary>
        /// Called when the frame is updated.
        /// </summary>
        public abstract void Update(double delta);
    }
}
