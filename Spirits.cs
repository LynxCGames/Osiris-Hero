using BTD_Mod_Helper.Api.Towers;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets.Projectiles;
using Il2CppAssets.Scripts.Models.CorvusSpells.Instant;
using Il2CppAssets.Scripts.Models.TowerSets;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Simulation.SMath;

namespace OsirisHero
{
    public class EmberSpirit : ModTower
    {
        public override TowerSet TowerSet => TowerSet.Hero;
        public override string BaseTower => TowerType.DartMonkey;
        public override int Cost => 0;
        public override string DisplayName => "Ember Spirit";
        public override int TopPathUpgrades => 0;
        public override int MiddlePathUpgrades => 0;
        public override int BottomPathUpgrades => 0;
        public override string Portrait => "EmberSpirit-Portrait";
        public override bool DontAddToShop => true;
        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            var display = Game.instance.model.GetTowerFromId("HotSauceCreatureTower").GetBehavior<DisplayModel>().display;
            towerModel.AddBehavior(new DisplayModel("", display, -1, DisplayCategory.Tower, new Vector3(0, 1, 10)));
            towerModel.displayScale = 1.5f;
            towerModel.isSubTower = true;
            towerModel.AddBehavior(new CreditPopsToParentTowerModel("CreditPopsToParentTowerModel_"));
            towerModel.AddBehavior(new TowerExpireOnParentDestroyedModel(""));
            towerModel.range = 45;

            towerModel.GetAttackModel().range = 45;
            towerModel.GetAttackModel().weapons[0].rate = 0.3f;
            towerModel.GetAttackModel().weapons[0].projectile.ApplyDisplay<SpiritFlame>();
            towerModel.GetAttackModel().weapons[0].projectile.pierce = 2;
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = 1;
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;

            var flame = Game.instance.model.GetTowerFromId("WizardMonkey-020").GetAttackModel(2).weapons[0].projectile.Duplicate();
            flame.display = Game.instance.model.GetTowerFromId("Corvus 20").GetDescendant<EmberModel>().attack.weapons[0].projectile.display;
            flame.GetBehavior<ClearHitBloonsModel>().interval = 0.25f;
            flame.GetBehavior<InstantModel>().dontFollowTarget = true;
            flame.GetBehavior<AgeModel>().lifespan = 3.5f;

            var ember = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
            ember.ApplyDisplay<SpiritFlame>();
            ember.pierce = 1;
            ember.maxPierce = 1;
            ember.GetDamageModel().damage = 2;
            ember.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
            ember.AddBehavior(new CreateProjectileOnExhaustFractionModel("", flame, new SingleEmissionModel("", null), 1, -1, false, false, true));

            towerModel.GetAttackModel().weapons[0].AddBehavior(new ChangeProjectilePerEmitModel("", towerModel.GetAttackModel().weapons[0].projectile, ember, 16, 6, 5, null, 0, 0, 0));
            towerModel.GetAttackModel().GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }

        public class EmberSpiritDisplay : ModTowerDisplay<EmberSpirit>
        {
            public override float Scale => 1f;
            public override string BaseDisplay => Game.instance.model.GetTowerFromId("DartlingGunner-500").GetAttackModel().weapons[0].projectile.display.guidRef;

