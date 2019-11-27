using System;

namespace Vivid.Script
{
    public class NewNodeScript : NodeScriptLink
    {
        public override void Begin()
        {
            Console.WriteLine("Begun Sciprt!");
            base.Begin();
        }

        public override void End()
        {
            Console.WriteLine("Ended Script!");
            base.End();
        }
    }
}