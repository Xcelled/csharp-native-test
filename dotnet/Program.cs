using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace dotnet
{
    struct timespec
    {
		public long tv_sec;
		public long tv_nsec;
	}
    class Program
    {
		[DllImport("libfunc")]
		private static extern void multi_arr(int[] x, int[] y, int[] res, int len);

		const int CLOCK_MONOTONIC = 1;
		[DllImport("librt")]
		private static extern int clock_gettime(int clk_id, ref timespec tp);

		static void Main(string[] args)
        {
            if (args.Length < 1)
            {
				Console.WriteLine("Usage: <program> <number>");
				Environment.Exit(1);
			}

            // long frequency = Stopwatch.Frequency;
            // long nanosecPerTick = (1000L*1000L*1000L) / frequency;
            // Console.WriteLine("  Timer is accurate within {0} nanoseconds. Hi res: {1}", nanosecPerTick, Stopwatch.IsHighResolution);
			unsafe
			{
				Console.WriteLine("  64 bit mode: " + (sizeof(IntPtr) == 8));
			}

			var len = int.Parse(args[0]);
			var r = new Random();

			multi_arr(null, null, null, 0);

            var min = long.MaxValue;
			long total = 0;

			for (var test = 0; test < 100; ++test)
			{
                var x = new int[len];
                var y = new int[len];
                var res = new int[len];

                for (var i = 0; i < len; ++i)
                {
                    x[i] = r.Next();
                    y[i] = r.Next();
                }

				//var s = Stopwatch.StartNew();

				var start = new timespec();
				clock_gettime(CLOCK_MONOTONIC, ref start);
				multi_arr(x, y, res, len);
				var stop = new timespec();
				clock_gettime(CLOCK_MONOTONIC, ref stop);

				//s.Stop();

				// Verify correctness and keep this from being optimized out
				for (var i = 0; i < len; ++i)
                {
                    if (res[i] != (x[i] * y[i]))
                    {
						Console.WriteLine("Yellyellyell");
					}
                }

				var t = ((stop.tv_sec - start.tv_sec) * 1000000000L) + (stop.tv_nsec - start.tv_nsec);

				//var t = s.ElapsedTicks * 1000000000L / Stopwatch.Frequency;
				total += t;

				if (t < min)
                {
					min = t;
				}
			}

			Console.Write(min + " ns,");
			Console.WriteLine((total / 100) + " ns");
		}
    }
}
