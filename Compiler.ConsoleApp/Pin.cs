using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.ConsoleApp
{
    public class Pin : IEquatable<Pin>, IEqualityComparer<Pin>
    { 
        public string Name;
        public DirectionEnum Direction;

        public bool Equals(Pin other)
        {
            return other.Direction == Direction && other.Name == Name;
        }

        public bool Equals(Pin x, Pin y)
        {
            return x.Name == y.Name && x.Direction == y.Direction;
        }

        public int GetHashCode(Pin obj)
        {
            return $"{obj.Direction + ""}|{obj.Name}".GetHashCode();
        }

        public enum DirectionEnum
        {
            None,
            In,
            Out
        }
    }
}
