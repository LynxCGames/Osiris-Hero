using MelonLoader;
using BTD_Mod_Helper;
using OsirisHero;

[assembly: MelonInfo(typeof(OsirisHero.OsirisHero), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace OsirisHero;

public class OsirisHero : BloonsTD6Mod
{

}