using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTCore.Classes
{
    public class TransactionsView
    {
        public int CategoryId
        {
            get; set;
        }
        public int TargetId
        {
            get; set;
        }
        public DateTime StartDate
        {
            get; set;
        }
        public DateTime EndDate
        {
            get; set;
        }

    }
}
