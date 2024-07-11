using StrucDefn;

namespace MathTxamA
{
    public class MathTxam
    {
        // Constants
        // Units conversion
        public const double My_Pi = 3.1415926535897932;
        public const double Deg2Rad = My_Pi / 180.0;
        public const double Rad2Deg = 180.0 / My_Pi;

        public MathVars mm;
        public StreamWriter? ostrm;

        // Default constructor
        public MathTxam()
        {
            ostrm = null;
        }

        // Default destructor
        ~MathTxam()
        {
        }

        // For logging functions
        void SetOutstream(StreamWriter? os) { ostrm = os; }

        public void XferMathVars(ref MathVars src)
        {
            //  mk = src; // Archive copy
            mm = src; // Math copy
        }

        public double ObjfValueTxa()
        {
            mm.f0 = Math.Sqrt(mm.x1);
            return (mm.f0);
        }

        public void ObjfGradTxa(double[] gd)
        {
            gd[0] = mm.g0 = 0.0;
            gd[1] = mm.g1 = 0.5 / Math.Sqrt(mm.x1);
        }


        // 1st of (2) constraints  (kludgey, but it works)
        public double CnstB1GradTxa(double[] grad)
        {
            mm.a1 = 2.0; mm.b1 = 0.0;
            mm.axobA = (mm.a1 * mm.x0 + mm.b1);
            mm.axobAsq = mm.axobA * mm.axobA;
            if (grad != null)
            {
                grad[0] = 3 * mm.a1 * mm.axobAsq;
                grad[1] = -1.0;
            }
            return (mm.axobA * mm.axobAsq - mm.x1);
        }

        // 2nd of (2) constraints  (kludgey, but it works)
        public double CnstB2GradTxa(double[] grad)
        {
            mm.a2 = -1.0; mm.b2 = 1.0;
            mm.axobB = (mm.a2 * mm.x0 + mm.b2);
            mm.axobBsq = mm.axobB * mm.axobB;
            if (grad != null)
            {
                grad[0] = 3 * mm.a2 * mm.axobBsq;
                grad[1] = -1.0;
            }
            return (mm.axobB * mm.axobBsq - mm.x1);
        }


        // Constraints (and gradients) given as vectors
        public void CnstMValGradTxa(double[] res, double[] gd)
        {
            mm.a1 = 2.0; mm.b1 = 0.0;
            mm.a2 = -1.0; mm.b2 = 1.0;

            mm.axobA = (mm.a1 * mm.x0 + mm.b1);
            mm.axobB = (mm.a2 * mm.x0 + mm.b2);
            mm.axobAsq = mm.axobA * mm.axobA;
            mm.axobBsq = mm.axobB * mm.axobB;

            if (gd != null)
            {
                gd[0] = mm.k0v = 3 * mm.a1 * mm.axobAsq;
                gd[1] = mm.k1v = -1.0;
                gd[2] = mm.k2v = 3 * mm.a2 * mm.axobBsq;
                gd[3] = mm.k3v = -1.0;
            }

            if (res != null)
            {
                res[0] = mm.h0 = (mm.axobA * mm.axobAsq - mm.x1);
                res[1] = mm.h1 = (mm.axobB * mm.axobBsq - mm.x1);
            }
        }

    } //end-class, MathTxam

} //end-namespace, MathTxamA
