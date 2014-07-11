using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectJson
{
    class Project
    {
        public string Name { get;set;}
        public List<User> children = new List<User>();
    }
}
