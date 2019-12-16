using System;
using Vivid;
using Vivid.Audio;
using Vivid.Scripting;
using OpenTK;
using Vivid.Draw;
using Vivid.Texture;
using Vivid.App;
using System.Collections.Generic;
using Vivid.Help;
using Vivid.Scene;
using Vivid.Physics;
public class PhysObj : NodeScript
{

    public float InertiaDrag{
        get{
            return _InertiaDrag;
        }
        set{
            _InertiaDrag = value;
        }
    }
    private float _InertiaDrag = 1.0f;

    public BodyType Body
    {
        get{
            return _Body;
        }
        set{
            _Body = value;
        }
    }
    private BodyType _Body = BodyType.DynamicBox;

    public override void InitNode(){


        Entity3D ent = Node as Entity3D;
        if(ent==null) return;
        switch(_Body)
        {
            case BodyType.Mesh:
                ent.EnablePy(Vivid.Physics.PyType.Mesh);


            break;
            case BodyType.DynamicBox:
                ent.EnablePy(Vivid.Physics.PyType.Box);
            break;
        }

    }

    
    public override void ApplyInEditor(){


        


    }


    public override void SaveNode(){

        IOHelp.WriteInt((int)_Body);
        IOHelp.WriteFloat(_InertiaDrag);

    }

    public override void LoadNode(){

        _Body = (BodyType)IOHelp.ReadInt();
        _InertiaDrag = IOHelp.ReadInt();

    }

}

public enum BodyType{

    Mesh,DynamicBox,DynamicSphere

}