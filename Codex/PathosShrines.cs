using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexShrines : CodexPage<ManifestShrines, ShrineEditor, Shrine>
  {
    private CodexShrines() { }
#if MASTER_CODEX
    internal CodexShrines(Codex Codex)
      : base(Codex.Manifest.Shrines)
    {
      var Genders = Codex.Genders;
      var Entities = Codex.Entities;
      var Features = Codex.Features;
      var Items = Codex.Items;
      var Strikes = Codex.Strikes;
      var Sanctities = Codex.Sanctities;
      var Elements = Codex.Elements;
      var Glyphs = Codex.Glyphs;
      var Sonics = Codex.Sonics;
      var Skills = Codex.Skills;
      var Attributes = Codex.Attributes;

      var MaleNameArray = new string[]
      {
        "Atral",
        "Bolan",
        "Cazedda",
        "Damano",
        "Ellent",
        "Faraze",
        "Gomack",
        "Haquian",
        "Ishgood",
        "Jeem",
        "Karbri",
        "Lant",
        "Melchai",
        "Norbant",
        "Ollac",
        "Paltrick",
        "Quibb",
        "Rambrant",
        "Sully",
        "Trank",
        "Uffle",
        "Vorzian",
        "Waltis",
        "Xamarin",
        "Yassage",
        "Zimn"
      };

      var FemaleNameArray = new string[]
      {
        "Atrya",
        "Belietha",
        "Cailie",
        "D'rey",
        "Emina",
        "Franzia",
        "Gmima",
        "Haique",
        "Illitasha",
        "Jessera",
        "Kierantha",
        "Lesha",
        "Meloty",
        "Ninerma",
        "Oscae",
        "Penevieve",
        "Quarah",
        "Rebenna",
        "Sarlia",
        "Tanyacka",
        "Umazi",
        "Viophie",
        "Wendry",
        "Xiolet",
        "Yorque",
        "Zultry"
      };

      Shrine AddShrine(string Name, Entity ShrineEntity, Glyph Glyph, Sonic Sonic, int Rarity, Action<ShrineEditor> Action)
      {
        return Register.Add(S =>
        {
          S.Name = Name;
          S.Glyph = Glyph;
          S.Sonic = Sonic;
          S.Rarity = Rarity;
          S.KeeperEntity = ShrineEntity;

          CodexRecruiter.Enrol(() =>
          {
            S.SetKeeperNames(ShrineEntity.Genders.Count == 1 && ShrineEntity.Genders[0] == Genders.female ? FemaleNameArray : MaleNameArray);

            Action(S);
          });
        });
      }

      holy_shrine = AddShrine("holy shrine", Entities.holy_cleric, Glyphs.holy_shrine, Sonics.bell, 30, S =>
      {
        S.KeeperFeature = Features.altar;

        S.AddBoon("divine", B =>
        {
          B.Description = "Learn the divine status of all carried items.";
          B.Cost = 50;
          B.SetCast().Strike(Strikes.magic, Dice.Zero);
          B.Apply.DivineItem();
        });

        S.AddBoon("rejuvenate", B =>
        {
          B.Description = "Heal damage and recover mana for yourself and your steed if mounted.";
          B.Cost = 100;
          B.SetCast().Strike(Strikes.spirit, Dice.One);
          B.Apply.HealEntity(Dice.Fixed(100), Modifier.Zero);
          B.Apply.EnergiseEntity(Dice.Fixed(100), Modifier.Zero);
        });

        S.AddBoon("remove curse", B =>
        {
          B.Description = "Remove the curse on one equipped or carried item.";
          B.Cost = 200;
          B.SetCast().FilterSanctity(Sanctities.Cursed);
          B.Apply.RemoveCurse(Dice.One, Sanctities.Uncursed);
        });

        S.AddBoon("bless", B =>
        {
          B.Description = "Bless one item in your inventory or on the ground.";
          B.Cost = 250;
          B.SetCast().FilterSanctity(Sanctities.Uncursed);
          B.Apply.Sanctify(Item: null, Sanctities.Blessed);
        });

        S.AddBoon("purify", B =>
        {
          B.Description = "Restore any lost ability and remove any negative transient conditions.";
          B.Cost = 150;
          B.SetCast().Strike(Strikes.spirit, Dice.One);
          B.Apply.UnafflictEntity();
          B.Apply.UnpunishEntity();
          B.Apply.UnpolymorphEntity();
          B.Apply.RestoreAbility();
          B.Apply.RemoveTransient(Codex.Properties.List.Where(P => P.Unwanted).ToArray());
        });

        S.AddBoon("raise dead", B =>
        {
          B.Description = "Return a corpse back to life.";
          B.Cost = 750;
          B.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
          B.Apply.RaiseDeadEntity(Percent: 50, CorruptProperty: null, CorruptDice: Dice.Zero, LoyalOnly: false);
        });
      });

      dark_sepulchre = AddShrine("dark sepulchre", Entities.dark_cleric, Glyphs.dark_sepulchre, Sonics.bell, 10, S =>
      {
        S.KeeperFeature = Features.grave; // or sarcophagus?

        //S.AddBoon("body disposal", B =>
        //{
        //  B.Description = "Discrete removal of your dearly departed and other unwanted corpses.";
        //  B.Cost = 50;
        //  B.Apply.CreateAsset(Dice.One, new[] { Items.animal_corpse, Items.vegetable_corpse });
        //});

        S.AddBoon("corpse wish", B =>
        {
          B.Description = "Request a fresh corpse with no questions asked.";
          B.Cost = 100;
          B.Apply.CreateItem(Dice.One, QuantityDice: null, [Items.animal_corpse, Items.vegetable_corpse]);
        });

        S.AddBoon("tinned meat", B =>
        {
          B.Description = "Butcher a corpse into an easy to carry container.";
          B.Cost = 150;
          B.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
          B.Apply.Tinning(Codex.Items.tin);
        });

        S.AddBoon("animate dead", B =>
        {
          B.Description = "Reanimate a corpse as a loyal revenant.";
          B.Cost = 500;
          B.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
          B.Apply.AnimateRevenant(CorruptProperty: null, CorruptDice: 6.d10());
        });

        S.AddBoon("undead army", B =>
        {
          B.Description = "Call upon the undead to fight for you.";
          B.Cost = 750;
          B.Apply.SummonEntity(1.d3(), Entities.human_zombie, Entities.elf_zombie, Entities.dwarf_zombie, Entities.orc_zombie, Entities.gnome_zombie);
          B.Apply.SummonEntity(1.d2(), Entities.ghoul);
          B.Apply.SummonEntity(Dice.One, Entities.human_mummy, Entities.elf_mummy, Entities.dwarf_mummy, Entities.orc_mummy, Entities.gnome_mummy);
        });

        S.AddBoon("nightmare steed", B =>
        {
          B.Description = "Summon a powerful steed from the underworld.";
          B.Cost = 1000;
          B.Apply.SummonEntity(Dice.Fixed(1), Entities.nightmare);
        });
      });

      sacred_grove = AddShrine("sacred grove", Entities.dryad, Glyphs.sacred_grove, Sonics.bell, 20, S =>
      {
        S.KeeperFeature = Features.fountain;

        S.AddBoon("erase", B =>
        {
          B.Description = "Erase the magic from a potion, scroll or book.";
          B.Cost = 50;
          B.SetCast().FilterStock(Codex.Stocks.potion, Codex.Stocks.scroll, Codex.Stocks.book);
          B.Apply.ConvertItem(Codex.Stocks.potion, WholeStack: true, Items.potion_of_water);
          B.Apply.ConvertItem(Codex.Stocks.scroll, WholeStack: true, Items.scroll_of_blank_paper);
          B.Apply.ConvertItem(Codex.Stocks.book, WholeStack: true, Items.book_of_blank_paper);
        });

        S.AddBoon("hatch", B =>
        {
          B.Description = "Hatch an egg and become a parent.";
          B.Cost = 100;
          B.SetCast().FilterItem(Codex.Items.egg);
          B.Apply.HatchEgg();
        });

        S.AddBoon("grow", B =>
        {
          B.Description = "Use nature magic to grow the power of your ally.";
          B.Cost = 200;
          B.SetCast().Strike(Strikes.spirit, Dice.One);
          B.Apply.GrowEntity();
        });

        S.AddBoon("bless", B =>
        {
          B.Description = "Bless one item in your inventory or on the ground.";
          B.Cost = 250;
          B.SetCast().FilterSanctity(Sanctities.Uncursed);
          B.Apply.Sanctify(Item: null, Sanctities.Blessed);
        });

        S.AddBoon("unicorn friend", B =>
        {
          B.Description = "Ask for a unicorn to join your party.";
          B.Cost = 500;
          B.Apply.SummonEntity(Dice.Fixed(1), Entities.white_unicorn, Entities.grey_unicorn, Entities.black_unicorn);
        });

        S.AddBoon("fantastical beast", B =>
        {
          B.Description = "Summon a tame forest beast as your ally.";
          B.Cost = 750;
          B.Apply.SummonEntity(Dice.Fixed(1),
            Entities.kirin,
            Entities.wyvern,
            Entities.wolverine,
            Entities.sasquatch,
            Entities.forest_centaur,
            Entities.displacer_beast,
            Entities.owlbear,
            Entities.minotaur_lord,
            Entities.guardian_naga,
            Entities.ettin,
            Entities.basilisk);
        });
      });

      mystic_coven = AddShrine("mystic coven", Entities.witch, Glyphs.mystic_coven, Sonics.bell, 20, S =>
      {
        S.KeeperFeature = Features.pentagram;

        S.AddBoon("place curse", B =>
        {
          B.Description = "Place a curse on one equipped or carried item.";
          B.Cost = 50;
          B.SetCast().FilterSanctity(Sanctities.Uncursed, Sanctities.Blessed);
          B.Apply.PlaceCurse(Dice.One, Sanctities.Cursed);
        });

        S.AddBoon("scribe", B =>
        {
          B.Description = "Write a random scroll onto your blank paper.";
          B.Cost = 250;
          B.SetCast().FilterItem(Items.scroll_of_blank_paper)
           .SetAssetIndividualised();
          B.Apply.ConvertItem(Codex.Stocks.scroll, WholeStack: false, Codex.Stocks.scroll.Items.Where(I => I != Items.scroll_of_blank_paper && !I.Grade.Unique && I.Rarity > 0).ToArray());
        });

        S.AddBoon("brew", B =>
        {
          B.Description = "Brew a random potion into your bottle of water.";
          B.Cost = 250;
          B.SetCast().FilterItem(Items.potion_of_water)
           .SetAssetIndividualised();
          B.Apply.ConvertItem(Codex.Stocks.potion, WholeStack: false, Codex.Stocks.potion.Items.Where(I => I != Items.potion_of_water && !I.Grade.Unique && I.Rarity > 0).ToArray());
        });

        S.AddBoon("polymorph", B =>
        {
          B.Description = "Request one cast of the polymorph spell.";
          B.Cost = 500;
          B.SetCast().Plain(Dice.One)
           .SetObjects(false);
          B.Apply.PolymorphEntity();
        });

        S.AddBoon("teach spell", B =>
        {
          B.Description = "Learn how to cast a random spell.";
          B.Cost = 750;
          B.SetCast().Plain(Dice.One);
          B.Apply.LearnSpell(Attributes.intelligence, Skills.literacy, Spell: null);
        });

        //S.AddBoon("life insurance", B =>
        //{
        //  B.Description = "Apply a policy that will save your life.";
        //  B.Cost = 1000;
        //  B.SetCast().Plain(Dice.One);
        //  B.Apply.ApplyTransient(Property.Lifesaving, Dice.Fixed(2000));
        //});
      });

      craft_station = AddShrine("craft station", Entities.artisan, Glyphs.craft_station, Sonics.bell, 20, S =>
      {
        S.KeeperFeature = Features.workbench;

        S.AddBoon("assess", B =>
        {
          B.Description = "Determine the enchantment and charges of all carried items.";
          B.Cost = 50;
          B.SetCast().Strike(Strikes.psychic, Dice.Zero);
          B.Apply.AssessItem();
        });

        S.AddBoon("rename", B =>
        {
          B.Description = "Rename an item for vanity and as a small protection against cancellation.";
          B.Cost = 100;
          B.SetCast().FilterAnyItem()
           .SetAssetIndividualised(false)
           .FilterCoins(false);
          B.AssetMotion = Codex.Motions.rename;
          B.Apply.Nothing();
        });

        // not useful until inscribing is mainstream, also a bit too similar in name with 'scribe' boon.
        //S.AddBoon("inscribe", B =>
        //{
        //  B.Description = "Write, engrave or carve something onto your item.";
        //  B.Cost = 100;
        //  B.Cast = Cast.AnyItem();
        //  B.AssetMotion = Motion.Inscribe;
        //  B.Apply.Nothing();
        //});

        S.AddBoon("enlighten", B =>
        {
          B.Description = "Learn the name of one random unknown item.";
          B.Cost = 100;
          B.Apply.DiscoverItem(null);
        });

        S.AddBoon("cancel", B =>
        {
          B.Description = "Remove all magic from an item.";
          B.Cost = 250;
          B.SetCast().FilterAnyItem();
          B.Apply.Cancellation(Elements.magical);
        });

        S.AddBoon("recharge", B =>
        {
          B.Description = "Partially recharge a spent item.";
          B.Cost = 250;
          B.SetCast().FilterCharged();
          B.Apply.ChargingItem(Dice.One, Dice.Fixed(75)); // 75%
        });

        S.AddBoon("reforge", B =>
        {
          B.Description = "Convert one item into a random alternative of the same type.";
          B.Cost = 500;
          B.SetCast()
           .FilterEquipped(false) // otherwise can reforge an equipped one-handed weapon into a two-handed weapon.
           .FilterUniques(false)
           .FilterCoins(false);
          B.Apply.PolymorphItem();
        });

        S.AddBoon("enchant", B =>
        {
          B.Description = "Upgrade an item or increase its power.";
          B.Cost = 500;
          B.SetCast().FilterEnchanted()
           .SetAssetIndividualised();
          B.Apply.EnchantItemUp(Dice.Fixed(+1));
        });

        S.AddBoon("replicate", B =>
        {
          B.Description = "Make an imitation copy of an item.";
          B.Cost = 1000;
          B.SetCast().FilterAnyItem();
          B.Apply.ReplicateItem();
        });
      });

      Register.Alias(holy_shrine, "altar");
      // >>> GENERATED SHRINES >>>
      martial_temple = AddShrine("martial temple", Entities.army_captain, Glyphs.holy_shrine, Sonics.bell, 20, S =>
      {
        S.KeeperFeature = Features.stall;

        S.AddBoon("sharpen blade", B =>
        {
          B.Description = "Have the temple armour smiths put a keener edge on one weapon, magical or not.";
          B.Cost = 200;
          B.SetCast().FilterStock(Codex.Stocks.weapon)
           .SetAssetIndividualised();
          B.Apply.EnchantItemUp(Dice.Fixed(+1));
        });

        S.AddBoon("shield wall", B =>
        {
          B.Description = "Stand in the temple square and receive the captain's defensive blessing.";
          B.Cost = 150;
          B.SetCast().Strike(Strikes.magic, Dice.Zero);
          B.Apply.ApplyTransient(Codex.Properties.deflection, 1.d15() + 16);
        });

        S.AddBoon("forced march", B =>
        {
          B.Description = "Drill under the captain's eye. You may be quickened, or you may be worn out.";
          B.Cost = 100;
          B.SetCast().Strike(Strikes.magic, Dice.Zero);
          B.Apply.WhenChance(Chance.OneIn2,
            T => T.ApplyTransient(Codex.Properties.quickness, 4.d6() + 4),
            F => F.ApplyTransient(Codex.Properties.slowness, 4.d6() + 4));
        });

        S.AddBoon("battle fury", B =>
        {
          B.Description = "Drink the warrior's drink and fight in a berserker rage. It does not spare your allies.";
          B.Cost = 250;
          B.SetCast().Strike(Strikes.magic, Dice.Zero);
          B.Apply.ApplyTransient(Codex.Properties.rage, 3.d20());
        });

        S.AddBoon("drill", B =>
        {
          B.Description = "Train under a veteran until a weapon or armour discipline sticks.";
          B.Cost = 600;
          B.SetCast().Plain(Dice.One);
          B.Apply.GainSkill(RandomPoints: false, Skills.axe, Skills.heavy_blade, Skills.medium_blade,
            Skills.mace, Skills.hammer, Skills.spear, Skills.polearm, Skills.flail,
            Skills.heavy_armour, Skills.medium_armour);
        });

        S.AddBoon("call the escort", B =>
        {
          B.Description = "Requisition soldiers from the garrison to march at your side.";
          B.Cost = 500;
          B.Apply.SummonEntity(1.d2(), Entities.army_soldier, Entities.army_sergeant, Entities.army_lieutenant);
        });
      });

      pheral_vault = AddShrine("pheral vault", Entities.pheral_vizier, Glyphs.dark_sepulchre, Sonics.bell, 15, S =>
      {
        S.KeeperFeature = Features.sarcophagus;

        S.AddBoon("commune with the dead", B =>
        {
          B.Description = "Whisper a question into the vault. The dead do not always tell the truth.";
          B.Cost = 150;
          B.SetCast().Strike(Strikes.spirit, Dice.Zero);
          B.Apply.Rumour(Attributes.wisdom, Skills.literacy, Truth: true, Lies: true);
        });

        S.AddBoon("sacred wrappings", B =>
        {
          B.Description = "Receive the linen wrappings used to prepare the dead for the afterlife.";
          B.Cost = 100;
          B.Apply.CreateItem(Dice.One, QuantityDice: null, [Items.mummy_wrapping]);
        });

        S.AddBoon("tomb guardian", B =>
        {
          B.Description = "Petition the vizier for a spectral cat to guard you as it once guarded the tomb.";
          B.Cost = 400;
          B.Apply.SummonEntity(Dice.Fixed(1), Entities.pheral_khit, Entities.pheral_sentinel);
        });

        S.AddBoon("court of the underworld", B =>
        {
          B.Description = "Summon an escort from the pharaoh's retinue to walk beside you.";
          B.Cost = 700;
          B.Apply.SummonEntity(Dice.Fixed(1), Entities.ashen_mummy, Entities.bone_reaper, Entities.frost_wraith);
        });

        S.AddBoon("black rite of revival", B =>
        {
          B.Description = "A darker resurrection than the temples offer: certain to succeed, but the dead come back enraged.";
          B.Cost = 600;
          B.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
          B.Apply.RaiseDeadEntity(Percent: 100, CorruptProperty: Codex.Properties.rage, CorruptDice: 6.d10(), LoyalOnly: false);
        });
      });

      elemental_forge = AddShrine("elemental forge", Entities.efreeti, Glyphs.craft_station, Sonics.bell, 20, S =>
      {
        S.KeeperFeature = Features.workbench;

        S.AddBoon("temper in flame", B =>
        {
          B.Description = "Step into the forge fire. It burns, but you rise with fire in your blood.";
          B.Cost = 350;
          B.SetCast().Strike(Strikes.flame, Dice.Zero);
          B.Apply.HarmEntity(Elements.fire, 3.d6());
          B.Apply.MajorResistance(Elements.fire);
        });

        S.AddBoon("temper in frost", B =>
        {
          B.Description = "Plunge into the quenching trough. It burns with cold, but you rise proof against it.";
          B.Cost = 350;
          B.SetCast().Strike(Strikes.frost, Dice.Zero);
          B.Apply.HarmEntity(Elements.cold, 3.d6());
          B.Apply.MajorResistance(Elements.cold);
        });

        S.AddBoon("temper in acid", B =>
        {
          B.Description = "Bathe in the etching vats. It burns, but you rise with a hide the acid cannot bite.";
          B.Cost = 350;
          B.SetCast().Strike(Strikes.acid, Dice.Zero);
          B.Apply.HarmEntity(Elements.acid, 3.d6());
          B.Apply.MajorResistance(Elements.acid);
        });

        S.AddBoon("ordeal of embers", B =>
        {
          B.Description = "Toss a coal on the forge fire and take whatever ward it grants you.";
          B.Cost = 200;
          B.SetCast().Strike(Strikes.magic, Dice.Zero);
          B.Apply.WhenProbability(Table =>
          {
            Table.Add(1, A => A.MinorResistance(Elements.poison));
            Table.Add(1, A => A.MinorResistance(Elements.disintegrate));
            Table.Add(1, A => A.MinorResistance(Elements.drain));
            Table.Add(1, A => A.MinorResistance(Elements.petrify));
            Table.Add(1, A => A.MinorResistance(Elements.sleep));
          });
        });

        S.AddBoon("bound elemental", B =>
        {
          B.Description = "Have the efreeti bind a captive elemental to your service.";
          B.Cost = 800;
          B.Apply.SummonEntity(Dice.Fixed(1), Entities.acid_sphere, Entities.sonic_sphere,
            Entities.explosive_sphere, Entities.air_sphere);
        });
      });

      star_observatory = AddShrine("star observatory", Entities.elder_wizard, Glyphs.mystic_coven, Sonics.bell, 20, S =>
      {
        S.KeeperFeature = Features.pentagram;

        S.AddBoon("identify all", B =>
        {
          B.Description = "Have the wizard read the true nature of everything you carry.";
          B.Cost = 600;
          B.SetCast().Strike(Strikes.psychic, Dice.Zero);
          B.Apply.IdentifyItem(All: true, Sanctity: null);
        });

        S.AddBoon("chart the level", B =>
        {
          B.Description = "Consult the observatory's charts for the full layout of this level.";
          B.Cost = 300;
          B.SetCast().Strike(Strikes.magic, Dice.Zero);
          B.Apply.Mapping(Range.Sq30, Chance.Always);
        });

        S.AddBoon("map the ranks", B =>
        {
          B.Description = "Watch the level through the great lens and mark every creature on it.";
          B.Cost = 250;
          B.SetCast().Strike(Strikes.magic, Dice.Zero);
          B.Apply.DetectEntity(Range.Sq30);
        });

        S.AddBoon("clairvoyant trance", B =>
        {
          B.Description = "Fall into a trance and see through walls and floors for a time.";
          B.Cost = 200;
          B.SetCast().Strike(Strikes.magic, Dice.Zero);
          B.Apply.ApplyTransient(Codex.Properties.clairvoyance, 6.d6());
        });

        S.AddBoon("read your destiny", B =>
        {
          B.Description = "Have your destiny read from the stars, hastening your growth.";
          B.Cost = 1200;
          B.SetCast().Strike(Strikes.magic, Dice.Zero);
          B.Apply.GainLevel(Dice.One, RandomExperience: true);
        });
      });

      fae_chapel = AddShrine("fae chapel", Entities.Nymph_Princess, Glyphs.sacred_grove, Sonics.bell, 10, S =>
      {
        S.KeeperFeature = Features.throne;

        S.AddBoon("fae grace", B =>
        {
          B.Description = "The princess trades you a measure of charm. Something else must give way for it.";
          B.Cost = 150;
          B.SetCast().Strike(Strikes.magic, Dice.Zero);
          B.Apply.TradeoffAbility(Attributes.charisma, Attributes.strength);
        });

        S.AddBoon("fae cunning", B =>
        {
          B.Description = "The princess trades you a measure of wit. Something else must give way for it.";
          B.Cost = 150;
          B.SetCast().Strike(Strikes.magic, Dice.Zero);
          B.Apply.TradeoffAbility(Attributes.intelligence, Attributes.constitution);
        });

        S.AddBoon("fae guile", B =>
        {
          B.Description = "The princess trades you a measure of grace. Something else must give way for it.";
          B.Cost = 150;
          B.SetCast().Strike(Strikes.magic, Dice.Zero);
          B.Apply.TradeoffAbility(Attributes.dexterity, Attributes.wisdom);
        });

        S.AddBoon("capricious wish", B =>
        {
          B.Description = "Ask the fae for a favour. What you get is entirely up to her mood.";
          B.Cost = 100;
          B.SetCast().Strike(Strikes.magic, Dice.Zero);
          B.Apply.WhenProbability(Table =>
          {
            Table.Add(1, A => A.HealEntity(Dice.Fixed(50), Modifier.Zero));
            Table.Add(1, A => A.HarmEntity(Elements.magical, 2.d6()));
            Table.Add(1, A => A.IncreaseOneAbility(Dice.One));
            Table.Add(1, A => A.DecreaseOneAbility(Dice.One));
            Table.Add(1, A => A.ApplyTransient(Codex.Properties.confusion, 2.d10()));
            Table.Add(1, A => A.TeleportEntity(Codex.Properties.teleportation));
          });
        });

        S.AddBoon("banquet of the fae", B =>
        {
          B.Description = "Eat at the fae table. You will not go hungry, but the wine shows you things that are not there.";
          B.Cost = 100;
          B.SetCast().Strike(Strikes.magic, Dice.Zero);
          B.Apply.GainNutrition(5.d100());
          B.Apply.ApplyTransient(Codex.Properties.hallucination, 4.d20());
        });

        S.AddBoon("godmother's blessing", B =>
        {
          B.Description = "A generous healing gift, fae-touched: your hands will not be quite steady later.";
          B.Cost = 300;
          B.SetCast().Strike(Strikes.spirit, Dice.One);
          B.Apply.HealEntity(Dice.Fixed(150), Modifier.Zero);
          B.Apply.ApplyTransient(Codex.Properties.fumbling, 2.d6() + 6);
        });
      });
      // <<< GENERATED SHRINES <<<
    }
#endif

    // >>> GENERATED SHRINES-FIELDS >>>
    public readonly Shrine martial_temple;
    public readonly Shrine pheral_vault;
    public readonly Shrine elemental_forge;
    public readonly Shrine star_observatory;
    public readonly Shrine fae_chapel;
    // <<< GENERATED SHRINES-FIELDS <<<
    public readonly Shrine craft_station;
    public readonly Shrine holy_shrine;
    public readonly Shrine dark_sepulchre;
    public readonly Shrine sacred_grove;
    public readonly Shrine mystic_coven;
  }
}