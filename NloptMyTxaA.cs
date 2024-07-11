using MathTxamA;
using NLoptNet;
using StrucDefn;


namespace NloptMyTxaA
{
    public class NloptMyTxa
    {
        public const double TolRelConverge = 1e-6;
        public const double CnstRelConverge = 1e-8;

        public const int txa_x0 = 0;
        public const int txa_x1 = 1;
        public const int NXV_TXA = 2;

        public uint Npar;
        public bool opt_ok;
        public int itrcnt;
        public double? minf;
        public NLoptAlgorithm AlgorSel;
        public TextWriter? ostrm;

        public MathTxam wm = new MathTxam();

        public double[] xv = new double[NXV_TXA];
        public double[] ub = new double[NXV_TXA];
        public double[] lb = new double[NXV_TXA];

        // Default constructor
        public NloptMyTxa()
        {
            ostrm = null;
            //mm = new MathVars();
            //mm.Zero();
        }

        // Default destructor
        ~NloptMyTxa()
        {
        }

        // For logging functions
        public void SetOutstream(StreamWriter? os)
        {
            ostrm = os;
        }

        // Set algorithm type
        public void SetAlgorSel(int ags)
        {
            AlgorSel = (NLoptAlgorithm)ags;
        }


        // Xfer 'nlopt' vars to math vars
        public void MathvarNloptvar(ref MathVars mmd, double[] xs)
        {
            mmd.x0 = xs[txa_x0];
            mmd.x1 = xs[txa_x1];
        }

        // Xfer math vars to 'nlopt' vars
        public void NloptvarMathvar(ref MathVars mms, double[] xd)
        {
            xd[txa_x0] = mms.x0;
            xd[txa_x1] = mms.x1;
        }

        // Set optimizer upper/lower bounds & STARTING POINT
        public void SetUblbStart()
        {
            ub[txa_x0] = double.PositiveInfinity;
            lb[txa_x0] = double.NegativeInfinity;
            ub[txa_x1] = double.PositiveInfinity;
            lb[txa_x1] = 0.0;

            // Fixed optimizer starting point included here for context
            xv[txa_x0] = 1.234;
            xv[txa_x1] = 5.678;
        }

        // 'NLOPT' Objective function
        public double MyObjFunc(double[] x, double[] grad)
        {
            // Xfer 'nlopt' vars to math vars
            MathvarNloptvar(ref wm.mm, x);

            itrcnt += 1;

            // Objective function gradient
            double[] gdv = { 0, 0 };
            if (grad != null)
            {
                wm.ObjfGradTxa(gdv);
                grad[0] = gdv[0];
                grad[1] = gdv[1];
            }

            // Objective function
            double f0 = wm.mm.f0 = wm.ObjfValueTxa();
            return f0;
        }

        public double MyConstraintB1(double[] x, double[] grad)
        {
            MathvarNloptvar(ref wm.mm, x);
            double h0 = wm.mm.h0 = wm.CnstB1GradTxa(grad);
            return (h0);
        }

        public double MyConstraintB2(double[] x, double[] grad)
        {
            MathvarNloptvar(ref wm.mm, x);
            double h1 = wm.mm.h1 = wm.CnstB2GradTxa(grad);
            return (h1);
        }

        // Constraint function, vector
        public void MyConstraintM(double[] x, double[] grad, double[] res)
        {
            MathvarNloptvar(ref wm.mm, x);
            wm.CnstMValGradTxa(res, grad);
        }

        public void ConvergeMsg()
        {
            // Sentinel
            if (ostrm == null)
                return;

            ostrm.WriteLine($"Found minimum ({minf:F7}) at f( xv[] ), where");
            for (int k = 0; k < NXV_TXA; k++)
            {
                ostrm.WriteLine($"xv[{k}] = {xv[k]:F7}");
            }
            ostrm.WriteLine();
            ostrm.WriteLine($"Found minimum after ({itrcnt}) evaluations");
        }

        // Run the optimizer
        public bool OptimizerRunTxa()
        {
            // Number of parameters to optimize
            Npar = NXV_TXA;

            opt_ok = false; // Convergence false
            itrcnt = 0;     // Iteration count

            // Qualify algorithm type
            // LD_MMA    - used by example,  gradient needed
            // LN_COBYLA - used by example,  gradient free

            switch (AlgorSel)
            {
                default: return opt_ok;
                case NLoptAlgorithm.LD_MMA:
                case NLoptAlgorithm.LN_COBYLA: break;
            }

            // algorithm and dimensionality
            var opt = new NLoptSolver(AlgorSel, Npar, TolRelConverge, 2000);

            // Resize optimizer vectors
            xv = new double[Npar];
            ub = new double[Npar];
            lb = new double[Npar];

            // Set optimizer starting point
            // MOVED into 'set_ublb_start()' for context

            // Set upper/lower bounds  & fixed STARTING POINT
            SetUblbStart();

            // Xfer bounds to optimizer
            opt.SetUpperBounds(ub);
            opt.SetLowerBounds(lb);

            // Set objective function
            opt.SetMinObjective(MyObjFunc);

            //// Inequality constraints have changed: had to define (2) functions
            //opt.AddLessOrEqualZeroConstraint(MyConstraintB1, CnstRelConverge);
            //opt.AddLessOrEqualZeroConstraint(MyConstraintB2, CnstRelConverge);

            // Try vector version of 'Add<=Zero' constraint
            double[] tolv = { CnstRelConverge, CnstRelConverge };
            opt.AddLessOrEqualZeroConstraints(MyConstraintM, tolv);

            //// Relative tolerance has changed
            //// opt.SetXtolRel(1e-8);
            //opt.SetRelativeToleranceOnOptimizationParameter( TolRelConverge );

            // Cannot find equiv. function
            // opt.SetStopVal(Math.Sqrt(8.0 / 27.0) + 1e-3);
            // Suspect it may this function:

            double expectedValue = Math.Sqrt(8.0 / 27.0) + 1e-6;
            opt.SetAbsoluteToleranceOnFunctionValue(expectedValue);

            // Optimize
            try
            {
                var result = opt.Optimize(xv, out minf);
                opt_ok = true;
                ConvergeMsg();
            }
            catch (Exception e)
            {
                opt_ok = false;
                if (ostrm != null)
                {
                    ostrm.WriteLine($"nlopt failed: {e.Message}");
                }
            }

            // Update vars with optimized values
            MathvarNloptvar(ref wm.mm, xv);
            return opt_ok;
        }
    }

} //end-namespace, NloptMyTxaA
