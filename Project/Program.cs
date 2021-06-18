using OpenTK.Mathematics;       
using OpenTK.Windowing.Desktop; 
using System;

namespace CG 
{
    class Program
    {
        static void Main(string[] args) 
        {

            var ourWindow = new NativeWindowSettings() 
            { 
                Size = new Vector2i(2000,2000), 
                Title = " UAS Grafkom" 
            };

            using (var win = new Windows(GameWindowSettings.Default, ourWindow))
            {
                win.Run();
            }

        }
    }
}
