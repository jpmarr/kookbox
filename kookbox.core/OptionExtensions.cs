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
    }
}
