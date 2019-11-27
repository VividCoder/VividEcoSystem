using Vivid.Resonance;
using Vivid.Resonance.Forms;
using Vivid.Texture;

using System;

namespace MapEditor.Forms
{
    public class InspectorForm : WindowForm
    {
        public static Texture2D MapSet = null, NoMap = null, Blank;
        public dynamic Obj = null;
        public static InspectorForm Main = null;
        public static Texture2D CamPic = null;

        public static void Inspect(dynamic obj)
        {
            Main.SetObj(obj);
        }

        public InspectorForm()
        {
            Main = this;
            Resized = () =>
            {
                if (Root != null)
                {
                    W = Root.W;
                    H = Root.H;
                    Console.WriteLine("Resized inspector. W:" + W + " H:" + H);
                    body.ViewX = GX;
                    body.ViewY = GY;
                    body.ViewW = W;
                    body.ViewH = H;
                    title.ViewX = GX;
                    title.ViewY = GY;
                    title.ViewW = W;
                    title.ViewH = H;

                    foreach (var f in body.Forms)
                    {
                        f.ViewX = GX + 2;
                        f.ViewY = GY + 25;
                        f.ViewW = W - 5;
                        f.ViewH = H - 35;
                    }
                    if (scroller != null)
                    {
                        scroller.ViewX = GX;
                        scroller.ViewY = GY + 25;
                        scroller.ViewW = W;
                        scroller.ViewH = H - 35;
                        scroller.ScrollBut.ViewX = GX;
                        scroller.ScrollBut.ViewY = GY + 25;
                        scroller.ScrollBut.ViewW = W;
                        scroller.ScrollBut.ViewH = H - 35;
                        scroller.X = W - 10;
                        //scroller.Set(body.W - 10, 1, 10, body.H);


                    }
                };
            };
        }

        public void Update()
        {
            if (Obj == null) return;
            BuildUI(Obj);
        }

        public void SetObj(dynamic obj)
        {
            Obj = obj;
            BuildUI(obj);
            body.ViewX = GX;
            body.ViewY = GY;
            body.ViewW = W;
            body.ViewH = H;
            foreach (var f in body.Forms)
            {
                f.ViewX = GX + 2;
                f.ViewY = GY + 25;
                f.ViewW = W - 5;
                f.ViewH = H - 35;
            }
        }

        private SelectionForm ACamL, AInputL, AAIL;

        private ScrollBarV scroller = null;

