using MathTxamA;
using NloptMyTxaA;
using NLoptNet;
using StrucDefn;

namespace NlopTxamA
{
    // Optimizer selection
    public enum NlpWcCstra
    {
        NUL_ENTRY = 0,
        TXA_LD_MMA,
        TXA_LN_COBYLA,
        NLW_CSTRA_NN
    }; //end-enum

    public class NlopTxam
    {


        // Conversion constants
        public const double My_Pi = 3.1415926535897932;
        public const double Deg2Rad = My_Pi / 180.0;
        public const double Rad2Deg = 180.0 / My_Pi;

        // Section divider string
        public const string SectDvStr =
        "//===========================================================================\n";

        // Section divider string
        public const string SubSectDvStr =
        "//---------------------------------------------------------------------------\n";

        public StreamWriter? ostrm = null;
        public bool optF_ok = false;
        public double minf_F = 0;
        public int itrcnt_F = 0;
        public int AlgSel = 0;
        public int CstraSel = 0;
        public int Nvar = 2;       // Number of optimize vars

        // Working storage, variables used by math functions
        public MathVars mk;       // Archival copy

        // Math routine instances used by NLOP's
        public MathTxam wm = new MathTxam();       // Working (math copy)

        // Default constructor
        public NlopTxam()
        {
            if (ostrm != null)
                ostrm = null;
        }

        // Default destructor
        ~NlopTxam()
        {
        }

        // For logging functions
        public void SetOutstream(StreamWriter? os) { ostrm = os; }

        // 'MathVars' storage
        public void XferMathVarA(ref MathVars src)
        {
            mk = src;                   // Archive copy
            wm.XferMathVars(ref src);   // Math copy
        }

        // Run optimizer 'Txa'
        // NLOPT Example
        public void OptimizerTxa(int ags)
        {
            NloptMyTxa opt = new NloptMyTxa();

            // Copy all math to optimizer
            opt.wm = wm;

            if ( ostrm != null )
            {
                opt.SetOutstream(ostrm);
            }

            opt.SetAlgorSel(ags);    // Select algorithm type
            opt.OptimizerRunTxa();   // Run the optimizer

            optF_ok = opt.opt_ok;        // Optimizer success
            minf_F = (double)opt.minf;   // ......... iterations
            itrcnt_F = opt.itrcnt;       // Obj. funct. minimum

            // Update math vars with optimized values
            opt.MathvarNloptvar(ref wm.mm, opt.xv);
        }

        // Optimizer switches
        public void OptimizerRun()
        {
            AlgSel = 0;
            switch (CstraSel)
            {
                default: return;
                case (int)NlpWcCstra.TXA_LD_MMA:
                    AlgSel = (int)NLoptAlgorithm.LD_MMA;
                    break;
                case (int)NlpWcCstra.TXA_LN_COBYLA:
                    AlgSel = (int)NLoptAlgorithm.LN_COBYLA;
                    break;
            }

            OptimizerTxa(AlgSel);
        }

        // STUB : Optimizer initialization
        public void OptimizerInit()
        {
            // switch (CstraSel) {
            //     default: break;
            // }
        }

        // STUB: Optimizer post processing
        public void OptimizerPost()
        {
            // switch (CstraSel)  {
            //     default: break;
            // }
        }

        public void CalcsNlopt(int csel)
        {
            CstraSel = csel;    // Local global, constraint selection, used everywhere

            OptimizerInit();   // STUB
            OptimizerRun();    // Run NLOPT optimizers
            OptimizerPost();   // STUB

        }

        public void FoutpNlopt(int csel)
        {
            string[] desc = {
            "NUL_ ENTRY", "TXA  LD_MMA", "TXA  LN_COBYLA"
        };

            if (ostrm != null)
            {
                // Section divider string
                ostrm.Write(SectDvStr);

                // Title
                ostrm.WriteLine($"\n{desc[csel]}");
                ostrm.WriteLine("\n");
            }

            // Do the selected optimization
            CalcsNlopt(csel);

            if (ostrm != null)
            {
                ostrm.WriteLine("\n");
            }
        }
    }

} //end-namespace, NlopTxam


