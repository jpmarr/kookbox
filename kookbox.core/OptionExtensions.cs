using System;

namespace kookbox.core
{
    public static class OptionExtensions
    {
        public static Option<T> IfHasValue<T>(this Option<T> option, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (option.HasValue)
                action(option.ValueOr(default(T)));

            return option;
        }

        public static void Else<T>(this Option<T> option, Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (!option.HasValue)
                action();
        }

        public static bool TryGetValue<T>(this Option<T> option, out T value)
        {
            if (option.HasValue)
            {
                value = option.ValueOr(default(T));
                return true;
            }
            value = default(T);
            return false;
        }
    }
}
