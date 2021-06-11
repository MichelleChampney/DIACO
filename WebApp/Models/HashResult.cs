using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Models
{
    public class HashResult
    {
        public string Hash { get; set; }
        public byte[] Salt { get; set; }
    }
}
