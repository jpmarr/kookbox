using kookbox.core.Interfaces;

namespace kookbox.core
{
    internal class Security : ISecurity
    {
        public void CheckUserHasPermission<T>(IUser listener, Permission requiredPermission, Option<T> target)
        {
            // todo: implement and throw PermissionDenied if fails
        }
    }
}
