using BTD_Mod_Helper.Api.Towers;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Unity;
using HarmonyLib;
using Il2CppAssets.Scripts.Simulation.SimulationBehaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets.Projectiles;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppNinjaKiwi.Common.ResourceUtils;

namespace OsirisHero
{
    public class Osiris : ModHero
    {
        public override string BaseTower => TowerType.DartMonkey;
        public override string Name => "OsirisHero";
        public override int Cost => 950;
        public override string DisplayName => "Osiris";
        public override string Title => "Keeper of Spirits";
        public override string Level1Description => "Summons spirit Bloons to travel along the track. Spirit Bloons hex regular Bloons making them take more damage temporarily.";
        public override string Description => "After honing his connection with the spirit realm, Osiris is now the protector of the spirits that fight along side him.";
        public override string Icon => VanillaSprites.SpookyBananaFarmerPortrait;
        public override string Portrait => VanillaSprites.SpookyBananaFarmerPortrait;
        public override string Square => "Osiris-ButtonIcon";
        public override string Button => "Osiris-Icon";
        public override string NameStyle => TowerType.Corvus;
        public override int MaxLevel => 20;
        public override float XpRatio => 1.425f;

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            towerModel.RemoveBehavior(towerModel.GetAttackModel());
            towerModel.radius = 7;
            towerModel.range = 50;

            var targetSelect = Game.instance.model.GetTowerFromId("EngineerMonkey-025").GetAttackModel(1).GetBehavior<TargetSelectedPointModel>().Duplicate();
            var ageModel = Game.instance.model.GetTowerFromId("SpikeFactory").GetAttackModel().weapons[0].projectile.GetBehavior<AgeModel>().Duplicate();
            ageModel.lifespan = 9999;
            ageModel.rounds = 999;
            
            var attackModel = Game.instance.model.GetTowerFromId("WizardMonkey-004").GetAttackModel(2).Duplicate();
            attackModel.name = "AttackModel_";
            attackModel.range = towerModel.range;
            attackModel.weapons[0].rate = 8;
            attackModel.weapons[0].emission = new NecromancerEmissionModel("NecromancerEmissionModel_", 1, 1, 1, 1, 1, 1, 0, null, null, null, -1, -1, -1, -1, 0);
            attackModel.weapons[0].projectile.ApplyDisplay<SpiritBloon>();
            attackModel.weapons[0].projectile.pierce = 8;
            attackModel.weapons[0].projectile.GetDamageModel().damage = 1;
            attackModel.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Lead | BloonProperties.Purple;
            attackModel.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespan = 9999;
            attackModel.weapons[0].projectile.AddBehavior(ageModel);
            attackModel.weapons[0].projectile.AddBehavior(new CantBeReflectedModel("CantBeReflectedModel_"));
            attackModel.weapons[0].projectile.AddBehavior(new ClearHitBloonsModel("ClearHitBloonsModel_", 1f));
            attackModel.weapons[0].projectile.AddBehavior(new DontDestroyOnContinueModel("DontDestroyOnContinueModel"));
            attackModel.weapons[0].projectile.AddBehavior(new AddBonusDamagePerHitToBloonModel("AddBonusDamagePerHitToBloonModel_", "SpiritHex", 2, 1, 6, true, false, false, "EziliVoodoo"));
            attackModel.AddBehavior(targetSelect);

