using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;

public static class NativeMethods {
    [DllImport ("user32.dll")]
    public static extern bool SetCursorPos (int X, int Y);

    #region Win32API
    [DllImport("User32.dll")]
    static extern bool GetCursorPos(out POINT point);
    [StructLayout(LayoutKind.Sequential)]
    struct POINT
    {
        public int X { get; set; }
        public int Y { get; set; }
        public static implicit operator Point(POINT point)
        {
            return new Point(point.X, point.Y);
        }
    }
    #endregion

    public static Point Get_Cursor_Position()
    {
        var point_data = new POINT();
        GetCursorPos(out point_data);
        return point_data;
    }
}


namespace mouse_noise_move
{
    class Program
    {

        static double Get_gauss_random(double u,double s,Random rand){
            var x = rand.NextDouble(); 
        	var y = rand.NextDouble(); 
	
            var z1 = Math.Sqrt(-2.0 * Math.Log(x)) * Math.Cos(2.0 * Math.PI * y); 

            return s*z1+u;
        }
        
        static void End_program(object s){
            while(true){
                if(Console.ReadKey().KeyChar=='q'){
                    Environment.Exit(0);
                }
            }
        }

        static void Main(string[] args)
        {
            var rand=new Random();
            ThreadPool.QueueUserWorkItem(new WaitCallback(End_program), null);

            Console.Write($"マウスを動かしています...\nqキー入力で終了\n");

            while(true){
                var pt = NativeMethods.Get_Cursor_Position();

                var n_x=Get_gauss_random(0,30,rand);
                var n_y=Get_gauss_random(0,30,rand);

                NativeMethods.SetCursorPos((int)Math.Round(pt.X+n_x),(int)Math.Round(pt.Y+n_y));
            }
        }
    }
}
