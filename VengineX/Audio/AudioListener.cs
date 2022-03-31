using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.ECS;

namespace VengineX.Audio
{
    public class AudioListener : Component
    {
        public AudioListener() : base(typeof(AudioListener)) { }
    }
}
