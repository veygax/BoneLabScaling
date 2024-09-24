using BoneLabScaling.Avatar;
using BoneLabScaling.Objects;
using MelonLoader;
using BoneLib.BoneMenu;
using Color = UnityEngine.Color;
using Main = BoneLabScaling.Main;

[assembly: MelonInfo(typeof(Main), "Scaling", "1.0.0", "VeygaX")]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace BoneLabScaling;

public class Main : MelonMod
{
    public static Page menuPage { get; private set; }

    public static Page avatarScalingSubCategory { get; private set; }
    public static Page objectScalingSubCategory { get; private set; }

    public static FloatElement setAvatarScaleMenu { get; private set; }
    public static FunctionElement applyAvatarScaleMenu { get; private set; }
    public static FunctionElement resetAvatarScaleMenu { get; private set; }


    public static FloatElement setObjectScaleMenu { get; private set; }
    public static FunctionElement applyObjectScaleLHandMenu { get; private set; }
    public static FunctionElement applyObjectScaleRHandMenu { get; private set; }
    public static FunctionElement resetObjectScaleMenu { get; private set; }

    public override void OnInitializeMelon()
    {
        MelonLogger.Msg("Scaling mod has initialized.");
        CreateMenu();
    }

    private void CreateMenu()
    {
        menuPage = Page.Root.CreatePage("Scaling", Color.blue);

        avatarScalingSubCategory = menuPage.CreatePage("Avatar", Color.green);
        objectScalingSubCategory = menuPage.CreatePage("Object", Color.blue);

        // Avatar scaling
        setAvatarScaleMenu = avatarScalingSubCategory.CreateFloat("Scale", Color.yellow, AvatarScale.scale, 0.1f, 0.1f, 10f, delegate (float f)
        {
            AvatarScale.scale = f;
        });
        applyAvatarScaleMenu = avatarScalingSubCategory.CreateFunction("Apply", Color.green, delegate
        {
            AvatarScale.ScaleAvatar();
        });
        resetAvatarScaleMenu = avatarScalingSubCategory.CreateFunction("Reset", Color.green, delegate
        {
            AvatarScale.ResetScale();
        });

        // Object scaling
        setObjectScaleMenu = objectScalingSubCategory.CreateFloat("Scale", Color.yellow, AvatarScale.scale, 0.1f, 0.1f, 10f, delegate (float f)
        {
            ObjectScale.scale = f;
        });
        applyObjectScaleLHandMenu = objectScalingSubCategory.CreateFunction("Scale from Left Hand", Color.green, delegate
        {
            ObjectScale.ScaleObject("left");
        });
        applyObjectScaleRHandMenu = objectScalingSubCategory.CreateFunction("Scale from Right Hand", Color.green, delegate
        {
            ObjectScale.ScaleObject("right");
        });
        resetObjectScaleMenu = objectScalingSubCategory.CreateFunction("Reset", Color.green, delegate
        {
            ObjectScale.ResetScale();
        });
    }

    public static void BoneMenuNotif(BoneLib.Notifications.NotificationType type, string content)
    {
        var notif = new BoneLib.Notifications.Notification
        {
            Title = "Scaling",
            Message = content,
            Type = type,
            PopupLength = 3,
            ShowTitleOnPopup = true
        };
        BoneLib.Notifications.Notifier.Send(notif);

#if DEBUG
        MelonLogger.Msg("Sent a notification: " + content);
#endif

    }
}