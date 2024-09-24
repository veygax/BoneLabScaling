using BoneLib;
using Il2CppSLZ.Marrow.Warehouse;
using Il2CppSLZ.VRMK;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSLZ.Bonelab;
using UnityEngine;
using UnityEngine.Rendering;
using Il2CppInterop.Runtime;
using BoneLib.Notifications;
using Il2CppSLZ.Marrow.AI;



namespace BoneLabScaling.Objects;

internal class ObjectScale
{
    public static float scale = 1f;
    public static GameObject inLeftHand = null;
    public static GameObject inRightHand = null;

    private static Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>();

    static ObjectScale()
    {
        inLeftHand = null;
        inRightHand = null;
    }

    public static void ScaleObject(string hand)
    {
        GameObject objectInHand = null;

        if (hand == "left")
        {
            objectInHand = Player.GetObjectInHand(Player.LeftHand);
        }
        else if (hand == "right")
        {
            objectInHand = Player.GetObjectInHand(Player.RightHand);
        }
        else
        {
            Main.BoneMenuNotif(NotificationType.Error, "Invalid hand specified.");
            return;
        }

        if (objectInHand != null)
        {
            var transform = objectInHand.transform;
            var rb = transform.GetComponentInParent<Rigidbody>();

            if (rb == null)
            {
#if DEBUG
                MelonLogger.Msg("No Rigidbody found for scaling.");
#endif
                return;
            }

            if (!originalScales.ContainsKey(rb.gameObject))
            {
                originalScales[rb.gameObject] = rb.transform.localScale;
            }

            Vector3 localScale = rb.transform.localScale;
            localScale.x *= scale;
            localScale.y *= scale;
            localScale.z *= scale;
            rb.transform.localScale = localScale;
            rb.mass *= scale * scale * scale;

            if (hand == "left")
            {
                inLeftHand = objectInHand;
            }
            else
            {
                inRightHand = objectInHand;
            }
        }
        else
        {
            Main.BoneMenuNotif(NotificationType.Error, $"No object found in {hand} hand.");
        }
    }


    public static void ResetScale()
    {
        if (inLeftHand != null)
        {
            ResetScaleForHand(inLeftHand);
        }

        if (inRightHand != null)
        {
            ResetScaleForHand(inRightHand);
        }
    }

    private static void ResetScaleForHand(GameObject obj)
    {
        if (obj == null)
        {
            Main.BoneMenuNotif(NotificationType.Error, "You are not holding anything.");
            return;
        }
        var rb = obj.GetComponentInParent<Rigidbody>();
        if (rb != null && originalScales.ContainsKey(rb.gameObject))
        {
            rb.transform.localScale = originalScales[rb.gameObject];
            rb.mass = rb.mass / (scale * scale * scale);
#if DEBUG
            MelonLogger.Msg($"Reset scale of {rb.gameObject.name} to original scale.");
#endif
        }
    }

}
