using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Graphics.Rendering.Batching
{

    public interface IBatchRenderer : IDisposable
    {
        public void Begin();
        public void End();
        public void Flush();
    }
}
