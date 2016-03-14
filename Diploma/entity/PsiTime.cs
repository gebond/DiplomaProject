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

        public PsiTime(Quaternion psi, double time) 
        {
            this.psi = psi;
            this.T = time;
        }    
    }
}
