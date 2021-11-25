using System;
using System.Collections.Generic;
using System.Text;

namespace FactCheckThisBitch.Models
{
    public class Organisation
    {
        public string Name;
        public Uri Image;
        public Uri Website;
    }

    //Use vibby to create highligt video of the segments
    //https://www.vibby.com/watch?vib=Xk8DTNMm3
    public class Segment
    {
        public DateTime FromTime;
        public DateTime ToTime;
    }
}
