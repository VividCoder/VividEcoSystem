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
using Vivid.Terrain;
public class TerrainObj : NodeScript
{

    public float Width
    {
        get{
            return _Width;
        }
        set{
            _Width = value;
        }
    }
    private float _Width = 100.0f;

    public float Depth{

    get{
     return _Depth;
    }
    set{
        _Depth = value;
        }
    }
    private float _Depth = 100.0f;

    public string HeightMapPath{
        get{
            return _HeightMapPath;
        }
        set{
            _HeightMapPath = value;
        }
    }
    private string _HeightMapPath ="";

    public float YScale{

        get{
            return _YScale;
        }
        set{
            _YScale = value;
        }
    }
    private float _YScale = 1.0f;

    private Terrain3D TerrainNode = null;

    public override void SaveNode(){

        IOHelp.WriteFloat(_Width);
        IOHelp.WriteFloat(_Depth);
        IOHelp.WriteFloat(_YScale);
        IOHelp.WriteString(_HeightMapPath);

    }

    public override void LoadNode(){

        _Width = IOHelp.ReadFloat();
        _Depth = IOHelp.ReadFloat();
        _YScale = IOHelp.ReadFloat();
        _HeightMapPath = IOHelp.ReadString();
        Node3D prev_t = SceneGraph3D.CurScene.FindNode("TerrainObjNode");
        if(prev_t!=null){
            TerrainNode = prev_t as Terrain3D;
        //    SceneGraph3D.CurScene.Remove(prev_t);
        }


    }

    public override void ApplyInEditor(){

        if(TerrainNode==null){
            TerrainNode = SceneGraph3D.CurScene.FindNode("TerrainObjNode") as Terrain3D;
        }
        if(TerrainNode!=null){

            SceneGraph3D.CurScene.Remove(TerrainNode);

        }

        if(_HeightMapPath=="")
        {

            TerrainNode = new Terrain3D(_Width,_Depth,0,32,32);
            TerrainNode.Name = "TerrainObjNode";
            SceneGraph3D.CurScene.Root.Add(TerrainNode);

        }else{

            TerrainNode = new Terrain3D(_Width,_Depth,0,_YScale,32,32,new Texture2D(_HeightMapPath,LoadMethod.Single,false));
            TerrainNode.Name = "TerrainObjNode";
            SceneGraph3D.CurScene.Root.Add(TerrainNode);

        }

    }




}