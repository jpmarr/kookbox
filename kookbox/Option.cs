using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

namespace kookbox
{
    public class Option<T>
    {
        private static readonly Option<T> none = new Option<T>();

        public static Option<T> Some(T value)
        {
            return new Option<T>(value);   
        }

        public static Option<T> None()
        {
            return none;
        }

        public T Value { get; }
        public bool HasValue { get; }

        private Option(T value)
        {
            Value = value;
            HasValue = true;
        }

        private Option()
        {
        }
    }
}
