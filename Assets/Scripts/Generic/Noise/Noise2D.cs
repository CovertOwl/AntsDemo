namespace Ants.Noise
{
    /// <summary>
    /// Class which generates so smoothish noise
    /// </summary>
    public class Noise2D
    {
        #region Properties
        /// <summary>
        /// Noise generation value - Amplitude scalar over each octave
        /// </summary>
        public float AmplitudePersistence { get; private set; }     //Amplitude scalar over each octave

        /// <summary>
        /// Noise generation value - Initial amplitude
        /// </summary>
        public float Amplitude { get; private set; }

        /// <summary>
        /// Noise generation value - Frequency scalar over each octave
        /// </summary>
        public float FrequencyPersistence { get; private set; }

        /// <summary>
        /// Noise generation value - Initial frequency
        /// </summary>
        public float Frequency { get; private set; }

        /// <summary>
        /// Noise generation value - Iterations
        /// </summary>
        public uint Octaves { get; private set; }

        /// <summary>
        /// Value for property "Seed"
        /// </summary>
        private int seed { get; set; }
        /// <summary>
        /// Seed for the random generation
        /// </summary>
        public int Seed
        {
            get
            {
                return seed;
            }

            private set
            {
                seed = value;
                System.Random rand = new System.Random(seed);

                seedOffset = (float)(rand.NextDouble() * 2000.0);
            }
        }

        /// <summary>
        /// Actual scalar generated from seed
        /// </summary>
        private float seedOffset { get; set; }
        #endregion

        #region Constructores
        /// <summary>
        /// Default constructor
        /// </summary>
        public Noise2D()
        {
            AmplitudePersistence = 0.0f;
            FrequencyPersistence = 0.0f;
            Frequency = 0.0f;
            Amplitude = 0.0f;
            Octaves = 0;
            Seed = 0;
        }

        /// <summary>
        /// Constructor initialising all properties neccessary for noise generation
        /// </summary>
        /// <param name="amplitudePersistence"></param>
        /// <param name="frequencyPersistence"></param>
        /// <param name="frequency"></param>
        /// <param name="amplitude"></param>
        /// <param name="octaves"></param>
        /// <param name="seed"></param>
        public Noise2D(float amplitudePersistence, float frequencyPersistence, float frequency, float amplitude, uint octaves, int seed)
        {
            Set(amplitudePersistence, frequencyPersistence, frequency, amplitude, octaves, seed);
        }
        #endregion

        #region PublicMethods
        /// <summary>
        /// Set function for all properties neccessary for noise generation
        /// </summary>
        /// <param name="amplitudePersistence"></param>
        /// <param name="frequencyPersistence"></param>
        /// <param name="frequency"></param>
        /// <param name="amplitude"></param>
        /// <param name="octaves"></param>
        /// <param name="seed"></param>
        public void Set(float amplitudePersistence, float frequencyPersistence, float frequency, float amplitude, uint octaves, int seed)
        {
            AmplitudePersistence = amplitudePersistence;
            FrequencyPersistence = frequencyPersistence;
            Frequency = frequency;
            Amplitude = amplitude;
            Octaves = octaves;
            Seed = seed;
        }

        /// <summary>
        /// Get the noise height based on 2d position
        /// </summary>
        /// <param name="x">x axis position</param>
        /// <param name="y">y axis position</param>
        /// <returns></returns>
        public float GetHeight(float x, float y)
        {
            return Amplitude * Total(x, y);
        }
        #endregion

        #region PrivateMethods
        private float Total(float x, float y)
        {
            //Variables that vary over the course of the octave iterations
            float total = 0.0f;
            float amplitude = 1.0f;
            float frequency = Frequency;

            for (uint a = 0; a < Octaves; ++a)
            {
                //Increment output by each octaves getvalue output to blend between
                total += this.GetValue(y * frequency + this.seedOffset, x * frequency + this.seedOffset) * amplitude;

                //Change amplitude/frequency between octaves based on their persistence
                amplitude *= this.AmplitudePersistence;
                frequency *= this.FrequencyPersistence;
            }

            return total;
        }

        private float GetValue(float x, float y)
        {
            //Split the inputs into whole numbers and their remainder
            int intX = (int)x;
            int intY = (int)y;
            float remainderX = x - intX;
            float remainderY = y - intY;

            //To get accurate values with float data, first find values for center based on integer values, then in each positive direction based on the remainder
            //so we can interpolate between them based on the fractional remainder. Ends up finding 4 positions (Center, right of center, up of center, right/up of center).
            //Once those 4 are found, the interpolation is done between them based on the x and y remainders above.

            //Required samples around chosen points for smooth noise
            float f01 = Noise(intX - 1, intY - 1);
            float f02 = Noise(intX + 1, intY - 1);
            float f03 = Noise(intX - 1, intY + 1);
            float f04 = Noise(intX + 1, intY + 1);
            float f05 = Noise(intX - 1, intY);
            float f06 = Noise(intX + 1, intY);
            float f07 = Noise(intX, intY - 1);
            float f08 = Noise(intX, intY + 1);
            float f09 = Noise(intX, intY);

            float f12 = Noise(intX + 2, intY - 1);
            float f14 = Noise(intX + 2, intY + 1);
            float f16 = Noise(intX + 2, intY);

            float f23 = Noise(intX - 1, intY + 2);
            float f24 = Noise(intX + 1, intY + 2);
            float f28 = Noise(intX, intY + 2);

            float f34 = Noise(intX + 2, intY + 2);

            //Add noise values together based of x/y position offsets (sampling a 3x3 square from center)
            //1/16 of of diagonals, 1/8 of sides, 1/4 of center
            float x0y0 = 0.0625f * (f01 + f02 + f03 + f04) + 0.125f * (f05 + f06 + f07 + f08) + 0.25f * (f09); //Sample 3x3 from original center position
            float x1y0 = 0.0625f * (f07 + f12 + f08 + f14) + 0.125f * (f09 + f16 + f02 + f04) + 0.25f * (f06); //Sample 3x3 from right of original center position
            float x0y1 = 0.0625f * (f05 + f06 + f23 + f24) + 0.125f * (f03 + f04 + f09 + f28) + 0.25f * (f08); //Sample 3x3 from up of original center position
            float x1y1 = 0.0625f * (f09 + f16 + f28 + f34) + 0.125f * (f08 + f14 + f06 + f24) + 0.25f * (f04); //Sample 3x3 from up/right of original center position

            //Interpolate between the 4 positions found above based on x and y remainders
            float fX1 = Interpolate(x0y0, x1y0, remainderX); //Interpolate from center to right of center using x remainder (moving in x direction)
            float fX2 = Interpolate(x0y1, x1y1, remainderX); //Interpolate from up of center to up/right of center using x remainder (moving in x direction)
            float fFinal = Interpolate(fX1, fX2, remainderY);  //Interpolate between the above 2 values using y remainder (moving in y direction)

	        return fFinal;
        }

        private float Interpolate(float startVal, float endVal, float time)
        {
            float negativeTime = 1.0f - time;
            float negativeTimeSqr = negativeTime * negativeTime;

            float timeSqr = time * time;

            float fFactor1 = 3.0f * (negativeTimeSqr) - 2.0f * (negativeTimeSqr * negativeTime);
            float fFactor2 = 3.0f * timeSqr - 2.0f * (timeSqr * time);

            return startVal * fFactor1 + endVal * fFactor2;
        }

        private float Noise(int x, int y)
        {
	        int n = x + y * 57;
            n = (n << 13) ^ n;
	        int t = (n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff;
	        return (float)(1.0 - t * 0.931322574615478515625e-9);
        }
        #endregion
    }
}
