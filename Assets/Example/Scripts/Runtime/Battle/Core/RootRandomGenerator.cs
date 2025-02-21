using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed class RootRandomGenerator
    {
        private readonly IGfRandomGenerator _innerRandomGenerator;

        // ==========================================================
        // parameters
        // ==========================================================

        // ==========================================================
        // public methods
        // ==========================================================

        public RootRandomGenerator(uint rootRandomSeed)
        {
            _innerRandomGenerator = CreateRandomGenerator(rootRandomSeed);

        }
        
        public IGfRandomGenerator CreateGenerator()
        {
            var randomSeed = _innerRandomGenerator.GetRandomUint32();
            return CreateRandomGenerator(randomSeed);
        }

        private IGfRandomGenerator CreateRandomGenerator(uint seed)
        {
            var randomGenerator = new GfXorShift();
            randomGenerator.SetSeed(seed);
            return randomGenerator;
        }
    }
}