using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;
using System.Collections.Generic;
using UnityEngine;
using Il2CppAssets.Scripts.Unity;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;

namespace OsirisHero
{
    public class LightningDisplay : ModDisplay
    {
        public override string BaseDisplay => Game.instance.model.GetTowerFromId("TackShooter-500").GetDescendant<CreateEffectWhileAttackingModel>().effectModel.assetId.AssetGUID;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
#if DEBUG
            ///node.PrintInfo();
#endif

            foreach (ParticleSystem ps in node.GetComponentsInChildren<ParticleSystem>())
            {
                ps.startColor = new Color(0.5f, 0.1f, 1f);
            }
        }
    }
}