            public override bool UseForTower(int[] tiers)
            {
                return true;
            }
            public override void ModifyDisplayNode(UnityDisplayNode node) { }
        }
    }

    public class FrostSpirit : ModTower
    {
        public override TowerSet TowerSet => TowerSet.Hero;
        public override string BaseTower => TowerType.DartMonkey;
        public override int Cost => 0;
        public override string DisplayName => "Frost Spirit";
        public override int TopPathUpgrades => 0;
        public override int MiddlePathUpgrades => 0;
        public override int BottomPathUpgrades => 0;
        public override string Portrait => "FrostSpirit-Portrait";
        public override bool DontAddToShop => true;
        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            var display = Game.instance.model.GetTowerFromId("GenieBottleTower").GetBehavior<AirUnitModel>().display;
            towerModel.AddBehavior(new DisplayModel("", display, -1, DisplayCategory.Tower, new Vector3(0, 0, 10)));
            towerModel.isSubTower = true;
            towerModel.AddBehavior(new CreditPopsToParentTowerModel("CreditPopsToParentTowerModel_"));
            towerModel.AddBehavior(new TowerExpireOnParentDestroyedModel(""));
            towerModel.range = 48;

            towerModel.GetAttackModel().range = 48;
            towerModel.GetAttackModel().weapons[0].rate = 0.8f;
            towerModel.GetAttackModel().weapons[0].projectile.pierce = 3;
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = 2;
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.White;
            towerModel.GetAttackModel().weapons[0].projectile.display = Game.instance.model.GetTowerFromId("IceMonkey-005").GetAttackModel().weapons[0].projectile.display;
            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new SlowModel("SlowModel_", 0.5f, 3, "Frost:Normal", 2, "", true, true, null, true, false, false, 0));
            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new SlowModifierForTagModel("SlowModifierForTagModel_", "Moabs", "Frost", 1, true, false, 0, false));
            towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new int[] { 0, -1 };

            var iceBomb = Game.instance.model.GetTowerFromId("IceMonkey-204").GetAttackModel().weapons[0].projectile.Duplicate();
            iceBomb.display = Game.instance.model.GetTowerFromId("Mermonkey-040").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.display;
            iceBomb.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 3;

            towerModel.GetAttackModel().weapons[0].AddBehavior(new ChangeProjectilePerEmitModel("", towerModel.GetAttackModel().weapons[0].projectile, iceBomb, 8, 6, 5, null, 0, 0, 0));
            towerModel.GetAttackModel().GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }

        public class FrostSpiritDisplay : ModTowerDisplay<FrostSpirit>
        {
            public override float Scale => 1f;
            public override string BaseDisplay => Game.instance.model.GetTowerFromId("DartlingGunner-500").GetAttackModel().weapons[0].projectile.display.guidRef;

            public override bool UseForTower(int[] tiers)
            {
                return true;
            }
            public override void ModifyDisplayNode(UnityDisplayNode node) { }
        }
    }

    public class NatureSpirit : ModTower
    {
        public override TowerSet TowerSet => TowerSet.Hero;
        public override string BaseTower => TowerType.DartMonkey;
        public override int Cost => 0;
        public override string DisplayName => "Nature Spirit";
        public override int TopPathUpgrades => 0;
        public override int MiddlePathUpgrades => 0;
        public override int BottomPathUpgrades => 0;
        public override string Portrait => "NatureSpirit-Portrait";
        public override bool DontAddToShop => true;
        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            var display = Game.instance.model.GetTowerFromId("SpiritTower").GetBehavior<DisplayModel>().display;
            towerModel.AddBehavior(new DisplayModel("", display, -1, DisplayCategory.Tower, new Vector3(0, -1, 10)));
            towerModel.displayScale = 1.5f;
            towerModel.isSubTower = true;
            towerModel.AddBehavior(new CreditPopsToParentTowerModel("CreditPopsToParentTowerModel_"));
            towerModel.AddBehavior(new TowerExpireOnParentDestroyedModel(""));
            towerModel.range = 36;

            towerModel.GetAttackModel().range = 36;
            towerModel.GetAttackModel().weapons[0].rate = 1.2f;
            towerModel.GetAttackModel().weapons[0].emission = new RandomEmissionModel("", 3, 30, 0, null, false, 0, 0, 0, false);
            towerModel.GetAttackModel().weapons[0].projectile.ApplyDisplay<NatureSpike>();
            towerModel.GetAttackModel().weapons[0].projectile.pierce = 3;
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = 2;
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Lead;

            var totem = Game.instance.model.GetTowerFromId("ObynGreenfoot 6").GetAttackModel(1).weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
            var sound = totem.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1.Duplicate();
            var effect = totem.GetBehavior<Il2CppAssets.Scripts.Models.Towers.Behaviors.CreateEffectOnExpireModel>().effectModel.Duplicate();

            var mossBomb = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.Duplicate();
            mossBomb.ApplyDisplay<SpiritMoss>();
            mossBomb.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 2;
            mossBomb.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Lead;
            mossBomb.GetBehavior<CreateSoundOnProjectileCollisionModel>().sound1 = sound;
            mossBomb.GetBehavior<CreateSoundOnProjectileCollisionModel>().sound2 = sound;
            mossBomb.GetBehavior<CreateSoundOnProjectileCollisionModel>().sound3 = sound;
            mossBomb.GetBehavior<CreateSoundOnProjectileCollisionModel>().sound4 = sound;
            mossBomb.GetBehavior<CreateSoundOnProjectileCollisionModel>().sound5 = sound;
            mossBomb.GetBehavior<CreateEffectOnContactModel>().effectModel = effect;
            mossBomb.AddBehavior(new CreateProjectileOnContactModel("", towerModel.GetAttackModel().weapons[0].projectile, new ArcEmissionModel("", 8, 0, 360, null, false, false), true, false, false));

            towerModel.GetAttackModel().weapons[0].AddBehavior(new ChangeProjectilePerEmitModel("", towerModel.GetAttackModel().weapons[0].projectile, mossBomb, 6, 6, 5, null, 0, 0, 0));
            towerModel.GetAttackModel().GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }

        public class NatureSpiritDisplay : ModTowerDisplay<NatureSpirit>
        {
            public override float Scale => 1f;
            public override string BaseDisplay => Game.instance.model.GetTowerFromId("DartlingGunner-500").GetAttackModel().weapons[0].projectile.display.guidRef;

            public override bool UseForTower(int[] tiers)
            {
                return true;
            }
            public override void ModifyDisplayNode(UnityDisplayNode node) { }
        }
    }
}