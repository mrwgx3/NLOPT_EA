
//
// This code demonstrates an example usage of class structures to
// implement the NLOPT tutorial in C#:
//
//    https://nlopt.readthedocs.io/en/latest/NLopt_Reference/
//
//    https://nlopt.readthedocs.io/en/latest/NLopt_Tutorial/
//
// The demonstartion uses Brannon King's C# wrapper for NLOPT 'C',
// which is available as a NUGET package:
//
//   Github page, NloptNet, C# wrapper around the NLopt C library
//   https://github.com/BrannonKing/NLoptNet
//
//   Another download page for C# Nlopt
//   https://www.nuget.org/packages/NLoptNet
//
//   Notes:
//
//   1) The wrapper syntax IS different than what's given in
//      the NLOPT reference.
//
//   2) The code was translated from C++ which used multiple
//      outstreams, hence the C# coding may be a bit strange.
//

using NlopTxamA;

namespace NLOPT_E
{
    public class Program
    {
        static void Main(string[] args)
        {
            NlopTxam WF = new NlopTxam(); // Nlopt optimizer calls
            const string fnx = "nlop_test_d.txt";

            // Open and output file
            using (StreamWriter? ostrm = new StreamWriter(fnx))
            {
                WF.SetOutstream(ostrm);

                // Optimize with results to file
                WF.FoutpNlopt((int)NlpWcCstra.TXA_LD_MMA);
                WF.FoutpNlopt((int)NlpWcCstra.TXA_LN_COBYLA);

                // Close file, null pointers
                ostrm.Close();
                WF.SetOutstream(null);
            }

            // Wait for keypress
            Console.WriteLine($"See file '...\\bin\\Debug\\net8.0\\{fnx}'");
            Console.WriteLine("Keypress to exit...");
            while (Console.ReadKey(true).Key == ConsoleKey.NoName) ;

        } // Main

    } //end-class, Program

} //end-namespace, NLOPT_E