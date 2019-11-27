using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid.Texture;

namespace Vivid.Resonance.Forms
{
    public class GroupForm : UIForm
    {

        public static Texture.Texture2D GroupImg = null;

        public GroupForm()
        {

            Draw = () =>
            {

                if (GroupImg == null)
                {
                    GroupImg = new Texture2D("data/ui/group1.png", LoadMethod.Single, true);
                }

                DrawForm(GroupImg);

            };

        }

    }
}
