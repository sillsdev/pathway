using System;
using System.Collections;
using System.Collections.Generic;

using System.Text;

namespace SIL.PublishingSolution.Filter
{
    public class LiftLangFilters : LiftFilterChooseStatments
    {
        public LiftLangFilters(List<LiftFilterChooseStatement> filters)
        {
            this.filters = filters;
        }

        public LiftLangFilters()
        {
            
        }

        public override object Clone()
        {
            return new LiftLangFilters(this.filters);
        }
    }
}
