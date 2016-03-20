using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diploma.entity
{
    public struct PsiTime
    {
        public Quaternion psi;
        public double T;
        public PsiTime(Object empty)
        {
            this.psi = null;
            this.T = 0;
        }
        public PsiTime(Quaternion psi, double time) 
        {
            this.psi = new Quaternion(psi);
            this.T = time;
        }
        public PsiTime(PsiTime newPsiTime)
        {
            this.psi = new Quaternion(newPsiTime.psi);
            this.T = newPsiTime.T;
        }

        public override string ToString()
        {
            String result = String.Format("T={0} PSI={1}", T, psi.ToString());
            return result;
        }
    }
}