            towerModel.AddBehavior(attackModel);
        }
    }
    public class L2 : ModHeroLevel<Osiris>
    {
        public override int Level => 2;
        public override string Description => "Summons spirit Bloons slightly faster.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAttackModel().weapons[0].rate /= 1.1f;
        }
    }
    public class L3 : ModHeroLevel<Osiris>
    {
        public override int Level => 3;
        public override string Description => "Revenant Ability: Summons a slow moving spirit on the track that fires bolts of energy at nearby Bloons.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var sound = Game.instance.model.GetTowerFromId("Corvus 10").GetAbility(2).GetBehavior<CreateSoundOnAbilityModel>().Duplicate();
            sound.heroSound = null;
            sound.heroSound2 = null;

            var ability = Game.instance.model.GetTowerFromId("BombShooter-040").GetAbility().Duplicate();
            ability.icon = CreateSpriteReference(VanillaSprites.BananaFarmerHalloweenIcon);
            ability.cooldown = 45;
            ability.displayName = "Revenant";
            ability.description = "";
            ability.addedViaUpgrade = "Corvus Upgrade 3";
            ability.GetBehavior<ActivateAttackModel>().cancelIfNoTargets = false;
            ability.RemoveBehavior<CreateSoundOnAbilityModel>();
            ability.AddBehavior(sound);

            var projectile = Game.instance.model.GetTowerFromId("WizardMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
            projectile.pierce = 6;
            projectile.GetDamageModel().damage = 4;
            projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
            projectile.GetBehavior<TravelStraitModel>().lifespan *= 2;

            var ageModel = Game.instance.model.GetTowerFromId("SpikeFactory").GetAttackModel().weapons[0].projectile.GetBehavior<AgeModel>().Duplicate();
            ageModel.lifespan = 9999;
            ageModel.rounds = 999;
            
            var attackModel = Game.instance.model.GetTowerFromId("WizardMonkey-004").GetAttackModel(2).Duplicate();
            attackModel.name = "AttackModel_";
            attackModel.range = 9999;
            attackModel.weapons[0].rate = 4;
            attackModel.weapons[0].emission = new NecromancerEmissionModel("NecromancerEmissionModel_", 1, 1, 1, 1, 1, 1, 0, null, null, null, -1, -1, -1, -1, 1);
            attackModel.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("ObynGreenfoot").GetAttackModel().weapons[0].projectile.display;
            attackModel.weapons[0].projectile.pierce = 9999;
            attackModel.weapons[0].projectile.GetDamageModel().damage = 2;
            attackModel.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            attackModel.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespan = 9999;
            attackModel.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speed /= 1.5f;
            attackModel.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().disableRotateWithPathDirection = false;
            attackModel.weapons[0].projectile.AddBehavior(ageModel);
            attackModel.weapons[0].projectile.AddBehavior(new CantBeReflectedModel("CantBeReflectedModel_"));
            attackModel.weapons[0].projectile.AddBehavior(new ClearHitBloonsModel("ClearHitBloonsModel_", 1f));
            //attackModel.weapons[0].projectile.AddBehavior(new DontDestroyOnContinueModel("DontDestroyOnContinueModel"));
            attackModel.weapons[0].projectile.AddBehavior(new CreateProjectileOnIntervalModel
                ("CreateProjectileOnIntervalModel_", projectile, new SingleEmmisionTowardsTargetModel("", null, 0), 20, true, 80, TargetType.First, false, false, false, null));
            
            ability.GetBehavior<ActivateAttackModel>().attacks[0] = attackModel;
            towerModel.AddBehavior(ability);
        }
    }
    public class L4 : ModHeroLevel<Osiris>
    {
        public override int Level => 4;
        public override string Description => "Spirit Bloons have increased damage and pierce.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAttackModel().weapons[0].projectile.pierce += 4;
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
        }
    }
    public class L5 : ModHeroLevel<Osiris>
    {
        public override int Level => 5;
        public override string Description => "Occassionally summons a spirit brute every 10 spirits that deals more damage and pushes small Bloons back.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var spirit = towerModel.GetAttackModel().weapons[0].projectile;
            var brute = spirit.Duplicate();
            brute.ApplyDisplay<SpiritBrute>();
            brute.GetDamageModel().damage += 3;
            brute.AddBehavior(new PushBackModel("PushBackModel_", 40, "", 0, 0, 0, 0, true));

            towerModel.GetAttackModel().weapons[0].AddBehavior(new ChangeProjectilePerEmitModel("ChangeProjectilePerEmitModel_", spirit, brute, 10, 6, 5, null, 0, 0, 0));
        }
    }
    public class L6 : ModHeroLevel<Osiris>
    {
        public override int Level => 6;
        public override string Description => "Spirit Bloons are summoned faster and can now pop Lead Bloons. Hex effect lasts longer.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAttackModel().weapons[0].rate /= 1.2f;
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().lifespan += 1;

            towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
            towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.GetBehavior<AddBonusDamagePerHitToBloonModel>().lifespan += 1;
        }
    }
    public class L7 : ModHeroLevel<Osiris>
    {
        public override int Level => 7;
        public override string Description => "Spirit Guardians Ability: Summon 3 permanent spirits to fight along side Osiris. Each spirit possesses their own unique attacks.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var ability = Game.instance.model.GetTowerFromId("EngineerMonkey-Paragon").GetAbility().Duplicate();
            ability.name = "AbilityModel_Guardians";
            ability.displayName = "Spirit Guardians";
            ability.addedViaUpgrade = "Corvus Upgrade 7";
            ability.cooldown = 10;
            ability.icon = GetSpriteReference("EmberSpirit-Portrait");
            ability.resetCooldownOnTierUpgrade = false;

            ability.GetBehavior<CreateTowersInSequenceAbilityIconModel>().icons[0] = GetSpriteReference("EmberSpirit-Portrait");
            ability.GetBehavior<CreateTowersInSequenceAbilityIconModel>().icons[1] = CreateSpriteReference(VanillaSprites.GenieBottlePortrait);
            ability.GetBehavior<CreateTowersInSequenceAbilityIconModel>().icons[2] = GetSpriteReference("NatureSpirit-Portrait");

            var display = Game.instance.model.GetTowerFromId("DartlingGunner-500").GetAttackModel().weapons[0].projectile.display;
            ability.GetBehavior<ActivateAttackCreateTowerPlacementModel>().attacks[0].RemoveBehavior<RotateToTargetModel>();
            var spirits = ability.GetBehavior<ActivateAttackCreateTowerPlacementModel>().attacks[0].weapons[0].GetBehavior<CreateSequencedTypedTowerCurrentIndexModel>();
            spirits.projectileDisplays = new PrefabReference[] { display, display, display };
            spirits.towers = new TowerModel[] { GetTowerModel<EmberSpirit>().Duplicate(), GetTowerModel<FrostSpirit>().Duplicate(), GetTowerModel<NatureSpirit>().Duplicate() };
            
            towerModel.AddBehavior(ability);
        }
    }
    public class L8 : ModHeroLevel<Osiris>
    {
        public override int Level => 8;
        public override string Description => "Revenant spirit is stronger. Spirit Bloons have more pierce and remove Camo properties from Bloons.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var ability = towerModel.GetAbility();
            ability.GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetDamageModel().damage += 2;
            ability.GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetBehavior<CreateProjectileOnIntervalModel>().projectile.GetDamageModel().damage += 2;
            ability.GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetBehavior<CreateProjectileOnIntervalModel>().intervalFrames -= 5;

            towerModel.GetAttackModel().weapons[0].projectile.pierce += 4;
            towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new int[] { 0, -1 }; ;
            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new RemoveBloonModifiersModel("", false, true, false, false, true, new string[] { }, new string[] { }));

            towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.pierce += 4;
            towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.collisionPasses = new int[] { 0, -1 }; ;
            towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.
                AddBehavior(new RemoveBloonModifiersModel("", false, true, false, false, true, new string[] { }, new string[] { }));
        }
    }
    public class L9 : ModHeroLevel<Osiris>
    {
        public override int Level => 9;
        public override string Description => "Spirit brutes are summoned every 8th spirit. Hexed Bloons recieve more additional damage.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().forProjectileCount -= 2;

            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().perHitDamageAddition += 1;
            towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.GetBehavior<AddBonusDamagePerHitToBloonModel>().perHitDamageAddition += 1;
        }
    }
    public class L10 : ModHeroLevel<Osiris>
    {
        public override int Level => 10;
        public override string Description => "Plumb the Void Ability: Opens a portal that sucks all nearby Bloons in and damages them continuously before releasing them back onto the track.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var sound = Game.instance.model.GetTowerFromId("Corvus 10").GetAbility(2).GetBehavior<CreateSoundOnAbilityModel>().Duplicate();
            sound.heroSound = null;
            sound.heroSound2 = null;

            var ability = Game.instance.model.GetTowerFromId("DartlingGunner-040").GetAbility().Duplicate();
            ability.icon = GetSpriteReference("PlumbTheVoid-Icon");
            ability.cooldown = 90;
            ability.displayName = "Plumb The Void";
            ability.description = "";
            ability.addedViaUpgrade = "Corvus Upgrade 10";
            ability.GetBehavior<ActivateAttackModel>().isOneShot = true;
            ability.GetBehavior<ActivateAttackModel>().lifespan = 8;
            ability.RemoveBehavior<CreateSoundOnAbilityModel>();
            ability.AddBehavior(sound);

            var damageProj = Game.instance.model.GetTowerFromId("TackShooter-400").GetAttackModel().weapons[0].projectile.Duplicate();
            damageProj.ApplyDisplay<LightningDisplay>();
            damageProj.scale *= 1.8f;
            damageProj.radius = 60;
            damageProj.pierce = 999;
            damageProj.GetDamageModel().damage = 10;
            damageProj.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            damageProj.RemoveBehavior<FreezeModel>();
            damageProj.GetBehavior<AgeModel>().lifespan = 8;
            damageProj.GetDescendants<FilterBloonIfDamageTypeModel>().ForEach(model => model.ifCantHitBloonProperties = BloonProperties.None);
            damageProj.AddBehavior(new ClearHitBloonsModel("", 0.25f));
            damageProj.AddBehavior(new RefreshPierceModel("", 0.5f, false));
            
            var vortex = Game.instance.model.GetTowerFromId("TranceTotem").GetAttackModel().Duplicate();
            vortex.range = towerModel.range;
            vortex.weapons[0].fireWithoutTarget = true;
            vortex.weapons[0].startInCooldown = false;
            vortex.weapons[0].GetBehavior<EjectEffectModel>().lifespan = 8;
            vortex.weapons[0].projectile.pierce = 10;
            vortex.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("TranceTotem").GetAttackModel().weapons[0].GetBehavior<EjectEffectModel>().effectModel.assetId;
            vortex.weapons[0].projectile.GetBehavior<AgeModel>().Lifespan = 8;
            vortex.weapons[0].projectile.GetBehavior<TranceBloonModel>().orbitRadius = 50;
            vortex.weapons[0].projectile.GetBehavior<TranceBloonModel>().duration = 4;
            vortex.weapons[0].projectile.RemoveBehavior<RemoveBloonModifiersModel>();
            
            var damage = ability.GetBehavior<ActivateAttackModel>().attacks[0].Duplicate();
            damage.range = 60;
            damage.weapons[0].rate = 10;
            damage.weapons[0].ejectX = 0;
            damage.weapons[0].ejectY = 0;
            damage.weapons[0].ejectZ = 1;
            damage.weapons[0].emission = new SingleEmissionModel("", null);
            damage.weapons[0].RemoveBehavior<CreateSoundOnProjectileCreatedModel>();
            damage.weapons[0].projectile = damageProj;

            ability.GetBehavior<ActivateAttackModel>().attacks = new AttackModel[] { damage, vortex };
            towerModel.AddBehavior(ability);
        }
    }
    public class L11 : ModHeroLevel<Osiris>
    {
        public override int Level => 11;
        public override string Description => "Spirit Guardians grow in strength. (Note: replace spirits to get effect)";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var towers = towerModel.GetAbility(1).GetBehavior<ActivateAttackCreateTowerPlacementModel>().attacks[0].weapons[0].GetBehavior<CreateSequencedTypedTowerCurrentIndexModel>();
            var emberSpirit = towers.towers[0];
            var frostSpirit = towers.towers[1];
            var natureSpirit = towers.towers[2];

            emberSpirit.GetAttackModel().weapons[0].rate /= 1.25f;
            emberSpirit.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
            var fireWall = emberSpirit.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel;
            fireWall.GetDamageModel().damage += 2;
            fireWall.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage += 1;
            fireWall.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<ClearHitBloonsModel>().interval -= 0.05f;

            frostSpirit.GetAttackModel().weapons[0].rate /= 1.15f;
            frostSpirit.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
            var iceBomb = frostSpirit.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel;
            iceBomb.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 2;

            natureSpirit.GetAttackModel().weapons[0].rate /= 1.2f;
            natureSpirit.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
            var mossBomb = natureSpirit.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel;
            mossBomb.GetDescendants<CreateProjectileOnContactModel>().ForEach(model => model.projectile.GetDamageModel().damage += 1);
        }
    }
    public class L12 : ModHeroLevel<Osiris>
    {
        public override int Level => 12;
        public override string Description => "Summons spirit Bloons faster. Spirit Bloons have a chance to apply a sigil to Bloons that explodes after a short delay.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAttackModel().weapons[0].rate /= 1.2f;

            var damageModifier = new DamageModifierForTagModel("MoabModifier_", "Moabs", 1, 30, false, false);
            var sigil = Game.instance.model.GetTowerFromId("NinjaMonkey-004").GetAttackModel(2).weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();
            sigil.mutationId = "HexSigil";
            sigil.lifespan = 2;
            sigil.overlayType = "EziliHex";
            sigil.chance = 0.25f;
            sigil.GetBehavior<DamageOverTimeModel>().name = "Sigil";
            sigil.GetBehavior<DamageOverTimeModel>().damage = 10;
            sigil.GetBehavior<DamageOverTimeModel>().interval = 1.95f;
            sigil.GetBehavior<DamageOverTimeModel>().damageModifierModels = new DamageModifierModel[] { damageModifier };
            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(sigil);
        }
    }
    public class L13 : ModHeroLevel<Osiris>
    {
        public override int Level => 13;
        public override string Description => "Revenant spirit is stronger. Spirit Bloons deal more damage.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var ability = towerModel.GetAbility();
            ability.GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetDamageModel().damage += 2;
            ability.GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetBehavior<CreateProjectileOnIntervalModel>().projectile.GetDamageModel().damage += 2;

            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
            towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.GetDamageModel().damage += 2;
        }
    }
    public class L14 : ModHeroLevel<Osiris>
    {
        public override int Level => 14;
        public override string Description => "Plumb the Void is more powerful.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAbility(2).GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetDamageModel().damage += 6;
            towerModel.GetAbility(2).GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetBehavior<ClearHitBloonsModel>().interval -= 0.05f;
        }
    }
    public class L15 : ModHeroLevel<Osiris>
    {
        public override int Level => 15;
        public override string Description => "Spirit brutes are summoned every 6th spirit and can now knockback MOABs.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().forProjectileCount -= 2;
            var pushback = towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.GetBehavior<PushBackModel>();
            pushback.multiplierMOAB = 0.25f;
            pushback.multiplierBFB = 0.2f;
            pushback.multiplierDDT = 0.2f;
            pushback.multiplierZOMG = 0.1f;
        }
    }
    public class L16 : ModHeroLevel<Osiris>
    {
        public override int Level => 16;
        public override string Description => "Spirit Guardians grow in strength further.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var towers = towerModel.GetAbility(1).GetBehavior<ActivateAttackCreateTowerPlacementModel>().attacks[0].weapons[0].GetBehavior<CreateSequencedTypedTowerCurrentIndexModel>();
            var emberSpirit = towers.towers[0];
            var frostSpirit = towers.towers[1];
            var natureSpirit = towers.towers[2];

            emberSpirit.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().forProjectileCount -= 4;
            emberSpirit.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
            var fireWall = emberSpirit.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel;
            fireWall.GetDamageModel().damage += 1;
            fireWall.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage += 1;
            fireWall.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<ClearHitBloonsModel>().interval -= 0.05f;

            frostSpirit.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().forProjectileCount -= 2;
            frostSpirit.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 2;
            var iceBomb = frostSpirit.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel;
            iceBomb.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 2;

            natureSpirit.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().forProjectileCount -= 2;
            natureSpirit.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
            var mossBomb = natureSpirit.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel;
            mossBomb.GetDescendants<CreateProjectileOnContactModel>().ForEach(model => model.projectile.GetDamageModel().damage += 1);
        }
    }
    public class L17 : ModHeroLevel<Osiris>
    {
        public override int Level => 17;
        public override string Description => "Plumb the Void is much more powerful.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAbility(2).GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetDamageModel().damage += 12;
        }
    }
    public class L18 : ModHeroLevel<Osiris>
    {
        public override int Level => 18;
        public override string Description => "Spirit sigils have increased chance and deal increased damage. Hexed Bloons recieve even more additional damage.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var damageModifier = new DamageModifierForTagModel("MoabModifier_", "Moabs", 1, 53, false, false);
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().chance *= 2;
            var dot = towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>();
            dot.damage += 2;
            dot.damageModifierModels = new DamageModifierModel[] { damageModifier };

            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().perHitDamageAddition += 1;
            towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.GetBehavior<AddBonusDamagePerHitToBloonModel>().perHitDamageAddition += 1;
        }
    }
    public class L19 : ModHeroLevel<Osiris>
    {
        public override int Level => 19;
        public override string Description => "Summons spirit Bloons significantly faster.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAttackModel().weapons[0].rate /= 2f;
        }
    }
    public class L20 : ModHeroLevel<Osiris>
    {
        public override int Level => 20;
        public override string Description => "Spirit Guardians reach their true power. Revenant ability summons a second spirit shortly after the first. Plumb the Void cooldown decreased.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var ability = towerModel.GetAbility(0);
            ability.GetBehavior<ActivateAttackModel>().isOneShot = false;
            ability.GetBehavior<ActivateAttackModel>().lifespan = 4.5f;


            var towers = towerModel.GetAbility(1).GetBehavior<ActivateAttackCreateTowerPlacementModel>().attacks[0].weapons[0].GetBehavior<CreateSequencedTypedTowerCurrentIndexModel>();
            var emberSpirit = towers.towers[0];
            var frostSpirit = towers.towers[1];
            var natureSpirit = towers.towers[2];

            emberSpirit.GetAttackModel().weapons[0].rate /= 1.5f;
            emberSpirit.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 2;
            var fireWall = emberSpirit.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel;
            fireWall.GetDamageModel().damage += 2;
            fireWall.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage += 1;
            fireWall.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<ClearHitBloonsModel>().interval -= 0.05f;

            frostSpirit.GetAttackModel().weapons[0].rate /= 1.25f;
            frostSpirit.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 2;
            var iceBomb = frostSpirit.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel;
            iceBomb.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 2;

            natureSpirit.GetAttackModel().weapons[0].rate /= 1.25f;
            natureSpirit.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
            var mossBomb = natureSpirit.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel;
            mossBomb.GetDescendants<CreateProjectileOnContactModel>().ForEach(model => model.projectile.GetDamageModel().damage += 1);


            towerModel.GetAbility(2).cooldown = 60;
        }
    }
    public class OsirisDisplay : ModTowerDisplay<Osiris>
    {
        public override float Scale => 1f;
        public override string BaseDisplay => "191cc21b4fb5dfa4ba4b81565d2a5d4c";

        public override bool UseForTower(int[] tiers)
        {
            return true;
        }
        public override void ModifyDisplayNode(UnityDisplayNode node) { }
    }
    [HarmonyPatch(typeof(NecroData), nameof(NecroData.RbePool))]
    internal static class Necro_RbePool
    {
        [HarmonyPrefix]
        private static bool Postfix(NecroData __instance, ref int __result)
        {
            var tower = __instance.tower;
            if (tower.towerModel.name.Contains("OsirisHero"))
            {
                __result = 9999;                
            }
            return false;
        }
    }
}