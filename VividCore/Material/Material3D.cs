using Vivid.Texture;

namespace Vivid.Material
{
    public class Material3D
    {
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        public string _Name = "MatDef";

        public Texture2D ColorMap
        {
            get;
            set;
        }

        public Texture2D NormalMap
        { get; set; }

        public Texture2D SpecularMap { get; set; }

        public Texture2D AmbientOcclusionMap
        {
            get;
            set;
        }

        public TextureCube ShadowMap
        {
            get;
            set;
        }

        public TextureCube EnvironmentMap
        {
            get;
            set;
        }

        public OpenTK.Vector3 EnvFactor
        {
            get;
            set;
        }

        public Texture2D ExtraMap
        {
            get;
            set;
        }

        public float envS = 0.1f;
        public OpenTK.Vector3 Diff = new OpenTK.Vector3(1, 1, 1);
        public OpenTK.Vector3 Spec = new OpenTK.Vector3(0.3f, 0.3f, 0.3f);
        public float Shine = 2.0f;
        public static Material3D Active = null;

        public Material3D()
        {
            NormalMap = new Texture.Texture2D("data/tex/normblank.png", Texture.LoadMethod.Single, false);
            ColorMap = new Texture.Texture2D("data/tex/diffblank.png", Texture.LoadMethod.Single, false);
            SpecularMap = new Texture.Texture2D("data/tex/specblank.png", Texture.LoadMethod.Single, false);
            ExtraMap = new Texture2D("data/tex/extraBlank.png", LoadMethod.Single, true);
            EnvFactor = new OpenTK.Vector3(0.2f, 0.2f, 0.2f);
        }

        public void Changed()
        {
        }

        public void Write()
        {
            Help.IOHelp.WriteVec(Diff);
            Help.IOHelp.WriteVec(Spec);
            Help.IOHelp.WriteFloat(Shine);
            Help.IOHelp.WriteBool(ColorMap != null);
            if (ColorMap != null)
            {
                ColorMap.Write();
            }
            Help.IOHelp.WriteBool(NormalMap != null);
            if (NormalMap != null)
            {
                NormalMap.Write();
            }
            Help.IOHelp.WriteBool(SpecularMap != null);
            if (SpecularMap != null)
            {
                SpecularMap.Write();
            }
            Help.IOHelp.WriteBool(ExtraMap != null);
            if (ExtraMap != null)
            {
                ExtraMap.Write();
            }
            Help.IOHelp.WriteBool(EnvironmentMap != null);
            if (EnvironmentMap != null)
            {
                Help.IOHelp.WriteString(EnvironmentMap.Path);
            }
        }

        public void Read()
        {
            // Console.WriteLine ( "Thread:" + System.Threading.Thread.CurrentThread.Name +
            // System.Threading.Thread.CurrentThread );
            Diff = Help.IOHelp.ReadVec3();
            Spec = Help.IOHelp.ReadVec3();
            Shine = Help.IOHelp.ReadFloat();
            if (Help.IOHelp.ReadBool())
            {
                ColorMap = new Texture2D();
                ColorMap.Read();
            }
            if (Help.IOHelp.ReadBool())
            {
                NormalMap = new Texture2D();
                NormalMap.Read();
            }
            if (Help.IOHelp.ReadBool())
            {
                SpecularMap = new Texture2D();
                SpecularMap.Read();
            }
            if (Help.IOHelp.ReadBool())
            {
                ExtraMap = new Texture2D();
                ExtraMap.Read();
            }
            if (Help.IOHelp.ReadBool())
            {
                var path = Help.IOHelp.ReadString();
                EnvironmentMap = new TextureCube(path);
            }
        }

        public void LoadTexs(string folder, string name)
        {
            ColorMap = new Texture2D(folder + "//" + name + "_c.png", LoadMethod.Single, false);
            NormalMap = new Texture2D(folder + "//" + name + "_n.png", LoadMethod.Single, false);
        }

        public virtual void BindLightmap()
        {
            if (ColorMap != null)
            {
                ColorMap.Bind(0);
            }

            Active = this;
        }

        public virtual void ReleaseLightmap()
        {
            if (ColorMap != null)
            {
                ColorMap.Release(0);
            }

            Active = null;
        }

        public virtual void Bind()
        {
            if (ColorMap != null)
            {
                ColorMap.Bind(0);
            }

            if (NormalMap != null)
            {
                NormalMap.Bind(1);
            }

            //if (TSpec != null) TSpec.Bind(2);

            if (ShadowMap != null)
            {
                // ShadowMap.Bind ( 2 );
            }

            if (EnvironmentMap != null)
            {
                EnvironmentMap.Bind(4);
            }

            if (SpecularMap != null)
            {
                SpecularMap.Bind(3);
            }

            if (ExtraMap != null)
            {
                ExtraMap.Bind(5);
            }

            Active = this;
        }

        public virtual void Release()
        {
            if (ColorMap != null)
            {
                ColorMap.Release(0);
            }

            if (NormalMap != null)
            {
                NormalMap.Release(1);
            }
            //if (TSpec != null) TSpec.Release(2);
            if (EnvironmentMap != null)
            {
                EnvironmentMap.Release(4);
            }

            if (SpecularMap != null)
            {
                SpecularMap.Release(3);
            }
            if (ExtraMap != null)
            {
                ExtraMap.Release(5);
            }

            Active = null;
        }
    }
}