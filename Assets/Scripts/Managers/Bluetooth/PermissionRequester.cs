using UnityEngine;

public class PermissionRequester : MonoBehaviour
{
    void Start()
    {
        // Check and request the Fine Location permission
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation))
        {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation);
        }
    }
}
