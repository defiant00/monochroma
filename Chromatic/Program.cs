using System;

namespace Chromatic
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Chromatic())
            {
                game.Run();
            }
        }
    }
}
