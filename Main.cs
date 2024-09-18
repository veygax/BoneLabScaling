using BoneLabScaling.Avatar;
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

    public static FloatElement setAvatarScaleMenu { get; private set; }
    public static FunctionElement applyAvatarScaleMenu { get; private set; }

    public override void OnInitializeMelon()
    {
        MelonLogger.Msg("Scaling mod has initialized.");
        CreateMenu();
    }

    private void CreateMenu()
    {
        menuPage = Page.Root.CreatePage("Scaling", Color.blue);

        avatarScalingSubCategory = menuPage.CreatePage("Avatar", Color.green);

        setAvatarScaleMenu = avatarScalingSubCategory.CreateFloat("Scale", Color.yellow, AvatarScale.scale, 0.1f, 0.1f, 10f, delegate (float f)
        {
            AvatarScale.scale = f;
        });
        applyAvatarScaleMenu = avatarScalingSubCategory.CreateFunction("Apply", Color.green, delegate
        {
            AvatarScale.ScaleAvatar();
        });
    }
}