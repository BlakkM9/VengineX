namespace VengineX.Core
{
    /// <summary>
    /// Static class, providing properties for timing.
    /// </summary>
    public static class Time
    {
        /// <summary>
        /// Time elapsed since first update (seconds)
        /// </summary>
        public static double Total { get; private set; } = 0;


        /// <summary>
        /// Sinus of the total time passed.
        /// </summary>
        public static double SinTotal { get; private set; } = 0;


        /// <summary>
        /// Time elapsed since last update (seconds)
        /// </summary>
        public static double DeltaUpdate { get; private set; } = 0;


        /// <summary>
        /// The current UPS (updates per second) based on <see cref="DeltaUpdate"/>
        /// </summary>
        public static double CurrentUPS { get; private set; } = 0;

        /// <summary>
        /// UPS averaged over one second
        /// </summary>
        public static double AverageUPS { get; private set; }

        private static double _currentUPSTime = 0;
        private static double _currentUpdates = 0;


        /// <summary>
        /// Time elapsed since last render (seconds)
        /// </summary>
        public static double DeltaRender { get; private set; } = 0;


        /// <summary>
        /// The current FPS (frames per second) based on <see cref="DeltaRender"/>
        /// </summary>
        public static double CurrentFPS { get; private set; } = 0;


        /// <summary>
        /// FPS averaged over one second.
        /// </summary>
        public static double AverageFPS { get; private set; }

        private static double _currentFPSTime = 0;
        private static double _currentFrames = 0;


        /// <summary>
        /// Updates all the update time properties.<br/>
        /// Do not call this function except in <see cref="Game{T}.Window_UpdateFrame(OpenTK.Windowing.Common.FrameEventArgs)"/>.
        /// </summary>
        internal static void Update(double delta)
        {
            Total += delta;
            SinTotal = Math.Sin(Total);
            DeltaUpdate = delta;
            CurrentUPS = 1.0 / DeltaUpdate;


            _currentUpdates++;
            _currentUPSTime += delta;

            if (_currentUPSTime >= 1)
            {
                AverageUPS = _currentUpdates / _currentUPSTime;

                _currentUpdates = 0;
                _currentUPSTime = 0;
            }
        }


        /// <summary>
        /// Updates all the render time properties.<br/>
        /// Do not call this function except in <see cref="Game{T}.Window_RenderFrame(OpenTK.Windowing.Common.FrameEventArgs)"/>.
        /// </summary>
        internal static void Render(double delta)
        {
            DeltaRender = delta;
            CurrentFPS = 1.0 / DeltaRender;


            _currentFrames++;
            _currentFPSTime += delta;

            if (_currentFPSTime >= 1)
            {
                AverageFPS = _currentFrames / _currentFPSTime;

                _currentFrames = 0;
                _currentFPSTime = 0;
            }

            //_renderFrameCount++;
            //_fpsSamples[_renderFrameCount % FPSSampleCount] = CurrentFPS;
            //AverageFPS = (double)_fpsSamples.Sum() / FPSSampleCount;
        }
    }
}
