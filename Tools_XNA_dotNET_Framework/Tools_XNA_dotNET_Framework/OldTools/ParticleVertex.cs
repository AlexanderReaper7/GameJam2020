using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_XNA
{
    /*
     * A struct is a value type which means that all information within the struct is allocated at the struct. 
     * Other structs/classes can copy information from a struct, but cannot edit it. This has a positive effect on
     * memory, however the code need to refer when to delete the garbage data.
     * 
     * How I would explain with a comparison, very simplified, classes are servers that is bonded as a web/network, everything
     * refers to each other and values changes all over the network. A struct is more of an database of information or an process that should not change until
     * the user wants/code needs to do so.
     * 
     * Structs are used when the code's instances are small and commonly short-lived or are commonly embedded in other objects
     * 
     * Information taken at: https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/choosing-between-class-and-struct
     */
    struct ParticleVertex : IVertexType
    {
        private Vector3 startPosition;
        private Vector2 uv;
        private Vector3 direction;
        private float speed;
        private float startTime;


        // Starting position of that particle (t = 0)
        public Vector3 StartPosition
        {
            get { return startPosition; }
            set { startPosition = value; }
        }

        // UV coordinate, used for texturing and to offset vertex in shader
        public Vector2 UV
        {
            get { return uv; }
            set { uv = value; }
        }

        // Movement direction of the particle
        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        // Speed of the particle in units/second
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        // The time since the particle system was created that this particle came to use
        public float StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public ParticleVertex(Vector3 StartPosition, Vector2 UV, Vector3 Direction, float Speed, float StartTime)
        {
            this.startPosition = StartPosition;
            this.uv = UV;
            this.direction = Direction;
            this.speed = Speed;
            this.startTime = StartTime;
        }

        // Vertex declaration
        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),              // Start position
            new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),    // UV coordinates
            new VertexElement(20, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 1),    // Movement direction
            new VertexElement(32, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 2),     // Movement speed   
            new VertexElement(36, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 3)      // Start time
            );

        VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }
    }
}
