using System;
using System.Collections.Generic;
using System.IO;
using Vivid.App;
using Vivid.Texture;

namespace Vivid.Resonance.Forms
{
    public delegate void SelectFile(string pth);



    public class RequestFileForm : WindowForm
    {
        public SelectFile Selected = null;
        public ListForm Files;
        public static Texture2D FolderPic = null;
        public static Texture2D FilePic = null;
        public static Texture2D BackPic = null;
        public ButtonForm BackFolder;
        public bool Folder = false;
        public TextBoxForm DirBox, FileBox;
        public static string DefDir = "";

        public RequestFileForm(string title = "", string defdir = "", bool folder = false)
        {
            Folder = folder;
            if (FolderPic == null)
            {
                FolderPic = new Texture2D("data/UI/folder1.png", LoadMethod.Single, true);
                FilePic = new Texture2D("data/UI/file1.png", LoadMethod.Single, true);
                BackPic = new Texture2D("data/ui/backbut1.png", LoadMethod.Single, true);
            }

            LockedSize = true;
            DirBox = new TextBoxForm().Set(55, 35, 300, 20) as TextBoxForm;
            FileBox = new TextBoxForm().Set(10, 415, 300, 20) as TextBoxForm;
            Add(DirBox);
            Add(FileBox);

            UIForm cancel = new ButtonForm().Set(10, 450, 120, 20, "Cancel");
            UIForm ok = new ButtonForm().Set(180, 450, 120, 20, "Select");

            cancel.Click = (b) =>
            {
                if (UI.CurUI.Top == this)
                {
                    UI.CurUI.Top = null;
                }
            };

            void SelectFunc(int b)
            {
                if (!Folder)
                {
                    Selected?.Invoke(DirBox.Text + "\\" + FileBox.Text);
                }
                else
                {
                    Selected?.Invoke(DirBox.Text + "\\");
                }
            }

            ok.Click = SelectFunc;

            Add(cancel);
            Add(ok);

            if (defdir == "")
            {
                defdir = DefDir; // System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }

            if (title == "")
            {
                title = "Select file";
            }

            Set(AppInfo.W / 2 - 200, AppInfo.H / 2 - 250, 400, 500, title);

            Files = new ListForm().Set(10, 60, 370, 350, "") as ListForm;
            Add(Files);
            Scan(defdir);
            BackFolder = new ButtonForm().Set(2, 30, 54, 22, "").SetImage(BackPic) as ButtonForm;

            void BackFunc(int b)
            {
                if (new DirectoryInfo(CurPath).Parent == null)
                {
                    return;
                }

                string curPath = new DirectoryInfo(CurPath).Parent.FullName;
                Forms.Remove(Files);
                Files = new ListForm().Set(10, 60, 370, 350, "") as ListForm;

                Add(Files);

                Scan(curPath);
            }

            BackFolder.Click = BackFunc;
            Add(BackFolder);
        }

        public List<string> LastPath = new List<string>();
        public string CurPath = "";

        public void Scan(string folder)
        {
            if (new DirectoryInfo(folder).Exists == false)
            {
                return;
            }

            DirBox.Text = new DirectoryInfo(folder).FullName;
            // LastPath.Add(folder);
            CurPath = folder;
            DirectoryInfo di = new DirectoryInfo(folder);
            foreach (DirectoryInfo fold in di.GetDirectories())
            {
                ItemForm ni = Files.AddItem(fold.Name, FolderPic);
                void DoubleClickFunc(int b)
                {
                    Console.WriteLine("Scanning:" + fold.FullName);
                    Forms.Remove(Files);
                    Files = new ListForm().Set(10, 60, 370, 350, "") as ListForm;
                    Add(Files);
                    Scan(fold.FullName);
                    // Files.Changed?.Invoke();
                }
                ni.DoubleClick = DoubleClickFunc;
            }
            if (!Folder)
            {
                foreach (FileInfo file in di.GetFiles())
                {
                    ItemForm newi = Files.AddItem(file.Name, FilePic);
                    void ClickFunc(int b)
                    {
                        FileBox.Text = file.Name;
                    }
                    void DoubleClickFunc(int b)
                    {
                        Selected?.Invoke(DirBox.Text + "/" + newi.Text);
                    }
                    newi.DoubleClick = DoubleClickFunc;
                    newi.Click = ClickFunc;
                }
                Files.Changed?.Invoke();
            }
            else
            {
                Files.Changed?.Invoke();
            }
        }
    }
}