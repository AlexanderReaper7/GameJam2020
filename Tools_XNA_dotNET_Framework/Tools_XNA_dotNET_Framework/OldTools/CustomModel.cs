using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_XNA
{
    public class CustomModel
    {
        // Variables for each property for a model
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        // Variable for loading in a model with all of its vertices 
        public Model Model { get; set; }

        // Variable for loading special effects from Material
        public Material Material { get; set; }

        // Variable for changing the vertices position in sync
        private Matrix[] modelTransforms;

        // Variable declaring that we will use graphicsDevice
        private GraphicsDevice graphicsDevice;

        // Constructor for loading a model
        public CustomModel(Model Model, Vector3 Position, Vector3 Rotation, Vector3 Scale,
            GraphicsDevice graphicsDevice)
        {
            // Model
            this.Model = Model;

            // Create vertices from model
            modelTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            // Generate Tags
            generateTags();

            // Set models vertices with the codes' vertices
            this.Position = Position;
            this.Rotation = Rotation;
            this.Scale = Scale;

            // Load graphics and material properties
            this.graphicsDevice = graphicsDevice;
            this.Material = new Material();
        }


        // Draw method
        public void Draw(Matrix View, Matrix Projection, Vector3 CameraPosition)
        {
            // Create a new matrix based on the matrices Scale, Rotation and Translation
            Matrix baseWorld = Matrix.CreateScale(Scale) *
                               Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) *
                               Matrix.CreateTranslation(Position);

            // For each model properties (vertices, bones, edges and faces)
            foreach (ModelMesh mesh in Model.Meshes)
            {
                // All the models will be transformed by the baseWorld and then create a local world
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index] * baseWorld;

                // View each model based by the position in the world, camera view and screen conversion
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    Effect effect = meshPart.Effect;

                    if (effect is BasicEffect)
                    {
                        ((BasicEffect)effect).World = localWorld;
                        ((BasicEffect)effect).View = View;
                        ((BasicEffect)effect).Projection = Projection;
                        ((BasicEffect)effect).EnableDefaultLighting();
                    }
                    else
                    {
                        setEffectParameter(effect, "World", localWorld);
                        setEffectParameter(effect, "View", View);
                        setEffectParameter(effect, "Projection", Projection);
                        setEffectParameter(effect, "CameraPosition", CameraPosition);

                        Material.SetEffectParameters(effect);
                    }
                }

                // Draw all the meshes
                mesh.Draw();
            }
        }

        void setEffectParameter(Effect effect, string paramName, object val)
        {
            if (effect.Parameters[paramName] == null) return;

            if (val is Vector3) effect.Parameters[paramName].SetValue((Vector3)val);
            else if (val is bool) effect.Parameters[paramName].SetValue((bool)val);
            else if (val is Matrix) effect.Parameters[paramName].SetValue((Matrix)val);
            else if (val is Texture2D) effect.Parameters[paramName].SetValue((Texture2D)val);
        }

        public void SetModelEffect(Effect effect, bool CopyEffect)
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    Effect toSet = effect;

                    // Copy the effect if necessary
                    if (CopyEffect)
                    {
                        toSet = effect.Clone();
                    }

                    MeshTag tag = ((MeshTag) part.Tag);

                    // If this ModelMeshPart has a texture, set it to the effect
                    if (tag.Texture != null)
                    {
                        setEffectParameter(toSet, "BasicTexture", tag.Texture);
                        setEffectParameter(toSet, "TextureEnabled", true);
                    }
                    else
                    {
                        setEffectParameter(toSet, "TextureEnabled", false);
                    }

                    // Set our remaining parameters to the effect
                    setEffectParameter(toSet, "DiffuseColor", tag.Color);
                    setEffectParameter(toSet, "SpecularPower", tag.SpecularPower);

                    part.Effect = toSet;
                
                }
        }

        private void generateTags()
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    if (part.Effect is BasicEffect)
                    {
                        BasicEffect effect = (BasicEffect) part.Effect;
                        MeshTag tag = new MeshTag(effect.DiffuseColor, effect.Texture, effect.SpecularPower);
                        part.Tag = tag;
                    }
                }
        }

        // Store references to all of the model's current effects
        public void CacheEffects()
        {
            foreach (ModelMesh mesh in Model.Meshes)
            foreach (ModelMeshPart part in mesh.MeshParts)
                ((MeshTag)part.Tag).CachedEffect = part.Effect;
        }

        // Restore the effects referenced by the model's cache
        public void RestoreEffects()
        {
            foreach (ModelMesh mesh in Model.Meshes)
            foreach (ModelMeshPart part in mesh.MeshParts)
                if (((MeshTag)part.Tag).CachedEffect != null)
                {
                    part.Effect = ((MeshTag) part.Tag).CachedEffect;
                }
        }




    }

    public class MeshTag
    {
        public Vector3 Color;
        public Texture2D Texture;
        public float SpecularPower;
        public Effect CachedEffect = null;

        public MeshTag(Vector3 Color, Texture2D Texture, float SpecularPower)
        {
            this.Color = Color;
            this.Texture = Texture;
            this.SpecularPower = SpecularPower;
        }
    }
}
