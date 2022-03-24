using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Tweening;
using VengineX.UI.Elements.Basic;

namespace VengineX.Screens
{
    public partial class SplashScreenUI : CompositeElement
    {
        /// <summary>
        /// Occurs when the SplashScreens animation is finished.
        /// </summary>
        public event Action? Finished;

        private Sequence _splashSequence;

        public SplashScreenUI(Element? parent) : base(parent)
        {
            // Create logo animation
            float startSize = _logoImage.Size.X;
            float sizeChange = 40;
            float aChange = 1;
            Tween inAnim = new Tween(1.5f, EasingFunction.EaseOutCubic, (t) =>
            {
                _logoImage.Tint = new Vector4(1, 1, 1, t * aChange);
                _logoImage.Width = startSize + t * sizeChange;
                _logoImage.Height = startSize + t * sizeChange;
                Canvas.UpdateLayout();
            });
            Tween outAnim = new Tween(0.5f, EasingFunction.EaseOutCubic, (t) =>
            {
                _logoImage.Tint = new Vector4(1, 1, 1, 1 - (t * aChange));
            });


            _splashSequence = new Sequence(inAnim, Tween.Delay(0.5f), outAnim);
            _splashSequence.Stopped += (_) => Finished?.Invoke();
            _splashSequence.Start();
        }


        public void Skip()
        {
            _splashSequence.Stop();
        }
    }
}
