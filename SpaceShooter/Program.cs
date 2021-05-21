using System;

namespace SpaceShooter
{
    /// <summary>
    /// Program contains the main method that starts the game.
    /// </summary>
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}
