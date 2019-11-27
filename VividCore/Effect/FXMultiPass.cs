using Vivid.Scene.Node;

namespace Vivid.Effect
{
    public class FXNoFX : Effect3D
    {
        public FXNoFX() : base("", "data/Shader/vsNoFX.glsl", "data/Shader/fsNoFX.glsl")
        {
        }

        public override void SetPars()
        {
            SetMat("model", Effect.FXG.Local);
            SetMat("view", FXG.Cam.CamWorld);
            SetMat("proj", FXG.Cam.ProjMat);
            SetTex("tC", 0);
        }
    }

    public class FXColorOnly : Effect3D
    {
        public FXColorOnly() : base("", "data/Shader/vsColorOnly.glsl", "data/Shader/fsColorOnly.glsl")
        {
        }

        public override void SetPars()
        {
            SetMat("model", Effect.FXG.Local);
            SetMat("view", FXG.Cam.CamWorld);
            SetMat("proj", FXG.Cam.ProjMat);
            SetVec3("oCol", new OpenTK.Vector3(1, 1, 1));
        }
    }

    public class FXLightMap : Effect3D
    {
        public static float LightMod = 1.0f;
        public static float MatMod = 1.0f;

        public FXLightMap() : base("", "data/Shader/vsLightMap.glsl", "data/Shader/fsLightMap.glsl")
        {
        }

        public override void SetPars()
        {
            //SetMat("MVP", Effect.FXG.Local * FXG.Proj);
            SetMat("model", Effect.FXG.Local);
            SetMat("view", FXG.Cam.CamWorld);
            SetMat("proj", FXG.Cam.ProjMat);

            SetTex("tC", 0);

            //SetTex("tSpec", 2);
        }
    }

    public class FXTerrain : Effect3D
    {
        public static float LightMod = 1.0f;
        public static float MatMod = 1.0f;

        public FXTerrain() : base("", "data/Shader/vsTerrain.glsl", "data/Shader/fsTerrain.glsl")
        {
        }

        public override void SetPars()
        {
            //SetMat("MVP", Effect.FXG.Local * FXG.Proj);
            SetMat("model", Effect.FXG.Local);
            SetMat("view", FXG.Cam.CamWorld);
            SetMat("proj", FXG.Cam.ProjMat);
            SetFloat("lightDepth", Settings.Quality.ShadowDepth);
            SetVec3("viewPos", FXG.Cam.LocalPos);
            SetVec3("lightPos", Light3D.Active.WorldPos);
            SetVec3("lightCol", Light3D.Active.Diff * LightMod);
            SetVec3("lightSpec", Light3D.Active.Spec * LightMod);
            SetFloat("lightRange", Light3D.Active.Range);
            SetFloat("atten", Light3D.Active.Atten);
            SetVec3("ambCE", Light3D.Active.Amb * LightMod);
            SetFloat("matS", Material.Material3D.Active.Shine * MatMod);
            SetVec3("matSpec", Material.Material3D.Active.Spec * MatMod);
            SetFloat("envS", Material.Material3D.Active.envS * MatMod);
            SetVec3("matDiff", Material.Material3D.Active.Diff * MatMod);
            SetTex("tC", 0);
            SetTex("tN", 1);
            //SetTex("tSpec", 2);

            SetTex("tS", 2);
            SetTex("tSpec", 3);
        }
    }

    public class FXMultiPass3D : Effect3D
    {
        public static float LightMod = 1.0f;
        public static float MatMod = 1.0f;

        public FXMultiPass3D() : base("", "data/Shader/vsMP1.glsl", "data/Shader/fsMP1.glsl")
        {
        }

        public override void SetPars()
        {
            //SetMat("MVP", Effect.FXG.Local * FXG.Proj);
            SetMat("model", Effect.FXG.Local);
            SetMat("view", FXG.Cam.CamWorld);
            SetMat("proj", FXG.Cam.ProjMat);
            SetMat("modelTurn", FXG.Local.ClearTranslation());
            SetFloat("lightDepth", Settings.Quality.ShadowDepth);
            SetVec3("viewVec", FXG.Cam.ViewVec);
            SetVec3("viewPos", FXG.Cam.LocalPos);
            SetVec3("lightPos", Light3D.Active.WorldPos);
            SetVec3("lightCol", Light3D.Active.Diff * LightMod);
            SetVec3("lightSpec", Light3D.Active.Spec * LightMod);
            SetFloat("lightRange", Light3D.Active.Range);
            SetFloat("atten", Light3D.Active.Atten);
            SetVec3("ambCE", Light3D.Active.Amb * LightMod);
            SetFloat("matS", Material.Material3D.Active.Shine * MatMod);
            SetVec3("matSpec", Material.Material3D.Active.Spec * MatMod);
            SetFloat("envS", Material.Material3D.Active.envS * MatMod);
            SetVec3("matDiff", Material.Material3D.Active.Diff * MatMod);
            SetVec3("envFactor", Material.Material3D.Active.EnvFactor);
            if (Material.Material3D.Active.ColorMap != null && (Material.Material3D.Active.ColorMap.smallDone || Material.Material3D.Active.ColorMap.bigDone))
            {
                SetBool("texCol", true);
            }
            else
            {
                SetBool("texCol", false);
            }
            if (Material.Material3D.Active.NormalMap != null && (Material.Material3D.Active.NormalMap.smallDone || Material.Material3D.Active.NormalMap.bigDone))
            {
                SetBool("texNorm", true);
            }
            else
            {
                SetBool("texNorm", false);
            }
            if (Material.Material3D.Active.SpecularMap != null && (Material.Material3D.Active.SpecularMap.smallDone || Material.Material3D.Active.SpecularMap.bigDone))
            {
                SetBool("texSpec", true);
            }
            else
            {
                SetBool("texSpec", false);
            }
            if (Material.Material3D.Active.EnvironmentMap != null)
            {
                SetBool("texEnv", true);
            }
            else
            {
                SetBool("texEnv", false);
            }

            SetTex("tC", 0);
            SetTex("tN", 1);
            //SetTex("tSpec", 2);

            SetTex("tS", 2);
            SetTex("tSpec", 3);
            SetTex("tEM", 4);
        }
    }
}