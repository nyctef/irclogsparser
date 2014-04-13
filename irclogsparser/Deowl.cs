using System;

namespace irclogsparser
{
    public class Deowl : IEquatable<Deowl>
    {
        public string Name;
        public bool Success;

        public Deowl(string name, bool success)
        {
            Name = name;
            Success = success;
        }

        public bool Equals(Deowl other)
        {
            return Name == other.Name && Success == other.Success;
        }

        public override bool Equals(object other)
        {
            return ((var otherDeowl = other as Deowl) != null) && Equals(otherDeowl);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + Success.GetHashCode();
        }
    }
}