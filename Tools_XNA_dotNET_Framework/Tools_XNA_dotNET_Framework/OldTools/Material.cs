﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_XNA.OldTools
{
    public class Material
    {
        public virtual void SetEffectParameters(Effect effect)
        {
        }
    }

    public class LightningMaterial : Material
    {
        public Vector3 AmbientColor { get; set; }
        public Vector3 LightDirection { get; set; }
        public Vector3 LightColor { get; set; }
        public Vector3 SpecularColor { get; set; }

        public LightningMaterial()
        {
            AmbientColor = new Vector3(.1f, .1f, .1f);
            LightDirection = new Vector3(1, 1, 1);
            LightColor = new Vector3(.9f, .9f, .9f);
            SpecularColor = new Vector3(1, 1, 1);
        }

        public override void SetEffectParameters(Effect effect)
        {
            if (effect.Parameters["AmbientColor"] != null)
                effect.Parameters["AmbientColor"].SetValue(AmbientColor);

            if (effect.Parameters["LightDirection"] != null)
                effect.Parameters["LightDirection"].SetValue(LightDirection);

            if (effect.Parameters["LightColor"] != null)
                effect.Parameters["LightColor"].SetValue(LightColor);

            if (effect.Parameters["SpecularColor"] != null)
                effect.Parameters["SpecularColor"].SetValue(SpecularColor);
        }
    }
}
