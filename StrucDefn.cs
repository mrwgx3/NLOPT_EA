namespace StrucDefn
{
    public struct MathVars
    {
        // Obj. function
        public double f0;     // Value
        public double x0, x1; // Args
        public double g0, g1; // Gradient

        // Constraint functions, vector format
        public double h0, h1;   // Value
        public double k0v, k1v; // Gradient
        public double k2v, k3v;
        public double a1, b1;   // Interms
        public double a2, b2;
        public double axobA, axobAsq;
        public double axobB, axobBsq;

        public void Zero()
        {
            f0 = x0 = x1 = g0 = g1 = 0;
            h0 = h1 = k0v = k1v = k2v = k3v = 0;
            a1 = b1 = a2 = b2 = 0;
            axobA = axobAsq = axobB = axobBsq = 0;
        }

    } //end-struc, MathVar

} //end-namespace, StrucDefn


