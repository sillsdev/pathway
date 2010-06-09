using System;
using System.Collections;
using System.Collections.Generic;

using System.Text;

namespace SIL.PublishingSolution.Filter
{
    public class LiftEntryAndSenseFilters : LiftFilterChooseStatments
    {
        public LiftEntryAndSenseFilters(List<LiftFilterChooseStatement> filters)
        {
            this.filters = filters;
        }

        public LiftEntryAndSenseFilters()
        {
            
        }

        public override object Clone()
        {
            return new LiftEntryAndSenseFilters(filters);
        }
    }

}
