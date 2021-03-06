﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid;
using Vivid.App;
using Vivid.State;
using Vivid.Resonance;
using Vivid.Resonance.Forms;
using Vivid.Tex;
using Vivid.Texture;
using Vivid;
using Vivid.Map.Layer;
using Vivid.Map;
using Vivid.Forms;
using Vivid.Map.Tile;
using Vivid.Map.TileSet;
using MapEditor.States;
using MapEditor.Forms;
namespace MapEditor.States
{
    public class EditMapState : VividState
    {
        public string ContentPath = "C:\\Projects\\VividEcoSystem\\Games\\ScopeNine\\bin\\x64\\Debug\\Corona\\";

        public static void SaveEditState()
        {

            tileBrowse.SaveState();
            mapEdit.SaveState();

        }

        public static void LoadEditState()
        {

            tileBrowse.LoadState();
            mapEdit.LoadState();
            nodeTree.SetMap(GameGlobal.EditMap);///mapEdit.CurMap);

        }
        public static MapEditForm mapEdit;
        public static TileBrowser tileBrowse = null;
        public static NodeGraphView nodeTree;

        public void Rebuild()
        {

        }

        public override void InitState()
        {
           // GameGlobal.ContentPath = ContentPath;
            base.InitState();

            var split1 = new HorizontalSplitterForm().Set(0, 0, AppInfo.W, AppInfo.H) as HorizontalSplitterForm;

            split1.SetSplit(AppInfo.H - AppInfo.H / 4);

            tileBrowse = new TileBrowser().Set(0, 0, split1.BotDock.W, split1.BotDock.H) as TileBrowser; ;

            split1.SetBottom(tileBrowse);

            var split2 = new HorizontalSplitterForm().Set(0, 0, split1.TopDock.W, split1.TopDock.H) as HorizontalSplitterForm;

            var menu = new MenuForm().Set(0, 0, AppInfo.W, 20) as MenuForm;
            UI.Menu = menu;

            var menu_map = menu.AddItem("Map");

            var loadm = menu_map.Menu.AddItem("Load Map");

            loadm.Click = (b) =>
            {

                var req = new RequestFileForm("Select map to load..", GameGlobal.ProjectPath);
                SUI.Top = req;
                req.Selected = (path) =>
                {
                    mapEdit.LoadMap(path);
                    SUI.Top = null;
                };

            };


            var savem = menu_map.Menu.AddItem("Save Map");

            savem.Click = (b) =>
            {

                var req = new RequestFileForm("Select a name to save..", GameGlobal.ProjectPath);
                SUI.Top = req;
                req.Selected = (path) =>
                {

                    mapEdit.SaveMap(path);
                    SUI.Top = null;

                };

            };

            var newm = menu_map.Menu.AddItem("New Map");

            newm.Click = (b) =>
            {

                mapEdit.NewMap();


                nodeTree.SetMap(GameGlobal.EditMap);

                SaveEditState();


            };
            
            var exitm=menu_map.Menu.AddItem("Exit");

            exitm.Click = (b) =>
            {
                SaveEditState();
                Environment.Exit(0);
            };
                
                //SaveEditState()
                //Environment.Exit(1)
                

            var menu_tiles = menu.AddItem("Tiles");

            var tiles_newSet = menu_tiles.Menu.AddItem("New Set");
            var tiles_addSet = menu_tiles.Menu.AddItem("Add set");

            tiles_newSet.Click = (b) =>
            {

                tileBrowse.newSet();
                SaveEditState();

            };

            tiles_addSet.Click = (b) =>
            {

                var reqs = new RequestFileForm("Load tileset .ts..", GameGlobal.ContentPath);
                SUI.Top = reqs;
                
                reqs.Selected = (path) =>
                {
                    var ts = new TileSet(path);
                    ts.Load(path);
                    SUI.Top = null;

                    tileBrowse.AddTileSet(ts);

                    SaveEditState();

                };



            };


            menu.AddItem("Edit");
            //menu.AddItem("")
            split1.SetTop(split2);

            UIForm topForm = new UIForm().Set(0, 0, split2.TopDock.W, split2.TopDock.H);

            //topForm.Add(menu);
            //split2.SetTop(topForm);

            //split1.SetTop(split2);

            split2.SetSplit(50);

            var split3 = new VerticalSplitterForm().Set(0, 0, split2.BotDock.W, split2.BotDock.H) as VerticalSplitterForm;

            var toolRoot = new UIForm().Set(0, 0, split2.TopDock.W, split2.TopDock.H);


            split2.SetTop(toolRoot);

            split2.SetBottom(split3);

            split3.SetSplit(150);

            nodeTree = new NodeGraphView().Set(0, 0, split3.LeftDock.W, split3.LeftDock.H, "Scene Nodes") as NodeGraphView;

            split3.SetLeft(nodeTree);

    

            var nodeBrowse = new NodeBrowser().Set(0, 0, split3.LeftDock.W, split3.LeftDock.H) as NodeBrowser;


            //split3.SetLeft(nodeBrowse);
            var mapSplit = new VerticalSplitterForm().Set(0, 0, split3.RightDock.W, split3.RightDock.H) as VerticalSplitterForm;

            var classIn = new InspectorForm().Set(0, 0, mapSplit.RightDock.W, mapSplit.RightDock.H) as InspectorForm;

            mapEdit = new MapEditForm().Set(0, 0, split3.RightDock.W, split3.RightDock.H) as MapEditForm;

            mapEdit.Selected = (node) =>
            {
                classIn.SetObj(node);
         
         //       if (node == null) return;
                //Console.WriteLine("Selected:" + node.Name);


            };

            nodeTree.SetMap(GameGlobal.EditMap);

            split3.SetRight(mapSplit);

            mapSplit.SetLeft(mapEdit);


            mapSplit.SetRight(classIn);

            mapSplit.SetSplit(split3.RightDock.W - 250);


            SUI = new UI();
            SUI.Root = split1;


            var toolBar = new ToolBarForm().Set(0, 0, toolRoot.W, 25) as ToolBarForm;

            var tb_pick = toolBar.AddItem("Pick");
            var tb_paste = toolBar.AddItem("Paste");
            var tb_rect = toolBar.AddItem("Rect");
            var tb_oval = toolBar.AddItem("Oval");
            var tb_fill = toolBar.AddItem("Fill");
            var tb_smartfill = toolBar.AddItem("Smart-Fill");

            tb_pick.Click = () =>
            {

                mapEdit.Mode = EditMode.Pick;
                mapEdit.Rebuild();

            };

            tb_paste.Click = () =>
            {
                mapEdit.Mode = EditMode.Paste;
                mapEdit.Rebuild();
            };


            tb_fill.Click = () =>
            {
                mapEdit.Mode = EditMode.Fill;
                mapEdit.Rebuild();
            };



            toolRoot.Add(toolBar);

            var mapMode = new Texture2D("content/edit/mapmode.png", LoadMethod.Single, true);
            var tileMode = new Texture2D("content/edit/tiles.png", LoadMethod.Single, true);

            SUI.Dock.AddIcon(mapMode, 64);
            var mode_Tiles = SUI.Dock.AddIcon(tileMode, 64);
            mode_Tiles.Click = ()=>{

                UI.Menu = null;
                Vivid.App.VividApp.PushState(new MapEditor.States.TileSetEditor());

            };

            LoadEditState();
        }

        public override void UpdateState()
        {
            base.UpdateState();
            Texture2D.UpdateLoading();
            SUI.Update();

        }

        public override void DrawState()
        {
            base.DrawState();
            SUI.Render();
        }



    }
}