        private void BuildUI(dynamic obj)
        {
            if (MapSet == null)
            {
                MapSet = new Texture2D("data/ui/mapset.png", LoadMethod.Single, false);
                NoMap = new Texture2D("data/ui/nomap.png", LoadMethod.Single, false);
                Blank = new Texture2D("data/ui/blank.jpg", LoadMethod.Single, false);
                CamPic = new Texture2D("data/ui/campic1.jpg", LoadMethod.Single, true);
            }
            body.Forms.Clear();

            if (scroller == null)
            {
                scroller = new ScrollBarV().Set(body.W - 10, 1, 10, body.H) as ScrollBarV;
                scroller.ViewX = GX;
                scroller.ViewY = GY + 25;
                scroller.ViewW = W;
                scroller.ViewH = H - 35;
                scroller.ScrollBut.ViewX = GX;
                scroller.ScrollBut.ViewY = GY + 25;
                scroller.ScrollBut.ViewW = W;
                scroller.ScrollBut.ViewH = H - 35;
            }

            scroller.ValueChange = (v) =>
            {
                foreach (var f in body.Forms)
                {
                    if (f == scroller) continue;
                    f.OffY = (int)-scroller.Cur;
                    Console.WriteLine("sc:" + scroller.Cur);
                }
            };

            body.Add(scroller);



            var lab = new LabelForm().Set(5, 10, 120, 25, "Class:" + obj.Name + " Type:" + obj.GetType().Name);

            body.Add(lab);

            object t = obj as object;

            int py = 40;

            if (Obj is Vivid.Scripting.NodeScript)
            {

                var applyBut = new ButtonForm().Set(5, py, 80, 25, "Update") as ButtonForm;
                applyBut.Click = (b) =>
                {

                    Obj.ApplyInEditor();

                };

                body.Add(applyBut);

                py += 35;

            }

            foreach (var prop in t.GetType().GetProperties())
            {
                var prop_lab = new LabelForm().Set(5, py, 80, 25, prop.Name);

                py += 30;

                var name = prop.PropertyType;
                //Console.WriteLine("Name:" + name.FullName);

                Console.WriteLine("Type:" + name.Name);

                var nn = name.Name;

                if (name.Name.Contains("List"))
                {

                    nn = "List";

                }

                if (prop.PropertyType.IsEnum)
                {

                    nn = "Enum";
                }



                bool use = false;
                switch (nn)
                {
                    case "Enum":

                        var p_enum = prop.GetValue(Obj);



                        DropDownListForm list_f = new DropDownListForm().Set(5, py, 160, 25) as DropDownListForm;



                        foreach (var enum_name in prop.PropertyType.GetEnumNames())
                        {
                            list_f.AddItem(enum_name);
                        }

                        list_f.CurrentItem = p_enum.ToString();

                        list_f.SelectedItem = (item) =>
                        {
                            string ai = "";
                            int ee = 0;
                            foreach (var enum_name in prop.PropertyType.GetEnumNames())
                            {
                                if (enum_name == item)
                                {

                                    break;
                                }
                                ee++;
                            }
                            prop.SetValue(Obj, ee);

                        };

                        body.Add(list_f);

                        py += 35;

                        use = true;
                        break;
                    case "List":

                        var p_list = prop.GetValue(Obj);
                        dynamic p_l = p_list;
                        if (p_l != null)
                        {
                            foreach (dynamic litem in p_l)
                            {
                                var l_itemname = new TextBoxForm().Set(5, py, 180, 25, litem.GetName());
                                var l_edit = new ButtonForm().Set(190, py, 60, 25, "Edit") as ButtonForm;
                                var l_remove = new ButtonForm().Set(260, py, 80, 25, "Remove") as ButtonForm;


                                body.Add(l_itemname);
                                body.Add(l_remove);
                                body.Add(l_edit);

                                l_edit.Click = (b) =>
                                {
                                    SetObj(litem);

                                };

                                l_remove.Click = (b) =>
                                {

                                    p_l.Remove(litem);
                                    SetObj(Obj);

                                };


                                py += 35;
                            }
                        }

                        var l_dragzone = new DragZoneForm().Set(5, py, 180, 25) as DragZoneForm;

                        l_dragzone.DraggedObj = (o) =>
                        {

                            if (o.DragObj is ContentEntry)
                            {
                                var ce = o.DragObj as ContentEntry;
                                var le = ce.Load();
                                p_l.Add(le);
                                if (le is Vivid.Scripting.NodeScript)
                                {
                                    le.Node = Obj;
                                }
                                SetObj(Obj);
                            };
                        };
                        body.Add(l_dragzone);

                        py += 35;

                        use = true;

                        break;
                    case "Scripts":



                        var p_al = prop.GetValue(Obj) as System.Collections.Generic.List<Vivid.Scripting.NodeScript>;

                        foreach (var s in p_al)
                        {

                            var scr_name = new TextBoxForm().Set(5, py, 180, 25, s.GetType().Name);
                            var scr_edit = new ButtonForm().Set(190, py, 80, 25, "Edit") as ButtonForm;
                            py += 35;

                            body.Add(scr_name);
                            body.Add(scr_edit);

                            scr_edit.Click = (b) =>
                            {

                                SetObj(s);

                            };
                        }

                        use = true;
                        break;
                    case "ClassLin4":



                        break;

                    case "TextureCube":

                        var texc = prop.GetValue(Obj) as TextureCube;

                        var tc_name = new TextBoxForm().Set(5, py, 120, 25, "EnvMap");

                        body.Add(tc_name);

                        py += 30;

                        Texture2D pi = null;

                        if (texc == null)
                        {
                            pi = NoMap;
                        }
                        else
                        {
                            pi = MapSet;
                        }
                        //     pi = Blank;

                        var texc_p = new ImageForm().Set(5, py, 128, 128, "").SetImage(pi) as ImageForm;

                        body.Add(texc_p);

                        texc_p.CanDrop = true;
                        py += 136;

                        texc_p.DraggedObj = (obj2) =>
                        {
                            var drago = obj2 as DragObject;
                            Console.WriteLine("Prev!");
                            if (drago.DragObj is ContentEntry)
                            {
                                var ce = drago.DragObj as ContentEntry;
                                var ntex = new TextureCube(ce.FullPath);
                                //Console.WriteLine("Setting Content");
                                try
                                {
                                    prop.SetValue(Obj, ntex);
                                    SetObj(Obj);
                                }
                                catch
                                {
                                }
                            }
                        };

                        use = true;

                        break;

                    case "Tex2D":
                        var tex = prop.GetValue(Obj) as Vivid.Tex.Tex2D;

                        if (tex == null)
                        {

                            prop.SetValue(Obj, WhiteTex2D);
                            tex = WhiteTex2D;
                        }

                        var t_name = new TextBoxForm().Set(5, py, 120, 25, tex.Name);

                        body.Add(t_name);

                        py += 30;

                        var m_prev = new ImageForm().Set(5, py, 128, 128, "").SetImage(tex.ToTexture2D()) as ImageForm;

                        var set_image = new ButtonForm().Set(140, py, 80, 25, "Set Image") as ButtonForm;

                        body.Add(set_image);

                        set_image.Click = (b) =>
                        {

                            var sir = new RequestFileForm("Select image..",GameGlobal.ContentPath);

                            UI.CurUI.Top = sir;

                            sir.Selected = (path) =>
                            {

                                UI.CurUI.Top = null;
                                prop.SetValue(Obj, new Vivid.Tex.Tex2D(path,true));
                                SetObj(Obj);


                            };

                        };

                        py += 136;

                        //var nm_name = new TextBoxForm().Set(5,py,120,25,N)

                        m_prev.DraggedObj = (o) =>
                        {
                            var drago = o as DragObject;
                            Console.WriteLine("Prev!");
                            if (drago.DragObj is ContentEntry)
                            {
                                var ce = drago.DragObj as ContentEntry;
                                var ntex = new Texture2D(ce.FullPath, LoadMethod.Single, true);
                                //Console.WriteLine("Setting Content");
                                try
                                {
                                    prop.SetValue(Obj, ntex.ToTex2D());
                                    SetObj(Obj);
                                }
                                catch
                                {
                                }
                            }
                        };

                        m_prev.CanDrop = true;

                        body.Add(m_prev);

                        use = true;

                        break;
                    case "Texture2D":

                        var tex2 = prop.GetValue(Obj) as Texture2D;

                        if (tex2 == null)
                        {

                            prop.SetValue(Obj, WhiteTex);
                            tex2 = WhiteTex;
                        }

                        var t_name2 = new TextBoxForm().Set(5, py, 120, 25, tex2.Name);

                        body.Add(t_name2);

                        py += 30;

                        var m_prev2 = new ImageForm().Set(5, py, 128, 128, "").SetImage(tex2) as ImageForm;


                        py += 136;

                        //var nm_name = new TextBoxForm().Set(5,py,120,25,N)

                        m_prev2.DraggedObj = (o) =>
                        {
                            var drago = o as DragObject;
                            Console.WriteLine("Prev!");
                            if (drago.DragObj is ContentEntry)
                            {
                                var ce = drago.DragObj as ContentEntry;
                                var ntex = new Texture2D(ce.FullPath, LoadMethod.Single, true);
                                //Console.WriteLine("Setting Content");
                                try
                                {
                                    prop.SetValue(Obj, ntex);
                                    SetObj(Obj);
                                }
                                catch
                                {
                                }
                            }
                        };

                        m_prev2.CanDrop = true;

                        body.Add(m_prev2);

                        use = true;

                        break;

                    case "ScriptList":

                        var sl = prop.GetValue(Obj) as Vivid.Script.ScriptList;

                        use = true;

                        int num = 0;
                        foreach (var ns in sl.Scripts)
                        {
                            var scr_name_lab = new TextBoxForm().Set(5, py, 240, 25, "Script" + num + ":" + ns.Name);
                            body.Add(scr_name_lab);
                            num++;
                            py += 30;
                        }

                        break;

                    case "Material3D":

                        var mat = prop.GetValue(Obj) as Vivid.Material.Material3D;

                        var m_name = new TextBoxForm().Set(5, py, 120, 25, mat.Name);

                        var m_edit = new ButtonForm().Set(130, py, 60, 25, "Edit");

                        m_edit.Click = (b) =>
                        {
                            SetObj(mat);
                        };

                        body.Add(m_name);
                        body.Add(m_edit);
                        py += 30;
                        use = true;

                        break;

                    case "string":
                    case "String":

                        var str = prop.GetValue(Obj) as string;
                        if (str == null)
                        {
                            str = "";
                        }
                        var str_box = new TextBoxForm().Set(5, py, 220, 25, str);
                        Console.WriteLine("TB==" + str + "!");
                        if (prop.Name.Contains("Path"))
                        {

                            var path_sel = new ButtonForm().Set(230, py, 60, 25, "Select");
                            body.Add(path_sel);


                            path_sel.Click = (b) =>
                            {
                                var path_r = new RequestFileForm("Select file...");
                                UI.CurUI.Top = path_r;
                                path_r.Selected = (path) =>
                                {

                                    prop.SetValue(Obj, path);
                                    UI.CurUI.Top = null;
                                    SetObj(Obj);


                                };

                            };

                        }

                        body.Add(str_box);
                        use = true;
                        py += 30;

                        break;

                    case "ClassLis4":
                        use = true;


                        break;
                    case "Int32":
                        use = true;
                        //while (true)
                        //{
                        //}

                        var ival = prop.GetValue(Obj);

                        var i_lab = new LabelForm().Set(5, py, 25, 25, "Val");
                        var i_box = new TextBoxForm().Set(50, py, 75, 25, ival.ToString()) as TextBoxForm;

                        i_box.Enter = (n44) =>
                        {
                            try
                            {
                                ival = int.Parse(n44);
                                prop.SetValue(Obj, ival);
                            }
                            catch
                            {
                                ival = 0;
                            }

                            try
                            {
                                Obj.Changed();
                            }
                            catch
                            {

                            }
                        };

                        body.Add(i_lab, i_box);

                        py += 30;

                        break;
                    case "Single":

                        use = true;
                        //while (true)
                        //{
                        //}

                        var fval = prop.GetValue(Obj);

                        var v_lab = new LabelForm().Set(5, py, 25, 25, "Val");
                        var v_box = new TextBoxForm().Set(50, py, 75, 25, fval.ToString()) as TextBoxForm;

                        v_box.Enter = (n44) =>
                        {
                            try
                            {
                                fval = float.Parse(n44);
                                prop.SetValue(Obj, fval);
                            }
                            catch
                            {
                                fval = 0;
                            }

                            try
                            {
                                Obj.Changed();
                            }
                            catch
                            {

                            }
                        };

                        body.Add(v_lab, v_box);

                        py += 30;

                        //  v_box.Enter = (sval) =>

                        //       fval = float.Parse(sval);/

                        break;


                    case "Vector3":
                        use = true;

                        var vec3 = prop.GetValue(Obj);

                        var x_lab = new LabelForm().Set(5, py, 25, 25, "X") as LabelForm;
                        var y_lab = new LabelForm().Set(110, py, 25, 25, "Y");
                        var z_lab = new LabelForm().Set(215, py, 25, 25, "Z");

                        var x_box = new TextBoxForm().Set(30, py, 75, 25, vec3.X.ToString()) as TextBoxForm;
                        var y_box = new TextBoxForm().Set(135, py, 75, 25, vec3.Y.ToString()) as TextBoxForm;
                        var z_box = new TextBoxForm().Set(240, py, 75, 25, vec3.Z.ToString()) as TextBoxForm;

                        x_box.Enter = (val) =>
                        {
                            try
                            {
                                vec3.X = float.Parse(val);
                            }
                            catch
                            {
                            }
                            prop.SetValue(Obj, vec3);
                            Obj.Changed();
                        };

                        y_box.Enter = (val) =>
                        {
                            try
                            {
                                vec3.Y = float.Parse(val);
                            }
                            catch
                            {
                            }
                            prop.SetValue(Obj, vec3);
                            Obj.Changed();
                        };

                        z_box.Enter = (val) =>
                        {
                            try
                            {
                                vec3.Z = float.Parse(val);
                            }
                            catch
                            {
                            }
                            prop.SetValue(Obj, vec3);
                            Obj.Changed();
                        };

                        body.Add(x_lab, y_lab, z_lab);
                        body.Add(x_box, y_box, z_box);

                        py += 30;

                        break;
                }
                if (use)
                {
                    var prop_type = new LabelForm().Set(5, py, 80, 25, name.Name);

                    body.Add(prop_lab);
                    //  body.Add(prop_type);

                    //py += 30;
                }
                else
                {
                    py -= 30;
                }
            }
            scroller.SetMax(py);
        }
    }
}