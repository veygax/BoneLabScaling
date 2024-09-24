using BoneLib;
using Il2CppSLZ.Marrow.Warehouse;
using Il2CppSLZ.VRMK;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSLZ.Bonelab;
using UnityEngine;

namespace BoneLabScaling.Avatar;

internal class AvatarScale
{
    public static float scale = 1f;

    private static GameObject gameObject;

    private static Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>();

    public static void ScaleAvatar()
    {
        AvatarCrate crate;
        if (AssetWarehouse.Instance.TryGetCrate(Player.RigManager._avatarCrate._barcode, out crate))
        {
            Action<GameObject> action = delegate (GameObject obj)
            {
                gameObject = UnityEngine.Object.Instantiate(obj);
                Vector3 localScale = gameObject.transform.localScale;
                localScale.x *= scale;
                localScale.y *= scale;
                localScale.z *= scale;
                gameObject.transform.localScale = localScale;
                gameObject.transform.parent = Player.RigManager.transform;
                gameObject.transform.localPosition = Vector3.zero;
                Il2CppSLZ.VRMK.Avatar componentInChildren = gameObject.GetComponentInChildren<Il2CppSLZ.VRMK.Avatar>();
                foreach (SkinnedMeshRenderer item in componentInChildren.hairMeshes)
                {
                    item.enabled = false;
                }

                if (!originalScales.ContainsKey(gameObject))
                {
                    originalScales[gameObject] = gameObject.transform.localScale;
                }
#if DEBUG
                MelonLogger.Msg("Changed scale to " + scale + "x");
#endif
                componentInChildren.PrecomputeAvatar();
                componentInChildren.RefreshBodyMeasurements();
                Player.RigManager.SwitchAvatar(componentInChildren);
                PlayerRefs.Instance._bodyVitals.PROPEGATE();
            };
            crate.LoadAsset(action);
        }
        else
        {
#if DEBUG
            MelonLogger.Msg("Failed to find avatar crate.");
#endif
        }

    }

    public static void ResetScale()
    {
        if (gameObject != null && originalScales.ContainsKey(gameObject))
        {
            gameObject.transform.localScale = originalScales[gameObject];
#if DEBUG
            MelonLogger.Msg($"Reset scale of avatar to original scale.");
#endif
        }
    }
}
