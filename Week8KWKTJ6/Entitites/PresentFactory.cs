using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week8KWKTJ6.Abstractions;

namespace Week8KWKTJ6.Entitites
{
    class PresnetFactory : IToyFactory
    {
        public Toy CreateNew()
        {
            return new Present();
        }
    }
}
