using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexZoos : CodexPage<ManifestZoos, ZooEditor, Zoo>
  {
    private CodexZoos() { }
#if MASTER_CODEX
    internal CodexZoos(Codex Codex)
      : base(Codex.Manifest.Zoos)
    {
      var Hordes = Codex.Hordes;
      var Features = Codex.Features;
      var Items = Codex.Items;
      var Entities = Codex.Entities;
      var Devices = Codex.Devices;
      var Materials = Codex.Materials;
      var Grounds = Codex.Grounds;
      var Sonics = Codex.Sonics;
      var Properties = Codex.Properties;

      Zoo AddZoo(string Name, Sonic Sonic, Action<ZooEditor> Action)
      {
        return Register.Add(Z =>
        {
          Z.Name = Name;
          Z.Sonic = Sonic;
          Z.AcquireTalent = Properties.sleeping;

          CodexRecruiter.Enrol(() => Action(Z));
        });
      }

      ant_hole = AddZoo("ant hole", Sonics.scuttle, Z =>
      {
        Z.Difficulty = Entities.giant_ant.Difficulty + 1;
        Z.Rarity = 2;
        Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.sandwich);
        Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.cheese);
        Z.Ground = Grounds.dirt;
        Z.Device = Devices.ant_hole;
        Z.AddSpawn(Chance.Always, 1.d4(), [Entities.giant_ant]);
      });

      bee_hive = AddZoo("bee hive", Sonics.buzz, Z =>
      {
        Z.Difficulty = Entities.killer_bee.Difficulty + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.hive_floor;
        Z.Loot.AddKit(Chance.OneIn5, Dice.One, Items.lump_of_royal_jelly);
        Z.AddSpawn(Chance.Always, Dice.One, [Entities.queen_bee]);
        Z.AddSpawn(Chance.Always, Count: null, [Entities.killer_bee]);
      });

      barracks = AddZoo("barracks", Sonics.bugle, Z =>
      {
        Z.Difficulty = Entities.captain.Difficulty + 1;
        Z.Rarity = 2;
        Z.Feature = Features.bed;
        Z.Loot.AddKit(Chance.OneIn10, Dice.One, Items.brass_bugle);
        Z.Loot.AddKit(Chance.OneIn10, 4.d4(), Items.bullet);
        Z.Loot.AddKit(Chance.OneIn10, 2.d2(), Items.shotgun_shell);
        Z.AddSpawn(Chance.Always, Dice.One, [Entities.captain]);
        Z.AddSpawn(Chance.ThreeIn4, 1.d2(), [Entities.lieutenant]);
        Z.AddSpawn(Chance.ThreeIn4, 1.d3(), [Entities.sergeant]);
        Z.AddSpawn(Chance.OneIn3, Count: null, [Entities.soldier]);
      });

      cocknest = AddZoo("cocknest", Sonics.cluck, Z =>
      {
        Z.Difficulty = Entities.cockatrice.Difficulty + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.cave_floor;
        Z.Loot.AddKit(Chance.OneIn5, Dice.One, Items.egg);
        Z.AddSpawn(Chance.OneIn2, Count: null, [Entities.cockatrice, Entities.pyrolisk, Entities.chickatrice, Entities.chicken, Entities.cockatoo]);
      });

      college_of_wizardry = AddZoo("college of wizardry", Sonics.craft, Z =>
      {
        // These are too high level for college:
        // - Entities.occultist, Entities.transmuter, Entities.shifter 
        // These are hostiles:
        // - Entities.gnomish_wizard, Entities.leprechaun_wizard
        var CollegeEntityArray = new[]
        {
          Entities.earth_seeker, Entities.frost_seeker, Entities.flame_seeker, Entities.shock_seeker, Entities.water_seeker,
          Entities.earth_binder, Entities.frost_binder, Entities.flame_binder, Entities.shock_binder, Entities.water_binder,
          Entities.student, Entities.apprentice, Entities.embalmer
        };
        Debug.Assert(CollegeEntityArray.All(E => E.IsMercenary), "All college entities are intended to be mercenaries.");
        Debug.Assert(CollegeEntityArray.All(E => E.Level <= 21), "All college entities are expected to be less than level 20.");

        Z.Difficulty = CollegeEntityArray.Max(E => E.Difficulty) + 1;
        Z.Rarity = 2;
        Z.Feature = Features.workbench;
        Z.Ground = Grounds.obsidian_floor;
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.book_of_blank_paper);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.scroll_of_blank_paper);
        Z.Loot.AddKit(Chance.OneIn50, Dice.One, Items.magic_marker);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.wand_of_nothing);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.ring_of_naught);
        Z.AddSpawn(Chance.OneIn3, Count: null, CollegeEntityArray);
      });

      dragon_nest = AddZoo("dragon nest", Sonics.roar, Z =>
      {
        Z.Difficulty = Entities.adult_red_dragon.Difficulty + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.obsidian_floor;
        foreach (var DragonScaleItem in Items.DragonScales)
          Z.Loot.AddKit(Chance.OneIn(10 * Items.DragonScales.Count), Dice.One, DragonScaleItem);
        Z.Loot.AddKit(Chance.OneIn10, Dice.One, Items.egg); // TODO: these ought to be dragon eggs, but right now, will be a random egg.
        Z.AddSpawn(Chance.Always, Dice.Fixed(2), Codex.Evolutions.AdultDragons);
        Z.AddSpawn(Chance.OneIn3, Count: null, Codex.Evolutions.BabyDragons);
      });

      graveyard = AddZoo("graveyard", Sonics.moan, Z =>
      {
        Z.Difficulty = 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.dirt;
        Z.Feature = Features.grave;
        Z.AddSpawn(Chance.Always, Dice.One, [Entities.ghost]);
      });

      gremlin_pit = AddZoo("gremlin pit", Sonics.cackle, Z =>
      {
        Z.Difficulty = Entities.gremlin.Difficulty + 1;
        Z.Rarity = 2;
        Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.tripe_ration);
        Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.fortune_cookie);
        Z.Ground = Grounds.obsidian_floor;
        Z.Device = Devices.water_trap;
        Z.AddSpawn(Chance.Always, Dice.One, [Entities.gremlin]);
      });

      leprechaun_hall = AddZoo("leprechaun hall", Sonics.giggle, Z =>
      {
        Z.Difficulty = Entities.leprechaun.Difficulty + 1;
        Z.Rarity = 2;
        Z.Loot.AddKit(Chance.Always, Dice.Zero, Items.gold_coin);
        Z.AddSpawn(Chance.Always, Dice.One, [Entities.leprechaun_wizard]);
        Z.AddSpawn(Chance.Always, Count: null, [Entities.leprechaun]);
      });

      salty_pool = AddZoo("salty pool", Sonics.water_splash, Z =>
      {
        var MarineArray = Entities.List.Where(E => E.HasOnlyTerrain(Materials.water)).ToArray();

        Z.Difficulty = MarineArray.Min(E => E.Level) + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.water;
        Z.AddSpawn(Chance.OneIn3, Count: null, MarineArray);
        Z.Loot.AddKit(Chance.OneIn10, 1.d5(), Items.kelp_frond);
      });

      science_lab = AddZoo("science lab", Sonics.potion, Z =>
      {
        Z.Difficulty = Math.Max(Math.Max(Entities.flesh_golem.Difficulty, Entities.quantum_mechanic.Difficulty), Entities.genetic_engineer.Difficulty) + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.marble_floor;
        Z.Loot.AddKit(Chance.OneIn10, Dice.One, Items.potion_of_acid);
        Z.Loot.AddKit(Chance.OneIn10, Dice.One, Items.potion_of_hallucination);
        Z.Loot.AddKit(Chance.OneIn10, Dice.One, Items.potion_of_speed);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.alchemy_smock);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.lab_coat);

        var EyewearArray = Items.List.Where(I => I.Type == ItemType.Eyewear && !I.Grade.Unique).ToArray();
        foreach (var Item in EyewearArray)
          Z.Loot.AddKit(Chance.OneIn(20 * EyewearArray.Length), Dice.One, Item);

        Z.AddSpawn(Chance.Always, Dice.One, [Entities.flesh_golem]);
        Z.AddSpawn(Chance.OneIn3, Count: null, [Entities.quantum_mechanic, Entities.genetic_engineer]);
      });

      spider_nest = AddZoo("spider nest", Sonics.scuttle, Z =>
      {
        Z.Difficulty = Entities.spider_queen.Difficulty + 1;
        Z.Rarity = 2;
        //Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.egg); // TODO: spider egg?
        Z.Ground = Grounds.dirt;
        Z.Device = Devices.web;
        Z.AddSpawn(Chance.Always, 1.d4(), [Entities.recluse_spider]);
        Z.AddSpawn(Chance.Always, Dice.One, [Entities.spider_queen]);
      });

      slumber_party = AddZoo("slumber party", Sonics.sigh, Z =>
      {
        Z.Difficulty = Entities.mountain_nymph.Difficulty + 1;
        Z.Rarity = 2;
        Z.Loot.AddKit(Chance.ThreeIn7, Dice.Zero, Items.gold_coin);
        Z.Loot.AddKit(Chance.OneIn60, Dice.One, Items.crystal_ball);
        Z.Loot.AddKit(Chance.OneIn60, Dice.One, Items.magic_marker);
        Z.Loot.AddKit(Chance.OneIn60, Dice.One, Items.blindfold);
        Z.Loot.AddKit(Chance.OneIn60, Dice.One, Items.expensive_camera);
        Z.AddSpawn(Chance.Always, Count: null, [Entities.mountain_nymph]);
      });

      tavern = AddZoo("tavern", Sonics.quaff, Z =>
      {
        var TavernEntityArray = Codex.Kinds.mercenary.Entities.Where(E => E.Frequency > 0 && E.IsEncounter && E.Difficulty <= 20).ToArray();
        Debug.Assert(TavernEntityArray.All(E => E.IsMercenary), "All tavern entities are intended to be mercenaries.");

        Z.Difficulty = 15;
        Z.Rarity = 2;
        Z.Feature = null;
        Z.Ground = Grounds.stone_floor;
        Z.Loot.AddKit(Chance.OneIn5, Dice.One, Items.potion_of_booze);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.cheese);
        Z.Loot.AddKit(Chance.OneIn50, Dice.One, Items.apple);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.meat_stick);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.fortune_cookie);
        Z.AddSpawn(Chance.OneIn3, Count: null, TavernEntityArray);
      });

      treasure_zoo = AddZoo("treasure zoo", Sonics.coins, Z =>
      {
        Z.Difficulty = 1;
        Z.Rarity = 8;
        Z.Loot.AddKit(Chance.ThreeIn4, Dice.Zero, Items.gold_coin);
        Z.AddSpawn(Chance.Always, Count: null, []);
      });

      // TODO: swamp.

      // TODO: royal chamber - throne & king and servants.
      // >>> GENERATED ZOOS >>>
      // ============================================================================================
      // 11 new themed zoos built around the recent monster batch (drow, pheral, angels, elementals,
      // golems, jellies, mimics, astral/blink dogs, the army chain, and the astral/fae/mystical
      // dragons). Shape copied verbatim from the 15 zoos above: AddZoo(Name, Sonic, Z => {...}),
      // Z.Difficulty derived from the toughest resident (+1, same as ant_hole/bee_hive/barracks/…),
      // Z.Rarity left at the series norm of 2 (only the reward-only treasure_zoo goes to 8), and every
      // Loot.AddKit chance picked to sit alongside the shipped college_of_wizardry/slumber_party/tavern
      // odds for a room of the same danger tier. Each block's opening comment names the shipped zoo or
      // monster it was calibrated against.
      // ============================================================================================

      // Calibrated against drow_priestess (Level 13, Difficulty 10) as the coven's difficulty anchor,
      // same "+1 over the toughest resident" rule as barracks (Difficulty = captain.Difficulty + 1).
      // Rarity 2, matching the standard-tier zoos (ant_hole, spider_nest, gremlin_pit…).
      drow_stronghold = AddZoo("drow stronghold", Sonics.hiss, Z =>
      {
        Z.Difficulty = Entities.drow_priestess.Difficulty + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.obsidian_floor;
        Z.Feature = Features.altar;
        Z.Loot.AddKit(Chance.OneIn10, Dice.One, Items.drow_dagger);
        Z.Loot.AddKit(Chance.OneIn15, Dice.One, Items.drow_bow);
        Z.Loot.AddKit(Chance.OneIn15, 2.d4(), Items.drow_arrow);
        Z.Loot.AddKit(Chance.OneIn15, Dice.One, Items.drow_short_sword);
        Z.Loot.AddKit(Chance.OneIn30, Dice.One, Items.drow_mithrilcoat); // the coven's prize: worth braving the priestess and her spiders for.
        Z.AddSpawn(Chance.Always, Dice.One, [Entities.drow_priestess]);
        Z.AddSpawn(Chance.ThreeIn4, Dice.One, [Entities.drow_mage]);
        Z.AddSpawn(Chance.ThreeIn4, Dice.One, [Entities.drow_monk, Entities.drow_thief]);
        Z.AddSpawn(Chance.OneIn3, Count: null, [Entities.giant_spider]); // Underdark web-guards, same role spiders play in spider_nest.
      });

      // Calibrated against seraphim (Level 19, Difficulty 20), the highest of the six angels; same
      // shape as dragon_nest (a single Difficulty anchor plus tiered AddSpawn calls). This is a
      // late-game vault: Rarity 2 like dragon_nest, not rarer, so it is exactly as easy/hard to find
      // as the other top-tier zoo already shipped.
      angelic_sanctuary = AddZoo("angelic sanctuary", Sonics.chant, Z =>
      {
        Z.Difficulty = Entities.seraphim.Difficulty + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.marble_floor;
        Z.Feature = Features.altar;
        Z.Loot.AddKit(Chance.OneIn5, Dice.One, Items.holy_wafer);
        Z.Loot.AddKit(Chance.OneIn30, Dice.One, Items.potion_of_divinity); // the whole reason to risk a room full of angels.
        Z.Loot.AddKit(Chance.OneIn30, Dice.One, Items.scroll_of_remove_curse);
        Z.Loot.AddKit(Chance.OneIn40, Dice.One, Items.ring_of_protection);
        Z.AddSpawn(Chance.Always, Dice.One, [Entities.seraphim]);
        Z.AddSpawn(Chance.TwoIn3, 1.d2(), [Entities.cherubim, Entities.galgalim]);
        Z.AddSpawn(Chance.OneIn2, Count: null, [Entities.eshim, Entities.angel_warrior]);
        Z.AddSpawn(Chance.OneIn4, Dice.One, [Entities.buraq]); // the seraphim's mount, rare like leprechaun_hall's single wizard.
      });

      // Calibrated against pheral_pharaoh (Level 26, Difficulty 22) with human_mummy (Level 21,
      // Difficulty 22, PathosEntities.cs) as the sarcophagus guard - same level band, same role
      // graveyard gives its ghost. Rarity 2, standard tier.
      pheral_crypt = AddZoo("pheral crypt", Sonics.hiss, Z =>
      {
        Z.Difficulty = Entities.pheral_pharaoh.Difficulty + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.sand;
        Z.Feature = Features.sarcophagus;
        Z.Loot.AddKit(Chance.ThreeIn4, Dice.Zero, Items.gold_coin); // same "always some gold" shape as leprechaun_hall/treasure_zoo.
        Z.Loot.AddKit(Chance.OneIn10, Dice.One, Items.mummy_wrapping);
        Z.Loot.AddKit(Chance.OneIn30, Dice.One, Items.diamond);
        Z.Loot.AddKit(Chance.OneIn30, Dice.One, Items.ruby);
        Z.Loot.AddKit(Chance.OneIn30, Dice.One, Items.emerald);
        Z.Loot.AddKit(Chance.OneIn40, Dice.One, Items.luckstone);
        Z.Loot.AddKit(Chance.OneIn40, Dice.One, Items.healthstone); // grave goods buried with the pharaoh's cats, not random gem-table noise.
        Z.AddSpawn(Chance.Always, Dice.One, [Entities.pheral_pharaoh]);
        Z.AddSpawn(Chance.ThreeIn4, Dice.One, [Entities.pheral_vizier]);
        Z.AddSpawn(Chance.OneIn2, 1.d2(), [Entities.pheral_sentinel, Entities.pheral_khit]);
        Z.AddSpawn(Chance.OneIn3, Count: null, [Entities.human_mummy]);
      });

      // Calibrated against air_maker (Level 30, Difficulty 25), the top of the new seeker/binder/maker
      // chain - same three-tier shape as college_of_wizardry's seeker/binder pair, one rank taller.
      // Rarity 2, standard tier.
      elemental_foundry = AddZoo("elemental foundry", Sonics.explosion, Z =>
      {
        Z.Difficulty = Entities.air_maker.Difficulty + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.obsidian_floor;
        Z.Loot.AddKit(Chance.OneIn10, Dice.One, Items.flint);
        Z.Loot.AddKit(Chance.OneIn15, Dice.One, Items.tinning_kit);
        Z.Loot.AddKit(Chance.OneIn15, Dice.One, Items.iron_chain);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.heavy_iron_ball);
        Z.Loot.AddKit(Chance.OneIn20, 1.d3(), Items.stick_of_dynamite);
        Z.Loot.AddKit(Chance.OneIn40, Dice.One, Items.wand_of_striking); // the foundry's actual product, worth outlasting the bound elementals.
        Z.AddSpawn(Chance.Always, Count: null, [Entities.air_sphere, Entities.acid_sphere, Entities.explosive_sphere, Entities.sonic_sphere, Entities.wind_sphere]);
        Z.AddSpawn(Chance.OneIn2, Count: null, [Entities.acid_elemental, Entities.ash_elemental, Entities.explosive_elemental, Entities.sonic_elemental, Entities.energy_elemental]);
        Z.AddSpawn(Chance.OneIn4, Dice.One, [Entities.air_seeker, Entities.air_binder]);
        Z.AddSpawn(Chance.OneIn10, Dice.One, [Entities.air_maker]); // rare master binder, matches its own Frequency=1.
      });

      // Calibrated against midnight_jelly (Level 21, Difficulty 23), the deadliest of the five. The
      // decision: each jelly dissolves gear as it hits, but the cistern's rare reward is the exact
      // resistance ring/amulet for that jelly's element (cyan=cold, green=poison, red=fire,
      // violet=shock, midnight=drain) - so clearing one colour pays for the risk of touching it.
      // Rarity 2, standard tier.
      jelly_vat = AddZoo("jelly vat", Sonics.burble, Z =>
      {
        Z.Difficulty = Entities.midnight_jelly.Difficulty + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.water;
        Z.Loot.AddKit(Chance.OneIn5, Dice.One, Items.slime_mould);
        Z.Loot.AddKit(Chance.OneIn40, Dice.One, Items.ring_of_cold_resistance);
        Z.Loot.AddKit(Chance.OneIn40, Dice.One, Items.ring_of_poison_resistance);
        Z.Loot.AddKit(Chance.OneIn40, Dice.One, Items.ring_of_fire_resistance);
        Z.Loot.AddKit(Chance.OneIn40, Dice.One, Items.ring_of_shock_resistance);
        Z.Loot.AddKit(Chance.OneIn60, Dice.One, Items.amulet_of_drain_resistance); // rarer: midnight jelly is the cistern's deadliest guardian.
        Z.AddSpawn(Chance.Always, Count: null, [Entities.cyan_jelly, Entities.violet_jelly]);
        Z.AddSpawn(Chance.OneIn2, Count: null, [Entities.green_jelly]);
        Z.AddSpawn(Chance.OneIn3, Dice.One, [Entities.red_jelly]);
        Z.AddSpawn(Chance.OneIn5, Dice.One, [Entities.midnight_jelly]);
      });

      // Calibrated against bronze_golem (Level 22, Difficulty 25), the rarest of the three metal
      // golems (Frequency 1, same as the others) - tin common, silver mid, bronze a rare boss-tier
      // find, same escalation science_lab uses for flesh_golem vs. quantum_mechanic/genetic_engineer.
      // Rarity 2, standard tier.
      golem_forge = AddZoo("golem forge", Sonics.clank, Z =>
      {
        Z.Difficulty = Entities.bronze_golem.Difficulty + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.metal_floor;
        Z.Feature = Features.workbench;
        Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.tinning_kit);
        Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.iron_chain);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.bronze_bell);
        Z.Loot.AddKit(Chance.OneIn30, Dice.One, Items.bronze_plate_mail);
        Z.Loot.AddKit(Chance.OneIn30, Dice.One, Items.silver_long_sword);
        Z.Loot.AddKit(Chance.OneIn30, Dice.One, Items.silver_mace);
        Z.AddSpawn(Chance.Always, Dice.One, [Entities.tin_golem]);
        Z.AddSpawn(Chance.OneIn3, Dice.One, [Entities.silver_golem]);
        Z.AddSpawn(Chance.OneIn10, Dice.One, [Entities.bronze_golem]); // rare, matches its own Frequency=1.
      });

      // Calibrated against astral_dog (Level 22, Difficulty 25); blink_dog/blink_puppy are the common
      // pack (Frequency 2 each, like leprechaun_hall's common leprechauns) and astral_dog/astral_puppy
      // are the rare alphas (Frequency 1). Reward is a teleport-magic set, matching what the dogs
      // themselves do. Rarity 2, standard tier.
      astral_kennel = AddZoo("astral kennel", Sonics.blink, Z =>
      {
        Z.Difficulty = Entities.astral_dog.Difficulty + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.stone_floor;
        Z.Device = Devices.teleporter;
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.scroll_of_teleportation);
        Z.Loot.AddKit(Chance.OneIn40, Dice.One, Items.wand_of_teleportation);
        Z.Loot.AddKit(Chance.OneIn40, Dice.One, Items.ring_of_teleportation);
        Z.Loot.AddKit(Chance.OneIn60, Dice.One, Items.ring_of_teleport_control); // the kennel's real prize, priced like slumber_party's crystal ball.
        Z.AddSpawn(Chance.Always, Count: null, [Entities.blink_dog, Entities.blink_puppy]);
        Z.AddSpawn(Chance.OneIn4, 1.d2(), [Entities.astral_puppy]);
        Z.AddSpawn(Chance.OneIn10, Dice.One, [Entities.astral_dog]); // rare alpha, matches its own Frequency=1.
      });

      // Calibrated against army_captain (Level 13, Difficulty 16), reusing the exact tiered shape of
      // barracks (captain always, lieutenant/sergeant likely, rank-and-file in numbers) but with the
      // new army_* chain instead of the shipped captain/lieutenant/sergeant/soldier, and the medieval
      // kit each rank's own Startup.Loot already carries (spear/leather up to banded mail/broadsword).
      // Rarity 2, standard tier.
      military_camp = AddZoo("military camp", Sonics.bugle, Z =>
      {
        Z.Difficulty = Entities.army_captain.Difficulty + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.grass;
        Z.Feature = Features.bed;
        Z.Loot.AddKit(Chance.OneIn5, Dice.One, Items.spear);
        Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.leather_armour);
        Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.small_shield);
        Z.Loot.AddKit(Chance.OneIn10, Dice.One, Items.low_boots);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.broadsword);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.banded_mail);
        Z.Loot.AddKit(Chance.OneIn30, Dice.One, Items.large_shield);
        Z.Loot.AddKit(Chance.OneIn30, Dice.One, Items.helmet);
        Z.AddSpawn(Chance.Always, Dice.One, [Entities.army_captain]);
        Z.AddSpawn(Chance.ThreeIn4, 1.d2(), [Entities.army_lieutenant]);
        Z.AddSpawn(Chance.ThreeIn4, 1.d3(), [Entities.army_sergeant]);
        Z.AddSpawn(Chance.OneIn3, Count: null, [Entities.army_soldier]);
      });

      // Calibrated against treant (Level 27, Difficulty 26) as the rare ancient guardian, with
      // shrieker/violet_fungus/wood_nymph/zombietree filling in the low-to-mid band - same escalation
      // dragon_nest uses for baby dragons under one adult. No dedicated "vine" monster exists in the
      // codex, so zombietree and treant cover the tangling threat the brief calls "viticci". Rarity 2,
      // standard tier.
      monster_greenhouse = AddZoo("monster greenhouse", Sonics.leaves, Z =>
      {
        Z.Difficulty = Entities.treant.Difficulty + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.grass;
        Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.apple);
        Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.orange);
        Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.pear);
        Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.banana);
        Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.melon);
        Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.carrot);
        Z.Loot.AddKit(Chance.OneIn10, Dice.One, Items.mushroom);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.eucalyptus_leaf);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.sprig_of_wolfsbane);
        Z.AddSpawn(Chance.Always, Count: null, [Entities.shrieker, Entities.violet_fungus]);
        Z.AddSpawn(Chance.OneIn3, 1.d2(), [Entities.wood_nymph]);
        Z.AddSpawn(Chance.OneIn4, Dice.One, [Entities.zombietree]);
        Z.AddSpawn(Chance.OneIn10, Dice.One, [Entities.treant]); // rare ancient guardian, matches its own Frequency=1.
      });

      // Calibrated against giant_mimic (Level 13, Difficulty 15), the toughest of the seven mimics.
      // The decision: real gold and gems sit on the floor alongside mimic_coins and the rest, so
      // picking anything up is a gamble - same trick the game already plays with mimic_coins alone,
      // just concentrated into one room. Rarity 2, standard tier.
      mimic_den = AddZoo("mimic den", Sonics.coins, Z =>
      {
        Z.Difficulty = Entities.giant_mimic.Difficulty + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.stone_floor;
        Z.Loot.AddKit(Chance.ThreeIn4, Dice.Zero, Items.gold_coin); // the real treasure hidden among the mimics.
        Z.Loot.AddKit(Chance.OneIn30, Dice.One, Items.ruby);
        Z.Loot.AddKit(Chance.OneIn30, Dice.One, Items.sapphire);
        Z.Loot.AddKit(Chance.OneIn30, Dice.One, Items.emerald);
        Z.Loot.AddKit(Chance.OneIn40, Dice.One, Items.diamond);
        Z.AddSpawn(Chance.Always, Count: null, [Entities.baby_mimic, Entities.mimic_coins, Entities.small_mimic]);
        Z.AddSpawn(Chance.OneIn2, Count: null, [Entities.mimic, Entities.grave_mimic]);
        Z.AddSpawn(Chance.OneIn5, Dice.One, [Entities.large_mimic]);
        Z.AddSpawn(Chance.OneIn10, Dice.One, [Entities.giant_mimic]); // rare apex mimic, matches large_mimic/giant_mimic's own Frequency=1.
      });

      // Calibrated against adult_mystical_dragon (Level 26, Difficulty 29), the toughest of the three, with
      // adult_fae_dragon (Difficulty 18) as the guaranteed "lesser" resident - same one-guaranteed/two-rare
      // shape as dragon_nest, one tier below it. adult_mystical_dragon's own description says it "hoards
      // grimoires as jealously as gold", so the loot loops every non-unique book like science_lab loops
      // eyewear; adult_fae_dragon's fey hoard gets the decorative rose_* weapons. Rarity 2, matching
      // dragon_nest rather than going rarer, since dragon_nest already sets the precedent for a
      // top-difficulty zoo at the series' normal rarity.
      dragon_rookery = AddZoo("dragon rookery", Sonics.roar, Z =>
      {
        var GrimoireArray = Items.List.Where(I => I.Type == ItemType.Book && !I.Grade.Unique).ToArray();

        Z.Difficulty = Entities.adult_mystical_dragon.Difficulty + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.obsidian_floor;
        Z.Loot.AddKit(Chance.ThreeIn4, Dice.Zero, Items.gold_coin);
        Z.Loot.AddKit(Chance.OneIn30, Dice.One, Items.diamond);
        Z.Loot.AddKit(Chance.OneIn30, Dice.One, Items.ruby);
        Z.Loot.AddKit(Chance.OneIn30, Dice.One, Items.sapphire);
        Z.Loot.AddKit(Chance.OneIn30, Dice.One, Items.amethyst);
        Z.Loot.AddKit(Chance.OneIn60, Dice.One, Items.rose_rapier); // the fae dragon's own trinket hoard.
        foreach (var Item in GrimoireArray)
          Z.Loot.AddKit(Chance.OneIn(40 * GrimoireArray.Length), Dice.One, Item); // the mystical dragon "hoards grimoires as jealously as gold".
        Z.AddSpawn(Chance.Always, Dice.One, [Entities.adult_fae_dragon]);
        Z.AddSpawn(Chance.OneIn3, Count: null, [Entities.adult_astral_dragon, Entities.adult_mystical_dragon]);
      });
      // <<< GENERATED ZOOS <<<
    }
#endif

    // >>> GENERATED ZOOS-FIELDS >>>
    public readonly Zoo drow_stronghold;
    public readonly Zoo angelic_sanctuary;
    public readonly Zoo pheral_crypt;
    public readonly Zoo elemental_foundry;
    public readonly Zoo jelly_vat;
    public readonly Zoo golem_forge;
    public readonly Zoo astral_kennel;
    public readonly Zoo military_camp;
    public readonly Zoo monster_greenhouse;
    public readonly Zoo mimic_den;
    public readonly Zoo dragon_rookery;
    // <<< GENERATED ZOOS-FIELDS <<<
    public readonly Zoo ant_hole;
    public readonly Zoo barracks;
    public readonly Zoo bee_hive;
    public readonly Zoo cocknest;
    public readonly Zoo college_of_wizardry;
    public readonly Zoo dragon_nest;
    public readonly Zoo graveyard;
    public readonly Zoo gremlin_pit;
    public readonly Zoo leprechaun_hall;
    public readonly Zoo salty_pool;
    public readonly Zoo science_lab;
    public readonly Zoo slumber_party;
    public readonly Zoo spider_nest;
    public readonly Zoo tavern;
    public readonly Zoo treasure_zoo;
  }
}