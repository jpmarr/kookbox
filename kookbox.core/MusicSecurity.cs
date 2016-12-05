using kookbox.core.Interfaces;

namespace kookbox.core
{
    internal class MusicSecurity : IMusicSecurity
    {
        public void CheckListenerHasPermission<T>(IMusicListener listener, Permission requiredPermission, Option<T> target)
        {
            // todo: implement and throw PermissionDenied if fails
        }
    }
}
