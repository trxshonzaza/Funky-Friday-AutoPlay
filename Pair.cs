﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunkyFridayAutoPlay
{
    internal class Pair<T1, T2>
    {
        public T1 key;
        public T2 value;

        public Pair(T1 key, T2 value) 
        { 
            this.key = key;
            this.value = value;
        }

        public T1 getKey()
        {
            return key;
        }

        public T2 getValue()
        {
            return value;
        }
    }
}
