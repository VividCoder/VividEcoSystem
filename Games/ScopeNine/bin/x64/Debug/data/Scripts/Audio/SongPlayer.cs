using System;
using Vivid;
using Vivid.Audio;
using Vivid.Scripting;
using Vivid.Help;

public class SongPlayer : NodeScript
{

    public string SongPath{
        get;
        set;
    }


    public override void InitNode(){

        Console.WriteLine("Playing song!");
        Songs.PlaySong(SongPath);

    }

    public override void SaveNode(){
        IOHelp.WriteString(SongPath);
    }

    public override void LoadNode(){
        SongPath = IOHelp.ReadString();
    }


}