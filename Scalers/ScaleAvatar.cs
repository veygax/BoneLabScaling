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
using Il2CppSLZ.Marrow;
using Il2CppSystem.Numerics;

namespace BoneLabScaling.Avatar;

internal class AvatarScale
{
    public static float scale = 1f;

    private static GameObject avatarObject;


    public static void ScaleAvatar()
    {
        AvatarCrate crate;
        if (AssetWarehouse.Instance.TryGetCrate(Player.RigManager._avatarCrate._barcode, out crate))
        {
            Action<GameObject> action = delegate(GameObject obj)
            {
                avatarObject = UnityEngine.Object.Instantiate(obj);
                avatarObject.transform.localScale = new Vector3(scale, scale, scale);
                avatarObject.transform.parent = Player.RigManager.transform;
                avatarObject.transform.localPosition = Vector3.zero;
                Il2CppSLZ.VRMK.Avatar componentInChildren =
                    avatarObject.GetComponentInChildren<Il2CppSLZ.VRMK.Avatar>();
                foreach (SkinnedMeshRenderer item in componentInChildren.hairMeshes)
                {
                    item.enabled = false;
                }
#if DEBUG
                MelonLogger.Msg("Changed scale to " + scale + "x");
#endif
                componentInChildren.PrecomputeAvatar();
                componentInChildren.RefreshBodyMeasurements();
                Player.RigManager.SwitchAvatar(componentInChildren);
                PlayerRefs.Instance._bodyVitals.PROPEGATE();

                PhysicsRig physrig = BoneLib.Player.GetPhysicsRig();
                PullCordDevice bodyLog = physrig.GetComponentInChildren<PullCordDevice>();

                if (bodyLog != null)
                {
                    bodyLog.transform.localScale = new Vector3(scale, scale, scale);
                }

                foreach (var slot in BoneLib.Player.RigManager.inventory.bodySlots)
                {
                    slot.transform.localScale = new Vector3(scale, scale, scale);
                    if (slot.name.Equals("BeltLf1"))
                    {
                        InventoryAmmoReceiver ammoReceiver = slot.GetComponentInChildren<InventoryAmmoReceiver>();

                        if (ammoReceiver == null)
                        {
                            continue;
                        }

                        foreach (var mag in ammoReceiver._magazineArts)
                        {
                            mag.transform.localScale = new Vector3(scale, scale, scale);
                        }
                    }
                }
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
        // This is the poorest way of doing it, but it works.
        AvatarCrate crate;
        if (AssetWarehouse.Instance.TryGetCrate(Player.RigManager._avatarCrate._barcode, out crate))
        {
            Action<GameObject> action = delegate(GameObject obj)
            {
                avatarObject = UnityEngine.Object.Instantiate(obj);
                avatarObject.transform.localScale = new Vector3(1f, 1f, 1f);
                avatarObject.transform.parent = Player.RigManager.transform;
                avatarObject.transform.localPosition = Vector3.zero;
                Il2CppSLZ.VRMK.Avatar componentInChildren =
                    avatarObject.GetComponentInChildren<Il2CppSLZ.VRMK.Avatar>();
                foreach (SkinnedMeshRenderer item in componentInChildren.hairMeshes)
                {
                    item.enabled = false;
                }
#if DEBUG
                MelonLogger.Msg("Changed scale to " + scale + "x");
#endif
                componentInChildren.PrecomputeAvatar();
                componentInChildren.RefreshBodyMeasurements();
                Player.RigManager.SwitchAvatar(componentInChildren);
                PlayerRefs.Instance._bodyVitals.PROPEGATE();

                PhysicsRig physrig = BoneLib.Player.GetPhysicsRig();
                PullCordDevice bodyLog = physrig.GetComponentInChildren<PullCordDevice>();

                if (bodyLog != null)
                {
                    bodyLog.transform.localScale = new Vector3(1f, 1f, 1f);
                }

                foreach (var slot in BoneLib.Player.RigManager.inventory.bodySlots)
                {
                    slot.transform.localScale = new Vector3(1f, 1f, 1f);
                    if (slot.name.Equals("BeltLf1"))
                    {
                        InventoryAmmoReceiver ammoReceiver = slot.GetComponentInChildren<InventoryAmmoReceiver>();

                        if (ammoReceiver == null)
                        {
                            continue;
                        }

                        foreach (var mag in ammoReceiver._magazineArts)
                        {
                            mag.transform.localScale = new Vector3(1f, 1f, 1f);
                        }
                    }
                }
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
}
