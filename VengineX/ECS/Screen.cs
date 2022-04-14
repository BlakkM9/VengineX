using VengineX.Graphics.Pipelines;

namespace VengineX.ECS
{
    public abstract class Screen
    {
        /// <summary>
        /// The registry (ECS) of this screen.
        /// </summary>
        public Registry Registry { get; }

        /// <summary>
        /// The render pipeline used for the game.
        /// </summary>
        protected RenderPipelineBase _pipeline;

        /// <summary>
        /// Creates a new screen
        /// </summary>
        public Screen(RenderPipelineBase pipeline)
        {
            _pipeline = pipeline;
            Registry = new Registry();
        }


        /// <summary>
        /// Called when the screen is loaded.
        /// </summary>
        public abstract void Load();


        /// <summary>
        /// Called when the screen is updated.<br/>
        /// Updates all the behavior components in this screens registry.
        /// </summary>
        public virtual void Update(double delta)
        {
            // If screen is changed from within a behavior component this will cause the collection to get modified
            // because behavior components are removed from registry when screen is unloaded, therefor we enumerate over a copy
            foreach (BehaviorComponent bhc in Registry.EnumerateComponents<BehaviorComponent>().ToArray())
            {
                if (bhc.Enabled)
                {
                    bhc.Update(delta);
                }
            }
        }


        /// <summary>
        /// Called when the screen frame is rendered.
        /// </summary>
        public virtual void Render(double delta) => _pipeline.Render();


        /// <summary>
        /// Called when the game's window is resized
        /// </summary>
        public abstract void Resize(int width, int height);


        /// <summary>
        /// Called when the screen is unloaded
        /// </summary>
        public virtual void Unload() => Registry.Clear();
    }
}
