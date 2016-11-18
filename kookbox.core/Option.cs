using System;
using System.Collections.Generic;

namespace kookbox.core
{
    public struct Option<T> : IEquatable<Option<T>>
    {
        private static readonly Option<T> SharedNone = new Option<T>();

        private readonly T value;

        public static Option<T> Some(T value)
        {
            return new Option<T>(value);   
        }

        public static Option<T> None()
        {
            return SharedNone;
        }

        public bool HasValue { get; }

        public T ValueOr(T alternative) => HasValue ? value : alternative;

        private Option(T value)
        {
            this.value = value;
            HasValue = true;
        }

        public void IfHasValue(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (HasValue)
                action(value);
        }

        public bool Equals(Option<T> other)
        {
            if (HasValue != other.HasValue)
                return false;
            if (!HasValue && !other.HasValue)
                return true;

            return EqualityComparer<T>.Default.Equals(value, other.value);
        }

        public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);
        public static bool operator !=(Option<T> left, Option<T> right) => !left.Equals(right);

        public override bool Equals(object obj) => obj is Option<T> && Equals((Option<T>)obj);

        public override int GetHashCode()
        {
            if (HasValue)
            {
                return value.GetHashCode();
            }
            return 0;
        }
    }
}
