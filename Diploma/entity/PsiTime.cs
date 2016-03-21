using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diploma.entity
{
    public struct PsiTime
    {
        public Vector3 psi;
        public double T;
        public PsiTime(Object empty)
        {
            this.psi = null;
            this.T = 0;
        }
        public PsiTime(Vector3 psi, double time) 
        {
            this.psi = new Vector3(psi);
            this.T = time;
        }
        public PsiTime(PsiTime newPsiTime)
        {
            this.psi = new Vector3(newPsiTime.psi);
            this.T = newPsiTime.T;
        }

        public override string ToString()
        {
            String result = String.Format("T={0} PSI={1}", T, psi.ToString());
            return result;
        }
    }
}
