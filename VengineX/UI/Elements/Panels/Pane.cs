﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering.Batching;
using VengineX.UI.Elements.Basic;

namespace VengineX.UI.Elements.Panels
{
    /// <summary>
    /// Most basic pane that does not have any specified layout.
    /// </summary>
    public class Pane : Element
    {
        public Pane(Element parent) : base(parent) { }

        public override IEnumerable<UIBatchQuad> EnumerateQuads()
        {
            yield break;
        }
    }
}
