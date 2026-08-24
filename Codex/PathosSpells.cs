using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexSpells : CodexPage<ManifestSpells, SpellEditor, Spell>
  {
    private CodexSpells() { }
#if MASTER_CODEX
    internal CodexSpells(Codex Codex)
      : base(Codex.Manifest.Spells)
    {
      var Schools = Codex.Schools;
      var Beams = Codex.Beams;
      var Strikes = Codex.Strikes;
      var Explosions = Codex.Explosions;
      var Properties = Codex.Properties;
      var Elements = Codex.Elements;
      var Stocks = Codex.Stocks;
      var Items = Codex.Items;
      var Glyphs = Codex.Glyphs;
      var Qualifications = Codex.Qualifications;
      var Sanctities = Codex.Sanctities;
      var Kinds = Codex.Kinds;
      var Entities = Codex.Entities;
      var Devices = Codex.Devices;
      var Gates = Codex.Gates;
      var Attributes = Codex.Attributes;
      var Skills = Codex.Skills;
      var Materials = Codex.Materials;
      var Grounds = Codex.Grounds;
      var Barriers = Codex.Barriers;
      var Volatiles = Codex.Volatiles;
      var Anatomies = Codex.Anatomies;
      var Motions = Codex.Motions;
      var Sonics = Codex.Sonics;
      var Blocks = Codex.Blocks;
      var Diets = Codex.Diets;
      var Races = Codex.Races;
      var Genders = Codex.Genders;
      var Warnings = Codex.Warnings;
      var Standings = Codex.Standings;
      var Appetites = Codex.Appetites;
      var Evolutions = Codex.Evolutions;
      var Slots = Codex.Slots;
      var AttackTypes = Codex.AttackTypes;
      var Encumbrances = Codex.Encumbrances;
      var Grades = Codex.Grades;

      Spell AddSpell(School School, string Name, int Level, Precept Precept, Glyph Glyph, Action<SpellEditor> Action)
      {
        Debug.Assert(School != null);
        Debug.Assert(Name != null);
        Debug.Assert(Glyph != null);

        return Register.Add(S =>
        {
          S.School = School;
          S.Name = Name;
          S.Level = Level;
          S.Mana = Level * 5;
          S.Precept = Precept ?? new Precept(Purpose.Unspecified);
          S.Glyph = Glyph;

          CodexRecruiter.Enrol(() => Action(S));
        });
      }

      void SetAdept(SpellEditor Spell, Action<AdeptEditor> Unskilled, Action<AdeptEditor> Proficient, Action<AdeptEditor> Specialist, Action<AdeptEditor> Expert, Action<AdeptEditor> Master, Action<AdeptEditor> Champion)
      {
        Champion?.Invoke(Spell.SetAdept(Qualifications.champion));
        Master?.Invoke(Spell.SetAdept(Qualifications.master));
        Expert?.Invoke(Spell.SetAdept(Qualifications.expert));
        Specialist?.Invoke(Spell.SetAdept(Qualifications.specialist));
        Proficient?.Invoke(Spell.SetAdept(Qualifications.proficient));
        Unskilled?.Invoke(Spell.SetAdept(Qualification: null));
      }

      acid_stream = AddSpell(Schools.evocation, "acid stream", 4, new Precept(Purpose.Blast, Elements.acid), Glyphs.acid_stream_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.acid, 1.d4() + 2);
            U.Apply.HarmEntity(Elements.acid, 4.d6());
          },
          P =>
          {
            P.SetCast().Beam(Beams.acid, 1.d4() + 3);
            P.Apply.HarmEntity(Elements.acid, 6.d6());
            P.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.acid, R => R.ApplyTransient(Properties.rage, 2.d4() + 2)));
          },
          S =>
          {
            S.SetCast().Beam(Beams.acid, 1.d4() + 5);
            S.Apply.HarmEntity(Elements.acid, 8.d6());
            S.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.acid, R => R.ApplyTransient(Properties.rage, 3.d4() + 3)));
          },
          E =>
          {
            E.SetCast().Beam(Beams.acid, 1.d4() + 7);
            E.Apply.HarmEntity(Elements.acid, 10.d6());
            E.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.acid, R => R.ApplyTransient(Properties.rage, 4.d4() + 4)));
          },
          M =>
          {
            M.SetCast().Beam(Beams.acid, 1.d4() + 9);
            M.Apply.HarmEntity(Elements.acid, 12.d6());
            M.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.acid, R => R.ApplyTransient(Properties.rage, 5.d4() + 5)));
          },
          C =>
          {
            C.SetCast().Beam(Beams.acid, 1.d4() + 11);
            C.Apply.HarmEntity(Elements.acid, 14.d6());
            C.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.acid, R => R.ApplyTransient(Properties.rage, 6.d4() + 6)));
          }
        );
      });

      animate_dead = AddSpell(Schools.necromancy, "animate dead", 2, new Precept(Purpose.SummonAlly, [Items.animal_corpse, Items.vegetable_corpse]), Glyphs.animate_dead_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            U.Apply.AnimateRevenant(CorruptProperty: Properties.rage, CorruptDice: 6.d10());
          },
          // TODO: adept scaling of effects.
          P =>
          {
            P.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            P.Apply.AnimateRevenant(CorruptProperty: null, CorruptDice: Dice.Zero);
          },
          S =>
          {
            S.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            S.Apply.AnimateRevenant(CorruptProperty: null, CorruptDice: Dice.Zero);
          },
          E =>
          {
            E.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            E.Apply.AnimateRevenant(CorruptProperty: null, CorruptDice: Dice.Zero);
          },
          M =>
          {
            M.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            M.Apply.AnimateRevenant(CorruptProperty: null, CorruptDice: Dice.Zero);
          },
          C =>
          {
            C.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            C.Apply.AnimateRevenant(CorruptProperty: null, CorruptDice: Dice.Zero);
          }
        );
      });

      animate_object = AddSpell(Schools.enchantment, "animate object", 5, new Precept(Purpose.Blast), Glyphs.animate_object_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.magic, 1.d4() + 1)
             .SetObjects();
            U.Apply.AnimateObject(ObjectEntity: Entities.animate_object, CorruptProperty: Properties.rage, CorruptDice: 6.d10());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.magic, 1.d4() + 3)
             .SetObjects();
            P.Apply.AnimateObject(ObjectEntity: Entities.animate_object, CorruptProperty: null, CorruptDice: Dice.Zero);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.magic, 1.d4() + 5)
             .SetObjects();
            S.Apply.AnimateObject(ObjectEntity: Entities.animate_object, CorruptProperty: null, CorruptDice: Dice.Zero);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.magic, 1.d4() + 7)
             .SetObjects();
            E.Apply.AnimateObject(ObjectEntity: Entities.animate_object, CorruptProperty: null, CorruptDice: Dice.Zero);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.magic, 1.d4() + 9)
             .SetObjects();
            M.Apply.AnimateObject(ObjectEntity: Entities.animate_object, CorruptProperty: null, CorruptDice: Dice.Zero);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.magic, 1.d4() + 11)
             .SetObjects();
            C.Apply.AnimateObject(ObjectEntity: Entities.animate_object, CorruptProperty: null, CorruptDice: Dice.Zero);
          }
        );
      });

      cancellation = AddSpell(Schools.transmutation, "cancellation", 7, new Precept(Purpose.Blast), Glyphs.cancellation_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, 1.d4() + 1)
             .SetObjects();
            U.Apply.Cancellation(Elements.magical);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4() + 3)
             .SetObjects();
            P.Apply.Cancellation(Elements.magical);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 5)
             .SetObjects();
            S.Apply.Cancellation(Elements.magical);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 7)
             .SetObjects();
            E.Apply.Cancellation(Elements.magical);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 9)
             .SetObjects();
            M.Apply.Cancellation(Elements.magical);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 11)
             .SetObjects();
            C.Apply.Cancellation(Elements.magical);
          }
        );
      });

      charm = AddSpell(Schools.enchantment, "charm", 3, null, Glyphs.charm_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.Fixed(1))
             .SetTargetSelf(false);
            U.Apply.CharmEntity(Elements.magical, Delay.FromTurns(10000), Kinds.Living.ToArray());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.Fixed(1))
             .SetTargetSelf(false);
            P.Apply.CharmEntity(Elements.magical, Delay.FromTurns(20000), Kinds.Living.ToArray());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Fixed(2))
             .SetTargetSelf(false);
            S.Apply.CharmEntity(Elements.magical, Delay.FromTurns(30000), Kinds.Living.ToArray());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Fixed(3))
             .SetTargetSelf(false);
            E.Apply.CharmEntity(Elements.magical, Delay.FromTurns(40000), Kinds.Living.ToArray());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Fixed(4))
             .SetTargetSelf(false);
            M.Apply.CharmEntity(Elements.magical, Delay.FromTurns(50000), Kinds.Living.ToArray());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Fixed(5))
             .SetTargetSelf(false);
            C.Apply.CharmEntity(Elements.magical, Delay.FromTurns(60000), Kinds.Living.ToArray());
          }
        );
      });

      // TODO: clairvoyance is not useful to the non-prime player character.
      clairvoyance = AddSpell(Schools.divination, "clairvoyance", 3, null/*new Precept(Purpose.Buff, Properties.Clairvoyance)*/, Glyphs.clairvoyance_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.Zero);
            U.Apply.ApplyTransient(Properties.clairvoyance, 1.d6());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.Zero);
            P.Apply.ApplyTransient(Properties.clairvoyance, 3.d6());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Zero);
            S.Apply.ApplyTransient(Properties.clairvoyance, 6.d6());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Zero);
            E.Apply.ApplyTransient(Properties.clairvoyance, 9.d6());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Zero);
            M.Apply.ApplyTransient(Properties.clairvoyance, 12.d6());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Zero);
            C.Apply.ApplyTransient(Properties.clairvoyance, 15.d6());
          }
        );
      });

      cone_of_cold = AddSpell(Schools.evocation, "cone of cold", 5, new Precept(Purpose.Blast, Elements.cold), Glyphs.cone_of_cold_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.cold, 1.d4() + 1);
            U.Apply.HarmEntity(Elements.cold, 4.d4() + 4);
          },
          P =>
          {
            P.SetCast().Beam(Beams.cold, 1.d4() + 3);
            P.Apply.HarmEntity(Elements.cold, 6.d4() + 6);
            P.Apply.WhenChance(Chance.OneIn8, T => T.UnlessTargetResistant(Elements.cold, R => R.ApplyTransient(Properties.paralysis, 1.d2() + 1)));
          },
          S =>
          {
            S.SetCast().Beam(Beams.cold, 1.d4() + 5);
            S.Apply.HarmEntity(Elements.cold, 8.d4() + 8);
            S.Apply.WhenChance(Chance.OneIn6, T => T.UnlessTargetResistant(Elements.cold, R => R.ApplyTransient(Properties.paralysis, 1.d4() + 2)));
          },
          E =>
          {
            E.SetCast().Beam(Beams.cold, 1.d4() + 7);
            E.Apply.HarmEntity(Elements.cold, 10.d4() + 10);
            E.Apply.WhenChance(Chance.OneIn4, T => T.UnlessTargetResistant(Elements.cold, R => R.ApplyTransient(Properties.paralysis, 1.d4() + 3)));
          },
          M =>
          {
            M.SetCast().Beam(Beams.cold, 1.d4() + 9);
            M.Apply.HarmEntity(Elements.cold, 12.d4() + 12);
            M.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.cold, R => R.ApplyTransient(Properties.paralysis, 1.d4() + 4)));
          },
          C =>
          {
            C.SetCast().Beam(Beams.cold, 1.d4() + 11);
            C.Apply.HarmEntity(Elements.cold, 14.d4() + 14);
            C.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.cold, R => R.ApplyTransient(Properties.paralysis, 1.d4() + 5)));
          }
        );
      });

      var ConfusionPrecept = new Precept(Purpose.Blast, Properties.confusion);
      var StunnedPrecept = new Precept(Purpose.Blast, Properties.stunned);

      confusion = AddSpell(Schools.enchantment, "confusion", 2, Precept: null, Glyphs.confusion_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.Precept = ConfusionPrecept;
            U.SetCast().Strike(Strikes.psychic, Dice.One);
            U.Apply.ApplyTransient(Properties.confusion, 2.d6());
          },
          P =>
          {
            P.Precept = ConfusionPrecept;
            P.SetCast().Strike(Strikes.psychic, Dice.One);
            P.Apply.ApplyTransient(Properties.confusion, 4.d6());
          },
          S =>
          {
            S.Precept = ConfusionPrecept;
            S.SetCast().Strike(Strikes.psychic, Dice.Fixed(2));
            S.Apply.ApplyTransient(Properties.confusion, 6.d6());
          },
          E =>
          {
            E.Precept = StunnedPrecept;
            E.SetCast().Strike(Strikes.psychic, Dice.Fixed(3));
            E.Apply.ApplyTransient(Properties.confusion, 8.d6());
            E.Apply.ApplyTransient(Properties.stunned, 1.d6());
          },
          M =>
          {
            M.Precept = StunnedPrecept;
            M.SetCast().Strike(Strikes.psychic, Dice.Fixed(4));
            M.Apply.ApplyTransient(Properties.confusion, 10.d6());
            M.Apply.ApplyTransient(Properties.stunned, 2.d6());
          },
          C =>
          {
            C.Precept = StunnedPrecept;
            C.SetCast().Strike(Strikes.psychic, Dice.Fixed(5));
            C.Apply.ApplyTransient(Properties.confusion, 12.d6());
            C.Apply.ApplyTransient(Properties.stunned, 3.d6());
          }
        );
      });

      bind_undead = AddSpell(Schools.necromancy, "bind undead", 5, null, Glyphs.bind_undead_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.Fixed(1))
             .SetTargetSelf(false);
            U.Apply.CharmEntity(Elements.magical, Delay.FromTurns(10000), Kinds.Undead.ToArray());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.Fixed(1))
             .SetTargetSelf(false);
            P.Apply.CharmEntity(Elements.magical, Delay.FromTurns(20000), Kinds.Undead.ToArray());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Fixed(2))
             .SetTargetSelf(false);
            S.Apply.CharmEntity(Elements.magical, Delay.FromTurns(40000), Kinds.Undead.ToArray());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Fixed(3))
             .SetTargetSelf(false);
            E.Apply.CharmEntity(Elements.magical, Delay.FromTurns(60000), Kinds.Undead.ToArray());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Fixed(4))
             .SetTargetSelf(false);
            M.Apply.CharmEntity(Elements.magical, Delay.FromTurns(80000), Kinds.Undead.ToArray());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Fixed(5))
             .SetTargetSelf(false);
            C.Apply.CharmEntity(Elements.magical, Delay.FromTurns(100000), Kinds.Undead.ToArray());
          }
        );
      });

      create_familiar = AddSpell(Schools.conjuration, "create familiar", 6, new Precept(Purpose.SummonAlly), Glyphs.create_familiar_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            U.Apply.WhenProbability(Table =>
            {
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.giant_bat)); // 2
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.chicken)); // 2
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.lichen));
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.lizard));
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.giant_cockroach));
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.newt));
            });
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            P.Apply.WhenProbability(Table =>
            {
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.kitten)); // 2
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.little_dog)); // 2
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.fledgling_raven)); // 2
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.black_rat)); // 2
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.monkey)); // 2
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.pony)); // 3
            });
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            S.Apply.WhenProbability(Table =>
            {
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.housecat)); // 4
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.dog)); // 4
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.juvenile_raven)); // 4
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.pack_rat)); // 4
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.ape)); // 4
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.horse)); // 5
            });
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            E.Apply.WhenProbability(Table =>
            {
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.large_cat)); // 6
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.large_dog)); // 6
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.adult_raven)); // 6
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.rat_king)); // 6
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.carnivorous_ape)); // 6
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.warhorse)); // 7
            });
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            M.Apply.WhenProbability(Table =>
            {
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.tiger)); // 12
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.leocrotta)); // 13
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.wolverine)); // 13
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.wyvern)); // 16
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.bugbear)); // 17
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.mountain_centaur)); // 18
            });
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            C.Apply.WhenProbability(Table =>
            {
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.komodo_dragon)); // 21
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.sabretoothed_cat)); // 23
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.pegasus)); // 24
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.giant_scorpion)); // 25
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.king_cobra)); // 29
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.elephant)); // 35
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.adult_white_dragon, Entities.adult_black_dragon, Entities.adult_red_dragon, Entities.adult_blue_dragon, Entities.adult_green_dragon));
            });
          }
        );
      });

      curing = AddSpell(Schools.clerical, "curing", 2, null, Glyphs.curing_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.One);
            U.Apply.RemoveTransient(Properties.blindness, Properties.deafness, Properties.inebriation);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetAfflictionOverride();
            P.Apply.UnafflictEntity();
            P.Apply.RemoveTransient(Properties.blindness, Properties.deafness, Properties.inebriation, Properties.sickness);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetAfflictionOverride();
            S.Apply.UnafflictEntity();
            S.Apply.RemoveTransient(Properties.blindness, Properties.deafness, Properties.inebriation, Properties.sickness, Properties.hallucination);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetAfflictionOverride();
            E.Apply.UnafflictEntity();
            E.Apply.RemoveTransient(Properties.blindness, Properties.deafness, Properties.inebriation, Properties.sickness, Properties.hallucination, Properties.confusion, Properties.stunned);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetAfflictionOverride();
            M.Apply.UnafflictEntity();
            M.Apply.RemoveTransient(Properties.blindness, Properties.deafness, Properties.inebriation, Properties.sickness, Properties.hallucination, Properties.confusion, Properties.stunned, Properties.rage);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetAfflictionOverride();
            C.Apply.UnafflictEntity();
            C.Apply.RemoveTransient(Properties.blindness, Properties.deafness, Properties.inebriation, Properties.sickness, Properties.hallucination, Properties.confusion, Properties.stunned, Properties.rage, Properties.fear);
          }
        );
      });

      detect_food = AddSpell(Schools.divination, "detect food", 2, null, Glyphs.detect_food_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            U.Apply.DetectItem(Range.Sq10, Stocks.food);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            P.Apply.DetectItem(Range.Sq15, Stocks.food);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            S.Apply.DetectItem(Range.Sq20, Stocks.food);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            E.Apply.DetectItem(Range.Sq25, Stocks.food, Stocks.potion);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            M.Apply.DetectItem(Range.Sq30, Stocks.food, Stocks.potion);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            C.Apply.DetectItem(Range.Sq35, Stocks.food, Stocks.potion);
          }
        );
      });

      detect_monsters = AddSpell(Schools.divination, "detect monsters", 1, null, Glyphs.detect_monsters_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            U.Apply.DetectEntity(Range.Sq10);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            P.Apply.DetectEntity(Range.Sq15);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            S.Apply.DetectEntity(Range.Sq20);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            E.Apply.DetectEntity(Range.Sq25);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            M.Apply.DetectEntity(Range.Sq30);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            C.Apply.DetectEntity(Range.Sq35);
          }
        );
      });

      detect_treasure = AddSpell(Schools.divination, "detect treasure", 4, null, Glyphs.detect_treasure_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            U.Apply.DetectItem(Range.Sq10, Stocks.gem);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            P.Apply.DetectItem(Range.Sq15, Stocks.gem);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            S.Apply.DetectItem(Range.Sq20, Stocks.gem, Stocks.ring);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            E.Apply.DetectItem(Range.Sq25, Stocks.gem, Stocks.ring, Stocks.amulet);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            M.Apply.DetectItem(Range.Sq30, Stocks.gem, Stocks.ring, Stocks.amulet, Stocks.wand);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            C.Apply.DetectItem(Range.Sq35, Stocks.gem, Stocks.ring, Stocks.amulet, Stocks.wand, Stocks.book);
          }
        );
      });

      detect_unseen = AddSpell(Schools.divination, "detect unseen", 3, new Precept(Purpose.Buff, Properties.see_invisible), Glyphs.detect_unseen_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            U.Apply.ApplyTransient(Properties.see_invisible, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Fixed(1))
             .SetTerminates();
            P.Apply.ApplyTransient(Properties.see_invisible, 1.d15() + 91);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Fixed(2))
             .SetTerminates();
            S.Apply.ApplyTransient(Properties.see_invisible, 1.d15() + 121);
            S.Apply.ApplyTransient(Properties.searching, 1.d15() + 121);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Fixed(3))
             .SetTerminates();
            E.Apply.ApplyTransient(Properties.see_invisible, 1.d15() + 151);
            E.Apply.ApplyTransient(Properties.searching, 1.d15() + 151);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Fixed(4))
             .SetTerminates();
            M.Apply.Searching(Range.Sq5);
            M.Apply.ApplyTransient(Properties.see_invisible, 1.d15() + 181);
            M.Apply.ApplyTransient(Properties.searching, 1.d15() + 181);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Fixed(5))
             .SetTerminates();
            C.Apply.Searching(Range.Sq10);
            C.Apply.ApplyTransient(Properties.see_invisible, 1.d15() + 211);
            C.Apply.ApplyTransient(Properties.searching, 1.d15() + 211);
          }
        );
      });

      dig = AddSpell(Schools.transmutation, "dig", 5, null, Glyphs.dig_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.digging, 1.d4() + 1)
             .SetBounces(false)
             .SetPenetrates();
            U.Apply.Digging(Elements.digging);
          },
          P =>
          {
            P.SetCast().Beam(Beams.digging, 1.d4() + 3)
             .SetBounces(false)
             .SetPenetrates();
            P.Apply.Digging(Elements.digging);
          },
          S =>
          {
            S.SetCast().Beam(Beams.digging, 1.d4() + 5)
             .SetBounces(false)
             .SetPenetrates();
            S.Apply.Digging(Elements.digging);
          },
          E =>
          {
            E.SetCast().Beam(Beams.digging, 1.d4() + 7)
             .SetBounces(false)
             .SetPenetrates();
            E.Apply.Digging(Elements.digging);
          },
          M =>
          {
            M.SetCast().Beam(Beams.digging, 1.d4() + 9)
             .SetBounces(false)
             .SetPenetrates();
            M.Apply.Digging(Elements.digging);
          },
          C =>
          {
            C.SetCast().Beam(Beams.digging, 1.d4() + 11)
             .SetBounces(false)
             .SetPenetrates();
            C.Apply.Digging(Elements.digging);
          }
        );
      });

      disintegrate = AddSpell(Schools.transmutation, "disintegrate", 7, new Precept(Purpose.Blast, Elements.disintegrate), Glyphs.disintegrate_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.disintegration, 1.d4() + 1);
            U.Apply.HarmEntity(Elements.disintegrate, 4.d6() + 4);
          },
          P =>
          {
            P.SetCast().Beam(Beams.disintegration, 1.d4() + 3);
            P.Apply.HarmEntity(Elements.disintegrate, 6.d6() + 6);
          },
          S =>
          {
            S.SetCast().Beam(Beams.disintegration, 1.d4() + 5);
            S.Apply.HarmEntity(Elements.disintegrate, 8.d6() + 8);
          },
          E =>
          {
            E.SetCast().Beam(Beams.disintegration, 1.d4() + 7);
            E.Apply.HarmEntity(Elements.disintegrate, 10.d6() + 10);
          },
          M =>
          {
            M.SetCast().Beam(Beams.disintegration, 1.d4() + 9);
            M.Apply.HarmEntity(Elements.disintegrate, 12.d6() + 12);
          },
          C =>
          {
            C.SetCast().Beam(Beams.disintegration, 1.d4() + 11);
            C.Apply.HarmEntity(Elements.disintegrate, 14.d6() + 14);
          }
        );
      });

      drain_life = AddSpell(Schools.necromancy, "drain life", 3, new Precept(Purpose.Blast), Glyphs.drain_life_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            U.Apply.DrainLife(Elements.drain, 1.d6() + 1);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            P.Apply.DrainLife(Elements.drain, 2.d6() + 2);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, 1.d4() + 1) // 2-5
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            S.Apply.DrainLife(Elements.drain, 3.d6() + 3);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, 1.d6() + 2) // 3-8
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            E.Apply.DrainLife(Elements.drain, 4.d6() + 4);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, 1.d8() + 3) // 4-11
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            M.Apply.DrainLife(Elements.drain, 5.d6() + 5);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, 1.d8() + 4) // 5-12
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            C.Apply.DrainLife(Elements.drain, 6.d6() + 6);
          }
        );
      });

      healing = AddSpell(Schools.clerical, "healing", 1, new Precept(Purpose.Healing), Glyphs.healing_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero); // only self.
            U.Apply.HealEntity(5.d2(), Modifier.Zero);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.HealEntity(5.d2(), Modifier.Zero);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.HealEntity(10.d2(), Modifier.Zero);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.HealEntity(15.d2(), Modifier.Zero);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.HealEntity(20.d2(), Modifier.Zero);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.HealEntity(25.d2(), Modifier.Zero);
          }
        );
      });

      extra_healing = AddSpell(Schools.clerical, "extra healing", 3, new Precept(Purpose.Healing), Glyphs.extra_healing_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero); // only self.
            U.Apply.HealEntity(5.d4(), Modifier.Zero);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.HealEntity(5.d4(), Modifier.Zero);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.HealEntity(10.d4(), Modifier.Zero);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.HealEntity(15.d4(), Modifier.Zero);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.HealEntity(20.d4(), Modifier.Zero);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.HealEntity(25.d4(), Modifier.Zero);
          }
        );
      });

      full_healing = AddSpell(Schools.clerical, "full healing", 5, new Precept(Purpose.Healing), Glyphs.full_healing_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero); // only self.
            U.Apply.HealEntity(5.d8(), Modifier.Zero);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.HealEntity(5.d8(), Modifier.Zero);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.HealEntity(10.d8(), Modifier.Zero);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.HealEntity(15.d8(), Modifier.Zero);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.HealEntity(20.d8(), Modifier.Zero);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.HealEntity(25.d8(), Modifier.Zero);
          }
        );
      });

      fear = AddSpell(Schools.enchantment, "fear", 3, new Precept(Purpose.AreaOfEffect, Properties.fear), Glyphs.fear_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.Zero);
            U.Apply.AreaTransient(Properties.fear, 2.d6(), Kinds.Living.ToArray());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.Zero);
            P.Apply.AreaTransient(Properties.fear, 3.d6(), Kinds.Living.ToArray());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Zero);
            S.Apply.AreaTransient(Properties.fear, 4.d6(), Kinds.Living.ToArray());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Zero);
            E.Apply.AreaTransient(Properties.fear, 5.d6(), Kinds.Living.ToArray());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Zero);
            M.Apply.AreaTransient(Properties.fear, 6.d6(), Kinds.Living.ToArray());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Zero);
            C.Apply.AreaTransient(Properties.fear, 7.d7(), Kinds.Living.ToArray());
          }
        );
      });

      finger_of_death = AddSpell(Schools.necromancy, "finger of death", 7, new Precept(Purpose.Blast), Glyphs.finger_of_death_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.death, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates();
            U.Apply.Death(Elements.magical, Kinds.Living.ToArray(), Strikes.death, Cause: null);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.death, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates();
            P.Apply.Death(Elements.magical, Kinds.Living.ToArray(), Strikes.death, Cause: null);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.death, Dice.Fixed(2))
             .SetTargetSelf(false)
             .SetPenetrates();
            S.Apply.Death(Elements.magical, Kinds.Living.ToArray(), Strikes.death, Cause: null);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.death, Dice.Fixed(3))
             .SetTargetSelf(false)
             .SetPenetrates();
            E.Apply.Death(Elements.magical, Kinds.Living.ToArray(), Strikes.death, Cause: null);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.death, Dice.Fixed(4))
             .SetTargetSelf(false)
             .SetPenetrates();
            M.Apply.Death(Elements.magical, Kinds.Living.ToArray(), Strikes.death, Cause: null);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.death, Dice.Fixed(5))
             .SetTargetSelf(false)
             .SetPenetrates();
            C.Apply.Death(Elements.magical, Kinds.Living.ToArray(), Strikes.death, Cause: null);
          }
        );
      });

      fireball = AddSpell(Schools.evocation, "fireball", 4, new Precept(Purpose.Blast, Elements.fire), Glyphs.fireball_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Explosion(Explosions.fiery, 1.d6());
            U.Apply.HarmEntity(Elements.fire, 6.d6());
          },
          P =>
          {
            P.SetCast().Explosion(Explosions.fiery, 1.d6() + 3);
            P.Apply.HarmEntity(Elements.fire, 6.d6());
          },
          S =>
          {
            S.SetCast().Explosion(Explosions.fiery, 1.d6() + 4);
            S.Apply.HarmEntity(Elements.fire, 8.d6());
            S.Apply.ApplyTransient(Properties.deafness, 1.d6() + 1);
          },
          E =>
          {
            E.SetCast().Explosion(Explosions.fiery, 1.d6() + 5);
            E.Apply.HarmEntity(Elements.fire, 10.d6());
            E.Apply.ApplyTransient(Properties.deafness, 2.d6() + 2);
          },
          M =>
          {
            M.SetCast().Explosion(Explosions.fiery, 1.d6() + 6);
            M.Apply.HarmEntity(Elements.fire, 12.d6());
            M.Apply.ApplyTransient(Properties.deafness, 4.d6() + 4);
          },
          C =>
          {
            C.SetCast().Explosion(Explosions.fiery, 1.d6() + 7);
            C.Apply.HarmEntity(Elements.fire, 14.d6());
            C.Apply.ApplyTransient(Properties.deafness, 6.d6() + 6);
          }
        );
      });

      ice_storm = AddSpell(Schools.evocation, "ice storm", 4, new Precept(Purpose.Blast, Elements.cold), Glyphs.ice_storm_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Explosion(Explosions.frosty, 1.d6() + 2);
            U.Apply.HarmEntity(Elements.cold, 2.d4() + 2);
          },
          P =>
          {
            P.SetCast().Explosion(Explosions.frosty, 1.d6() + 3);
            P.Apply.HarmEntity(Elements.cold, 4.d4() + 4);
          },
          S =>
          {
            S.SetCast().Explosion(Explosions.frosty, 1.d6() + 4);
            S.Apply.HarmEntity(Elements.cold, 6.d4() + 6);
            S.Apply.UnlessTargetResistant(Elements.cold, R =>
            {
              R.ApplyTransient(Properties.fumbling, 1.d6() + 4);
            });
          },
          E =>
          {
            E.SetCast().Explosion(Explosions.frosty, 1.d6() + 5);
            E.Apply.HarmEntity(Elements.cold, 8.d4() + 8);
            E.Apply.UnlessTargetResistant(Elements.cold, R =>
            {
              R.ApplyTransient(Properties.fumbling, 1.d6() + 4);
              R.ApplyTransient(Properties.slowness, 1.d6() + 4);
            });
          },
          M =>
          {
            M.SetCast().Explosion(Explosions.frosty, 1.d6() + 6);
            M.Apply.HarmEntity(Elements.cold, 10.d4() + 10);
            M.Apply.UnlessTargetResistant(Elements.cold, R =>
            {
              R.ApplyTransient(Properties.fumbling, 1.d6() + 4);
              R.ApplyTransient(Properties.slowness, 1.d6() + 4);
              R.ApplyTransient(Properties.paralysis, 1.d6() + 4);
            });
          },
          C =>
          {
            C.SetCast().Explosion(Explosions.frosty, 1.d6() + 6);
            C.Apply.HarmEntity(Elements.cold, 12.d4() + 12);
            C.Apply.UnlessTargetResistant(Elements.cold, R =>
            {
              R.ApplyTransient(Properties.fumbling, 1.d6() + 6);
              R.ApplyTransient(Properties.slowness, 1.d6() + 6);
              R.ApplyTransient(Properties.paralysis, 1.d6() + 6);
            });
          }
        );
      });

      flaming_sphere = AddSpell(Schools.conjuration, "flaming sphere", 2, new Precept(Purpose.SummonAlly, Elements.fire), Glyphs.flaming_sphere_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            U.Apply.CreateEntity(Dice.Fixed(1), Entities.flame_sphere);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            P.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.flame_sphere);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Fixed(2))
             .SetTerminates();
            S.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.flame_sphere);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Fixed(3))
             .SetTerminates();
            E.Apply.SummonEntity(Dice.Fixed(2), Constructed: true, Entities.flame_sphere);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Fixed(4))
             .SetTerminates();
            M.Apply.SummonEntity(Dice.Fixed(3), Constructed: true, Entities.flame_sphere);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Fixed(5))
             .SetTerminates();
            C.Apply.SummonEntity(Dice.Fixed(4), Constructed: true, Entities.flame_sphere);
          }
        );
      });

      freezing_sphere = AddSpell(Schools.conjuration, "freezing sphere", 2, new Precept(Purpose.SummonAlly, Elements.cold), Glyphs.freezing_sphere_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            U.Apply.CreateEntity(Dice.Fixed(1), Entities.frost_sphere);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            P.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.frost_sphere);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Fixed(2))
             .SetTerminates();
            S.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.frost_sphere);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Fixed(3))
             .SetTerminates();
            E.Apply.SummonEntity(Dice.Fixed(2), Constructed: true, Entities.frost_sphere);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Fixed(4))
             .SetTerminates();
            M.Apply.SummonEntity(Dice.Fixed(3), Constructed: true, Entities.frost_sphere);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Fixed(5))
             .SetTerminates();
            C.Apply.SummonEntity(Dice.Fixed(4), Constructed: true, Entities.frost_sphere);
          }
        );
      });

      shocking_sphere = AddSpell(Schools.conjuration, "shocking sphere", 2, new Precept(Purpose.SummonAlly, Elements.shock), Glyphs.shocking_sphere_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            U.Apply.CreateEntity(Dice.Fixed(1), Entities.shock_sphere);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            P.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.shock_sphere);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Fixed(2))
             .SetTerminates();
            S.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.shock_sphere);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Fixed(3))
             .SetTerminates();
            E.Apply.SummonEntity(Dice.Fixed(2), Constructed: true, Entities.shock_sphere);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Fixed(4))
             .SetTerminates();
            M.Apply.SummonEntity(Dice.Fixed(3), Constructed: true, Entities.shock_sphere);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Fixed(5))
             .SetTerminates();
            C.Apply.SummonEntity(Dice.Fixed(4), Constructed: true, Entities.shock_sphere);
          }
        );
      });

      soaking_sphere = AddSpell(Schools.conjuration, "soaking sphere", 2, new Precept(Purpose.SummonAlly, Elements.shock), Glyphs.soaking_sphere_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            U.Apply.CreateEntity(Dice.Fixed(1), Entities.water_sphere);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            P.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.water_sphere);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Fixed(2))
             .SetTerminates();
            S.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.water_sphere);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Fixed(3))
             .SetTerminates();
            E.Apply.SummonEntity(Dice.Fixed(2), Constructed: true, Entities.water_sphere);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Fixed(4))
             .SetTerminates();
            M.Apply.SummonEntity(Dice.Fixed(3), Constructed: true, Entities.water_sphere);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Fixed(5))
             .SetTerminates();
            C.Apply.SummonEntity(Dice.Fixed(4), Constructed: true, Entities.water_sphere);
          }
        );
      });

      crushing_sphere = AddSpell(Schools.conjuration, "crushing sphere", 2, new Precept(Purpose.SummonAlly, Elements.shock), Glyphs.crushing_sphere_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            U.Apply.CreateEntity(Dice.Fixed(1), Entities.earth_sphere);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            P.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.earth_sphere);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Fixed(2))
             .SetTerminates();
            S.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.earth_sphere);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Fixed(3))
             .SetTerminates();
            E.Apply.SummonEntity(Dice.Fixed(2), Constructed: true, Entities.earth_sphere);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Fixed(4))
             .SetTerminates();
            M.Apply.SummonEntity(Dice.Fixed(3), Constructed: true, Entities.earth_sphere);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Fixed(5))
             .SetTerminates();
            C.Apply.SummonEntity(Dice.Fixed(4), Constructed: true, Entities.earth_sphere);
          }
        );
      });

      force_bolt = AddSpell(Schools.evocation, "force bolt", 1, new Precept(Purpose.Blast, Elements.force), Glyphs.force_bolt_spell, Z =>
      {
        Z.Description = "Projects a ball of energy that impacts on both monsters and objects.";
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, 2.d3() + 1)
             .SetTargetSelf(false)
             .SetObjects()
             .SetPenetrates();
            U.Apply.HarmEntity(Elements.force, 1.d6() + 1);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 2.d3() + 2)
             .SetTargetSelf(false)
             .SetObjects()
             .SetPenetrates();
            P.Apply.HarmEntity(Elements.force, 2.d6() + 2);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 2.d3() + 3)
             .SetTargetSelf(false)
             .SetObjects()
             .SetPenetrates();
            S.Apply.HarmEntity(Elements.force, 3.d6() + 3);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 2.d3() + 4)
             .SetTargetSelf(false)
             .SetObjects()
             .SetPenetrates();
            E.Apply.HarmEntity(Elements.force, 4.d6() + 4);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 2.d3() + 5)
             .SetTargetSelf(false)
             .SetObjects()
             .SetPenetrates();
            M.Apply.HarmEntity(Elements.force, 5.d6() + 5);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 2.d3() + 6)
             .SetTargetSelf(false)
             .SetObjects()
             .SetPenetrates();
            C.Apply.HarmEntity(Elements.force, 6.d6() + 6);
          }
        );
      });

      haste = AddSpell(Schools.abjuration, "haste", 3, new Precept(Purpose.Buff, Properties.quickness), Glyphs.haste_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.magic, Dice.Zero);
            U.Apply.ApplyTransient(Properties.quickness, 1.d10() + 50);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.magic, Dice.One);
            P.Apply.ApplyTransient(Properties.quickness, 1.d10() + 100);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.magic, Dice.One);
            S.Apply.ApplyTransient(Properties.quickness, 1.d10() + 200);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.magic, Dice.One);
            E.Apply.ApplyTransient(Properties.quickness, 1.d10() + 300);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.magic, Dice.One);
            M.Apply.ApplyTransient(Properties.quickness, 1.d10() + 400);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.magic, Dice.One);
            C.Apply.ApplyTransient(Properties.quickness, 1.d10() + 500);
          }
        );
      });

      identify = AddSpell(Schools.divination, "identify", 5, new Precept(Purpose.Identify), Glyphs.identify_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Plain(Dice.Zero) // identify a random item.
             .SetTerminates();
            U.Apply.IdentifyItem(All: false, Sanctity: null);
          },
          P =>
          {
            P.SetCast().FilterIdentified(false)
             .SetTerminates();
            P.Apply.IdentifyItem(All: false, Sanctity: null);
          },
          null,
          null,
          null,
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Zero)
             .SetTerminates();
            C.Apply.IdentifyItem(All: true, Sanctity: null);
          }
        );
      });

      invisibility = AddSpell(Schools.abjuration, "invisibility", 4, new Precept(Purpose.Buff, Properties.invisibility), Glyphs.invisibility_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.invisibility, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.invisibility, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.invisibility, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.invisibility, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.invisibility, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.invisibility, 1.d15() + 151);
          }
        );
      });

      blinking = AddSpell(Schools.abjuration, "blinking", 1, new Precept(Purpose.Buff, Properties.blinking), Glyphs.blinking_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.blinking, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.blinking, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.blinking, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.blinking, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.blinking, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.blinking, 1.d15() + 151);
          }
        );
      });

      phasing = AddSpell(Schools.enchantment, "phasing", 6, new Precept(Purpose.Buff, Properties.phasing), Glyphs.phasing_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.phasing, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.phasing, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.phasing, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.phasing, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.phasing, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.phasing, 1.d15() + 151);
          }
        );
      });

      jumping = AddSpell(Schools.abjuration, "jumping", 2, new Precept(Purpose.Buff, Properties.jumping), Glyphs.jumping_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.jumping, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.jumping, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.jumping, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.jumping, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.jumping, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.jumping, 1.d15() + 151);
          }
        );
      });

      knock = AddSpell(Schools.transmutation, "knock", 1, Precept: null, Glyphs.knock_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.Fixed(1))
             .SetObjects();
            U.Apply.Opening();
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4() + 1)
             .SetObjects();
            P.Apply.Opening();
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 3)
             .SetObjects();
            S.Apply.Opening();
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 5)
             .SetObjects();
            E.Apply.Opening();
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 7)
             .SetObjects();
            M.Apply.Opening();
            M.Apply.CreateNook();
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 9)
             .SetObjects();
            C.Apply.Opening();
            C.Apply.CreateNook();
          }
        );
      });

      var LevitationPrecept = new Precept(Purpose.Blast, Properties.levitation);
      var FlightPrecept = new Precept(Purpose.Buff, Properties.flight);

      levitation = AddSpell(Schools.abjuration, "levitation", 4, Precept: null, Glyphs.levitation_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.Precept = LevitationPrecept;
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.levitation, 1.d140() + 10);
          },
          P =>
          {
            P.Precept = LevitationPrecept;
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.levitation, 1.d140() + 100);
          },
          S =>
          {
            S.Precept = LevitationPrecept;
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.levitation, 1.d140() + 200);
          },
          E =>
          {
            E.Precept = FlightPrecept;
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.flight, 1.d140() + 100); // this is flight now!
          },
          M =>
          {
            M.Precept = FlightPrecept;
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.flight, 1.d140() + 200);
          },
          C =>
          {
            C.Precept = FlightPrecept;
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.flight, 1.d140() + 300);
          }
        );
      });

      lightning_bolt = AddSpell(Schools.evocation, "lightning bolt", 4, new Precept(Purpose.Blast, Elements.shock), Glyphs.lightning_bolt_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.lightning, 1.d4() + 2);
            U.Apply.HarmEntity(Elements.shock, 4.d6());
          },
          P =>
          {
            P.SetCast().Beam(Beams.lightning, 1.d4() + 3);
            P.Apply.HarmEntity(Elements.shock, 6.d6());
            P.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.blindness, 1.d4() + 1));
          },
          S =>
          {
            S.SetCast().Beam(Beams.lightning, 1.d4() + 5);
            S.Apply.HarmEntity(Elements.shock, 8.d6());
            S.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.blindness, 2.d4() + 2));
          },
          E =>
          {
            E.SetCast().Beam(Beams.lightning, 1.d4() + 7);
            E.Apply.HarmEntity(Elements.shock, 10.d6());
            E.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.blindness, 3.d4() + 3));
          },
          M =>
          {
            M.SetCast().Beam(Beams.lightning, 1.d4() + 9);
            M.Apply.HarmEntity(Elements.shock, 12.d6());
            M.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.blindness, 4.d4() + 4));
          },
          C =>
          {
            C.SetCast().Beam(Beams.lightning, 1.d4() + 11);
            C.Apply.HarmEntity(Elements.shock, 14.d6());
            C.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.blindness, 5.d4() + 5));
          }
        );
      });

      living_wall = AddSpell(Schools.necromancy, "living wall", 6, new Precept(Purpose.SummonAlly), Glyphs.living_wall_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.One)
             .SetTerminates();
            U.Apply.CreateEntity(Dice.One, Entities.living_wall);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.One)
             .SetTerminates();
            P.Apply.SummonEntity(Dice.One, Constructed: true, Entities.living_wall);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.One)
             .SetTerminates();
            S.Apply.SummonEntity(Dice.Two, Constructed: true, Entities.living_wall);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.One)
             .SetTerminates();
            E.Apply.SummonEntity(Dice.Three, Constructed: true, Entities.living_wall);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.One)
             .SetTerminates();
            M.Apply.SummonEntity(Dice.Four, Constructed: true, Entities.living_wall);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.One)
             .SetTerminates();
            C.Apply.SummonEntity(Dice.Five, Constructed: true, Entities.living_wall);
          }
        );
      });

      darkness = AddSpell(Schools.necromancy, "darkness", 1, new Precept(Purpose.AreaOfEffect), Glyphs.darkness_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            U.Apply.Light(false, Locality.Area);
            U.Apply.ApplyTransient(Properties.deafness, 1.d3() + 3);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            P.Apply.Light(false, Locality.Area);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            S.Apply.Light(false, Locality.Area);
            S.Apply.AreaTransient(Properties.fear, 4.d6(), Kinds.angel, Kinds.human);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            E.Apply.Light(false, Locality.Area);
            E.Apply.AreaTransient(Properties.fear, 4.d6(), Kinds.angel, Kinds.human);
            E.Apply.AreaTransient(Properties.silence, 4.d6() + 4);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            M.Apply.Light(false, Locality.Area);
            M.Apply.AreaTransient(Properties.fear, 4.d6(), Kinds.Living.ToArray());
            M.Apply.AreaTransient(Properties.silence, 4.d6() + 4);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            C.Apply.Light(false, Locality.Area);
            C.Apply.AreaTransient(Properties.fear, 5.d6(), Kinds.Living.ToArray());
            C.Apply.AreaTransient(Properties.silence, 5.d6() + 5);
          }
        );
      });

      light = AddSpell(Schools.abjuration, "light", 1, null, Glyphs.light_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            U.Apply.Light(true, Locality.Area);
            U.Apply.ApplyTransient(Properties.blindness, 1.d3() + 3);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            P.Apply.Light(true, Locality.Area);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            S.Apply.Light(true, Locality.Area);
            S.Apply.AreaTransient(Properties.fear, 4.d6(), Kinds.demon, Kinds.vampire, Kinds.orc);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            E.Apply.Light(true, Locality.Area);
            E.Apply.AreaTransient(Properties.fear, 5.d6(), Kinds.demon, Kinds.vampire, Kinds.orc);
            E.Apply.AreaTransient(Properties.blindness, 3.d6() + 3);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            M.Apply.Light(true, Locality.Area);
            M.Apply.AreaTransient(Properties.fear, 6.d6(), Kinds.Undead.ToArray().Union([Kinds.demon, Kinds.orc]).ToArray());
            M.Apply.AreaTransient(Properties.blindness, 4.d6() + 4);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            C.Apply.Light(true, Locality.Area);
            C.Apply.AreaTransient(Properties.fear, 7.d6(), Kinds.Undead.ToArray().Union([Kinds.demon, Kinds.orc]).ToArray());
            C.Apply.AreaTransient(Properties.blindness, 5.d6() + 5);
          }
        );
      });

      magic_mapping = AddSpell(Schools.divination, "magic mapping", 5, null, Glyphs.magic_mapping_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            U.Apply.Mapping(Range.Sq15, Chance.ThreeIn4);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            P.Apply.Mapping(Range.Sq15, Chance.Always);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            S.Apply.Mapping(Range.Sq20, Chance.Always);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            E.Apply.Mapping(Range.Sq25, Chance.Always);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            M.Apply.Mapping(Range.Sq30, Chance.Always);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            C.Apply.Mapping(Range.Sq35, Chance.Always);
          }
        );
      });

      magic_missile = AddSpell(Schools.evocation, "magic missile", 2, new Precept(Purpose.Blast, Elements.magical), Glyphs.magic_missile_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.magic_missile, 1.d4() + 2);
            U.Apply.HarmEntity(Elements.magical, 1.d4() + 2);
          },
          P =>
          {
            P.SetCast().Beam(Beams.magic_missile, 1.d4() + 3);
            P.Apply.HarmEntity(Elements.magical, 2.d4() + 4);
          },
          S =>
          {
            S.SetCast().Beam(Beams.magic_missile, 1.d4() + 5);
            S.Apply.HarmEntity(Elements.magical, 3.d4() + 6);
          },
          E =>
          {
            E.SetCast().Beam(Beams.magic_missile, 1.d4() + 7);
            E.Apply.HarmEntity(Elements.magical, 4.d4() + 8);
          },
          M =>
          {
            M.SetCast().Beam(Beams.magic_missile, 1.d4() + 9);
            M.Apply.HarmEntity(Elements.magical, 5.d4() + 10);
          },
          C =>
          {
            C.SetCast().Beam(Beams.magic_missile, 1.d4() + 11);
            C.Apply.HarmEntity(Elements.magical, 6.d4() + 12);
          }
        );
      });

      poison_blast = AddSpell(Schools.evocation, "poison blast", 4, new Precept(Purpose.Blast, Elements.poison), Glyphs.poison_blast_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.poison, 1.d4() + 2);
            U.Apply.HarmEntity(Elements.poison, 4.d6());
          },
          P =>
          {
            P.SetCast().Beam(Beams.poison, 1.d4() + 3);
            P.Apply.HarmEntity(Elements.poison, 6.d6());
            P.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.poison, R => R.ApplyTransient(Properties.sickness, 1.d4() + 1)));
          },
          S =>
          {
            S.SetCast().Beam(Beams.poison, 1.d4() + 5);
            S.Apply.HarmEntity(Elements.poison, 8.d6());
            S.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.poison, R => R.ApplyTransient(Properties.sickness, 2.d4() + 2)));
          },
          E =>
          {
            E.SetCast().Beam(Beams.poison, 1.d4() + 7);
            E.Apply.HarmEntity(Elements.poison, 10.d6());
            E.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.poison, R => R.ApplyTransient(Properties.sickness, 3.d4() + 3)));
          },
          M =>
          {
            M.SetCast().Beam(Beams.poison, 1.d4() + 9);
            M.Apply.HarmEntity(Elements.poison, 12.d6());
            M.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.poison, R => R.ApplyTransient(Properties.sickness, 4.d4() + 4)));
          },
          C =>
          {
            C.SetCast().Beam(Beams.poison, 1.d4() + 11);
            C.Apply.HarmEntity(Elements.poison, 14.d6());
            C.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.poison, R => R.ApplyTransient(Properties.sickness, 5.d4() + 5)));
          }
        );
      });

      polymorph = AddSpell(Schools.transmutation, "polymorph", 6, new Precept(Purpose.Buff), Glyphs.polymorph_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero) // can only target self.
             .SetObjects(false);
            U.Apply.PolymorphEntity();
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetObjects(false);
            P.Apply.PolymorphEntity();
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, 1.d4() + 3)
             .SetObjects();
            S.Apply.PolymorphEntityAndTrap();
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, 1.d4() + 5)
             .SetObjects();
            E.Apply.PolymorphEntityAndTrap();
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, 1.d4() + 7)
             .SetObjects();
            M.Apply.PolymorphItemAndEntityAndTrap();
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, 1.d4() + 9)
             .SetObjects();
            C.Apply.PolymorphItemAndEntityAndTrap();
          }
        );
      });

      deflection = AddSpell(Schools.abjuration, "deflection", 1, new Precept(Purpose.Buff, Properties.deflection), Glyphs.deflection_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.shield, Dice.Zero);
            U.Apply.ApplyTransient(Properties.deflection, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.shield, Dice.One);
            P.Apply.ApplyTransient(Properties.deflection, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.shield, Dice.One);
            S.Apply.ApplyTransient(Properties.deflection, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.shield, Dice.One);
            E.Apply.ApplyTransient(Properties.deflection, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.shield, Dice.One);
            M.Apply.ApplyTransient(Properties.deflection, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.shield, Dice.One);
            C.Apply.ApplyTransient(Properties.deflection, 1.d15() + 151);
          }
        );
      });

      telekinesis = AddSpell(Schools.abjuration, "telekinesis", 3, new Precept(Purpose.Buff, Properties.telekinesis), Glyphs.telekinesis_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.Zero);
            U.Apply.ApplyTransient(Properties.telekinesis, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.One);
            P.Apply.ApplyTransient(Properties.telekinesis, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.One);
            S.Apply.ApplyTransient(Properties.telekinesis, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.One);
            E.Apply.ApplyTransient(Properties.telekinesis, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.One);
            M.Apply.ApplyTransient(Properties.telekinesis, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.One);
            C.Apply.ApplyTransient(Properties.telekinesis, 1.d15() + 151);
          }
        );
      });

      raise_dead = AddSpell(Schools.necromancy, "raise dead", 7, new Precept(Purpose.SummonEnemy, [Items.animal_corpse, Items.vegetable_corpse]), Glyphs.raise_dead_spell, Z =>  // TODO: this is ENEMY, because raise dead does not have any charming effects.
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            U.Apply.RaiseDeadEntity(Percent: 50, CorruptProperty: Properties.rage, CorruptDice: 6.d10(), LoyalOnly: false);
          },
          P =>
          {
            P.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            P.Apply.RaiseDeadEntity(Percent: 20, CorruptProperty: null, CorruptDice: Dice.Zero, LoyalOnly: false);
          },
          S =>
          {
            S.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            S.Apply.RaiseDeadEntity(Percent: 40, CorruptProperty: null, CorruptDice: Dice.Zero, LoyalOnly: false);
          },
          E =>
          {
            E.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            E.Apply.RaiseDeadEntity(Percent: 60, CorruptProperty: null, CorruptDice: Dice.Zero, LoyalOnly: false);
          },
          M =>
          {
            M.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            M.Apply.RaiseDeadEntity(Percent: 80, CorruptProperty: null, CorruptDice: Dice.Zero, LoyalOnly: false);
          },
          C =>
          {
            C.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            C.Apply.RaiseDeadEntity(Percent: 100, CorruptProperty: null, CorruptDice: Dice.Zero, LoyalOnly: false);
          }
        );
      });

      remove_curse = AddSpell(Schools.clerical, "remove curse", 5, null, Glyphs.remove_curse_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Plain(Dice.Zero);
            U.Apply.RemoveCurse(Dice.One, Sanctities.Uncursed);
          },
          P =>
          {
            P.SetCast().FilterSanctity(Sanctities.Cursed);
            P.Apply.RemoveCurse(Dice.One, Sanctities.Uncursed);
          },
          S =>
          {
            S.SetCast().FilterSanctity(Sanctities.Cursed)
             .SetPunishmentOverride();
            S.Apply.UnpunishEntity();
            S.Apply.RemoveCurse(Dice.Fixed(1), Sanctities.Uncursed);
          },
          E =>
          {
            E.SetCast().FilterSanctity(Sanctities.Cursed)
             .SetPunishmentOverride();
            E.Apply.UnpunishEntity();
            E.Apply.RemoveCurse(Dice.Fixed(2), Sanctities.Uncursed);
          },
          M =>
          {
            M.SetCast().FilterSanctity(Sanctities.Cursed)
             .SetPunishmentOverride();
            M.Apply.UnpunishEntity();
            M.Apply.RemoveCurse(Dice.Fixed(3), Sanctities.Uncursed);
          },
          C =>
          {
            C.SetCast().FilterSanctity(Sanctities.Cursed)
             .SetPunishmentOverride();
            C.Apply.UnpunishEntity();
            C.Apply.RemoveCurse(Dice.Fixed(4), Sanctities.Uncursed);
          }
        );
      });

      regenerate = AddSpell(Schools.clerical, "regenerate", 4, new Precept(Purpose.Buff, Properties.life_regeneration), Glyphs.regenerate_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.life_regeneration, 1.d60());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.Zero);
            P.Apply.ApplyTransient(Properties.life_regeneration, 3.d60());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.life_regeneration, 6.d60());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.life_regeneration, 9.d60());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.life_regeneration, 12.d60());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.life_regeneration, 15.d60());
          }
        );
      });

      restoration = AddSpell(Schools.clerical, "restoration", 4, null, Glyphs.restoration_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.RestoreAbility();
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.RestoreAbility();
          },
          null, // TODO: adept effects?
          null,
          null,
          null
        );
      });

      walling = AddSpell(Schools.transmutation, "walling", 6, null, Glyphs.walling_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.tunnel, Dice.One);
            U.Apply.CreateBarrier(WallStructure.Illusionary, Barrier: null);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.tunnel, Dice.One);
            P.Apply.ConvertBlockToBarrier();
            P.Apply.CreateBarrier(WallStructure.Illusionary, Barrier: null);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.tunnel, Dice.One);
            S.Apply.ConvertBlockToBarrier();
            S.Apply.CreateBarrier(WallStructure.Solid, Barrier: null);
          },
          null,
          null,
          null
        );
      });

      sleep = AddSpell(Schools.enchantment, "sleep", 1, new Precept(Purpose.Blast, Properties.sleeping, Elements.sleep), Glyphs.sleep_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.sleep, 1.d4() + 2)
             .SetAudibility(0);
            U.Apply.ApplyTransient(Properties.sleeping, 10.d4());
          },
          P =>
          {
            P.SetCast().Beam(Beams.sleep, 1.d4() + 3)
             .SetAudibility(0);
            P.Apply.ApplyTransient(Properties.sleeping, 10.d6());
          },
          S =>
          {
            S.SetCast().Beam(Beams.sleep, 1.d4() + 5)
             .SetAudibility(0);
            S.Apply.ApplyTransient(Properties.sleeping, 10.d8());
          },
          E =>
          {
            E.SetCast().Beam(Beams.sleep, 1.d4() + 7)
             .SetAudibility(0);
            E.Apply.ApplyTransient(Properties.sleeping, 10.d10());
          },
          M =>
          {
            M.SetCast().Beam(Beams.sleep, 1.d4() + 9)
             .SetAudibility(0);
            M.Apply.ApplyTransient(Properties.sleeping, 10.d12());
          },
          C =>
          {
            C.SetCast().Beam(Beams.sleep, 1.d4() + 11)
             .SetAudibility(0);
            C.Apply.ApplyTransient(Properties.sleeping, 10.d14());
          }
        );
      });

      slow = AddSpell(Schools.enchantment, "slow", 2, new Precept(Purpose.Blast, Properties.slowness), Glyphs.slow_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, 1.d4() + 2);
            U.Apply.ApplyTransient(Properties.slowness, 5.d6() + 5);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4() + 3);
            P.Apply.ApplyTransient(Properties.slowness, 10.d6() + 10);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 5);
            S.Apply.ApplyTransient(Properties.slowness, 20.d6() + 20);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 7);
            E.Apply.ApplyTransient(Properties.slowness, 30.d6() + 30);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 9);
            M.Apply.ApplyTransient(Properties.slowness, 40.d6() + 40);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 11);
            C.Apply.ApplyTransient(Properties.slowness, 50.d6() + 50);
          }
        );
      });

      summoning = AddSpell(Schools.conjuration, "summoning", 2, new Precept(Purpose.SummonEnemy), Glyphs.summoning_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            U.Apply.CreateEntity(Dice.One);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            P.Apply.CreateEntity(Dice.One);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            S.Apply.CreateEntity(Dice.Fixed(2));
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            E.Apply.CreateEntity(Dice.Fixed(3));
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            M.Apply.CreateEntity(Dice.Fixed(4));
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            C.Apply.CreateEntity(Dice.Fixed(5));
          }
        );
      });

      teleport_away = AddSpell(Schools.abjuration, "teleport away", 6, new Precept(Purpose.Teleport), Glyphs.teleport_away_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.Zero);
            U.Apply.TeleportEntity(Properties.teleportation);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, Dice.One);
            P.Apply.TeleportEntity(Properties.teleportation);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 3);
            S.Apply.TeleportEntity(Properties.teleportation);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 5)
             .SetObjects();
            E.Apply.TeleportFloorItem();
            E.Apply.TeleportEntity(Properties.teleportation);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 7)
             .SetObjects();
            M.Apply.TeleportFloorItem();
            M.Apply.TeleportEntity(Properties.teleportation);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 9)
             .SetObjects();
            C.Apply.TeleportFloorItem();
            C.Apply.TeleportEntity(Properties.teleportation);
          }
        );
      });

      toxic_spray = AddSpell(Schools.transmutation, "toxic spray", 3, new Precept(Purpose.Blast), Glyphs.toxic_spray_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.acid, 2.d3() + 1)
             .SetPenetrates(false);
            U.Apply.HarmEntity(Elements.acid, 1.d6() + 1);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.acid, 2.d3() + 2)
             .SetPenetrates(false);
            P.Apply.HarmEntity(Elements.acid, 2.d6() + 2);
            P.Apply.CreateDevice(Devices.noxious_pool, Destruction: false);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.acid, 2.d3() + 3)
             .SetPenetrates();
            S.Apply.HarmEntity(Elements.acid, 3.d6() + 3);
            S.Apply.ApplyTransient(Properties.hallucination, 2.d6());
            S.Apply.CreateDevice(Devices.noxious_pool, Destruction: false);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.acid, 2.d3() + 4)
             .SetPenetrates();
            E.Apply.HarmEntity(Elements.acid, 4.d6() + 4);
            E.Apply.ApplyTransient(Properties.hallucination, 3.d6());
            E.Apply.ApplyTransient(Properties.confusion, 3.d6());
            E.Apply.CreateDevice(Devices.acid_trap, Destruction: false);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.acid, 2.d3() + 6)
             .SetPenetrates();
            M.Apply.HarmEntity(Elements.acid, 5.d6() + 5);
            M.Apply.ApplyTransient(Properties.hallucination, 4.d6());
            M.Apply.ApplyTransient(Properties.stunned, 4.d6());
            M.Apply.CreateDevice(Devices.toxic_trap, Destruction: false);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.acid, 2.d3() + 8)
             .SetPenetrates();
            C.Apply.HarmEntity(Elements.acid, 6.d6() + 6);
            C.Apply.ApplyTransient(Properties.hallucination, 5.d6());
            C.Apply.ApplyTransient(Properties.stunned, 5.d6());
            C.Apply.CreateDevice(Devices.toxic_trap, Destruction: false);
          }
        );
      });

      turn_undead = AddSpell(Schools.clerical, "turn undead", 3, null, Glyphs.turn_undead_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.One);
            U.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.physical, 2.d6() + 2));
            U.Apply.AreaTransient(Properties.fear, 2.d6(), Kinds.Undead.ToArray());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.One);
            P.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.physical, 4.d6() + 4));
            P.Apply.AreaTransient(Properties.fear, 3.d6(), Kinds.Undead.ToArray());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Fixed(2));
            S.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.physical, 6.d6() + 6));
            S.Apply.AreaTransient(Properties.fear, 4.d6(), Kinds.Undead.ToArray());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Fixed(3));
            E.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.physical, 8.d6() + 8));
            E.Apply.AreaTransient(Properties.fear, 5.d6(), Kinds.Undead.ToArray());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Fixed(4));
            M.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.physical, 10.d6() + 10));
            M.Apply.AreaTransient(Properties.fear, 6.d6(), Kinds.Undead.ToArray());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Fixed(5));
            C.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.physical, 12.d6() + 12));
            C.Apply.AreaTransient(Properties.fear, 7.d6(), Kinds.Undead.ToArray());
          }
        );
      });

      wizard_lock = AddSpell(Schools.transmutation, "wizard lock", 2, null, Glyphs.wizard_lock_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.Fixed(1))
             .SetObjects();
            U.Apply.Locking();
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4() + 1)
             .SetObjects();
            P.Apply.Locking();
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 3)
             .SetObjects()
             .SetPenetrates();
            S.Apply.Locking();
            S.Apply.WhenTargetKind([Kinds.golem], T => T.ApplyTransient(Properties.paralysis, Dice.One));
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 5)
             .SetObjects()
             .SetPenetrates();
            E.Apply.Locking();
            E.Apply.WhenTargetKind([Kinds.golem], T => T.ApplyTransient(Properties.paralysis, 1.d2()));
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 7)
             .SetObjects()
             .SetPenetrates();
            M.Apply.Locking();
            M.Apply.WhenTargetKind([Kinds.golem], T => T.ApplyTransient(Properties.paralysis, 1.d4()));
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 9)
             .SetObjects()
             .SetPenetrates();
            C.Apply.Locking();
            C.Apply.WhenTargetKind([Kinds.golem], T => T.ApplyTransient(Properties.paralysis, 2.d4()));
          }
        );
      });

      // >>> GENERATED SPELLS >>>
      stone_to_flesh = AddSpell(Schools.transmutation, "stone to flesh", 4, new Precept(Purpose.Healing), Glyphs.stone_to_flesh_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.magic, Dice.Zero)
             .SetTargetSelf();
            U.Apply.RemoveTransient(Properties.petrifying);
            U.Apply.HealEntity(1.d8(), Modifier.Zero);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.magic, Dice.Zero)
             .SetTargetSelf();
            P.Apply.RemoveTransient(Properties.petrifying);
            P.Apply.HealEntity(2.d8(), Modifier.Zero);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.magic, Dice.Zero)
             .SetTargetSelf();
            S.Apply.RemoveTransient(Properties.petrifying);
            S.Apply.HealEntity(3.d8(), Modifier.Zero);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.magic, Dice.Zero)
             .SetTargetSelf();
            E.Apply.RemoveTransient(Properties.petrifying);
            E.Apply.HealEntity(4.d8(), Modifier.Zero);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.magic, Dice.Zero)
             .SetTargetSelf();
            M.Apply.RemoveTransient(Properties.petrifying);
            M.Apply.HealEntity(5.d8(), Modifier.Zero);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.magic, Dice.Zero)
             .SetTargetSelf();
            C.Apply.RemoveTransient(Properties.petrifying);
            C.Apply.HealEntity(6.d8(), Modifier.Zero);
          }
        );
      });

      clear_sight = AddSpell(Schools.divination, "clear sight", 1, new Precept(Purpose.Buff, Properties.dark_vision), Glyphs.clear_sight_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            U.Apply.ApplyTransient(Properties.dark_vision, 1.d20() + 20);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            P.Apply.ApplyTransient(Properties.dark_vision, 1.d20() + 40);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            S.Apply.ApplyTransient(Properties.dark_vision, 1.d20() + 60);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            E.Apply.ApplyTransient(Properties.dark_vision, 1.d20() + 80);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            M.Apply.ApplyTransient(Properties.dark_vision, 1.d20() + 100);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            C.Apply.ApplyTransient(Properties.dark_vision, 1.d20() + 120);
          }
        );
      });

      danger_sense = AddSpell(Schools.divination, "danger sense", 2, new Precept(Purpose.Buff, Properties.warning), Glyphs.danger_sense_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            U.Apply.ApplyTransient(Properties.warning, 1.d20() + 15);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            P.Apply.ApplyTransient(Properties.warning, 1.d20() + 30);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            S.Apply.ApplyTransient(Properties.warning, 1.d20() + 60);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            E.Apply.ApplyTransient(Properties.warning, 1.d20() + 90);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            M.Apply.ApplyTransient(Properties.warning, 1.d20() + 120);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            C.Apply.ApplyTransient(Properties.warning, 1.d20() + 150);
          }
        );
      });

      mirror_ward = AddSpell(Schools.abjuration, "mirror ward", 4, new Precept(Purpose.Buff, Properties.reflection), Glyphs.mirror_ward_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            U.Apply.ApplyTransient(Properties.reflection, 1.d10() + 10);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            P.Apply.ApplyTransient(Properties.reflection, 1.d10() + 25);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            S.Apply.ApplyTransient(Properties.reflection, 1.d10() + 45);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            E.Apply.ApplyTransient(Properties.reflection, 1.d10() + 65);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            M.Apply.ApplyTransient(Properties.reflection, 1.d10() + 85);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            C.Apply.ApplyTransient(Properties.reflection, 1.d10() + 105);
          }
        );
      });

      planar_anchor = AddSpell(Schools.divination, "planar anchor", 3, new Precept(Purpose.Buff, Properties.teleport_control), Glyphs.planar_anchor_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            U.Apply.ApplyTransient(Properties.teleport_control, 1.d15() + 15);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            P.Apply.ApplyTransient(Properties.teleport_control, 1.d15() + 30);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            S.Apply.ApplyTransient(Properties.teleport_control, 1.d15() + 55);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            E.Apply.ApplyTransient(Properties.teleport_control, 1.d15() + 80);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            M.Apply.ApplyTransient(Properties.teleport_control, 1.d15() + 105);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            C.Apply.ApplyTransient(Properties.teleport_control, 1.d15() + 130);
          }
        );
      });

      hexbind = AddSpell(Schools.necromancy, "hexbind", 2, new Precept(Purpose.Debuff), Glyphs.hexbind_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.magic, 1.d4() + 2);
            U.Apply.PlaceCurse(Dice.One, Sanctities.Cursed);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.magic, 1.d4() + 4);
            P.Apply.PlaceCurse(Dice.One, Sanctities.Cursed);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.magic, 1.d4() + 6);
            S.Apply.PlaceCurse(1.d2(), Sanctities.Cursed);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.magic, 1.d4() + 8);
            E.Apply.PlaceCurse(1.d2() + 1, Sanctities.Cursed);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.magic, 1.d4() + 10);
            M.Apply.PlaceCurse(1.d3() + 1, Sanctities.Cursed);
            M.Apply.WhenChance(Chance.OneIn2, T => T.DisenchantItem(Dice.One));
          },
          C =>
          {
            C.SetCast().Strike(Strikes.magic, 1.d4() + 12);
            C.Apply.PlaceCurse(1.d4() + 1, Sanctities.Cursed);
            C.Apply.WhenChance(Chance.ThreeIn4, T => T.DisenchantItem(Dice.One));
          }
        );
      });

      blade_blessing = AddSpell(Schools.clerical, "blade blessing", 3, new Precept(Purpose.Buff), Glyphs.blade_blessing_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().FilterEquipped()
             .SetAssetIndividualised();
            U.Apply.EnchantItemUp(Dice.One);
          },
          P =>
          {
            P.SetCast().FilterEquipped()
             .SetAssetIndividualised();
            P.Apply.EnchantItemUp(Dice.One);
          },
          S =>
          {
            S.SetCast().FilterEquipped()
             .SetAssetIndividualised();
            S.Apply.EnchantItemUp(1.d2());
          },
          E =>
          {
            E.SetCast().FilterEquipped()
             .SetAssetIndividualised();
            E.Apply.EnchantItemUp(1.d2());
          },
          M =>
          {
            M.SetCast().FilterEquipped()
             .SetAssetIndividualised();
            M.Apply.EnchantItemUp(1.d3());
          },
          C =>
          {
            C.SetCast().FilterEquipped()
             .SetAssetIndividualised();
            C.Apply.EnchantItemUp(1.d3());
            C.Apply.RemoveCurse(Dice.One, Sanctities.Uncursed);
          }
        );
      });

      atonement = AddSpell(Schools.clerical, "atonement", 2, new Precept(Purpose.Buff), Glyphs.atonement_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTargetSelf();
            U.Apply.UnpunishEntity();
            U.Apply.IncreaseKarma(1.d4());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTargetSelf();
            P.Apply.UnpunishEntity();
            P.Apply.IncreaseKarma(2.d4());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTargetSelf();
            S.Apply.UnpunishEntity();
            S.Apply.IncreaseKarma(3.d4());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTargetSelf();
            E.Apply.UnpunishEntity();
            E.Apply.IncreaseKarma(4.d4());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTargetSelf();
            M.Apply.UnpunishEntity();
            M.Apply.IncreaseKarma(5.d4());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTargetSelf();
            C.Apply.UnpunishEntity();
            C.Apply.IncreaseKarma(6.d4());
          }
        );
      });

      rally_cry = AddSpell(Schools.enchantment, "rally cry", 4, new Precept(Purpose.SummonAlly), Glyphs.rally_cry_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Plain(Dice.Zero)
             .SetAudibility(200);
            U.Apply.RecallParty();
          },
          P =>
          {
            P.SetCast().Plain(Dice.Zero)
             .SetAudibility(400);
            P.Apply.RecallParty();
          },
          S =>
          {
            S.SetCast().Plain(Dice.Zero)
             .SetAudibility(600);
            S.Apply.RecallParty();
          },
          E =>
          {
            E.SetCast().Plain(Dice.Zero)
             .SetAudibility(800);
            E.Apply.RecallParty();
          },
          M =>
          {
            M.SetCast().Plain(Dice.Zero)
             .SetAudibility(1000);
            M.Apply.RecallParty();
          },
          C =>
          {
            C.SetCast().Plain(Dice.Zero)
             .SetAudibility(1200);
            C.Apply.RecallParty();
          }
        );
      });

      chromatic_orb = AddSpell(Schools.evocation, "chromatic orb", 3, new Precept(Purpose.Blast), Glyphs.chromatic_orb_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.magic, 1.d4() + 3);
            U.Apply.WhenProbability(R =>
            {
              R.Add(20, X => X.HarmEntity(Elements.fire, 2.d6()));
              R.Add(20, X => X.HarmEntity(Elements.cold, 2.d6()));
              R.Add(20, X => X.HarmEntity(Elements.shock, 2.d6()));
              R.Add(20, X => X.HarmEntity(Elements.acid, 2.d6()));
              R.Add(20, X => X.HarmEntity(Elements.poison, 2.d6()));
            });
          },
          P =>
          {
            P.SetCast().Strike(Strikes.magic, 1.d4() + 5);
            P.Apply.WhenProbability(R =>
            {
              R.Add(20, X => X.HarmEntity(Elements.fire, 3.d6()));
              R.Add(20, X => X.HarmEntity(Elements.cold, 3.d6()));
              R.Add(20, X => X.HarmEntity(Elements.shock, 3.d6()));
              R.Add(20, X => X.HarmEntity(Elements.acid, 3.d6()));
              R.Add(20, X => X.HarmEntity(Elements.poison, 3.d6()));
            });
          },
          S =>
          {
            S.SetCast().Strike(Strikes.magic, 1.d4() + 7);
            S.Apply.WhenProbability(R =>
            {
              R.Add(20, X => X.HarmEntity(Elements.fire, 4.d6()));
              R.Add(20, X => X.HarmEntity(Elements.cold, 4.d6()));
              R.Add(20, X => X.HarmEntity(Elements.shock, 4.d6()));
              R.Add(20, X => X.HarmEntity(Elements.acid, 4.d6()));
              R.Add(20, X => X.HarmEntity(Elements.poison, 4.d6()));
            });
          },
          E =>
          {
            E.SetCast().Strike(Strikes.magic, 1.d4() + 9);
            E.Apply.WhenProbability(R =>
            {
              R.Add(20, X => X.HarmEntity(Elements.fire, 5.d6()));
              R.Add(20, X => X.HarmEntity(Elements.cold, 5.d6()));
              R.Add(20, X => X.HarmEntity(Elements.shock, 5.d6()));
              R.Add(20, X => X.HarmEntity(Elements.acid, 5.d6()));
              R.Add(20, X => X.HarmEntity(Elements.poison, 5.d6()));
            });
          },
          M =>
          {
            M.SetCast().Strike(Strikes.magic, 1.d4() + 11);
            M.Apply.WhenProbability(R =>
            {
              R.Add(20, X => X.HarmEntity(Elements.fire, 6.d6()));
              R.Add(20, X => X.HarmEntity(Elements.cold, 6.d6()));
              R.Add(20, X => X.HarmEntity(Elements.shock, 6.d6()));
              R.Add(20, X => X.HarmEntity(Elements.acid, 6.d6()));
              R.Add(20, X => X.HarmEntity(Elements.poison, 6.d6()));
            });
          },
          C =>
          {
            C.SetCast().Strike(Strikes.magic, 1.d4() + 13);
            C.Apply.WhenProbability(R =>
            {
              R.Add(20, X => X.HarmEntity(Elements.fire, 7.d6()));
              R.Add(20, X => X.HarmEntity(Elements.cold, 7.d6()));
              R.Add(20, X => X.HarmEntity(Elements.shock, 7.d6()));
              R.Add(20, X => X.HarmEntity(Elements.acid, 7.d6()));
              R.Add(20, X => X.HarmEntity(Elements.poison, 7.d6()));
            });
          }
        );
      });

      prismatic_spray = AddSpell(Schools.evocation, "prismatic spray", 6, new Precept(Purpose.Blast), Glyphs.prismatic_spray_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Explosion(Explosions.magical, 1.d2());
            U.Apply.WhenProbability(Table =>
            {
              Table.Add(15, X => X.HarmEntity(Elements.fire, 4.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.cold, 4.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.shock, 4.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.acid, 4.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.poison, 4.d6()));
              Table.Add(25, X =>
              {
                X.HarmEntity(Elements.magical, 3.d6());
                X.ApplyTransient(Properties.blindness, 1.d20() + 10);
              });
            });
          },
          P =>
          {
            P.SetCast().Explosion(Explosions.magical, 1.d2());
            P.Apply.WhenProbability(Table =>
            {
              Table.Add(15, X => X.HarmEntity(Elements.fire, 5.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.cold, 5.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.shock, 5.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.acid, 5.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.poison, 5.d6()));
              Table.Add(25, X =>
              {
                X.HarmEntity(Elements.magical, 4.d6());
                X.ApplyTransient(Properties.blindness, 1.d20() + 20);
              });
            });
          },
          S =>
          {
            S.SetCast().Explosion(Explosions.magical, 1.d3());
            S.Apply.WhenProbability(Table =>
            {
              Table.Add(15, X => X.HarmEntity(Elements.fire, 6.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.cold, 6.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.shock, 6.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.acid, 6.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.poison, 6.d6()));
              Table.Add(25, X =>
              {
                X.HarmEntity(Elements.magical, 5.d6());
                X.ApplyTransient(Properties.blindness, 1.d20() + 30);
              });
            });
          },
          E =>
          {
            E.SetCast().Explosion(Explosions.magical, 1.d3());
            E.Apply.WhenProbability(Table =>
            {
              Table.Add(15, X => X.HarmEntity(Elements.fire, 7.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.cold, 7.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.shock, 7.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.acid, 7.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.poison, 7.d6()));
              Table.Add(25, X =>
              {
                X.HarmEntity(Elements.magical, 6.d6());
                X.ApplyTransient(Properties.blindness, 1.d20() + 40);
              });
            });
          },
          M =>
          {
            M.SetCast().Explosion(Explosions.magical, 2.d2());
            M.Apply.WhenProbability(Table =>
            {
              Table.Add(15, X => X.HarmEntity(Elements.fire, 8.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.cold, 8.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.shock, 8.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.acid, 8.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.poison, 8.d6()));
              Table.Add(25, X =>
              {
                X.HarmEntity(Elements.magical, 7.d6());
                X.ApplyTransient(Properties.blindness, 1.d20() + 50);
              });
            });
          },
          C =>
          {
            C.SetCast().Explosion(Explosions.magical, 2.d3());
            C.Apply.WhenProbability(Table =>
            {
              Table.Add(15, X => X.HarmEntity(Elements.fire, 9.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.cold, 9.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.shock, 9.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.acid, 9.d6()));
              Table.Add(15, X => X.HarmEntity(Elements.poison, 9.d6()));
              Table.Add(25, X =>
              {
                X.HarmEntity(Elements.magical, 8.d6());
                X.ApplyTransient(Properties.blindness, 1.d20() + 60);
              });
            });
          }
        );
      });

      hold_monster = AddSpell(Schools.enchantment, "hold monster", 5, new Precept(Purpose.Debuff, Properties.paralysis), Glyphs.hold_monster_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.magic, 1.d6() + 2);
            U.Apply.ApplyTransient(Properties.paralysis, 1.d4() + 2);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.magic, 1.d6() + 2);
            P.Apply.ApplyTransient(Properties.paralysis, 1.d4() + 4);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.magic, 1.d6() + 3);
            S.Apply.ApplyTransient(Properties.paralysis, 1.d6() + 4);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.magic, 1.d6() + 3);
            E.Apply.ApplyTransient(Properties.paralysis, 1.d6() + 6);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.magic, 1.d6() + 4);
            M.Apply.ApplyTransient(Properties.paralysis, 1.d8() + 6);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.magic, 1.d6() + 4);
            C.Apply.ApplyTransient(Properties.paralysis, 1.d8() + 10);
          }
        );
      });

      concussive_blast = AddSpell(Schools.evocation, "concussive blast", 2, new Precept(Purpose.Debuff, Properties.stunned), Glyphs.concussive_blast_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Explosion(Explosions.electric, Dice.One);
            U.Apply.HarmEntity(Elements.shock, 1.d6());
            U.Apply.ApplyTransient(Properties.stunned, 1.d3());
          },
          P =>
          {
            P.SetCast().Explosion(Explosions.electric, Dice.One);
            P.Apply.HarmEntity(Elements.shock, 2.d6());
            P.Apply.ApplyTransient(Properties.stunned, 1.d3() + 1);
          },
          S =>
          {
            S.SetCast().Explosion(Explosions.electric, 1.d2());
            S.Apply.HarmEntity(Elements.shock, 3.d6());
            S.Apply.ApplyTransient(Properties.stunned, 1.d4() + 1);
          },
          E =>
          {
            E.SetCast().Explosion(Explosions.electric, 1.d2());
            E.Apply.HarmEntity(Elements.shock, 4.d6());
            E.Apply.ApplyTransient(Properties.stunned, 1.d4() + 2);
          },
          M =>
          {
            M.SetCast().Explosion(Explosions.electric, 1.d3());
            M.Apply.HarmEntity(Elements.shock, 5.d6());
            M.Apply.ApplyTransient(Properties.stunned, 1.d6() + 2);
          },
          C =>
          {
            C.SetCast().Explosion(Explosions.electric, 1.d3());
            C.Apply.HarmEntity(Elements.shock, 6.d6());
            C.Apply.ApplyTransient(Properties.stunned, 1.d6() + 3);
          }
        );
      });

      death_ward = AddSpell(Schools.abjuration, "death ward", 6, new Precept(Purpose.Buff, Properties.lifesaving), Glyphs.death_ward_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero).SetTargetSelf();
            U.Apply.ApplyTransient(Properties.lifesaving, 1.d100() + 200);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero).SetTargetSelf();
            P.Apply.ApplyTransient(Properties.lifesaving, 1.d100() + 400);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Zero).SetTargetSelf();
            S.Apply.ApplyTransient(Properties.lifesaving, 1.d100() + 600);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Zero).SetTargetSelf();
            E.Apply.ApplyTransient(Properties.lifesaving, 1.d100() + 800);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Zero).SetTargetSelf();
            M.Apply.ApplyTransient(Properties.lifesaving, 1.d100() + 1000);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Zero).SetTargetSelf();
            C.Apply.ApplyTransient(Properties.lifesaving, 1.d100() + 1500);
          }
        );
      });

      stoneskin = AddSpell(Schools.transmutation, "stoneskin", 4, new Precept(Purpose.Buff, Properties.sustain_ability), Glyphs.stoneskin_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            U.Apply.ApplyTransient(Properties.sustain_ability, 1.d20() + 20);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            P.Apply.ApplyTransient(Properties.sustain_ability, 1.d20() + 40);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            S.Apply.ApplyTransient(Properties.sustain_ability, 1.d20() + 60);
            S.Apply.ApplyTransient(Properties.slippery, 1.d20() + 20);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            E.Apply.ApplyTransient(Properties.sustain_ability, 1.d20() + 80);
            E.Apply.ApplyTransient(Properties.slippery, 1.d20() + 40);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            M.Apply.ApplyTransient(Properties.sustain_ability, 1.d20() + 100);
            M.Apply.ApplyTransient(Properties.slippery, 1.d20() + 60);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            C.Apply.ApplyTransient(Properties.sustain_ability, 1.d20() + 140);
            C.Apply.ApplyTransient(Properties.slippery, 1.d20() + 100);
          }
        );
      });

      displacement = AddSpell(Schools.abjuration, "displacement", 3, new Precept(Purpose.Buff, Properties.displacement), Glyphs.displacement_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            U.Apply.ApplyTransient(Properties.displacement, 1.d20() + 15);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            P.Apply.ApplyTransient(Properties.displacement, 1.d20() + 30);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            S.Apply.ApplyTransient(Properties.displacement, 1.d20() + 50);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            E.Apply.ApplyTransient(Properties.displacement, 1.d20() + 70);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            M.Apply.ApplyTransient(Properties.displacement, 1.d20() + 90);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            C.Apply.ApplyTransient(Properties.displacement, 1.d20() + 120);
          }
        );
      });

      discord = AddSpell(Schools.enchantment, "discord", 4, new Precept(Purpose.Debuff, Properties.conflict), Glyphs.discord_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Explosion(Explosions.magical, Dice.One);
            U.Apply.AreaTransient(Properties.conflict, 1.d20() + 10);
          },
          P =>
          {
            P.SetCast().Explosion(Explosions.magical, Dice.One);
            P.Apply.AreaTransient(Properties.conflict, 1.d20() + 20);
          },
          S =>
          {
            S.SetCast().Explosion(Explosions.magical, 1.d2());
            S.Apply.AreaTransient(Properties.conflict, 1.d20() + 30);
          },
          E =>
          {
            E.SetCast().Explosion(Explosions.magical, 1.d2());
            E.Apply.AreaTransient(Properties.conflict, 1.d20() + 40);
          },
          M =>
          {
            M.SetCast().Explosion(Explosions.magical, 1.d3());
            M.Apply.AreaTransient(Properties.conflict, 1.d20() + 50);
          },
          C =>
          {
            C.SetCast().Explosion(Explosions.magical, 1.d3());
            C.Apply.AreaTransient(Properties.conflict, 1.d20() + 70);
          }
        );
      });

      warning_ward = AddSpell(Schools.divination, "warning ward", 1, new Precept(Purpose.Buff, Properties.warning), Glyphs.warning_ward_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.Zero);
            U.Apply.ApplyTransient(Properties.warning, 1.d6());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.One);
            P.Apply.ApplyTransient(Properties.warning, 3.d6());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.One);
            S.Apply.ApplyTransient(Properties.warning, 6.d6());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.One);
            E.Apply.ApplyTransient(Properties.warning, 9.d6());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.One);
            M.Apply.ApplyTransient(Properties.warning, 12.d6());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.One);
            C.Apply.ApplyTransient(Properties.warning, 15.d6());
          }
        );
      });

      owl_eyes = AddSpell(Schools.divination, "owl eyes", 1, new Precept(Purpose.Buff, Properties.dark_vision), Glyphs.owl_eyes_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.Zero);
            U.Apply.ApplyTransient(Properties.dark_vision, 1.d6());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.One);
            P.Apply.ApplyTransient(Properties.dark_vision, 3.d6());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.One);
            S.Apply.ApplyTransient(Properties.dark_vision, 6.d6());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.One);
            E.Apply.ApplyTransient(Properties.dark_vision, 9.d6());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.One);
            M.Apply.ApplyTransient(Properties.dark_vision, 12.d6());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.One);
            C.Apply.ApplyTransient(Properties.dark_vision, 15.d6());
          }
        );
      });

      augury = AddSpell(Schools.divination, "augury", 1, null, Glyphs.augury_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            U.Apply.Rumour(Attributes.wisdom, Skills.literacy, Truth: true, Lies: true);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            P.Apply.Rumour(Attributes.wisdom, Skills.literacy, Truth: true, Lies: true);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            S.Apply.Rumour(Attributes.wisdom, Skills.literacy, Truth: true, Lies: false);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            E.Apply.Rumour(Attributes.wisdom, Skills.literacy, Truth: true, Lies: false);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            M.Apply.Rumour(Attributes.wisdom, Skills.literacy, Truth: true, Lies: false);
            M.Apply.Rumour(Attributes.wisdom, Skills.literacy, Truth: true, Lies: false);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            C.Apply.Rumour(Attributes.wisdom, Skills.literacy, Truth: true, Lies: false);
            C.Apply.Rumour(Attributes.wisdom, Skills.literacy, Truth: true, Lies: false);
            C.Apply.Rumour(Attributes.wisdom, Skills.literacy, Truth: true, Lies: false);
          }
        );
      });

      keen_search = AddSpell(Schools.divination, "keen search", 2, null, Glyphs.keen_search_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U => { U.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); U.Apply.Searching(Range.Sq2); },
          P => { P.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); P.Apply.Searching(Range.Sq3); },
          S => { S.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); S.Apply.Searching(Range.Sq4); },
          E => { E.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); E.Apply.Searching(Range.Sq5); },
          M => { M.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); M.Apply.Searching(Range.Sq6); },
          C => { C.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); C.Apply.Searching(Range.Sq10); }
        );
      });

      find_traps = AddSpell(Schools.divination, "find traps", 2, null, Glyphs.find_traps_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U => { U.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); U.Apply.DetectTrap(Range.Sq10); },
          P => { P.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); P.Apply.DetectTrap(Range.Sq15); },
          S => { S.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); S.Apply.DetectTrap(Range.Sq20); },
          E => { E.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); E.Apply.DetectTrap(Range.Sq25); },
          M => { M.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); M.Apply.DetectTrap(Range.Sq30); },
          C => { C.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); C.Apply.DetectTrap(Range.Sq35, Reveal: true); }
        );
      });

      detect_undead = AddSpell(Schools.divination, "detect undead", 2, null, Glyphs.detect_undead_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U => { U.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); U.Apply.DetectEntity(Range.Sq10, Kinds.Undead.ToArray()); },
          P => { P.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); P.Apply.DetectEntity(Range.Sq15, Kinds.Undead.ToArray()); },
          S => { S.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); S.Apply.DetectEntity(Range.Sq20, Kinds.Undead.ToArray()); },
          E => { E.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); E.Apply.DetectEntity(Range.Sq25, Kinds.Undead.ToArray()); },
          M => { M.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); M.Apply.DetectEntity(Range.Sq30, Kinds.Undead.ToArray()); },
          C => { C.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); C.Apply.DetectEntity(Range.Sq35, Kinds.Undead.ToArray()); }
        );
      });

      sense_curse = AddSpell(Schools.divination, "sense curse", 3, null, Glyphs.sense_curse_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U => { U.SetCast().Plain(Dice.Zero).SetTerminates(); U.Apply.DivineItem(); },
          P => { P.SetCast().FilterDivined(false).SetTerminates(); P.Apply.DivineItem(); },
          S => { S.SetCast().FilterDivined(false).SetTerminates(); S.Apply.DivineItem(); },
          E => { E.SetCast().FilterDivined(false).SetTerminates(); E.Apply.DivineItem(); },
          M => { M.SetCast().FilterDivined(false).SetTerminates(); M.Apply.DivineItem(); },
          C => { C.SetCast().FilterAnyItem().SetTerminates(); C.Apply.DivineItem(); }
        );
      });

      revelation = AddSpell(Schools.divination, "revelation", 3, null, Glyphs.revelation_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U => { U.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); U.Apply.DiscoverItem(null); },
          P => { P.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); P.Apply.DiscoverItem(null); P.Apply.DiscoverItem(null); },
          S => { S.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); S.Apply.DiscoverItem(null); S.Apply.DiscoverItem(null); S.Apply.DiscoverItem(null); },
          E => { E.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); for (int i = 0; i < 4; i++) E.Apply.DiscoverItem(null); },
          M => { M.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); for (int i = 0; i < 5; i++) M.Apply.DiscoverItem(null); },
          C => { C.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); for (int i = 0; i < 6; i++) C.Apply.DiscoverItem(null); }
        );
      });

      find_doors = AddSpell(Schools.divination, "find doors", 3, null, Glyphs.find_doors_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U => { U.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); U.Apply.DetectGate(Range.Sq10); },
          P => { P.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); P.Apply.DetectGate(Range.Sq15); },
          S => { S.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); S.Apply.DetectGate(Range.Sq20); },
          E => { E.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); E.Apply.DetectGate(Range.Sq25); },
          M => { M.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); M.Apply.DetectGate(Range.Sq30); },
          C => { C.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); C.Apply.DetectGate(Range.Sq35); }
        );
      });

      detect_metal = AddSpell(Schools.divination, "detect metal", 3, null, Glyphs.detect_metal_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U => { U.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); U.Apply.DetectMaterial(Range.Sq10, Materials.gold); },
          P => { P.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); P.Apply.DetectMaterial(Range.Sq15, Materials.gold, Materials.gemstone); },
          S => { S.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); S.Apply.DetectMaterial(Range.Sq20, Materials.gold, Materials.gemstone, Materials.mithril); },
          E => { E.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); E.Apply.DetectMaterial(Range.Sq25, Materials.gold, Materials.gemstone, Materials.mithril, Materials.adamantine); },
          M => { M.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); M.Apply.DetectMaterial(Range.Sq30, Materials.gold, Materials.gemstone, Materials.mithril, Materials.adamantine); },
          C => { C.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); C.Apply.DetectMaterial(Range.Sq35, Materials.gold, Materials.gemstone, Materials.mithril, Materials.adamantine); }
        );
      });

      mind_ken = AddSpell(Schools.divination, "mind ken", 4, new Precept(Purpose.Buff, Properties.telepathy), Glyphs.mind_ken_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U => { U.SetCast().Strike(Strikes.psychic, Dice.Zero); U.Apply.ApplyTransient(Properties.telepathy, 1.d6()); },
          P => { P.SetCast().Strike(Strikes.psychic, Dice.One); P.Apply.ApplyTransient(Properties.telepathy, 3.d6()); },
          S => { S.SetCast().Strike(Strikes.psychic, Dice.One); S.Apply.ApplyTransient(Properties.telepathy, 6.d6()); },
          E => { E.SetCast().Strike(Strikes.psychic, Dice.One); E.Apply.ApplyTransient(Properties.telepathy, 9.d6()); },
          M => { M.SetCast().Strike(Strikes.psychic, Dice.One); M.Apply.ApplyTransient(Properties.telepathy, 12.d6()); },
          C => { C.SetCast().Strike(Strikes.psychic, Dice.One); C.Apply.ApplyTransient(Properties.telepathy, 15.d6()); }
        );
      });

      foreknowledge = AddSpell(Schools.divination, "foreknowledge", 4, new Precept(Purpose.Buff, Properties.teleport_control), Glyphs.foreknowledge_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U => { U.SetCast().Strike(Strikes.psychic, Dice.Zero); U.Apply.ApplyTransient(Properties.teleport_control, 1.d6()); },
          P => { P.SetCast().Strike(Strikes.psychic, Dice.One); P.Apply.ApplyTransient(Properties.teleport_control, 3.d6()); },
          S => { S.SetCast().Strike(Strikes.psychic, Dice.One); S.Apply.ApplyTransient(Properties.teleport_control, 6.d6()); },
          E => { E.SetCast().Strike(Strikes.psychic, Dice.One); E.Apply.ApplyTransient(Properties.teleport_control, 9.d6()); },
          M => { M.SetCast().Strike(Strikes.psychic, Dice.One); M.Apply.ApplyTransient(Properties.teleport_control, 12.d6()); },
          C => { C.SetCast().Strike(Strikes.psychic, Dice.One); C.Apply.ApplyTransient(Properties.teleport_control, 15.d6()); }
        );
      });

      true_appraisal = AddSpell(Schools.divination, "true appraisal", 4, new Precept(Purpose.Buff, Properties.appraisal), Glyphs.true_appraisal_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U => { U.SetCast().Strike(Strikes.psychic, Dice.Zero); U.Apply.AssessItem(); U.Apply.ApplyTransient(Properties.appraisal, 1.d6()); },
          P => { P.SetCast().Strike(Strikes.psychic, Dice.One); P.Apply.AssessItem(); P.Apply.ApplyTransient(Properties.appraisal, 3.d6()); },
          S => { S.SetCast().Strike(Strikes.psychic, Dice.One); S.Apply.AssessItem(); S.Apply.ApplyTransient(Properties.appraisal, 6.d6()); },
          E => { E.SetCast().Strike(Strikes.psychic, Dice.One); E.Apply.AssessItem(); E.Apply.ApplyTransient(Properties.appraisal, 9.d6()); },
          M => { M.SetCast().Strike(Strikes.psychic, Dice.One); M.Apply.AssessItem(); M.Apply.ApplyTransient(Properties.appraisal, 12.d6()); },
          C => { C.SetCast().Strike(Strikes.psychic, Dice.One); C.Apply.AssessItem(); C.Apply.ApplyTransient(Properties.appraisal, 15.d6()); }
        );
      });

      oracles_eye = AddSpell(Schools.divination, "oracle's eye", 5, null, Glyphs.oracles_eye_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            U.Apply.DetectEntity(Range.Sq10);
            U.Apply.DetectItem(Range.Sq10);
            U.Apply.DetectTrap(Range.Sq10);
            U.Apply.DetectGate(Range.Sq10);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            P.Apply.DetectEntity(Range.Sq15);
            P.Apply.DetectItem(Range.Sq15);
            P.Apply.DetectTrap(Range.Sq15);
            P.Apply.DetectGate(Range.Sq15);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            S.Apply.DetectEntity(Range.Sq20);
            S.Apply.DetectItem(Range.Sq20);
            S.Apply.DetectTrap(Range.Sq20);
            S.Apply.DetectGate(Range.Sq20);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            E.Apply.DetectEntity(Range.Sq25);
            E.Apply.DetectItem(Range.Sq25);
            E.Apply.DetectTrap(Range.Sq25);
            E.Apply.DetectGate(Range.Sq25);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            M.Apply.DetectEntity(Range.Sq30);
            M.Apply.DetectItem(Range.Sq30);
            M.Apply.DetectTrap(Range.Sq30);
            M.Apply.DetectGate(Range.Sq30);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            C.Apply.DetectEntity(Range.Sq35);
            C.Apply.DetectItem(Range.Sq35);
            C.Apply.DetectTrap(Range.Sq35);
            C.Apply.DetectGate(Range.Sq35);
          }
        );
      });

      premonition = AddSpell(Schools.divination, "premonition", 6, new Precept(Purpose.Buff, Properties.warning), Glyphs.premonition_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.Zero);
            U.Apply.ApplyTransient(Properties.warning, 1.d6());
            U.Apply.ApplyTransient(Properties.telepathy, 1.d6());
            U.Apply.ApplyTransient(Properties.see_invisible, 1.d6());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.One);
            P.Apply.ApplyTransient(Properties.warning, 3.d6());
            P.Apply.ApplyTransient(Properties.telepathy, 3.d6());
            P.Apply.ApplyTransient(Properties.see_invisible, 3.d6());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.One);
            S.Apply.ApplyTransient(Properties.warning, 6.d6());
            S.Apply.ApplyTransient(Properties.telepathy, 6.d6());
            S.Apply.ApplyTransient(Properties.see_invisible, 6.d6());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.One);
            E.Apply.ApplyTransient(Properties.warning, 9.d6());
            E.Apply.ApplyTransient(Properties.telepathy, 9.d6());
            E.Apply.ApplyTransient(Properties.see_invisible, 9.d6());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.One);
            M.Apply.ApplyTransient(Properties.warning, 12.d6());
            M.Apply.ApplyTransient(Properties.telepathy, 12.d6());
            M.Apply.ApplyTransient(Properties.see_invisible, 12.d6());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.One);
            C.Apply.ApplyTransient(Properties.warning, 15.d6());
            C.Apply.ApplyTransient(Properties.telepathy, 15.d6());
            C.Apply.ApplyTransient(Properties.see_invisible, 15.d6());
          }
        );
      });

      world_vision = AddSpell(Schools.divination, "world vision", 7, null, Glyphs.world_vision_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            U.Apply.Mapping(Range.Sq15, Chance.ThreeIn4);
            U.Apply.DetectEntity(Range.Sq15);
            U.Apply.DetectItem(Range.Sq15);
            U.Apply.DetectTrap(Range.Sq15);
            U.Apply.DetectGate(Range.Sq15);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            P.Apply.Mapping(Range.Sq15, Chance.Always);
            P.Apply.DetectEntity(Range.Sq15);
            P.Apply.DetectItem(Range.Sq15);
            P.Apply.DetectTrap(Range.Sq15);
            P.Apply.DetectGate(Range.Sq15);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            S.Apply.Mapping(Range.Sq20, Chance.Always);
            S.Apply.DetectEntity(Range.Sq20);
            S.Apply.DetectItem(Range.Sq20);
            S.Apply.DetectTrap(Range.Sq20);
            S.Apply.DetectGate(Range.Sq20);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            E.Apply.Mapping(Range.Sq25, Chance.Always);
            E.Apply.DetectEntity(Range.Sq25);
            E.Apply.DetectItem(Range.Sq25);
            E.Apply.DetectTrap(Range.Sq25);
            E.Apply.DetectGate(Range.Sq25);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            M.Apply.Mapping(Range.Sq30, Chance.Always);
            M.Apply.DetectEntity(Range.Sq30);
            M.Apply.DetectItem(Range.Sq30);
            M.Apply.DetectTrap(Range.Sq30);
            M.Apply.DetectGate(Range.Sq30);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            C.Apply.Mapping(Range.Sq35, Chance.Always);
            C.Apply.DetectEntity(Range.Sq35);
            C.Apply.DetectItem(Range.Sq35);
            C.Apply.DetectTrap(Range.Sq35);
            C.Apply.DetectGate(Range.Sq35);
          }
        );
      });

      hex = AddSpell(Schools.enchantment, "hex", 1, new Precept(Purpose.Debuff, Properties.fumbling), Glyphs.hex_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            U.Apply.ApplyTransient(Properties.fumbling, 1.d4() + 2);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            P.Apply.ApplyTransient(Properties.fumbling, 1.d4() + 4);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Fixed(2)).SetTargetSelf(false);
            S.Apply.ApplyTransient(Properties.fumbling, 1.d6() + 4);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Fixed(3)).SetTargetSelf(false);
            E.Apply.ApplyTransient(Properties.fumbling, 1.d6() + 6);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Fixed(4)).SetTargetSelf(false);
            M.Apply.ApplyTransient(Properties.fumbling, 1.d8() + 6);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Fixed(5)).SetTargetSelf(false);
            C.Apply.ApplyTransient(Properties.fumbling, 1.d8() + 8);
          }
        );
      });

      daze = AddSpell(Schools.enchantment, "daze", 1, new Precept(Purpose.Debuff, Properties.stunned), Glyphs.daze_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            U.Apply.ApplyTransient(Properties.stunned, 1.d2());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            P.Apply.ApplyTransient(Properties.stunned, 1.d3());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Fixed(2)).SetTargetSelf(false);
            S.Apply.ApplyTransient(Properties.stunned, 1.d3() + 1);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Fixed(3)).SetTargetSelf(false);
            E.Apply.ApplyTransient(Properties.stunned, 1.d4() + 1);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Fixed(4)).SetTargetSelf(false);
            M.Apply.ApplyTransient(Properties.stunned, 1.d4() + 2);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Fixed(5)).SetTargetSelf(false);
            C.Apply.ApplyTransient(Properties.stunned, 1.d6() + 2);
          }
        );
      });

      swoon = AddSpell(Schools.enchantment, "swoon", 2, new Precept(Purpose.Debuff, Properties.fainting), Glyphs.swoon_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            U.Apply.ApplyTransient(Properties.fainting, 1.d4() + 1);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            P.Apply.ApplyTransient(Properties.fainting, 1.d4() + 3);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Fixed(2)).SetTargetSelf(false);
            S.Apply.ApplyTransient(Properties.fainting, 1.d6() + 3);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Fixed(3)).SetTargetSelf(false);
            E.Apply.ApplyTransient(Properties.fainting, 1.d6() + 5);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Fixed(4)).SetTargetSelf(false);
            M.Apply.ApplyTransient(Properties.fainting, 1.d8() + 5);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Fixed(5)).SetTargetSelf(false);
            C.Apply.ApplyTransient(Properties.fainting, 1.d8() + 7);
          }
        );
      });

      calm = AddSpell(Schools.enchantment, "calm", 1, new Precept(Purpose.Debuff, Elements.magical), Glyphs.calm_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            U.Apply.WhenChance(Chance.OneIn2, T => T.PacifyEntity(Elements.magical, Kinds.Living.ToArray()));
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            P.Apply.WhenChance(Chance.OneIn3, T => T.PacifyEntity(Elements.magical, Kinds.Living.ToArray()));
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Fixed(2)).SetTargetSelf(false);
            S.Apply.PacifyEntity(Elements.magical, Kinds.Living.ToArray());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Fixed(3)).SetTargetSelf(false);
            E.Apply.PacifyEntity(Elements.magical, Kinds.Living.ToArray());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Fixed(4)).SetTargetSelf(false);
            M.Apply.PacifyEntity(Elements.magical, Kinds.Living.ToArray());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Fixed(5)).SetTargetSelf(false);
            C.Apply.PacifyEntity(Elements.magical, Kinds.Living.ToArray());
          }
        );
      });

      hideous_laughter = AddSpell(Schools.enchantment, "hideous laughter", 2, new Precept(Purpose.Debuff, Properties.hallucination), Glyphs.hideous_laughter_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            U.Apply.ApplyTransient(Properties.hallucination, 1.d6() + 3);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            P.Apply.ApplyTransient(Properties.hallucination, 1.d6() + 5);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Fixed(2)).SetTargetSelf(false);
            S.Apply.ApplyTransient(Properties.hallucination, 1.d8() + 5);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Fixed(3)).SetTargetSelf(false);
            E.Apply.ApplyTransient(Properties.hallucination, 1.d8() + 7);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Fixed(4)).SetTargetSelf(false);
            M.Apply.ApplyTransient(Properties.hallucination, 1.d10() + 7);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Fixed(5)).SetTargetSelf(false);
            C.Apply.ApplyTransient(Properties.hallucination, 1.d10() + 9);
          }
        );
      });

      psychic_shove = AddSpell(Schools.enchantment, "psychic shove", 2, new Precept(Purpose.Toss, Elements.magical), Glyphs.psychic_shove_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.One).SetTargetSelf(false);
            U.Apply.HarmEntity(Elements.magical, 1.d4());
            U.Apply.Knockback();
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, Dice.One).SetTargetSelf(false);
            P.Apply.HarmEntity(Elements.magical, 1.d6());
            P.Apply.Knockback();
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, Dice.Fixed(2)).SetTargetSelf(false);
            S.Apply.HarmEntity(Elements.magical, 2.d4());
            S.Apply.Knockback();
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, Dice.Fixed(3)).SetTargetSelf(false);
            E.Apply.HarmEntity(Elements.magical, 2.d6());
            E.Apply.Knockback();
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, Dice.Fixed(3)).SetTargetSelf(false);
            M.Apply.HarmEntity(Elements.magical, 3.d4());
            M.Apply.Knockback();
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, Dice.Fixed(4)).SetTargetSelf(false);
            C.Apply.HarmEntity(Elements.magical, 3.d6());
            C.Apply.Knockback();
          }
        );
      });

      battle_fury = AddSpell(Schools.enchantment, "battle fury", 3, new Precept(Purpose.Buff, Properties.rage), Glyphs.battle_fury_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.magic, Dice.Zero);
            U.Apply.ApplyTransient(Properties.rage, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.magic, Dice.One);
            P.Apply.ApplyTransient(Properties.rage, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.magic, Dice.One);
            S.Apply.ApplyTransient(Properties.rage, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.magic, Dice.One);
            E.Apply.ApplyTransient(Properties.rage, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.magic, Dice.One);
            M.Apply.ApplyTransient(Properties.rage, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.magic, Dice.One);
            C.Apply.ApplyTransient(Properties.rage, 1.d15() + 151);
          }
        );
      });

      tongue_tied = AddSpell(Schools.enchantment, "tongue tied", 2, new Precept(Purpose.Debuff, Properties.silence), Glyphs.tongue_tied_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            U.Apply.ApplyTransient(Properties.silence, 1.d6() + 4);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            P.Apply.ApplyTransient(Properties.silence, 1.d6() + 8);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Fixed(2)).SetTargetSelf(false);
            S.Apply.ApplyTransient(Properties.silence, 1.d8() + 8);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Fixed(3)).SetTargetSelf(false);
            E.Apply.ApplyTransient(Properties.silence, 1.d8() + 12);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Fixed(4)).SetTargetSelf(false);
            M.Apply.ApplyTransient(Properties.silence, 1.d10() + 12);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Fixed(5)).SetTargetSelf(false);
            C.Apply.ApplyTransient(Properties.silence, 1.d10() + 16);
          }
        );
      });

      grasping_mind = AddSpell(Schools.enchantment, "grasping mind", 3, new Precept(Purpose.Debuff, Elements.magical), Glyphs.grasping_mind_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            U.Apply.GrappleEntity(1.d4() + 2);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            P.Apply.GrappleEntity(1.d4() + 4);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Fixed(2)).SetTargetSelf(false);
            S.Apply.GrappleEntity(1.d6() + 4);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Fixed(3)).SetTargetSelf(false);
            E.Apply.GrappleEntity(1.d6() + 6);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Fixed(4)).SetTargetSelf(false);
            M.Apply.GrappleEntity(1.d8() + 6);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Fixed(5)).SetTargetSelf(false);
            C.Apply.GrappleEntity(1.d8() + 8);
          }
        );
      });

      mind_spike = AddSpell(Schools.enchantment, "mind spike", 3, new Precept(Purpose.Punish, Elements.magical), Glyphs.mind_spike_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            U.Apply.DrainMana(Elements.magical, 2.d4());
            U.Apply.HarmEntity(Elements.magical, 1.d4());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            P.Apply.DrainMana(Elements.magical, 3.d4());
            P.Apply.HarmEntity(Elements.magical, 1.d4());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Fixed(2)).SetTargetSelf(false);
            S.Apply.DrainMana(Elements.magical, 4.d4());
            S.Apply.HarmEntity(Elements.magical, 1.d4());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Fixed(3)).SetTargetSelf(false);
            E.Apply.DrainMana(Elements.magical, 5.d4());
            E.Apply.HarmEntity(Elements.magical, 1.d4());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Fixed(4)).SetTargetSelf(false);
            M.Apply.DrainMana(Elements.magical, 6.d4());
            M.Apply.HarmEntity(Elements.magical, 1.d4());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Fixed(5)).SetTargetSelf(false);
            C.Apply.DrainMana(Elements.magical, 7.d4());
            C.Apply.HarmEntity(Elements.magical, 1.d4());
          }
        );
      });

      iron_will = AddSpell(Schools.enchantment, "iron will", 2, new Precept(Purpose.Buff, Properties.clarity), Glyphs.iron_will_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.magic, Dice.Zero).SetTargetSelf(true);
            U.Apply.ApplyTransient(Properties.clarity, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.magic, Dice.One).SetTargetSelf(true);
            P.Apply.ApplyTransient(Properties.clarity, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.magic, Dice.One).SetTargetSelf(true);
            S.Apply.ApplyTransient(Properties.clarity, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.magic, Dice.One).SetTargetSelf(true);
            E.Apply.ApplyTransient(Properties.clarity, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.magic, Dice.One).SetTargetSelf(true);
            M.Apply.ApplyTransient(Properties.clarity, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.magic, Dice.One).SetTargetSelf(true);
            C.Apply.ApplyTransient(Properties.clarity, 1.d15() + 151);
          }
        );
      });

      hold_person = AddSpell(Schools.enchantment, "hold person", 4, new Precept(Purpose.Debuff, Properties.paralysis), Glyphs.hold_person_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            U.Apply.WhenTargetKind(Kinds.Living, T => T.ApplyTransient(Properties.paralysis, 1.d3()));
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            P.Apply.WhenTargetKind(Kinds.Living, T => T.ApplyTransient(Properties.paralysis, 1.d4()));
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Fixed(2)).SetTargetSelf(false);
            S.Apply.WhenTargetKind(Kinds.Living, T => T.ApplyTransient(Properties.paralysis, 1.d4() + 1));
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Fixed(3)).SetTargetSelf(false);
            E.Apply.WhenTargetKind(Kinds.Living, T => T.ApplyTransient(Properties.paralysis, 1.d6() + 1));
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Fixed(4)).SetTargetSelf(false);
            M.Apply.WhenTargetKind(Kinds.Living, T => T.ApplyTransient(Properties.paralysis, 1.d6() + 2));
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Fixed(5)).SetTargetSelf(false);
            C.Apply.WhenTargetKind(Kinds.Living, T => T.ApplyTransient(Properties.paralysis, 1.d8() + 2));
          }
        );
      });

      mind_link = AddSpell(Schools.enchantment, "mind link", 3, new Precept(Purpose.Buff, Properties.telepathy), Glyphs.mind_link_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.magic, Dice.Zero).SetTargetSelf(true);
            U.Apply.ApplyTransient(Properties.telepathy, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.magic, Dice.One).SetTargetSelf(true);
            P.Apply.ApplyTransient(Properties.telepathy, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.magic, Dice.One).SetTargetSelf(true);
            S.Apply.ApplyTransient(Properties.telepathy, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.magic, Dice.One).SetTargetSelf(true);
            E.Apply.ApplyTransient(Properties.telepathy, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.magic, Dice.One).SetTargetSelf(true);
            M.Apply.ApplyTransient(Properties.telepathy, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.magic, Dice.One).SetTargetSelf(true);
            C.Apply.ApplyTransient(Properties.telepathy, 1.d15() + 151);
          }
        );
      });

      clouded_mind = AddSpell(Schools.enchantment, "clouded mind", 4, new Precept(Purpose.Buff, Properties.displacement), Glyphs.clouded_mind_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero).SetTargetSelf(true);
            U.Apply.ApplyTransient(Properties.displacement, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One).SetTargetSelf(true);
            P.Apply.ApplyTransient(Properties.displacement, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One).SetTargetSelf(true);
            S.Apply.ApplyTransient(Properties.displacement, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One).SetTargetSelf(true);
            E.Apply.ApplyTransient(Properties.displacement, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One).SetTargetSelf(true);
            M.Apply.ApplyTransient(Properties.displacement, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One).SetTargetSelf(true);
            C.Apply.ApplyTransient(Properties.displacement, 1.d15() + 151);
          }
        );
      });

      song_of_discord = AddSpell(Schools.enchantment, "song of discord", 5, new Precept(Purpose.AreaOfEffect, Properties.conflict), Glyphs.song_of_discord_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Explosion(Explosions.magical, 1.d6());
            U.Apply.AreaTransient(Properties.conflict, 1.d4() + 2, Kinds.Living.ToArray());
          },
          P =>
          {
            P.SetCast().Explosion(Explosions.magical, 1.d6() + 3);
            P.Apply.AreaTransient(Properties.conflict, 1.d4() + 4, Kinds.Living.ToArray());
          },
          S =>
          {
            S.SetCast().Explosion(Explosions.magical, 1.d6() + 4);
            S.Apply.AreaTransient(Properties.conflict, 1.d6() + 4, Kinds.Living.ToArray());
          },
          E =>
          {
            E.SetCast().Explosion(Explosions.magical, 1.d6() + 5);
            E.Apply.AreaTransient(Properties.conflict, 1.d6() + 6, Kinds.Living.ToArray());
          },
          M =>
          {
            M.SetCast().Explosion(Explosions.magical, 1.d6() + 6);
            M.Apply.AreaTransient(Properties.conflict, 1.d8() + 6, Kinds.Living.ToArray());
          },
          C =>
          {
            C.SetCast().Explosion(Explosions.magical, 1.d6() + 7);
            C.Apply.AreaTransient(Properties.conflict, 1.d8() + 8, Kinds.Living.ToArray());
          }
        );
      });

      creeping_palsy = AddSpell(Schools.enchantment, "creeping palsy", 4, new Precept(Purpose.Debuff, Properties.fumbling), Glyphs.creeping_palsy_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            U.Apply.ApplyTransient(Properties.fumbling, 1.d6() + 4);
            U.Apply.WhenChance(Chance.OneIn6, T => T.ApplyTransient(Properties.stunned, 1.d2()));
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.One).SetTargetSelf(false);
            P.Apply.ApplyTransient(Properties.fumbling, 1.d6() + 8);
            P.Apply.WhenChance(Chance.OneIn4, T => T.ApplyTransient(Properties.stunned, 1.d2()));
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Fixed(2)).SetTargetSelf(false);
            S.Apply.ApplyTransient(Properties.fumbling, 1.d8() + 8);
            S.Apply.WhenChance(Chance.OneIn4, T => T.ApplyTransient(Properties.stunned, 1.d3()));
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Fixed(3)).SetTargetSelf(false);
            E.Apply.ApplyTransient(Properties.fumbling, 1.d8() + 12);
            E.Apply.WhenChance(Chance.OneIn3, T => T.ApplyTransient(Properties.stunned, 1.d3()));
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Fixed(4)).SetTargetSelf(false);
            M.Apply.ApplyTransient(Properties.fumbling, 1.d10() + 12);
            M.Apply.WhenChance(Chance.OneIn3, T => T.ApplyTransient(Properties.stunned, 1.d4()));
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Fixed(5)).SetTargetSelf(false);
            C.Apply.ApplyTransient(Properties.fumbling, 1.d10() + 16);
            C.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.stunned, 1.d4()));
          }
        );
      });

      dominate_mind = AddSpell(Schools.enchantment, "dominate mind", 6, new Precept(Purpose.AreaOfEffect, Elements.magical), Glyphs.dominate_mind_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Explosion(Explosions.magical, 1.d6() + 2);
            U.Apply.CharmEntity(Elements.magical, Delay.FromTurns(200), Kinds.Living.ToArray());
          },
          P =>
          {
            P.SetCast().Explosion(Explosions.magical, 1.d6() + 5);
            P.Apply.CharmEntity(Elements.magical, Delay.FromTurns(500), Kinds.Living.ToArray());
          },
          S =>
          {
            S.SetCast().Explosion(Explosions.magical, 1.d6() + 6);
            S.Apply.CharmEntity(Elements.magical, Delay.FromTurns(1000), Kinds.Living.ToArray());
          },
          E =>
          {
            E.SetCast().Explosion(Explosions.magical, 1.d6() + 7);
            E.Apply.CharmEntity(Elements.magical, Delay.FromTurns(2000), Kinds.Living.ToArray());
          },
          M =>
          {
            M.SetCast().Explosion(Explosions.magical, 1.d6() + 8);
            M.Apply.CharmEntity(Elements.magical, Delay.FromTurns(4000), Kinds.Living.ToArray());
          },
          C =>
          {
            C.SetCast().Explosion(Explosions.magical, 1.d6() + 9);
            C.Apply.CharmEntity(Elements.magical, Delay.FromTurns(8000), Kinds.Living.ToArray());
          }
        );
      });

      mindrend = AddSpell(Schools.enchantment, "mindrend", 7, new Precept(Purpose.Punish, Elements.magical), Glyphs.mindrend_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates();
            U.Apply.HarmEntity(Elements.magical, 8.d6());
            U.Apply.DrainMana(Elements.magical, 3.d6() + 3);
            U.Apply.ApplyTransient(Properties.paralysis, 1.d4());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.Fixed(2))
             .SetTargetSelf(false)
             .SetPenetrates();
            P.Apply.HarmEntity(Elements.magical, 10.d6());
            P.Apply.DrainMana(Elements.magical, 4.d6() + 4);
            P.Apply.ApplyTransient(Properties.paralysis, 1.d4() + 1);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Fixed(3))
             .SetTargetSelf(false)
             .SetPenetrates();
            S.Apply.HarmEntity(Elements.magical, 12.d6());
            S.Apply.DrainMana(Elements.magical, 5.d6() + 5);
            S.Apply.ApplyTransient(Properties.paralysis, 1.d4() + 2);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Fixed(4))
             .SetTargetSelf(false)
             .SetPenetrates();
            E.Apply.HarmEntity(Elements.magical, 14.d6());
            E.Apply.DrainMana(Elements.magical, 6.d6() + 6);
            E.Apply.ApplyTransient(Properties.paralysis, 1.d6() + 2);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Fixed(5))
             .SetTargetSelf(false)
             .SetPenetrates();
            M.Apply.HarmEntity(Elements.magical, 16.d6());
            M.Apply.DrainMana(Elements.magical, 7.d6() + 7);
            M.Apply.ApplyTransient(Properties.paralysis, 1.d6() + 3);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Fixed(5))
             .SetTargetSelf(false)
             .SetPenetrates();
            C.Apply.HarmEntity(Elements.magical, 18.d6());
            C.Apply.DrainMana(Elements.magical, 8.d6() + 8);
            C.Apply.ApplyTransient(Properties.paralysis, 1.d6() + 4);
          }
        );
      });

      bless = AddSpell(Schools.clerical, "bless", 1, new Precept(Purpose.Buff, Properties.clarity), Glyphs.bless_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U => { U.SetCast().Strike(Strikes.spirit, Dice.Zero); U.Apply.ApplyTransient(Properties.clarity, 1.d20() + 20); },
          P => { P.SetCast().Strike(Strikes.spirit, Dice.One); P.Apply.ApplyTransient(Properties.clarity, 1.d20() + 40); },
          S => { S.SetCast().Strike(Strikes.spirit, Dice.One); S.Apply.ApplyTransient(Properties.clarity, 1.d20() + 60); },
          E => { E.SetCast().Strike(Strikes.spirit, Dice.One); E.Apply.ApplyTransient(Properties.clarity, 1.d20() + 80); },
          M => { M.SetCast().Strike(Strikes.spirit, Dice.One); M.Apply.ApplyTransient(Properties.clarity, 1.d20() + 100); },
          C => { C.SetCast().Strike(Strikes.spirit, Dice.One); C.Apply.ApplyTransient(Properties.clarity, 1.d20() + 120); }
        );
      });

      sense_the_restless = AddSpell(Schools.clerical, "sense the restless", 1, null, Glyphs.sense_the_restless_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U => { U.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); U.Apply.DetectEntity(Range.Sq10, Kinds.Undead.ToArray()); },
          P => { P.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); P.Apply.DetectEntity(Range.Sq15, Kinds.Undead.ToArray()); },
          S => { S.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates(); S.Apply.DetectEntity(Range.Sq20, Kinds.Undead.ToArray()); },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            E.Apply.DetectEntity(Range.Sq25, Kinds.Undead.ToArray());
            E.Apply.DetectEntity(Range.Sq25, [Kinds.demon]);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            M.Apply.DetectEntity(Range.Sq30, Kinds.Undead.ToArray());
            M.Apply.DetectEntity(Range.Sq30, [Kinds.demon]);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Zero).SetTerminates();
            C.Apply.DetectEntity(Range.Sq35, Kinds.Undead.ToArray());
            C.Apply.DetectEntity(Range.Sq35, [Kinds.demon]);
          }
        );
      });

      command = AddSpell(Schools.clerical, "command", 2, new Precept(Purpose.Blast, Properties.paralysis), Glyphs.command_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U => { U.SetCast().Strike(Strikes.holy, Dice.One); U.Apply.ApplyTransient(Properties.paralysis, 1.d2()); },
          P => { P.SetCast().Strike(Strikes.holy, Dice.One); P.Apply.ApplyTransient(Properties.paralysis, 1.d3()); },
          S => { S.SetCast().Strike(Strikes.holy, Dice.Fixed(2)); S.Apply.ApplyTransient(Properties.paralysis, 1.d4()); },
          E => { E.SetCast().Strike(Strikes.holy, Dice.Fixed(2)); E.Apply.ApplyTransient(Properties.paralysis, 1.d4() + 1); },
          M => { M.SetCast().Strike(Strikes.holy, Dice.Fixed(3)); M.Apply.ApplyTransient(Properties.paralysis, 1.d4() + 2); },
          C => { C.SetCast().Strike(Strikes.holy, Dice.Fixed(3)); C.Apply.ApplyTransient(Properties.paralysis, 1.d4() + 3); }
        );
      });

      bane = AddSpell(Schools.clerical, "bane", 2, new Precept(Purpose.Blast, Properties.fumbling), Glyphs.bane_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U => { U.SetCast().Strike(Strikes.holy, Dice.One); U.Apply.ApplyTransient(Properties.fumbling, 1.d6() + 2); },
          P => { P.SetCast().Strike(Strikes.holy, Dice.One); P.Apply.ApplyTransient(Properties.fumbling, 1.d6() + 4); },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Fixed(2));
            S.Apply.ApplyTransient(Properties.fumbling, 1.d6() + 6);
            S.Apply.ApplyTransient(Properties.slowness, 1.d6() + 2);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Fixed(2));
            E.Apply.ApplyTransient(Properties.fumbling, 1.d6() + 8);
            E.Apply.ApplyTransient(Properties.slowness, 1.d6() + 4);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Fixed(3));
            M.Apply.ApplyTransient(Properties.fumbling, 1.d6() + 10);
            M.Apply.ApplyTransient(Properties.slowness, 1.d6() + 6);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Fixed(3));
            C.Apply.ApplyTransient(Properties.fumbling, 1.d6() + 12);
            C.Apply.ApplyTransient(Properties.slowness, 1.d6() + 8);
          }
        );
      });

      sanctuary = AddSpell(Schools.clerical, "sanctuary", 2, new Precept(Purpose.Block), Glyphs.sanctuary_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U => { U.SetCast().Strike(Strikes.shield, Dice.Zero).SetTargetSelf(); U.Apply.Repel(Range.Sq2, Items: false, Characters: true, Boulders: false); },
          P => { P.SetCast().Strike(Strikes.shield, Dice.Zero).SetTargetSelf(); P.Apply.Repel(Range.Sq3, Items: false, Characters: true, Boulders: false); },
          S => { S.SetCast().Strike(Strikes.shield, Dice.Zero).SetTargetSelf(); S.Apply.Repel(Range.Sq3, Items: true, Characters: true, Boulders: false); },
          E => { E.SetCast().Strike(Strikes.shield, Dice.Zero).SetTargetSelf(); E.Apply.Repel(Range.Sq4, Items: true, Characters: true, Boulders: false); },
          M => { M.SetCast().Strike(Strikes.shield, Dice.Zero).SetTargetSelf(); M.Apply.Repel(Range.Sq4, Items: true, Characters: true, Boulders: true); },
          C => { C.SetCast().Strike(Strikes.shield, Dice.Zero).SetTargetSelf(); C.Apply.Repel(Range.Sq5, Items: true, Characters: true, Boulders: true); }
        );
      });

      commune = AddSpell(Schools.clerical, "commune", 2, null, Glyphs.commune_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U => { U.SetCast().Plain(Dice.Zero); U.Apply.Rumour(Attributes.wisdom, Skills.literacy, Truth: true, Lies: true); },
          P => { P.SetCast().Plain(Dice.Zero); P.Apply.Rumour(Attributes.wisdom, Skills.literacy, Truth: true, Lies: true); },
          S => { S.SetCast().Plain(Dice.Zero); S.Apply.Rumour(Attributes.wisdom, Skills.literacy, Truth: true, Lies: false); },
          E => { E.SetCast().Plain(Dice.Zero); E.Apply.Rumour(Attributes.wisdom, Skills.literacy, Truth: true, Lies: false); },
          M =>
          {
            M.SetCast().Plain(Dice.Zero);
            M.Apply.Rumour(Attributes.wisdom, Skills.literacy, Truth: true, Lies: false);
            M.Apply.Rumour(Attributes.wisdom, Skills.divination, Truth: true, Lies: false);
          },
          C =>
          {
            C.SetCast().Plain(Dice.Zero);
            C.Apply.Rumour(Attributes.wisdom, Skills.literacy, Truth: true, Lies: false);
            C.Apply.Rumour(Attributes.wisdom, Skills.divination, Truth: true, Lies: false);
          }
        );
      });

      silence = AddSpell(Schools.clerical, "silence", 3, new Precept(Purpose.Blast, Properties.silence), Glyphs.silence_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U => { U.SetCast().Strike(Strikes.holy, Dice.One); U.Apply.ApplyTransient(Properties.silence, 4.d6()); },
          P => { P.SetCast().Strike(Strikes.holy, Dice.One); P.Apply.ApplyTransient(Properties.silence, 6.d6()); },
          S => { S.SetCast().Strike(Strikes.holy, Dice.Fixed(2)); S.Apply.ApplyTransient(Properties.silence, 8.d6()); },
          E => { E.SetCast().Strike(Strikes.holy, Dice.Fixed(2)); E.Apply.ApplyTransient(Properties.silence, 10.d6()); },
          M => { M.SetCast().Strike(Strikes.holy, Dice.Fixed(3)); M.Apply.ApplyTransient(Properties.silence, 12.d6()); },
          C => { C.SetCast().Strike(Strikes.holy, Dice.Fixed(3)); C.Apply.ApplyTransient(Properties.silence, 14.d6()); }
        );
      });

      freedom_of_movement = AddSpell(Schools.clerical, "freedom of movement", 3, new Precept(Purpose.Buff, Properties.free_action), Glyphs.freedom_of_movement_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U => { U.SetCast().Strike(Strikes.spirit, Dice.Zero); U.Apply.ApplyTransient(Properties.free_action, 1.d15() + 15); },
          P => { P.SetCast().Strike(Strikes.spirit, Dice.One); P.Apply.ApplyTransient(Properties.free_action, 1.d15() + 30); },
          S => { S.SetCast().Strike(Strikes.spirit, Dice.One); S.Apply.ApplyTransient(Properties.free_action, 1.d15() + 60); },
          E => { E.SetCast().Strike(Strikes.spirit, Dice.One); E.Apply.ApplyTransient(Properties.free_action, 1.d15() + 90); },
          M => { M.SetCast().Strike(Strikes.spirit, Dice.One); M.Apply.ApplyTransient(Properties.free_action, 1.d15() + 120); },
          C => { C.SetCast().Strike(Strikes.spirit, Dice.One); C.Apply.ApplyTransient(Properties.free_action, 1.d15() + 150); }
        );
      });

      consecration = AddSpell(Schools.clerical, "consecration", 3, null, Glyphs.consecration_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U => { U.SetCast().Plain(Dice.Zero); U.Apply.Sanctify(Item: null, Sanctities.Blessed); },
          P => { P.SetCast().FilterSanctity(Sanctities.Uncursed); P.Apply.Sanctify(Item: null, Sanctities.Blessed); },
          S => { S.SetCast().FilterSanctity(Sanctities.Uncursed); S.Apply.Sanctify(Item: null, Sanctities.Blessed); },
          E => { E.SetCast().FilterSanctity(Sanctities.Uncursed, Sanctities.Cursed); E.Apply.Sanctify(Item: null, Sanctities.Blessed); },
          M => { M.SetCast().FilterSanctity(Sanctities.Uncursed, Sanctities.Cursed); M.Apply.Sanctify(Item: null, Sanctities.Blessed); },
          C => { C.SetCast().FilterSanctity(Sanctities.List.ToArray()); C.Apply.Sanctify(Item: null, Sanctities.Blessed); }
        );
      });

      ward_of_return = AddSpell(Schools.clerical, "ward of return", 4, new Precept(Purpose.Buff, Properties.lifesaving), Glyphs.ward_of_return_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.lifesaving, 2.d60());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.lifesaving, 4.d60());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.lifesaving, 6.d60());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.lifesaving, 8.d60());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.lifesaving, 10.d60());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.lifesaving, 12.d60());
          }
        );
      });

      searing_light = AddSpell(Schools.clerical, "searing light", 4, new Precept(Purpose.Blast), Glyphs.searing_light_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.One);
            U.Apply.HarmEntity(Elements.fire, 3.d6());
            U.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.fire, 3.d6()));
            U.Apply.WhenTargetKind([Kinds.demon], T => T.HarmEntity(Elements.fire, 3.d6()));
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.One);
            P.Apply.HarmEntity(Elements.fire, 4.d6());
            P.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.fire, 4.d6()));
            P.Apply.WhenTargetKind([Kinds.demon], T => T.HarmEntity(Elements.fire, 4.d6()));
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Fixed(2));
            S.Apply.HarmEntity(Elements.fire, 5.d6());
            S.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.fire, 5.d6()));
            S.Apply.WhenTargetKind([Kinds.demon], T => T.HarmEntity(Elements.fire, 5.d6()));
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Fixed(2));
            E.Apply.HarmEntity(Elements.fire, 6.d6());
            E.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.fire, 6.d6()));
            E.Apply.WhenTargetKind([Kinds.demon], T => T.HarmEntity(Elements.fire, 6.d6()));
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Fixed(3));
            M.Apply.HarmEntity(Elements.fire, 7.d6());
            M.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.fire, 7.d6()));
            M.Apply.WhenTargetKind([Kinds.demon], T => T.HarmEntity(Elements.fire, 7.d6()));
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Fixed(3));
            C.Apply.HarmEntity(Elements.fire, 8.d6());
            C.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.fire, 8.d6()));
            C.Apply.WhenTargetKind([Kinds.demon], T => T.HarmEntity(Elements.fire, 8.d6()));
          }
        );
      });

      prayer = AddSpell(Schools.clerical, "prayer", 4, new Precept(Purpose.Buff, Properties.warning), Glyphs.prayer_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.warning, 2.d60());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.warning, 4.d60());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.warning, 6.d60());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.warning, 8.d60());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.warning, 10.d60());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.warning, 12.d60());
          }
        );
      });

      spirit_shield = AddSpell(Schools.clerical, "spirit shield", 5, new Precept(Purpose.Buff, Properties.reflection), Glyphs.spirit_shield_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.shield, Dice.Zero);
            U.Apply.ApplyTransient(Properties.reflection, 1.d15() + 15);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.shield, Dice.One);
            P.Apply.ApplyTransient(Properties.reflection, 1.d15() + 30);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.shield, Dice.One);
            S.Apply.ApplyTransient(Properties.reflection, 1.d15() + 60);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.shield, Dice.One);
            E.Apply.ApplyTransient(Properties.reflection, 1.d15() + 90);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.shield, Dice.One);
            M.Apply.ApplyTransient(Properties.reflection, 1.d15() + 120);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.shield, Dice.One);
            C.Apply.ApplyTransient(Properties.reflection, 1.d15() + 150);
          }
        );
      });

      holy_word = AddSpell(Schools.clerical, "holy word", 6, null, Glyphs.holy_word_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Explosion(Explosions.light, 1.d4() + 2);
            U.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.fire, 4.d6()));
            U.Apply.WhenTargetKind([Kinds.demon], T => T.HarmEntity(Elements.fire, 4.d6()));
            U.Apply.AreaTransient(Properties.stunned, 1.d4(), Kinds.Undead.ToArray());
            U.Apply.AreaTransient(Properties.stunned, 1.d4(), [Kinds.demon]);
          },
          P =>
          {
            P.SetCast().Explosion(Explosions.light, 1.d4() + 3);
            P.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.fire, 6.d6()));
            P.Apply.WhenTargetKind([Kinds.demon], T => T.HarmEntity(Elements.fire, 6.d6()));
            P.Apply.AreaTransient(Properties.stunned, 1.d4() + 1, Kinds.Undead.ToArray());
            P.Apply.AreaTransient(Properties.stunned, 1.d4() + 1, [Kinds.demon]);
          },
          S =>
          {
            S.SetCast().Explosion(Explosions.light, 1.d4() + 4);
            S.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.fire, 8.d6()));
            S.Apply.WhenTargetKind([Kinds.demon], T => T.HarmEntity(Elements.fire, 8.d6()));
            S.Apply.AreaTransient(Properties.stunned, 1.d4() + 2, Kinds.Undead.ToArray());
            S.Apply.AreaTransient(Properties.stunned, 1.d4() + 2, [Kinds.demon]);
          },
          E =>
          {
            E.SetCast().Explosion(Explosions.light, 1.d4() + 5);
            E.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.fire, 10.d6()));
            E.Apply.WhenTargetKind([Kinds.demon], T => T.HarmEntity(Elements.fire, 10.d6()));
            E.Apply.AreaTransient(Properties.stunned, 1.d6() + 2, Kinds.Undead.ToArray());
            E.Apply.AreaTransient(Properties.stunned, 1.d6() + 2, [Kinds.demon]);
          },
          M =>
          {
            M.SetCast().Explosion(Explosions.light, 1.d4() + 6);
            M.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.fire, 12.d6()));
            M.Apply.WhenTargetKind([Kinds.demon], T => T.HarmEntity(Elements.fire, 12.d6()));
            M.Apply.AreaTransient(Properties.stunned, 1.d6() + 3, Kinds.Undead.ToArray());
            M.Apply.AreaTransient(Properties.stunned, 1.d6() + 3, [Kinds.demon]);
          },
          C =>
          {
            C.SetCast().Explosion(Explosions.light, 1.d4() + 7);
            C.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.fire, 14.d6()));
            C.Apply.WhenTargetKind([Kinds.demon], T => T.HarmEntity(Elements.fire, 14.d6()));
            C.Apply.AreaTransient(Properties.stunned, 1.d6() + 4, Kinds.Undead.ToArray());
            C.Apply.AreaTransient(Properties.stunned, 1.d6() + 4, [Kinds.demon]);
          }
        );
      });

      divine_favor = AddSpell(Schools.clerical, "divine favor", 6, null, Glyphs.divine_favor_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.IncreaseAbility(Attributes.wisdom, Dice.One);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.IncreaseAbility(Attributes.wisdom, Dice.One);
          },
          null,
          null,
          null,
          null
        );
      });

      divine_intervention = AddSpell(Schools.clerical, "divine intervention", 7, null, Glyphs.divine_intervention_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.GainLevel(Dice.One, false);
            U.Apply.HealEntity(Dice.Fixed(100), Modifier.Zero);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.GainLevel(Dice.One, false);
            P.Apply.HealEntity(Dice.Fixed(150), Modifier.Zero);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.GainLevel(1.d2(), false);
            S.Apply.HealEntity(Dice.Fixed(200), Modifier.Zero);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.GainLevel(1.d2(), false);
            E.Apply.HealEntity(Dice.Fixed(250), Modifier.Zero);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.GainLevel(1.d2() + 1, false);
            M.Apply.HealEntity(Dice.Fixed(300), Modifier.Zero);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.GainLevel(1.d2() + 1, false);
            C.Apply.HealEntity(Dice.Fixed(350), Modifier.Zero);
          }
        );
      });

      caltrop_swarm = AddSpell(Schools.conjuration, "caltrop swarm", 1, new Precept(Purpose.Toss), Glyphs.caltrop_swarm_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, 1.d4() + 5);
            U.Apply.CreateDevice(Devices.caltrops, Destruction: false);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4() + 6);
            P.Apply.CreateDevice(Devices.caltrops, Destruction: false);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 7);
            S.Apply.CreateDevice(Devices.caltrops, Destruction: false);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 8);
            E.Apply.CreateDevice(Devices.caltrops, Destruction: false);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 9);
            M.Apply.CreateDevice(Devices.caltrops, Destruction: false);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 10);
            C.Apply.CreateDevice(Devices.caltrops, Destruction: false);
          }
        );
      });

      unseen_hand = AddSpell(Schools.conjuration, "unseen hand", 1, null, Glyphs.unseen_hand_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Plain(Dice.Zero).SetTargetSelf();
            U.Apply.Gather(Range.Sq5, Items: true, Characters: false, Boulders: false);
          },
          P =>
          {
            P.SetCast().Plain(Dice.Zero).SetTargetSelf();
            P.Apply.Gather(Range.Sq10, Items: true, Characters: false, Boulders: false);
          },
          S =>
          {
            S.SetCast().Plain(Dice.Zero).SetTargetSelf();
            S.Apply.Gather(Range.Sq15, Items: true, Characters: false, Boulders: false);
          },
          E =>
          {
            E.SetCast().Plain(Dice.Zero).SetTargetSelf();
            E.Apply.Gather(Range.Sq20, Items: true, Characters: false, Boulders: false);
          },
          M =>
          {
            M.SetCast().Plain(Dice.Zero).SetTargetSelf();
            M.Apply.Gather(Range.Sq20, Items: true, Characters: false, Boulders: false);
          },
          C =>
          {
            C.SetCast().Plain(Dice.Zero).SetTargetSelf();
            C.Apply.Gather(Range.Sq25, Items: true, Characters: false, Boulders: true);
          }
        );
      });

      web_snare = AddSpell(Schools.conjuration, "web snare", 2, new Precept(Purpose.Block), Glyphs.web_snare_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.magic, 1.d4() + 5);
            U.Apply.CreateDevice(Devices.web, Destruction: false);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.magic, 1.d4() + 6);
            P.Apply.CreateDevice(Devices.web, Destruction: false);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.magic, 1.d4() + 7);
            S.Apply.CreateDevice(Devices.web, Destruction: false);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.magic, 1.d4() + 8);
            E.Apply.CreateDevice(Devices.web, Destruction: false);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.magic, 1.d4() + 9);
            M.Apply.CreateDevice(Devices.web, Destruction: false);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.magic, 1.d4() + 10);
            C.Apply.CreateDevice(Devices.web, Destruction: false);
          }
        );
      });

      entangling_vines = AddSpell(Schools.conjuration, "entangling vines", 2, new Precept(Purpose.Debuff), Glyphs.entangling_vines_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, 1.d4() + 2);
            U.Apply.GrappleEntity(Dice.One);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4() + 3);
            P.Apply.GrappleEntity(Dice.One);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 5);
            S.Apply.GrappleEntity(1.d4());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 7);
            E.Apply.GrappleEntity(1.d4());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 9);
            M.Apply.GrappleEntity(1.d4() + 1);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 11);
            C.Apply.GrappleEntity(1.d4() + 1);
          }
        );
      });

      conjure_homunculus = AddSpell(Schools.conjuration, "conjure homunculus", 2, new Precept(Purpose.SummonAlly), Glyphs.conjure_homunculus_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            U.Apply.CreateEntity(Dice.Fixed(1), Entities.homunculus);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            P.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.homunculus);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Fixed(2))
             .SetTerminates();
            S.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.homunculus);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Fixed(3))
             .SetTerminates();
            E.Apply.SummonEntity(Dice.Fixed(2), Constructed: true, Entities.homunculus);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Fixed(4))
             .SetTerminates();
            M.Apply.SummonEntity(Dice.Fixed(3), Constructed: true, Entities.homunculus);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Fixed(5))
             .SetTerminates();
            C.Apply.SummonEntity(Dice.Fixed(4), Constructed: true, Entities.homunculus);
          }
        );
      });

      conjured_pit = AddSpell(Schools.conjuration, "conjured pit", 2, new Precept(Purpose.Block), Glyphs.conjured_pit_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.tunnel, Dice.One);
            U.Apply.CreateDevice(Devices.pit, Destruction: false);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.tunnel, 1.d4() + 1);
            P.Apply.CreateDevice(Devices.pit, Destruction: false);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.tunnel, 1.d4() + 2);
            S.Apply.CreateDevice(Devices.spiked_pit, Destruction: false);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.tunnel, 1.d4() + 3);
            E.Apply.CreateDevice(Devices.spiked_pit, Destruction: false);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.tunnel, 1.d4() + 4);
            M.Apply.CreateDevice(Devices.spiked_pit, Destruction: false);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.tunnel, 1.d4() + 5);
            C.Apply.CreateDevice(Devices.spiked_pit, Destruction: false);
          }
        );
      });

      conjure_mount = AddSpell(Schools.conjuration, "conjure mount", 3, new Precept(Purpose.SummonAlly), Glyphs.conjure_mount_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            U.Apply.AnimateMount(ObjectEntity: Entities.animate_object);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            P.Apply.AnimateMount(ObjectEntity: Entities.animate_object);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            S.Apply.AnimateMount(ObjectEntity: Entities.animate_object);
            S.Apply.ApplyTransient(Properties.quickness, 1.d10() + 10);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            E.Apply.AnimateMount(ObjectEntity: Entities.animate_object);
            E.Apply.ApplyTransient(Properties.quickness, 1.d10() + 20);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            M.Apply.AnimateMount(ObjectEntity: Entities.animate_object);
            M.Apply.ApplyTransient(Properties.quickness, 1.d10() + 30);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            C.Apply.AnimateMount(ObjectEntity: Entities.animate_object);
            C.Apply.ApplyTransient(Properties.quickness, 1.d10() + 40);
          }
        );
      });

      planar_swap = AddSpell(Schools.conjuration, "planar swap", 3, new Precept(Purpose.Teleport), Glyphs.planar_swap_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.Zero);
            U.Apply.ExchangeEntity();
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, Dice.One);
            P.Apply.ExchangeEntity();
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 3);
            S.Apply.ExchangeEntity();
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 5);
            E.Apply.ExchangeEntity();
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 7);
            M.Apply.ExchangeEntity();
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 9);
            C.Apply.ExchangeEntity();
          }
        );
      });

      conjure_boulder = AddSpell(Schools.conjuration, "conjure boulder", 3, new Precept(Purpose.Block), Glyphs.conjure_boulder_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.One);
            U.Apply.CreateBlock(Dice.One, Codex.Blocks.stone_boulder);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4() + 1);
            P.Apply.CreateBlock(Dice.One, Codex.Blocks.stone_boulder);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 2);
            S.Apply.CreateBlock(1.d3(), Codex.Blocks.stone_boulder);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 3);
            E.Apply.CreateBlock(1.d3(), Codex.Blocks.stone_boulder);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 4);
            M.Apply.CreateBlock(1.d3() + 1, Codex.Blocks.stone_boulder);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 5);
            C.Apply.CreateBlock(1.d3() + 1, Codex.Blocks.stone_boulder);
          }
        );
      });

      call_of_the_pack = AddSpell(Schools.conjuration, "call of the pack", 3, new Precept(Purpose.SummonAlly), Glyphs.call_of_the_pack_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            U.Apply.CreateSpecificHorde(Dice.One, Codex.Hordes.wolf);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            P.Apply.CreateSpecificHorde(Dice.One, Codex.Hordes.wolf);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            S.Apply.CreateSpecificHorde(Dice.Fixed(2), Codex.Hordes.wolf);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            E.Apply.CreateSpecificHorde(Dice.Fixed(3), Codex.Hordes.wolf);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            M.Apply.CreateSpecificHorde(Dice.Fixed(4), Codex.Hordes.wolf);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            C.Apply.CreateSpecificHorde(Dice.Fixed(5), Codex.Hordes.wolf);
          }
        );
      });

      wall_of_thorns = AddSpell(Schools.conjuration, "wall of thorns", 4, new Precept(Purpose.Block), Glyphs.wall_of_thorns_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.tunnel, Dice.One);
            U.Apply.CreateBarrier(WallStructure.Illusionary, Barriers.tree);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.tunnel, Dice.One);
            P.Apply.CreateBarrier(WallStructure.Illusionary, Barriers.tree);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.tunnel, Dice.One);
            S.Apply.CreateBarrier(WallStructure.Solid, Barriers.tree);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.tunnel, Dice.One);
            E.Apply.CreateBarrier(WallStructure.Solid, Barriers.tree);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.tunnel, Dice.One);
            M.Apply.CreateBarrier(WallStructure.Permanent, Barriers.tree);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.tunnel, Dice.One);
            C.Apply.CreateBarrier(WallStructure.Permanent, Barriers.tree);
          }
        );
      });

      conjure_guardian = AddSpell(Schools.conjuration, "conjure guardian", 4, new Precept(Purpose.SummonAlly), Glyphs.conjure_guardian_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            U.Apply.CreateEntity(Dice.Fixed(1), Entities.gargoyle);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            P.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.gargoyle);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Fixed(2))
             .SetTerminates();
            S.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.gargoyle);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Fixed(3))
             .SetTerminates();
            E.Apply.SummonEntity(Dice.Fixed(2), Constructed: true, Entities.winged_gargoyle);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Fixed(4))
             .SetTerminates();
            M.Apply.SummonEntity(Dice.Fixed(3), Constructed: true, Entities.winged_gargoyle);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Fixed(5))
             .SetTerminates();
            C.Apply.SummonEntity(Dice.Fixed(4), Constructed: true, Entities.winged_gargoyle);
          }
        );
      });

      repelling_ward = AddSpell(Schools.conjuration, "repelling ward", 4, new Precept(Purpose.AreaOfEffect), Glyphs.repelling_ward_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            U.Apply.Repel(Range.Sq2, Items: true, Characters: true, Boulders: true);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            P.Apply.Repel(Range.Sq2, Items: true, Characters: true, Boulders: true);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            S.Apply.Repel(Range.Sq3, Items: true, Characters: true, Boulders: true);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            E.Apply.Repel(Range.Sq4, Items: true, Characters: true, Boulders: true);
            E.Apply.AreaTransient(Properties.fear, 5.d6(), Kinds.Living.ToArray());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            M.Apply.Repel(Range.Sq5, Items: true, Characters: true, Boulders: true);
            M.Apply.AreaTransient(Properties.fear, 6.d6(), Kinds.Living.ToArray());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            C.Apply.Repel(Range.Sq6, Items: true, Characters: true, Boulders: true);
            C.Apply.AreaTransient(Properties.fear, 7.d7(), Kinds.Living.ToArray());
          }
        );
      });

      elemental_servant = AddSpell(Schools.conjuration, "elemental servant", 5, new Precept(Purpose.SummonAlly), Glyphs.elemental_servant_spell, Z =>
      {
        Z.Description = null;
        void Elemental(ApplyEditor A, Dice N)
        {
          A.WhenTargetGround(Grounds.lava, T => T.SummonEntity(N, Constructed: true, Entities.fire_elemental), E1 =>
          E1.WhenTargetGround(Grounds.water, T => T.SummonEntity(N, Constructed: true, Entities.water_elemental), E2 =>
          E2.WhenTargetGround(Grounds.stone_floor, T => T.SummonEntity(N, Constructed: true, Entities.earth_elemental), E3 =>
          E3.SummonEntity(N, Constructed: true, Entities.air_elemental))));
        }
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.One);
            Elemental(U.Apply, Dice.One);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, 1.d4() + 2);
            Elemental(P.Apply, Dice.One);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, 1.d4() + 4);
            Elemental(S.Apply, Dice.Two);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, 1.d4() + 6);
            Elemental(E.Apply, Dice.Two);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, 1.d4() + 8);
            Elemental(M.Apply, Dice.Three);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, 1.d4() + 9);
            Elemental(C.Apply, Dice.Three);
          }
        );
      });

      binding_sphere = AddSpell(Schools.conjuration, "binding sphere", 5, new Precept(Purpose.Punish, Elements.force), Glyphs.binding_sphere_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, 1.d4() + 1);
            U.Apply.WhenChance(Chance.OneIn4, T => T.IncarcerateEntity(Elements.force, Codex.Blocks.crystal_boulder));
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4() + 2);
            P.Apply.WhenChance(Chance.OneIn3, T => T.IncarcerateEntity(Elements.force, Codex.Blocks.crystal_boulder));
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 3);
            S.Apply.WhenChance(Chance.OneIn2, T => T.IncarcerateEntity(Elements.force, Codex.Blocks.crystal_boulder));
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 4);
            E.Apply.WhenChance(Chance.ThreeIn4, T => T.IncarcerateEntity(Elements.force, Codex.Blocks.crystal_boulder));
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 5);
            M.Apply.WhenChance(Chance.ThreeIn4, T => T.IncarcerateEntity(Elements.force, Codex.Blocks.crystal_boulder));
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 6);
            C.Apply.WhenChance(Chance.Always, T => T.IncarcerateEntity(Elements.force, Codex.Blocks.crystal_boulder));
          }
        );
      });

      earthen_colossus = AddSpell(Schools.conjuration, "earthen colossus", 6, new Precept(Purpose.SummonAlly), Glyphs.earthen_colossus_spell, Z =>
      {
        Z.Description = null;
        void Golem(ApplyEditor A, Dice N)
        {
          A.WhenTargetGround(Grounds.stone_floor, T => T.SummonEntity(N, Constructed: true, Entities.stone_golem), E1 =>
          E1.WhenTargetGround(Grounds.wooden_floor, T => T.SummonEntity(N, Constructed: true, Entities.wood_golem), E2 =>
          E2.WhenTargetGround(Grounds.sand, T => T.SummonEntity(N, Constructed: true, Entities.glass_golem), E3 =>
          E3.SummonEntity(N, Constructed: true, Entities.clay_golem))));
        }
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            Golem(U.Apply, Dice.One);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            Golem(P.Apply, Dice.One);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            Golem(S.Apply, Dice.Two);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            Golem(E.Apply, Dice.Two);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            Golem(M.Apply, Dice.Three);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            Golem(C.Apply, Dice.Four);
          }
        );
      });

      simulacrum = AddSpell(Schools.conjuration, "simulacrum", 6, new Precept(Purpose.SummonAlly), Glyphs.simulacrum_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Plain(Dice.Zero).SetTargetSelf();
            U.Apply.CloneSourceEntity(Dice.One);
          },
          P =>
          {
            P.SetCast().Plain(Dice.Zero).SetTargetSelf();
            P.Apply.CloneSourceEntity(Dice.One);
          },
          S =>
          {
            S.SetCast().Plain(Dice.Zero).SetTargetSelf();
            S.Apply.CloneSourceEntity(Dice.Two);
          },
          E =>
          {
            E.SetCast().Plain(Dice.Zero).SetTargetSelf();
            E.Apply.CloneSourceEntity(Dice.Two);
          },
          M =>
          {
            M.SetCast().Plain(Dice.Zero).SetTargetSelf();
            M.Apply.CloneSourceEntity(Dice.Fixed(3));
          },
          C =>
          {
            C.SetCast().Plain(Dice.Zero).SetTargetSelf();
            C.Apply.CloneSourceEntity(Dice.Fixed(3));
          }
        );
      });

      gate = AddSpell(Schools.conjuration, "gate", 7, new Precept(Purpose.SummonAlly), Glyphs.gate_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.magic, 1.d4() + 1);
            U.Apply.ConnectPortal(Codex.Portals.rift);
            U.Apply.CreateSpecificHorde(Dice.One, Codex.Hordes.demon);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.magic, 1.d4() + 2);
            P.Apply.ConnectPortal(Codex.Portals.rift);
            P.Apply.CreateSpecificHorde(Dice.One, Codex.Hordes.demon);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.magic, 1.d4() + 3);
            S.Apply.ConnectPortal(Codex.Portals.rift);
            S.Apply.CreateSpecificHorde(Dice.Two, Codex.Hordes.demon);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.magic, 1.d4() + 4);
            E.Apply.ConnectPortal(Codex.Portals.rift);
            E.Apply.CreateSpecificHorde(Dice.Two, Codex.Hordes.demon);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.magic, 1.d4() + 5);
            M.Apply.ConnectPortal(Codex.Portals.rift);
            M.Apply.CreateSpecificHorde(Dice.Fixed(3), Codex.Hordes.demon);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.magic, 1.d4() + 6);
            C.Apply.ConnectPortal(Codex.Portals.rift);
            C.Apply.CreateSpecificHorde(Dice.Fixed(3), Codex.Hordes.demon);
            C.Apply.SummonEntity(Dice.One, Constructed: true, Entities.archon);
          }
        );
      });

      sentinel_ward = AddSpell(Schools.abjuration, "sentinel ward", 1, new Precept(Purpose.Buff, Properties.warning), Glyphs.sentinel_ward_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.warning, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.warning, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.warning, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.warning, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.warning, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.warning, 1.d15() + 151);
          }
        );
      });

      hazard_sense = AddSpell(Schools.abjuration, "hazard sense", 1, null, Glyphs.hazard_sense_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            U.Apply.DetectTrap(Range.Sq10);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            P.Apply.DetectTrap(Range.Sq15);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            S.Apply.DetectTrap(Range.Sq20);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            E.Apply.DetectTrap(Range.Sq25);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            M.Apply.DetectTrap(Range.Sq30);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            C.Apply.DetectTrap(Range.Sq35);
          }
        );
      });

      shimmer_shield = AddSpell(Schools.abjuration, "shimmer shield", 2, new Precept(Purpose.Buff, Properties.reflection), Glyphs.shimmer_shield_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.shield, Dice.Zero);
            U.Apply.ApplyTransient(Properties.reflection, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.shield, Dice.One);
            P.Apply.ApplyTransient(Properties.reflection, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.shield, Dice.One);
            S.Apply.ApplyTransient(Properties.reflection, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.shield, Dice.One);
            E.Apply.ApplyTransient(Properties.reflection, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.shield, Dice.One);
            M.Apply.ApplyTransient(Properties.reflection, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.shield, Dice.One);
            C.Apply.ApplyTransient(Properties.reflection, 1.d15() + 151);
          }
        );
      });

      slipping_free = AddSpell(Schools.abjuration, "slipping free", 2, new Precept(Purpose.Buff, Properties.slippery), Glyphs.slipping_free_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.UnstuckEntity();
            U.Apply.ApplyTransient(Properties.slippery, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.UnstuckEntity();
            P.Apply.ApplyTransient(Properties.slippery, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.UnstuckEntity();
            S.Apply.ApplyTransient(Properties.slippery, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.UnstuckEntity();
            E.Apply.ApplyTransient(Properties.slippery, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.UnstuckEntity();
            M.Apply.ApplyTransient(Properties.slippery, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.UnstuckEntity();
            C.Apply.ApplyTransient(Properties.slippery, 1.d15() + 151);
          }
        );
      });

      freedom = AddSpell(Schools.abjuration, "freedom", 2, new Precept(Purpose.Buff, Properties.free_action), Glyphs.freedom_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.free_action, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.free_action, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.free_action, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.free_action, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.free_action, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.free_action, 1.d15() + 151);
          }
        );
      });

      veil_of_shadows = AddSpell(Schools.abjuration, "veil of shadows", 2, new Precept(Purpose.Buff, Properties.stealth), Glyphs.veil_of_shadows_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.stealth, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.stealth, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.stealth, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.stealth, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.stealth, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.stealth, 1.d15() + 151);
          }
        );
      });

      inner_calm = AddSpell(Schools.abjuration, "inner calm", 3, new Precept(Purpose.Buff, Properties.clarity), Glyphs.inner_calm_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.Zero).SetTerminates();
            U.Apply.PacifyEntity(Elements.magical);
            U.Apply.RemoveTransient(Properties.fear, Properties.aggravation, Properties.rage);
            U.Apply.ApplyTransient(Properties.clarity, 2.d20());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.Zero).SetTerminates();
            P.Apply.PacifyEntity(Elements.magical);
            P.Apply.RemoveTransient(Properties.fear, Properties.aggravation, Properties.rage);
            P.Apply.ApplyTransient(Properties.clarity, 2.d40());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Zero).SetTerminates();
            S.Apply.PacifyEntity(Elements.magical);
            S.Apply.RemoveTransient(Properties.fear, Properties.aggravation, Properties.rage);
            S.Apply.ApplyTransient(Properties.clarity, 2.d60());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Zero).SetTerminates();
            E.Apply.PacifyEntity(Elements.magical);
            E.Apply.RemoveTransient(Properties.fear, Properties.aggravation, Properties.rage, Properties.confusion);
            E.Apply.ApplyTransient(Properties.clarity, 2.d80());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Zero).SetTerminates();
            M.Apply.PacifyEntity(Elements.magical);
            M.Apply.RemoveTransient(Properties.fear, Properties.aggravation, Properties.rage, Properties.confusion);
            M.Apply.ApplyTransient(Properties.clarity, 2.d100());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Zero).SetTerminates();
            C.Apply.PacifyEntity(Elements.magical);
            C.Apply.RemoveTransient(Properties.fear, Properties.aggravation, Properties.rage, Properties.confusion);
            C.Apply.ApplyTransient(Properties.clarity, 2.d120());
          }
        );
      });

      steadfast_ward = AddSpell(Schools.abjuration, "steadfast ward", 3, new Precept(Purpose.Buff, Properties.sustain_ability), Glyphs.steadfast_ward_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.sustain_ability, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.sustain_ability, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.sustain_ability, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.sustain_ability, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.sustain_ability, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.sustain_ability, 1.d15() + 151);
          }
        );
      });

      unbinding = AddSpell(Schools.abjuration, "unbinding", 3, null, Glyphs.unbinding_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.One);
            U.Apply.UnstuckEntity();
            U.Apply.RemoveTransient(Properties.paralysis, Properties.sleeping);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.UnstuckEntity();
            P.Apply.RemoveTransient(Properties.paralysis, Properties.sleeping, Properties.slowness);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.UnstuckEntity();
            S.Apply.RemoveTransient(Properties.paralysis, Properties.sleeping, Properties.slowness, Properties.fumbling);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.UnstuckEntity();
            E.Apply.RemoveTransient(Properties.paralysis, Properties.sleeping, Properties.slowness, Properties.fumbling, Properties.petrifying);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.UnstuckEntity();
            M.Apply.RemoveTransient(Properties.paralysis, Properties.sleeping, Properties.slowness, Properties.fumbling, Properties.petrifying);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.UnstuckEntity();
            C.Apply.RemoveTransient(Properties.paralysis, Properties.sleeping, Properties.slowness, Properties.fumbling, Properties.petrifying);
          }
        );
      });

      blurred_form = AddSpell(Schools.abjuration, "blurred form", 3, new Precept(Purpose.Buff, Properties.displacement), Glyphs.blurred_form_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.displacement, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.displacement, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.displacement, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.displacement, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.displacement, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.displacement, 1.d15() + 151);
          }
        );
      });

      disarming_ward = AddSpell(Schools.abjuration, "disarming ward", 4, new Precept(Purpose.Debuff), Glyphs.disarming_ward_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.One).SetTargetSelf(false);
            U.Apply.DisarmEntity(Codex.Attributes.dexterity);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, Dice.Fixed(2)).SetTargetSelf(false);
            P.Apply.DisarmEntity(Codex.Attributes.dexterity);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, Dice.Fixed(3)).SetTargetSelf(false);
            S.Apply.DisarmEntity(Codex.Attributes.dexterity);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, Dice.Fixed(4)).SetTargetSelf(false);
            E.Apply.DisarmEntity(Codex.Attributes.dexterity);
            E.Apply.Knockback();
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, Dice.Fixed(5)).SetTargetSelf(false);
            M.Apply.DisarmEntity(Codex.Attributes.dexterity);
            M.Apply.Knockback();
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, Dice.Fixed(6)).SetTargetSelf(false);
            C.Apply.DisarmEntity(Codex.Attributes.dexterity);
            C.Apply.Knockback();
          }
        );
      });

      repulsion = AddSpell(Schools.abjuration, "repulsion", 4, new Precept(Purpose.Block), Glyphs.repulsion_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.Zero);
            U.Apply.Repel(Range.Sq3, Items: false, Characters: true, Boulders: false);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, Dice.One);
            P.Apply.Repel(Range.Sq4, Items: false, Characters: true, Boulders: false);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, Dice.One);
            S.Apply.Repel(Range.Sq4, Items: true, Characters: true, Boulders: false);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, Dice.One);
            E.Apply.Repel(Range.Sq5, Items: true, Characters: true, Boulders: false);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, Dice.One);
            M.Apply.Repel(Range.Sq6, Items: true, Characters: true, Boulders: true);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, Dice.One);
            C.Apply.Repel(Range.Sq6, Items: true, Characters: true, Boulders: true);
          }
        );
      });

      guided_path = AddSpell(Schools.abjuration, "guided path", 4, new Precept(Purpose.Buff, Properties.teleport_control), Glyphs.guided_path_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.teleport_control, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.teleport_control, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.teleport_control, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.teleport_control, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.teleport_control, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.teleport_control, 1.d15() + 151);
          }
        );
      });

      changeless_ward = AddSpell(Schools.abjuration, "changeless ward", 4, new Precept(Purpose.Buff, Properties.unchanging), Glyphs.changeless_ward_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.unchanging, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.unchanging, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.unchanging, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.unchanging, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.unchanging, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.unchanging, 1.d15() + 151);
          }
        );
      });

      neutralize_poison = AddSpell(Schools.abjuration, "neutralize poison", 5, null, Glyphs.neutralize_poison_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.One).SetAfflictionOverride();
            U.Apply.UnafflictEntity();
            U.Apply.MinorResistance(Elements.poison);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One).SetAfflictionOverride();
            P.Apply.UnafflictEntity();
            P.Apply.MinorResistance(Elements.poison);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One).SetAfflictionOverride();
            S.Apply.UnafflictEntity();
            S.Apply.MajorResistance(Elements.poison);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One).SetAfflictionOverride();
            E.Apply.UnafflictEntity();
            E.Apply.MajorResistance(Elements.poison);
            E.Apply.HealEntity(1.d8(), Modifier.Zero);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One).SetAfflictionOverride();
            M.Apply.UnafflictEntity();
            M.Apply.MajorResistance(Elements.poison);
            M.Apply.HealEntity(2.d8(), Modifier.Zero);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One).SetAfflictionOverride();
            C.Apply.UnafflictEntity();
            C.Apply.MajorResistance(Elements.poison);
            C.Apply.HealEntity(3.d8(), Modifier.Zero);
          }
        );
      });

      elemental_warding = AddSpell(Schools.abjuration, "elemental warding", 5, new Precept(Purpose.Buff), Glyphs.elemental_warding_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.One);
            U.Apply.MinorResistance(Elements.fire);
            U.Apply.MinorResistance(Elements.cold);
            U.Apply.MinorResistance(Elements.shock);
            U.Apply.MinorResistance(Elements.acid);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.MinorResistance(Elements.fire);
            P.Apply.MinorResistance(Elements.cold);
            P.Apply.MinorResistance(Elements.shock);
            P.Apply.MinorResistance(Elements.acid);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.MinorResistance(Elements.fire);
            S.Apply.MinorResistance(Elements.cold);
            S.Apply.MinorResistance(Elements.shock);
            S.Apply.MinorResistance(Elements.acid);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.MajorResistance(Elements.fire);
            E.Apply.MajorResistance(Elements.cold);
            E.Apply.MinorResistance(Elements.shock);
            E.Apply.MinorResistance(Elements.acid);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.MajorResistance(Elements.fire);
            M.Apply.MajorResistance(Elements.cold);
            M.Apply.MajorResistance(Elements.shock);
            M.Apply.MinorResistance(Elements.acid);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.MajorResistance(Elements.fire);
            C.Apply.MajorResistance(Elements.cold);
            C.Apply.MajorResistance(Elements.shock);
            C.Apply.MinorResistance(Elements.acid);
          }
        );
      });

      dispel_magic = AddSpell(Schools.abjuration, "dispel magic", 6, new Precept(Purpose.Debuff), Glyphs.dispel_magic_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.One)
             .SetTargetSelf(false);
            U.Apply.RemoveTransient(Properties.invisibility, Properties.deflection, Properties.reflection);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, Dice.Fixed(2))
             .SetTargetSelf(false);
            P.Apply.RemoveTransient(Properties.invisibility, Properties.deflection, Properties.reflection, Properties.phasing, Properties.blinking);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, Dice.Fixed(3))
             .SetTargetSelf(false);
            S.Apply.RemoveTransient(Properties.invisibility, Properties.deflection, Properties.reflection, Properties.phasing, Properties.blinking, Properties.telekinesis, Properties.levitation);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, Dice.Fixed(4))
             .SetTargetSelf(false)
             .SetPenetrates();
            E.Apply.RemoveTransient(Properties.invisibility, Properties.deflection, Properties.reflection, Properties.phasing, Properties.blinking, Properties.telekinesis, Properties.levitation, Properties.displacement, Properties.stealth);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, Dice.Fixed(5))
             .SetTargetSelf(false)
             .SetPenetrates();
            M.Apply.RemoveTransient(Properties.invisibility, Properties.deflection, Properties.reflection, Properties.phasing, Properties.blinking, Properties.telekinesis, Properties.levitation, Properties.displacement, Properties.stealth, Properties.quickness, Properties.rage);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, Dice.Fixed(6))
             .SetTargetSelf(false)
             .SetPenetrates();
            C.Apply.RemoveTransient(Properties.invisibility, Properties.deflection, Properties.reflection, Properties.phasing, Properties.blinking, Properties.telekinesis, Properties.levitation, Properties.displacement, Properties.stealth, Properties.quickness, Properties.rage, Properties.polymorph);
            C.Apply.Cancellation(Elements.magical);
          }
        );
      });

      undying_ward = AddSpell(Schools.abjuration, "undying ward", 7, new Precept(Purpose.Buff, Properties.lifesaving), Glyphs.undying_ward_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.One);
            U.Apply.ApplyTransient(Properties.lifesaving, 1.d10() + 50);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.lifesaving, 1.d10() + 100);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.lifesaving, 1.d10() + 200);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.lifesaving, 1.d10() + 300);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.lifesaving, 1.d10() + 400);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.lifesaving, 1.d10() + 500);
          }
        );
      });

      chill_grasp = AddSpell(Schools.necromancy, "chill grasp", 1, new Precept(Purpose.Blast), Glyphs.chill_grasp_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            U.Apply.HarmEntity(Elements.necrotic, 1.d6() + 1);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            P.Apply.HarmEntity(Elements.necrotic, 2.d6() + 2);
            P.Apply.WhenChance(Chance.OneIn4, T => T.ApplyTransient(Properties.fumbling, 1.d6() + 4));
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, 1.d4() + 1)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            S.Apply.HarmEntity(Elements.necrotic, 3.d6() + 3);
            S.Apply.WhenChance(Chance.OneIn3, T => T.ApplyTransient(Properties.fumbling, 1.d6() + 6));
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, 1.d6() + 2)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            E.Apply.HarmEntity(Elements.necrotic, 4.d6() + 4);
            E.Apply.WhenChance(Chance.OneIn3, T => T.ApplyTransient(Properties.fumbling, 1.d6() + 8));
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, 1.d8() + 3)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            M.Apply.HarmEntity(Elements.necrotic, 5.d6() + 5);
            M.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.fumbling, 1.d6() + 10));
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, 1.d8() + 4)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            C.Apply.HarmEntity(Elements.necrotic, 6.d6() + 6);
            C.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.fumbling, 1.d6() + 12));
          }
        );
      });

      false_life = AddSpell(Schools.necromancy, "false life", 1, new Precept(Purpose.Buff, Properties.vitality), Glyphs.false_life_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.vitality, 1.d20() + 20);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.Zero);
            P.Apply.ApplyTransient(Properties.vitality, 1.d20() + 40);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.Zero);
            S.Apply.ApplyTransient(Properties.vitality, 1.d20() + 60);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.Zero);
            E.Apply.ApplyTransient(Properties.vitality, 1.d20() + 80);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.Zero);
            M.Apply.ApplyTransient(Properties.vitality, 1.d20() + 100);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.Zero);
            C.Apply.ApplyTransient(Properties.vitality, 1.d20() + 120);
          }
        );
      });

      deathwatch = AddSpell(Schools.necromancy, "deathwatch", 1, null, Glyphs.deathwatch_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.Zero)
             .SetTerminates();
            U.Apply.DetectEntity(Range.Sq10, Kinds.Undead.ToArray());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.Zero)
             .SetTerminates();
            P.Apply.DetectEntity(Range.Sq15, Kinds.Undead.ToArray());
            P.Apply.ApplyTransient(Properties.warning, 1.d10() + 20);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Zero)
             .SetTerminates();
            S.Apply.DetectEntity(Range.Sq20, Kinds.Undead.ToArray());
            S.Apply.ApplyTransient(Properties.warning, 1.d10() + 40);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Zero)
             .SetTerminates();
            E.Apply.DetectEntity(Range.Sq25, Kinds.Undead.ToArray());
            E.Apply.ApplyTransient(Properties.warning, 1.d10() + 60);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Zero)
             .SetTerminates();
            M.Apply.DetectEntity(Range.Sq30, Kinds.Undead.ToArray());
            M.Apply.ApplyTransient(Properties.warning, 1.d10() + 80);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Zero)
             .SetTerminates();
            C.Apply.DetectEntity(Range.Sq35, Kinds.Undead.ToArray());
            C.Apply.ApplyTransient(Properties.warning, 1.d10() + 100);
          }
        );
      });

      spectral_shroud = AddSpell(Schools.necromancy, "spectral shroud", 2, new Precept(Purpose.Buff, Properties.displacement), Glyphs.spectral_shroud_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.displacement, 1.d10() + 10);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.Zero);
            P.Apply.ApplyTransient(Properties.displacement, 1.d10() + 20);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.Zero);
            S.Apply.ApplyTransient(Properties.displacement, 1.d10() + 30);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.Zero);
            E.Apply.ApplyTransient(Properties.displacement, 1.d10() + 40);
            E.Apply.ApplyTransient(Properties.dark_vision, 1.d10() + 40);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.Zero);
            M.Apply.ApplyTransient(Properties.displacement, 1.d10() + 50);
            M.Apply.ApplyTransient(Properties.dark_vision, 1.d10() + 50);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.Zero);
            C.Apply.ApplyTransient(Properties.displacement, 1.d10() + 60);
            C.Apply.ApplyTransient(Properties.dark_vision, 1.d10() + 60);
          }
        );
      });

      ray_of_enfeeblement = AddSpell(Schools.necromancy, "ray of enfeeblement", 2, new Precept(Purpose.Debuff), Glyphs.ray_of_enfeeblement_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            U.Apply.DecreaseAbility(Attributes.strength, Dice.One);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            P.Apply.DecreaseAbility(Attributes.strength, 1.d2());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, 1.d4() + 1)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            S.Apply.DecreaseAbility(Attributes.strength, 1.d3());
            S.Apply.WhenChance(Chance.OneIn3, T => T.ApplyTransient(Properties.fumbling, 1.d6() + 4));
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, 1.d6() + 2)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            E.Apply.DecreaseAbility(Attributes.strength, 1.d4());
            E.Apply.WhenChance(Chance.OneIn3, T => T.ApplyTransient(Properties.fumbling, 1.d6() + 6));
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, 1.d8() + 3)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            M.Apply.DecreaseAbility(Attributes.strength, 1.d4());
            M.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.fumbling, 1.d6() + 8));
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, 1.d8() + 4)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            C.Apply.DecreaseAbility(Attributes.strength, 1.d4());
            C.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.fumbling, 1.d6() + 10));
          }
        );
      });

      grave_rot = AddSpell(Schools.necromancy, "grave rot", 2, new Precept(Purpose.Afflict), Glyphs.grave_rot_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.venom, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            U.Apply.WhenChance(Chance.OneIn10, T => T.AfflictEntity(Codex.Afflictions.worms), F => F.ApplyTransient(Properties.sickness, 1.d6() + 4));
          },
          P =>
          {
            P.SetCast().Strike(Strikes.venom, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            P.Apply.WhenChance(Chance.OneIn8, T => T.AfflictEntity(Codex.Afflictions.worms), F => F.ApplyTransient(Properties.sickness, 1.d6() + 8));
          },
          S =>
          {
            S.SetCast().Strike(Strikes.venom, 1.d4() + 1)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            S.Apply.WhenChance(Chance.OneIn6, T => T.AfflictEntity(Codex.Afflictions.worms), F => F.ApplyTransient(Properties.sickness, 1.d6() + 12));
          },
          E =>
          {
            E.SetCast().Strike(Strikes.venom, 1.d6() + 2)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            E.Apply.WhenChance(Chance.OneIn5, T => T.AfflictEntity(Codex.Afflictions.worms), F => F.ApplyTransient(Properties.sickness, 1.d6() + 16));
          },
          M =>
          {
            M.SetCast().Strike(Strikes.venom, 1.d8() + 3)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            M.Apply.WhenChance(Chance.OneIn3, T => T.AfflictEntity(Codex.Afflictions.worms), F => F.ApplyTransient(Properties.sickness, 1.d6() + 20));
          },
          C =>
          {
            C.SetCast().Strike(Strikes.venom, 1.d8() + 4)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            C.Apply.WhenChance(Chance.OneIn2, T => T.AfflictEntity(Codex.Afflictions.worms), F => F.ApplyTransient(Properties.sickness, 1.d6() + 24));
          }
        );
      });

      spirit_leech = AddSpell(Schools.necromancy, "spirit leech", 3, new Precept(Purpose.Blast), Glyphs.spirit_leech_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            U.Apply.DrainMana(Elements.drain, 1.d6() + 1);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            P.Apply.DrainMana(Elements.drain, 2.d6() + 2);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, 1.d4() + 1)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            S.Apply.DrainMana(Elements.drain, 3.d6() + 3);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, 1.d6() + 2)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            E.Apply.DrainMana(Elements.drain, 4.d6() + 4);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, 1.d8() + 3)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            M.Apply.DrainMana(Elements.drain, 5.d6() + 5);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, 1.d8() + 4)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            C.Apply.DrainMana(Elements.drain, 6.d6() + 6);
          }
        );
      });

      contagion = AddSpell(Schools.necromancy, "contagion", 3, new Precept(Purpose.Blast, Elements.poison), Glyphs.contagion_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.venom, 2.d3() + 1)
             .SetPenetrates(false);
            U.Apply.HarmEntity(Elements.poison, 1.d6() + 1);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.venom, 2.d3() + 2)
             .SetPenetrates(false);
            P.Apply.HarmEntity(Elements.poison, 2.d6() + 2);
            P.Apply.WhenChance(Chance.OneIn10, T => T.UnlessTargetResistant(Elements.poison, R =>
            {
              R.AfflictEntity(Codex.Afflictions.poisoning);
            }));
          },
          S =>
          {
            S.SetCast().Strike(Strikes.venom, 2.d3() + 3)
             .SetPenetrates();
            S.Apply.HarmEntity(Elements.poison, 3.d6() + 3);
            S.Apply.WhenChance(Chance.OneIn8, T => T.UnlessTargetResistant(Elements.poison, R =>
            {
              R.AfflictEntity(Codex.Afflictions.poisoning);
            }));
          },
          E =>
          {
            E.SetCast().Strike(Strikes.venom, 2.d3() + 4)
             .SetPenetrates();
            E.Apply.HarmEntity(Elements.poison, 4.d6() + 4);
            E.Apply.WhenChance(Chance.OneIn6, T => T.UnlessTargetResistant(Elements.poison, R =>
            {
              R.AfflictEntity(Codex.Afflictions.poisoning);
            }));
          },
          M =>
          {
            M.SetCast().Strike(Strikes.venom, 2.d3() + 6)
             .SetPenetrates();
            M.Apply.HarmEntity(Elements.poison, 5.d6() + 5);
            M.Apply.WhenChance(Chance.OneIn3, T => T.UnlessTargetResistant(Elements.poison, R =>
            {
              R.AfflictEntity(Codex.Afflictions.poisoning);
            }));
          },
          C =>
          {
            C.SetCast().Strike(Strikes.venom, 2.d3() + 8)
             .SetPenetrates();
            C.Apply.HarmEntity(Elements.poison, 6.d6() + 6);
            C.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.poison, R =>
            {
              R.AfflictEntity(Codex.Afflictions.poisoning);
            }));
          }
        );
      });

      banshee_wail = AddSpell(Schools.necromancy, "banshee wail", 3, new Precept(Purpose.Blast, Elements.necrotic), Glyphs.banshee_wail_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.wail, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            U.Apply.HarmEntity(Elements.necrotic, 1.d6());
            U.Apply.WhenChance(Chance.OneIn3, T => T.ApplyTransient(Properties.stunned, 1.d4()));
          },
          P =>
          {
            P.SetCast().Strike(Strikes.wail, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            P.Apply.HarmEntity(Elements.necrotic, 2.d6());
            P.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.stunned, 1.d6()));
          },
          S =>
          {
            S.SetCast().Strike(Strikes.wail, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            S.Apply.HarmEntity(Elements.necrotic, 3.d6());
            S.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.stunned, 2.d6()));
            S.Apply.WhenChance(Chance.OneIn3, T => T.ApplyTransient(Properties.fear, 2.d6()));
          },
          E =>
          {
            E.SetCast().Strike(Strikes.wail, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            E.Apply.HarmEntity(Elements.necrotic, 4.d6());
            E.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.stunned, 3.d6()));
            E.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.fear, 3.d6()));
          },
          M =>
          {
            M.SetCast().Strike(Strikes.wail, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            M.Apply.HarmEntity(Elements.necrotic, 5.d6());
            M.Apply.WhenChance(Chance.ThreeIn4, T => T.ApplyTransient(Properties.stunned, 4.d6()));
            M.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.fear, 4.d6()));
          },
          C =>
          {
            C.SetCast().Strike(Strikes.wail, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            C.Apply.HarmEntity(Elements.necrotic, 6.d6());
            C.Apply.WhenChance(Chance.ThreeIn4, T => T.ApplyTransient(Properties.stunned, 5.d6()));
            C.Apply.WhenChance(Chance.ThreeIn4, T => T.ApplyTransient(Properties.fear, 5.d6()));
          }
        );
      });

      whispers_of_madness = AddSpell(Schools.necromancy, "whispers of madness", 4, new Precept(Purpose.AreaOfEffect), Glyphs.whispers_of_madness_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.gas, Dice.Zero)
             .SetTerminates();
            U.Apply.AreaTransient(Properties.conflict, 2.d6(), Kinds.Living.ToArray());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.gas, Dice.Zero)
             .SetTerminates();
            P.Apply.AreaTransient(Properties.conflict, 3.d6(), Kinds.Living.ToArray());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.gas, Dice.Zero)
             .SetTerminates();
            S.Apply.AreaTransient(Properties.conflict, 4.d6(), Kinds.Living.ToArray());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.gas, Dice.Zero)
             .SetTerminates();
            E.Apply.AreaTransient(Properties.conflict, 5.d6(), Kinds.Living.ToArray());
            E.Apply.AreaTransient(Properties.confusion, 3.d6(), Kinds.Living.ToArray());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.gas, Dice.Zero)
             .SetTerminates();
            M.Apply.AreaTransient(Properties.conflict, 6.d6(), Kinds.Living.ToArray());
            M.Apply.AreaTransient(Properties.confusion, 4.d6(), Kinds.Living.ToArray());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.gas, Dice.Zero)
             .SetTerminates();
            C.Apply.AreaTransient(Properties.conflict, 7.d6(), Kinds.Living.ToArray());
            C.Apply.AreaTransient(Properties.confusion, 5.d6(), Kinds.Living.ToArray());
          }
        );
      });

      wither = AddSpell(Schools.necromancy, "wither", 4, new Precept(Purpose.Debuff), Glyphs.wither_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            U.Apply.DecreaseAllAbilities(Dice.One);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            P.Apply.DecreaseAllAbilities(Dice.One);
            P.Apply.HarmEntity(Elements.necrotic, 1.d6());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            S.Apply.DecreaseAllAbilities(Dice.One);
            S.Apply.HarmEntity(Elements.necrotic, 2.d6());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            E.Apply.DecreaseAllAbilities(1.d2());
            E.Apply.HarmEntity(Elements.necrotic, 3.d6());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            M.Apply.DecreaseAllAbilities(1.d2());
            M.Apply.HarmEntity(Elements.necrotic, 4.d6());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            C.Apply.DecreaseAllAbilities(1.d3());
            C.Apply.HarmEntity(Elements.necrotic, 5.d6());
          }
        );
      });

      corpse_explosion = AddSpell(Schools.necromancy, "corpse explosion", 4, new Precept(Purpose.Blast, Elements.necrotic), Glyphs.corpse_explosion_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Explosion(Explosions.dark, 1.d6());
            U.Apply.HarmEntity(Elements.necrotic, 5.d6());
          },
          P =>
          {
            P.SetCast().Explosion(Explosions.dark, 1.d6() + 1);
            P.Apply.HarmEntity(Elements.necrotic, 7.d6());
            P.Apply.Knockback();
          },
          S =>
          {
            S.SetCast().Explosion(Explosions.dark, 1.d6() + 2);
            S.Apply.HarmEntity(Elements.necrotic, 9.d6());
            S.Apply.Knockback();
          },
          E =>
          {
            E.SetCast().Explosion(Explosions.dark, 1.d6() + 3);
            E.Apply.HarmEntity(Elements.necrotic, 10.d6());
            E.Apply.Knockback();
          },
          M =>
          {
            M.SetCast().Explosion(Explosions.dark, 1.d6() + 5);
            M.Apply.HarmEntity(Elements.necrotic, 12.d6());
            M.Apply.Knockback();
          },
          C =>
          {
            C.SetCast().Explosion(Explosions.dark, 1.d6() + 6);
            C.Apply.HarmEntity(Elements.necrotic, 13.d6());
            C.Apply.Knockback();
          }
        );
      });

      legion_of_bone = AddSpell(Schools.necromancy, "legion of bone", 5, new Precept(Purpose.SummonAlly), Glyphs.legion_of_bone_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero)
             .SetTerminates();
            U.Apply.SummonEntity(Dice.One, Entities.skeleton);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.Zero)
             .SetTerminates();
            P.Apply.SummonEntity(1.d2(), Entities.skeleton);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.Zero)
             .SetTerminates();
            S.Apply.SummonEntity(1.d3(), Entities.skeleton);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.Zero)
             .SetTerminates();
            E.Apply.SummonEntity(1.d3() + 1, Entities.skeleton);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.Zero)
             .SetTerminates();
            M.Apply.SummonEntity(1.d4() + 1, Entities.skeleton);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.Zero)
             .SetTerminates();
            C.Apply.SummonEntity(1.d4() + 2, Entities.skeleton);
          }
        );
      });

      reap = AddSpell(Schools.necromancy, "reap", 5, new Precept(Purpose.Blast, Elements.necrotic), Glyphs.reap_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.sever, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            U.Apply.HarmEntity(Elements.necrotic, 4.d6());
            U.Apply.WhenChance(Chance.OneIn20, T => T.DecapitateEntity(Codex.Anatomies.head, Strikes.sever));
          },
          P =>
          {
            P.SetCast().Strike(Strikes.sever, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            P.Apply.HarmEntity(Elements.necrotic, 5.d6());
            P.Apply.WhenChance(Chance.OneIn15, T => T.DecapitateEntity(Codex.Anatomies.head, Strikes.sever));
          },
          S =>
          {
            S.SetCast().Strike(Strikes.sever, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            S.Apply.HarmEntity(Elements.necrotic, 6.d6());
            S.Apply.WhenChance(Chance.OneIn12, T => T.DecapitateEntity(Codex.Anatomies.head, Strikes.sever));
          },
          E =>
          {
            E.SetCast().Strike(Strikes.sever, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            E.Apply.HarmEntity(Elements.necrotic, 7.d6());
            E.Apply.WhenChance(Chance.OneIn10, T => T.DecapitateEntity(Codex.Anatomies.head, Strikes.sever));
          },
          M =>
          {
            M.SetCast().Strike(Strikes.sever, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            M.Apply.HarmEntity(Elements.necrotic, 8.d6());
            M.Apply.WhenChance(Chance.OneIn8, T => T.DecapitateEntity(Codex.Anatomies.head, Strikes.sever));
          },
          C =>
          {
            C.SetCast().Strike(Strikes.sever, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            C.Apply.HarmEntity(Elements.necrotic, 9.d6());
            C.Apply.WhenChance(Chance.OneIn6, T => T.DecapitateEntity(Codex.Anatomies.head, Strikes.sever));
          }
        );
      });

      vampiric_feast = AddSpell(Schools.necromancy, "vampiric feast", 5, new Precept(Purpose.Blast, Elements.drain), Glyphs.vampiric_feast_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Explosion(Explosions.dark, 1.d4());
            U.Apply.DrainLife(Elements.drain, 2.d6());
          },
          P =>
          {
            P.SetCast().Explosion(Explosions.dark, 1.d4() + 1);
            P.Apply.DrainLife(Elements.drain, 3.d6());
          },
          S =>
          {
            S.SetCast().Explosion(Explosions.dark, 1.d4() + 2);
            S.Apply.DrainLife(Elements.drain, 4.d6());
          },
          E =>
          {
            E.SetCast().Explosion(Explosions.dark, 1.d4() + 3);
            E.Apply.DrainLife(Elements.drain, 5.d6());
          },
          M =>
          {
            M.SetCast().Explosion(Explosions.dark, 1.d4() + 4);
            M.Apply.DrainLife(Elements.drain, 6.d6());
          },
          C =>
          {
            C.SetCast().Explosion(Explosions.dark, 1.d4() + 5);
            C.Apply.DrainLife(Elements.drain, 7.d6());
          }
        );
      });

      deaths_bargain = AddSpell(Schools.necromancy, "death's bargain", 6, new Precept(Purpose.Buff), Glyphs.deaths_bargain_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.lifesaving, 1.d100() + 100);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.Zero);
            P.Apply.ApplyTransient(Properties.lifesaving, 1.d100() + 200);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.Zero);
            S.Apply.ApplyTransient(Properties.lifesaving, 1.d100() + 300);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.Zero);
            E.Apply.ApplyTransient(Properties.lifesaving, 1.d100() + 400);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.Zero);
            M.Apply.ApplyTransient(Properties.lifesaving, 1.d100() + 500);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.Zero);
            C.Apply.ApplyTransient(Properties.lifesaving, 1.d100() + 600);
          }
        );
      });

      black_plague = AddSpell(Schools.necromancy, "black plague", 6, new Precept(Purpose.Blast, Elements.poison), Glyphs.black_plague_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Explosion(Explosions.dark, 1.d6() + 2);
            U.Apply.HarmEntity(Elements.poison, 6.d6());
            U.Apply.UnlessTargetResistant(Elements.poison, R =>
            {
              R.ApplyTransient(Properties.sickness, 2.d10());
            });
          },
          P =>
          {
            P.SetCast().Explosion(Explosions.dark, 1.d6() + 3);
            P.Apply.HarmEntity(Elements.poison, 7.d6());
            P.Apply.UnlessTargetResistant(Elements.poison, R =>
            {
              R.ApplyTransient(Properties.sickness, 3.d10());
            });
          },
          S =>
          {
            S.SetCast().Explosion(Explosions.dark, 1.d6() + 4);
            S.Apply.HarmEntity(Elements.poison, 8.d6());
            S.Apply.UnlessTargetResistant(Elements.poison, R =>
            {
              R.ApplyTransient(Properties.sickness, 4.d10());
              R.WhenChance(Chance.OneIn10, T => T.AfflictEntity(Codex.Afflictions.poisoning));
            });
          },
          E =>
          {
            E.SetCast().Explosion(Explosions.dark, 1.d6() + 5);
            E.Apply.HarmEntity(Elements.poison, 9.d6());
            E.Apply.UnlessTargetResistant(Elements.poison, R =>
            {
              R.ApplyTransient(Properties.sickness, 5.d10());
              R.WhenChance(Chance.OneIn8, T => T.AfflictEntity(Codex.Afflictions.poisoning));
            });
          },
          M =>
          {
            M.SetCast().Explosion(Explosions.dark, 1.d6() + 6);
            M.Apply.HarmEntity(Elements.poison, 10.d6());
            M.Apply.UnlessTargetResistant(Elements.poison, R =>
            {
              R.ApplyTransient(Properties.sickness, 6.d10());
              R.WhenChance(Chance.OneIn6, T => T.AfflictEntity(Codex.Afflictions.poisoning));
            });
          },
          C =>
          {
            C.SetCast().Explosion(Explosions.dark, 1.d6() + 7);
            C.Apply.HarmEntity(Elements.poison, 11.d6());
            C.Apply.UnlessTargetResistant(Elements.poison, R =>
            {
              R.ApplyTransient(Properties.sickness, 7.d10());
              R.WhenChance(Chance.OneIn4, T => T.AfflictEntity(Codex.Afflictions.poisoning));
            });
          }
        );
      });

      spark_bolt = AddSpell(Schools.evocation, "spark bolt", 1, new Precept(Purpose.Blast, Elements.shock), Glyphs.spark_bolt_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.energy, 1.d4() + 1).SetTargetSelf(false);
            U.Apply.HarmEntity(Elements.shock, 2.d4());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.energy, 1.d4() + 2).SetTargetSelf(false);
            P.Apply.HarmEntity(Elements.shock, 3.d4());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.energy, 1.d4() + 3).SetTargetSelf(false);
            S.Apply.HarmEntity(Elements.shock, 4.d4());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.energy, 1.d4() + 4).SetTargetSelf(false);
            E.Apply.HarmEntity(Elements.shock, 5.d4());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.energy, 1.d4() + 5).SetTargetSelf(false);
            M.Apply.HarmEntity(Elements.shock, 6.d4());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.energy, 1.d4() + 6).SetTargetSelf(false);
            C.Apply.HarmEntity(Elements.shock, 7.d4());
          }
        );
      });

      ember_lance = AddSpell(Schools.evocation, "ember lance", 3, new Precept(Purpose.Blast, Elements.fire), Glyphs.ember_lance_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.fire, 1.d4() + 3).SetPenetrates();
            U.Apply.HarmEntity(Elements.fire, 5.d6());
          },
          P =>
          {
            P.SetCast().Beam(Beams.fire, 1.d4() + 4).SetPenetrates();
            P.Apply.HarmEntity(Elements.fire, 6.d6());
          },
          S =>
          {
            S.SetCast().Beam(Beams.fire, 1.d4() + 5).SetPenetrates();
            S.Apply.HarmEntity(Elements.fire, 8.d6());
          },
          E =>
          {
            E.SetCast().Beam(Beams.fire, 1.d4() + 6).SetPenetrates();
            E.Apply.HarmEntity(Elements.fire, 10.d6());
          },
          M =>
          {
            M.SetCast().Beam(Beams.fire, 1.d4() + 7).SetPenetrates();
            M.Apply.HarmEntity(Elements.fire, 12.d6());
          },
          C =>
          {
            C.SetCast().Beam(Beams.fire, 1.d4() + 8).SetPenetrates();
            C.Apply.HarmEntity(Elements.fire, 14.d6());
          }
        );
      });

      frost_breath = AddSpell(Schools.evocation, "frost breath", 2, new Precept(Purpose.Blast, Elements.cold), Glyphs.frost_breath_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.cold, 1.d3() + 1);
            U.Apply.HarmEntity(Elements.cold, 3.d4() + 2);
            U.Apply.WhenChance(Chance.OneIn3, T => T.UnlessTargetResistant(Elements.cold, R => R.ApplyTransient(Properties.slippery, 1.d6() + 2)));
          },
          P =>
          {
            P.SetCast().Beam(Beams.cold, 1.d3() + 2);
            P.Apply.HarmEntity(Elements.cold, 4.d4() + 2);
            P.Apply.WhenChance(Chance.OneIn3, T => T.UnlessTargetResistant(Elements.cold, R => R.ApplyTransient(Properties.slippery, 1.d6() + 3)));
          },
          S =>
          {
            S.SetCast().Beam(Beams.cold, 1.d3() + 3);
            S.Apply.HarmEntity(Elements.cold, 5.d4() + 2);
            S.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.cold, R => R.ApplyTransient(Properties.slippery, 1.d6() + 3)));
          },
          E =>
          {
            E.SetCast().Beam(Beams.cold, 1.d3() + 4);
            E.Apply.HarmEntity(Elements.cold, 6.d4() + 2);
            E.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.cold, R => R.ApplyTransient(Properties.slippery, 1.d6() + 4)));
          },
          M =>
          {
            M.SetCast().Beam(Beams.cold, 1.d3() + 5);
            M.Apply.HarmEntity(Elements.cold, 7.d4() + 2);
            M.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.cold, R => R.ApplyTransient(Properties.slippery, 1.d6() + 4)));
          },
          C =>
          {
            C.SetCast().Beam(Beams.cold, 1.d3() + 6);
            C.Apply.HarmEntity(Elements.cold, 8.d4() + 2);
            C.Apply.WhenChance(Chance.ThreeIn4, T => T.UnlessTargetResistant(Elements.cold, R => R.ApplyTransient(Properties.slippery, 1.d6() + 5)));
          }
        );
      });

      arcing_bolt = AddSpell(Schools.evocation, "arcing bolt", 3, new Precept(Purpose.Blast, Elements.shock), Glyphs.arcing_bolt_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.lightning, 1.d4() + 2).SetBounces();
            U.Apply.HarmEntity(Elements.shock, 5.d6());
          },
          P =>
          {
            P.SetCast().Beam(Beams.lightning, 1.d4() + 3).SetBounces();
            P.Apply.HarmEntity(Elements.shock, 6.d6());
          },
          S =>
          {
            S.SetCast().Beam(Beams.lightning, 1.d4() + 4).SetBounces();
            S.Apply.HarmEntity(Elements.shock, 8.d6());
          },
          E =>
          {
            E.SetCast().Beam(Beams.lightning, 1.d4() + 5).SetBounces();
            E.Apply.HarmEntity(Elements.shock, 10.d6());
          },
          M =>
          {
            M.SetCast().Beam(Beams.lightning, 1.d4() + 6).SetBounces();
            M.Apply.HarmEntity(Elements.shock, 12.d6());
          },
          C =>
          {
            C.SetCast().Beam(Beams.lightning, 1.d4() + 7).SetBounces();
            C.Apply.HarmEntity(Elements.shock, 14.d6());
          }
        );
      });

      timed_combustion = AddSpell(Schools.evocation, "timed combustion", 5, new Precept(Purpose.Blast, Elements.fire), Glyphs.timed_combustion_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Explosion(Explosions.fiery, 1.d6() + 4);
            U.Apply.HarmEntity(Elements.fire, 10.d6());
          },
          P =>
          {
            P.SetCast().Explosion(Explosions.fiery, 1.d6() + 5);
            P.Apply.HarmEntity(Elements.fire, 12.d6());
          },
          S =>
          {
            S.SetCast().Explosion(Explosions.fiery, 1.d6() + 6);
            S.Apply.HarmEntity(Elements.fire, 15.d6());
          },
          E =>
          {
            E.SetCast().Explosion(Explosions.fiery, 1.d6() + 7);
            E.Apply.HarmEntity(Elements.fire, 18.d6());
          },
          M =>
          {
            M.SetCast().Explosion(Explosions.fiery, 1.d6() + 8);
            M.Apply.HarmEntity(Elements.fire, 22.d6());
          },
          C =>
          {
            C.SetCast().Explosion(Explosions.fiery, 1.d6() + 9);
            C.Apply.HarmEntity(Elements.fire, 26.d6());
          }
        );
      });

      scalding_cloud = AddSpell(Schools.evocation, "scalding cloud", 3, new Precept(Purpose.Blast, Elements.fire), Glyphs.scalding_cloud_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.gas, 1.d4() + 2);
            U.Apply.HarmEntity(Elements.fire, 4.d6());
            U.Apply.CreateVolatile(Volatiles.steam, 1.d20() + 20);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.gas, 1.d4() + 3);
            P.Apply.HarmEntity(Elements.fire, 5.d6());
            P.Apply.CreateVolatile(Volatiles.steam, 1.d20() + 25);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.gas, 1.d4() + 4);
            S.Apply.HarmEntity(Elements.fire, 6.d6());
            S.Apply.CreateVolatile(Volatiles.steam, 1.d20() + 30);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.gas, 1.d4() + 5);
            E.Apply.HarmEntity(Elements.fire, 8.d6());
            E.Apply.CreateVolatile(Volatiles.steam, 1.d20() + 35);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.gas, 1.d4() + 6);
            M.Apply.HarmEntity(Elements.fire, 9.d6());
            M.Apply.CreateVolatile(Volatiles.steam, 1.d20() + 40);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.gas, 1.d4() + 7);
            C.Apply.HarmEntity(Elements.fire, 11.d6());
            C.Apply.CreateVolatile(Volatiles.steam, 1.d20() + 45);
          }
        );
      });

      thunderclap = AddSpell(Schools.evocation, "thunderclap", 2, new Precept(Purpose.Blast, Elements.force), Glyphs.thunderclap_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Explosion(Explosions.electric, 1.d4() + 2);
            U.Apply.HarmEntity(Elements.force, 3.d6() + 2);
            U.Apply.Knockback();
          },
          P =>
          {
            P.SetCast().Explosion(Explosions.electric, 1.d4() + 3);
            P.Apply.HarmEntity(Elements.force, 4.d6() + 2);
            P.Apply.Knockback();
          },
          S =>
          {
            S.SetCast().Explosion(Explosions.electric, 1.d4() + 4);
            S.Apply.HarmEntity(Elements.force, 5.d6() + 2);
            S.Apply.Knockback();
          },
          E =>
          {
            E.SetCast().Explosion(Explosions.electric, 1.d4() + 5);
            E.Apply.HarmEntity(Elements.force, 6.d6() + 2);
            E.Apply.Knockback();
          },
          M =>
          {
            M.SetCast().Explosion(Explosions.electric, 1.d4() + 6);
            M.Apply.HarmEntity(Elements.force, 7.d6() + 2);
            M.Apply.Knockback();
          },
          C =>
          {
            C.SetCast().Explosion(Explosions.electric, 1.d4() + 7);
            C.Apply.HarmEntity(Elements.force, 8.d6() + 2);
            C.Apply.Knockback();
          }
        );
      });

      corroding_ray = AddSpell(Schools.evocation, "corroding ray", 4, new Precept(Purpose.Blast, Elements.disintegrate), Glyphs.corroding_ray_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.disintegration, 1.d4() + 3);
            U.Apply.HarmEntity(Elements.disintegrate, 6.d6());
            U.Apply.WhenChance(Chance.OneIn4, T => T.DegradeItem());
          },
          P =>
          {
            P.SetCast().Beam(Beams.disintegration, 1.d4() + 4);
            P.Apply.HarmEntity(Elements.disintegrate, 7.d6());
            P.Apply.WhenChance(Chance.OneIn4, T => T.DegradeItem());
          },
          S =>
          {
            S.SetCast().Beam(Beams.disintegration, 1.d4() + 5);
            S.Apply.HarmEntity(Elements.disintegrate, 9.d6());
            S.Apply.WhenChance(Chance.OneIn3, T => T.DegradeItem());
          },
          E =>
          {
            E.SetCast().Beam(Beams.disintegration, 1.d4() + 6);
            E.Apply.HarmEntity(Elements.disintegrate, 11.d6());
            E.Apply.WhenChance(Chance.OneIn3, T => T.DegradeItem());
          },
          M =>
          {
            M.SetCast().Beam(Beams.disintegration, 1.d4() + 7);
            M.Apply.HarmEntity(Elements.disintegrate, 13.d6());
            M.Apply.WhenChance(Chance.OneIn2, T => T.DegradeItem());
          },
          C =>
          {
            C.SetCast().Beam(Beams.disintegration, 1.d4() + 8);
            C.Apply.HarmEntity(Elements.disintegrate, 16.d6());
            C.Apply.WhenChance(Chance.OneIn2, T => T.DegradeItem());
          }
        );
      });

      sunburst_smite = AddSpell(Schools.evocation, "sunburst smite", 3, new Precept(Purpose.Blast, Elements.physical), Glyphs.sunburst_smite_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, 1.d4() + 2).SetTargetSelf(false);
            U.Apply.HarmEntity(Elements.physical, 4.d6());
            U.Apply.WhenChance(Chance.OneIn2, T => T.HarmEntity(Elements.physical, 2.d6()));
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, 1.d4() + 3).SetTargetSelf(false);
            P.Apply.HarmEntity(Elements.physical, 5.d6());
            P.Apply.WhenChance(Chance.OneIn2, T => T.HarmEntity(Elements.physical, 3.d6()));
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, 1.d4() + 4).SetTargetSelf(false);
            S.Apply.HarmEntity(Elements.physical, 6.d6());
            S.Apply.WhenChance(Chance.ThreeIn4, T => T.HarmEntity(Elements.physical, 3.d6()));
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, 1.d4() + 5).SetTargetSelf(false);
            E.Apply.HarmEntity(Elements.physical, 8.d6());
            E.Apply.WhenChance(Chance.ThreeIn4, T => T.HarmEntity(Elements.physical, 4.d6()));
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, 1.d4() + 6).SetTargetSelf(false);
            M.Apply.HarmEntity(Elements.physical, 9.d6());
            M.Apply.WhenChance(Chance.Always, T => T.HarmEntity(Elements.physical, 4.d6()));
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, 1.d4() + 7).SetTargetSelf(false);
            C.Apply.HarmEntity(Elements.physical, 11.d6());
            C.Apply.WhenChance(Chance.Always, T => T.HarmEntity(Elements.physical, 5.d6()));
          }
        );
      });

      black_ice_patch = AddSpell(Schools.evocation, "black ice patch", 3, new Precept(Purpose.Blast, Elements.cold), Glyphs.black_ice_patch_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.cold, 1.d4() + 2);
            U.Apply.HarmEntity(Elements.cold, 5.d6());
            U.Apply.CreateVolatile(Volatiles.freeze, 1.d20() + 20);
          },
          P =>
          {
            P.SetCast().Beam(Beams.cold, 1.d4() + 3);
            P.Apply.HarmEntity(Elements.cold, 6.d6());
            P.Apply.CreateVolatile(Volatiles.freeze, 1.d20() + 25);
          },
          S =>
          {
            S.SetCast().Beam(Beams.cold, 1.d4() + 4);
            S.Apply.HarmEntity(Elements.cold, 8.d6());
            S.Apply.CreateVolatile(Volatiles.freeze, 1.d20() + 30);
          },
          E =>
          {
            E.SetCast().Beam(Beams.cold, 1.d4() + 5);
            E.Apply.HarmEntity(Elements.cold, 10.d6());
            E.Apply.CreateVolatile(Volatiles.freeze, 1.d20() + 35);
          },
          M =>
          {
            M.SetCast().Beam(Beams.cold, 1.d4() + 6);
            M.Apply.HarmEntity(Elements.cold, 12.d6());
            M.Apply.CreateVolatile(Volatiles.freeze, 1.d20() + 40);
          },
          C =>
          {
            C.SetCast().Beam(Beams.cold, 1.d4() + 7);
            C.Apply.HarmEntity(Elements.cold, 14.d6());
            C.Apply.CreateVolatile(Volatiles.freeze, 1.d20() + 45);
          }
        );
      });

      wildfire_brand = AddSpell(Schools.evocation, "wildfire brand", 4, new Precept(Purpose.Blast, Elements.fire), Glyphs.wildfire_brand_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.flame, 1.d4() + 3);
            U.Apply.HarmEntity(Elements.fire, 4.d6());
            U.Apply.CreateVolatile(Volatiles.blaze, 1.d20() + 10);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.flame, 1.d4() + 3);
            P.Apply.HarmEntity(Elements.fire, 6.d6());
            P.Apply.CreateVolatile(Volatiles.blaze, 1.d20() + 20);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.flame, 1.d4() + 4);
            S.Apply.HarmEntity(Elements.fire, 7.d6());
            S.Apply.CreateVolatile(Volatiles.blaze, 1.d20() + 25);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.flame, 1.d4() + 4);
            E.Apply.HarmEntity(Elements.fire, 8.d6());
            E.Apply.CreateVolatile(Volatiles.blaze, 1.d20() + 30);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.flame, 1.d4() + 5);
            M.Apply.HarmEntity(Elements.fire, 9.d6());
            M.Apply.CreateVolatile(Volatiles.blaze, 1.d20() + 35);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.flame, 1.d4() + 5);
            C.Apply.HarmEntity(Elements.fire, 11.d6());
            C.Apply.CreateVolatile(Volatiles.blaze, 1.d20() + 40);
          }
        );
      });

      crimson_ichor_lance = AddSpell(Schools.evocation, "crimson ichor lance", 4, new Precept(Purpose.Blast, Elements.poison), Glyphs.crimson_ichor_lance_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.poison, 1.d4() + 3);
            U.Apply.HarmEntity(Elements.poison, 4.d6());
            U.Apply.CreateVolatile(Volatiles.blood, 1.d20() + 8);
          },
          P =>
          {
            P.SetCast().Beam(Beams.poison, 1.d4() + 3);
            P.Apply.HarmEntity(Elements.poison, 6.d6());
            P.Apply.CreateVolatile(Volatiles.blood, 1.d20() + 15);
          },
          S =>
          {
            S.SetCast().Beam(Beams.poison, 1.d4() + 4);
            S.Apply.HarmEntity(Elements.poison, 7.d6());
            S.Apply.CreateVolatile(Volatiles.blood, 1.d20() + 18);
          },
          E =>
          {
            E.SetCast().Beam(Beams.poison, 1.d4() + 4);
            E.Apply.HarmEntity(Elements.poison, 8.d6());
            E.Apply.CreateVolatile(Volatiles.blood, 1.d20() + 21);
          },
          M =>
          {
            M.SetCast().Beam(Beams.poison, 1.d4() + 5);
            M.Apply.HarmEntity(Elements.poison, 9.d6());
            M.Apply.CreateVolatile(Volatiles.blood, 1.d20() + 24);
          },
          C =>
          {
            C.SetCast().Beam(Beams.poison, 1.d4() + 5);
            C.Apply.HarmEntity(Elements.poison, 11.d6());
            C.Apply.CreateVolatile(Volatiles.blood, 1.d20() + 28);
          }
        );
      });

      slick_detonation = AddSpell(Schools.evocation, "slick detonation", 4, new Precept(Purpose.Blast, Elements.physical), Glyphs.slick_detonation_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, 1.d4() + 3);
            U.Apply.HarmEntity(Elements.physical, 3.d6());
            U.Apply.CreateVolatile(Volatiles.oil, 1.d20() + 10);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4() + 3);
            P.Apply.HarmEntity(Elements.physical, 5.d6());
            P.Apply.CreateVolatile(Volatiles.oil, 1.d20() + 20);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 4);
            S.Apply.HarmEntity(Elements.physical, 6.d6());
            S.Apply.CreateVolatile(Volatiles.oil, 1.d20() + 24);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 4);
            E.Apply.HarmEntity(Elements.physical, 7.d6());
            E.Apply.CreateVolatile(Volatiles.oil, 1.d20() + 28);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 5);
            M.Apply.HarmEntity(Elements.physical, 8.d6());
            M.Apply.CreateVolatile(Volatiles.oil, 1.d20() + 32);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 5);
            C.Apply.HarmEntity(Elements.physical, 9.d6());
            C.Apply.CreateVolatile(Volatiles.oil, 1.d20() + 36);
          }
        );
      });

      arc_conduit = AddSpell(Schools.evocation, "arc conduit", 4, new Precept(Purpose.Blast, Elements.shock), Glyphs.arc_conduit_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.energy, 1.d4() + 3);
            U.Apply.HarmEntity(Elements.shock, 4.d6());
            U.Apply.CreateVolatile(Volatiles.electricity, 1.d20() + 10);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.energy, 1.d4() + 3);
            P.Apply.HarmEntity(Elements.shock, 6.d6());
            P.Apply.CreateVolatile(Volatiles.electricity, 1.d20() + 20);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.energy, 1.d4() + 4);
            S.Apply.HarmEntity(Elements.shock, 7.d6());
            S.Apply.CreateVolatile(Volatiles.electricity, 1.d20() + 24);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.energy, 1.d4() + 4);
            E.Apply.HarmEntity(Elements.shock, 8.d6());
            E.Apply.CreateVolatile(Volatiles.electricity, 1.d20() + 28);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.energy, 1.d4() + 5);
            M.Apply.HarmEntity(Elements.shock, 9.d6());
            M.Apply.CreateVolatile(Volatiles.electricity, 1.d20() + 32);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.energy, 1.d4() + 5);
            C.Apply.HarmEntity(Elements.shock, 11.d6());
            C.Apply.CreateVolatile(Volatiles.electricity, 1.d20() + 36);
          }
        );
      });

      acid_splash = AddSpell(Schools.evocation, "acid splash", 1, new Precept(Purpose.Blast, Elements.acid), Glyphs.acid_splash_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Explosion(Explosions.acid, Dice.Zero);
            U.Apply.HarmEntity(Elements.acid, 2.d4() + 1);
          },
          P =>
          {
            P.SetCast().Explosion(Explosions.acid, Dice.Zero);
            P.Apply.HarmEntity(Elements.acid, 3.d4() + 2);
          },
          S =>
          {
            S.SetCast().Explosion(Explosions.acid, Dice.Zero);
            S.Apply.HarmEntity(Elements.acid, 4.d4() + 2);
          },
          E =>
          {
            E.SetCast().Explosion(Explosions.acid, Dice.Zero);
            E.Apply.HarmEntity(Elements.acid, 5.d4() + 3);
          },
          M =>
          {
            M.SetCast().Explosion(Explosions.acid, Dice.Zero);
            M.Apply.HarmEntity(Elements.acid, 6.d4() + 3);
          },
          C =>
          {
            C.SetCast().Explosion(Explosions.acid, Dice.Zero);
            C.Apply.HarmEntity(Elements.acid, 6.d4() + 4);
          }
        );
      });

      entropic_siphon = AddSpell(Schools.evocation, "entropic siphon", 5, new Precept(Purpose.Blast, Elements.disintegrate), Glyphs.entropic_siphon_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.disintegration, 1.d4() + 4);
            U.Apply.HarmEntity(Elements.disintegrate, 5.d6());
            U.Apply.DrainMana(Elements.drain, 2.d4() + 1);
          },
          P =>
          {
            P.SetCast().Beam(Beams.disintegration, 1.d4() + 4);
            P.Apply.HarmEntity(Elements.disintegrate, 7.d6());
            P.Apply.DrainMana(Elements.drain, 3.d4() + 2);
          },
          S =>
          {
            S.SetCast().Beam(Beams.disintegration, 1.d4() + 5);
            S.Apply.HarmEntity(Elements.disintegrate, 8.d6());
            S.Apply.DrainMana(Elements.drain, 3.d4() + 3);
          },
          E =>
          {
            E.SetCast().Beam(Beams.disintegration, 1.d4() + 5);
            E.Apply.HarmEntity(Elements.disintegrate, 9.d6());
            E.Apply.DrainMana(Elements.drain, 4.d4() + 3);
          },
          M =>
          {
            M.SetCast().Beam(Beams.disintegration, 1.d4() + 6);
            M.Apply.HarmEntity(Elements.disintegrate, 10.d6());
            M.Apply.DrainMana(Elements.drain, 4.d4() + 4);
          },
          C =>
          {
            C.SetCast().Beam(Beams.disintegration, 1.d4() + 6);
            C.Apply.HarmEntity(Elements.disintegrate, 13.d6());
            C.Apply.DrainMana(Elements.drain, 5.d4() + 5);
          }
        );
      });

      glacial_spike = AddSpell(Schools.evocation, "glacial spike", 4, new Precept(Purpose.Blast, Elements.cold), Glyphs.glacial_spike_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.frost, 1.d4() + 3).SetTargetSelf(false);
            U.Apply.HarmEntity(Elements.cold, 5.d6());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.frost, 1.d4() + 3).SetTargetSelf(false);
            P.Apply.HarmEntity(Elements.cold, 8.d6());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.frost, 1.d4() + 4).SetTargetSelf(false);
            S.Apply.HarmEntity(Elements.cold, 9.d6());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.frost, 1.d4() + 4).SetTargetSelf(false);
            E.Apply.HarmEntity(Elements.cold, 10.d6());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.frost, 1.d4() + 5).SetTargetSelf(false);
            M.Apply.HarmEntity(Elements.cold, 12.d6());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.frost, 1.d4() + 5).SetTargetSelf(false);
            C.Apply.HarmEntity(Elements.cold, 14.d6());
          }
        );
      });

      withering_ray = AddSpell(Schools.evocation, "withering ray", 5, new Precept(Purpose.Blast, Elements.necrotic), Glyphs.withering_ray_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, 1.d4() + 4).SetTargetSelf(false);
            U.Apply.HarmEntity(Elements.necrotic, 5.d6());
            U.Apply.WhenChance(Chance.OneIn3, T => T.UnlessTargetResistant(Elements.necrotic, R => R.DecreaseOneAbility(1.d2())));
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, 1.d4() + 4).SetTargetSelf(false);
            P.Apply.HarmEntity(Elements.necrotic, 7.d6());
            P.Apply.WhenChance(Chance.OneIn3, T => T.UnlessTargetResistant(Elements.necrotic, R => R.DecreaseOneAbility(1.d2())));
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, 1.d4() + 5).SetTargetSelf(false);
            S.Apply.HarmEntity(Elements.necrotic, 8.d6());
            S.Apply.WhenChance(Chance.OneIn3, T => T.UnlessTargetResistant(Elements.necrotic, R => R.DecreaseOneAbility(1.d2() + 1)));
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, 1.d4() + 5).SetTargetSelf(false);
            E.Apply.HarmEntity(Elements.necrotic, 9.d6());
            E.Apply.WhenChance(Chance.OneIn3, T => T.UnlessTargetResistant(Elements.necrotic, R => R.DecreaseOneAbility(1.d2() + 1)));
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, 1.d4() + 6).SetTargetSelf(false);
            M.Apply.HarmEntity(Elements.necrotic, 10.d6());
            M.Apply.WhenChance(Chance.OneIn3, T => T.UnlessTargetResistant(Elements.necrotic, R => R.DecreaseOneAbility(1.d2() + 2)));
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, 1.d4() + 6).SetTargetSelf(false);
            C.Apply.HarmEntity(Elements.necrotic, 13.d6());
            C.Apply.WhenChance(Chance.OneIn3, T => T.UnlessTargetResistant(Elements.necrotic, R => R.DecreaseOneAbility(1.d2() + 2)));
          }
        );
      });

      juggernaut_ray = AddSpell(Schools.evocation, "juggernaut ray", 6, new Precept(Purpose.Blast, Elements.force), Glyphs.juggernaut_ray_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, 1.d4() + 5).SetPenetrates().SetTargetSelf(false);
            U.Apply.HarmEntity(Elements.force, 7.d6());
            U.Apply.Knockback();
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4() + 5).SetPenetrates().SetTargetSelf(false);
            P.Apply.HarmEntity(Elements.force, 10.d6());
            P.Apply.Knockback();
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 6).SetPenetrates().SetTargetSelf(false);
            S.Apply.HarmEntity(Elements.force, 11.d6());
            S.Apply.Knockback();
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 6).SetPenetrates().SetTargetSelf(false);
            E.Apply.HarmEntity(Elements.force, 13.d6());
            E.Apply.Knockback();
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 7).SetPenetrates().SetTargetSelf(false);
            M.Apply.HarmEntity(Elements.force, 15.d6());
            M.Apply.Knockback();
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 7).SetPenetrates().SetTargetSelf(false);
            C.Apply.HarmEntity(Elements.force, 19.d6());
            C.Apply.Knockback();
          }
        );
      });

      starfall = AddSpell(Schools.evocation, "starfall", 6, new Precept(Purpose.Blast, Elements.fire), Glyphs.starfall_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Explosion(Explosions.light, 1.d6() + 5);
            U.Apply.HarmEntity(Elements.fire, 8.d6());
          },
          P =>
          {
            P.SetCast().Explosion(Explosions.light, 1.d6() + 5);
            P.Apply.HarmEntity(Elements.fire, 12.d6());
          },
          S =>
          {
            S.SetCast().Explosion(Explosions.light, 1.d6() + 6);
            S.Apply.HarmEntity(Elements.fire, 14.d6());
          },
          E =>
          {
            E.SetCast().Explosion(Explosions.light, 1.d6() + 6);
            E.Apply.HarmEntity(Elements.fire, 16.d6());
          },
          M =>
          {
            M.SetCast().Explosion(Explosions.light, 1.d6() + 7);
            M.Apply.HarmEntity(Elements.fire, 18.d6());
          },
          C =>
          {
            C.SetCast().Explosion(Explosions.light, 1.d6() + 7);
            C.Apply.HarmEntity(Elements.fire, 22.d6());
          }
        );
      });

      chain_lightning = AddSpell(Schools.evocation, "chain lightning", 6, new Precept(Purpose.Blast, Elements.shock), Glyphs.chain_lightning_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.lightning, 1.d4() + 5).SetBounces().SetPenetrates();
            U.Apply.HarmEntity(Elements.shock, 6.d6());
          },
          P =>
          {
            P.SetCast().Beam(Beams.lightning, 1.d4() + 6).SetBounces().SetPenetrates();
            P.Apply.HarmEntity(Elements.shock, 8.d6());
          },
          S =>
          {
            S.SetCast().Beam(Beams.lightning, 1.d4() + 7).SetBounces().SetPenetrates();
            S.Apply.HarmEntity(Elements.shock, 9.d6());
            S.Apply.WhenChance(Chance.OneIn4, T => T.UnlessTargetResistant(Elements.shock, R => R.ApplyTransient(Properties.stunned, 1.d2())));
          },
          E =>
          {
            E.SetCast().Beam(Beams.lightning, 1.d4() + 8).SetBounces().SetPenetrates();
            E.Apply.HarmEntity(Elements.shock, 10.d6());
            E.Apply.WhenChance(Chance.OneIn4, T => T.UnlessTargetResistant(Elements.shock, R => R.ApplyTransient(Properties.stunned, 1.d3())));
          },
          M =>
          {
            M.SetCast().Beam(Beams.lightning, 1.d4() + 9).SetBounces().SetPenetrates();
            M.Apply.HarmEntity(Elements.shock, 10.d6() + 5);
            M.Apply.WhenChance(Chance.OneIn4, T => T.UnlessTargetResistant(Elements.shock, R => R.ApplyTransient(Properties.stunned, 1.d4())));
          },
          C =>
          {
            C.SetCast().Beam(Beams.lightning, 1.d4() + 10).SetBounces().SetPenetrates();
            C.Apply.HarmEntity(Elements.shock, 11.d6());
            C.Apply.WhenChance(Chance.OneIn4, T => T.UnlessTargetResistant(Elements.shock, R => R.ApplyTransient(Properties.stunned, 1.d4())));
          }
        );
      });

      meteoric_cataclysm = AddSpell(Schools.evocation, "meteoric cataclysm", 7, new Precept(Purpose.Blast, Elements.fire), Glyphs.meteoric_cataclysm_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Explosion(Explosions.fiery, 2.d6() + 3);
            U.Apply.HarmEntity(Elements.fire, 10.d6());
            U.Apply.Knockback();
            U.Apply.CreateVolatile(Volatiles.blaze, 2.d20() + 10);
          },
          P =>
          {
            P.SetCast().Explosion(Explosions.fiery, 2.d6() + 4);
            P.Apply.HarmEntity(Elements.fire, 12.d6());
            P.Apply.Knockback();
            P.Apply.CreateVolatile(Volatiles.blaze, 2.d20() + 15);
          },
          S =>
          {
            S.SetCast().Explosion(Explosions.fiery, 2.d6() + 5);
            S.Apply.HarmEntity(Elements.fire, 13.d6());
            S.Apply.Knockback();
            S.Apply.CreateVolatile(Volatiles.blaze, 2.d20() + 18);
          },
          E =>
          {
            E.SetCast().Explosion(Explosions.fiery, 2.d6() + 6);
            E.Apply.HarmEntity(Elements.fire, 15.d6());
            E.Apply.Knockback();
            E.Apply.CreateVolatile(Volatiles.blaze, 2.d20() + 22);
          },
          M =>
          {
            M.SetCast().Explosion(Explosions.fiery, 2.d6() + 7);
            M.Apply.HarmEntity(Elements.fire, 16.d6());
            M.Apply.Knockback();
            M.Apply.CreateVolatile(Volatiles.blaze, 2.d20() + 26);
          },
          C =>
          {
            C.SetCast().Explosion(Explosions.fiery, 2.d6() + 8);
            C.Apply.HarmEntity(Elements.fire, 18.d6());
            C.Apply.Knockback();
            C.Apply.CreateVolatile(Volatiles.blaze, 2.d20() + 30);
          }
        );
      });

      wall_breach = AddSpell(Schools.transmutation, "wall breach", 2, null, Glyphs.wall_breach_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.tunnel, Dice.Zero).SetObjects();
            U.Apply.DestroyBarrier(WallStructure.Illusionary);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.tunnel, Dice.One).SetObjects();
            P.Apply.DestroyBarrier(WallStructure.Illusionary);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.tunnel, 1.d4()).SetObjects();
            S.Apply.DestroyBarrier(WallStructure.Solid);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.tunnel, 1.d4() + 2).SetObjects();
            E.Apply.DestroyBarrier(WallStructure.Solid, Barriers.iron_bars);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.tunnel, 1.d4() + 4).SetObjects();
            M.Apply.DestroyBarrier(WallStructure.Solid, Barriers.iron_bars);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.tunnel, 1.d4() + 6).SetObjects();
            C.Apply.DestroyBarrier(WallStructure.Permanent);
          }
        );
      });

      causeway = AddSpell(Schools.transmutation, "causeway", 2, null, Glyphs.causeway_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.tunnel, Dice.Zero).SetObjects();
            U.Apply.ConvertGround(FromGround: null, ToGround: Grounds.stone_path, Locality.Square);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.tunnel, 1.d4() + 1).SetObjects();
            P.Apply.ConvertGround(FromGround: null, ToGround: Grounds.stone_path, Locality.Square);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.tunnel, 1.d4() + 2).SetObjects();
            S.Apply.ConvertGround(FromGround: null, ToGround: Grounds.stone_path, Locality.Zone);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.tunnel, 1.d4() + 3).SetObjects();
            E.Apply.ConvertGround(FromGround: null, ToGround: Grounds.stone_path, Locality.Zone);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.tunnel, 1.d4() + 4).SetObjects();
            M.Apply.ConvertGround(FromGround: null, ToGround: Grounds.stone_path, Locality.Area);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.tunnel, 1.d4() + 5).SetObjects();
            C.Apply.ConvertGround(FromGround: null, ToGround: Grounds.stone_path, Locality.Area);
          }
        );
      });

      ironbind_door = AddSpell(Schools.transmutation, "ironbind door", 2, new Precept(Purpose.Buff), Glyphs.ironbind_door_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.tunnel, Dice.Zero).SetObjects();
            U.Apply.ConvertGate(FromGate: null, ToGate: Gates.iron_door, Locality.Square);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.tunnel, 1.d4() + 1).SetObjects();
            P.Apply.ConvertGate(FromGate: null, ToGate: Gates.iron_door, Locality.Square);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.tunnel, 1.d4() + 2).SetObjects();
            S.Apply.ConvertGate(FromGate: null, ToGate: Gates.iron_door, Locality.Zone);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.tunnel, 1.d4() + 3).SetObjects();
            E.Apply.ConvertGate(FromGate: null, ToGate: Gates.iron_door, Locality.Zone);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.tunnel, 1.d4() + 4).SetObjects();
            M.Apply.ConvertGate(FromGate: null, ToGate: Gates.crystal_door, Locality.Zone);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.tunnel, 1.d4() + 5).SetObjects();
            C.Apply.ConvertGate(FromGate: null, ToGate: Gates.crystal_door, Locality.Zone);
          }
        );
      });

      porters_reach = AddSpell(Schools.transmutation, "porter's reach", 1, new Precept(Purpose.Toss), Glyphs.porters_reach_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.Zero).SetObjects();
            U.Apply.TeleportFloorItem();
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, Dice.One).SetObjects();
            P.Apply.TeleportFloorItem();
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 1).SetObjects();
            S.Apply.TeleportFloorItem();
            S.Apply.TeleportInventoryItem();
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 3).SetObjects();
            E.Apply.TeleportFloorItem();
            E.Apply.TeleportInventoryItem();
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 5).SetObjects();
            M.Apply.TeleportFloorItem();
            M.Apply.TeleportInventoryItem();
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 7).SetObjects();
            C.Apply.TeleportFloorItem();
            C.Apply.TeleportInventoryItem();
          }
        );
      });

      sunder_gear = AddSpell(Schools.transmutation, "sunder gear", 3, new Precept(Purpose.Debuff), Glyphs.sunder_gear_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.Zero);
            U.Apply.DestroyTargetItem(Dice.One);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4() + 1);
            P.Apply.DestroyTargetItem(Dice.One);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 3);
            S.Apply.DestroyTargetItem(Dice.One);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 5);
            E.Apply.DestroyTargetItem(1.d2());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 7);
            M.Apply.DestroyTargetItem(1.d2());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 9);
            C.Apply.DestroyTargetItem(1.d3());
          }
        );
      });

      gilding_touch = AddSpell(Schools.transmutation, "gilding touch", 4, new Precept(Purpose.Debuff), Glyphs.gilding_touch_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.Zero);
            U.Apply.TransmuteItem(Materials.gold);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4() + 1);
            P.Apply.TransmuteItem(Materials.gold);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 3);
            S.Apply.TransmuteItem(Materials.gold);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 5);
            E.Apply.TransmuteItem(Materials.gold);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 7);
            M.Apply.TransmuteItem(Materials.gold);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 9);
            C.Apply.TransmuteItem(Materials.gold);
          }
        );
      });

      claybind_touch = AddSpell(Schools.transmutation, "claybind touch", 3, new Precept(Purpose.Debuff), Glyphs.claybind_touch_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.Zero);
            U.Apply.TransmuteItem(Materials.clay);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4() + 1);
            P.Apply.TransmuteItem(Materials.clay);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 3);
            S.Apply.TransmuteItem(Materials.clay);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 5);
            E.Apply.TransmuteItem(Materials.clay);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 7);
            M.Apply.TransmuteItem(Materials.clay);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 9);
            C.Apply.TransmuteItem(Materials.clay);
          }
        );
      });

      arcane_whetstone = AddSpell(Schools.transmutation, "arcane whetstone", 3, new Precept(Purpose.Buff), Glyphs.arcane_whetstone_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.magic, Dice.Zero).SetObjects();
            U.Apply.EnchantItemUp(Dice.One);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.magic, 1.d4()).SetObjects();
            P.Apply.EnchantItemUp(Dice.One);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.magic, 1.d4() + 2).SetObjects();
            S.Apply.EnchantItemUp(1.d2());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.magic, 1.d4() + 4).SetObjects();
            E.Apply.EnchantItemUp(1.d2());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.magic, 1.d4() + 6).SetObjects();
            M.Apply.EnchantItemUp(1.d3());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.magic, 1.d4() + 8).SetObjects();
            C.Apply.EnchantItemUp(1.d3());
          }
        );
      });

      arcane_blight = AddSpell(Schools.transmutation, "arcane blight", 3, new Precept(Purpose.Debuff), Glyphs.arcane_blight_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.magic, Dice.Zero);
            U.Apply.EnchantItemDown(Dice.One);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.magic, 1.d4());
            P.Apply.EnchantItemDown(Dice.One);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.magic, 1.d4() + 2);
            S.Apply.EnchantItemDown(1.d2());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.magic, 1.d4() + 4);
            E.Apply.EnchantItemDown(1.d2());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.magic, 1.d4() + 6);
            M.Apply.EnchantItemDown(1.d3());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.magic, 1.d4() + 8);
            C.Apply.EnchantItemDown(1.d3());
          }
        );
      });

      petrifying_touch = AddSpell(Schools.transmutation, "petrifying touch", 6, new Precept(Purpose.Debuff), Glyphs.petrifying_touch_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.Zero);
            U.Apply.IncarcerateEntity(Elements.petrify, Codex.Blocks.statue);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, Dice.One);
            P.Apply.IncarcerateEntity(Elements.petrify, Codex.Blocks.statue);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 1);
            S.Apply.IncarcerateEntity(Elements.petrify, Codex.Blocks.statue);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 3);
            E.Apply.IncarcerateEntity(Elements.petrify, Codex.Blocks.statue);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 5);
            M.Apply.IncarcerateEntity(Elements.petrify, Codex.Blocks.statue);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 7);
            C.Apply.IncarcerateEntity(Elements.petrify, Codex.Blocks.statue);
          }
        );
      });

      shrink = AddSpell(Schools.transmutation, "shrink", 3, new Precept(Purpose.Debuff), Glyphs.shrink_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.Zero);
            var wormBrood = Codex.Kinds.worm.Entities;
            U.Apply.PolymorphEntity(wormBrood.Where(X => X.IsEncounter).ToArray());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4());
            var wormBrood = Codex.Kinds.worm.Entities;
            P.Apply.PolymorphEntity(wormBrood.Where(X => X.IsEncounter).ToArray());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 2);
            var wormBrood = Codex.Kinds.worm.Entities;
            S.Apply.PolymorphEntity(wormBrood.Where(X => X.IsEncounter).ToArray());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 4);
            var wormBrood = Codex.Kinds.worm.Entities;
            E.Apply.PolymorphEntity(wormBrood.Where(X => X.IsEncounter).ToArray());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 6);
            var wormBrood = Codex.Kinds.worm.Entities;
            M.Apply.PolymorphEntity(wormBrood.Where(X => X.IsEncounter).ToArray());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 8);
            var wormBrood = Codex.Kinds.worm.Entities;
            C.Apply.PolymorphEntity(wormBrood.Where(X => X.IsEncounter).ToArray());
          }
        );
      });

      enlarge = AddSpell(Schools.transmutation, "enlarge", 3, new Precept(Purpose.Buff), Glyphs.enlarge_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero) // can only target self.
             .SetObjects(false);
            U.Apply.GrowEntity();
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetObjects(false);
            P.Apply.GrowEntity();
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, 1.d4() + 1)
             .SetObjects(false);
            S.Apply.GrowEntity();
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, 1.d4() + 3)
             .SetObjects(false);
            E.Apply.GrowEntity();
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, 1.d4() + 5)
             .SetObjects(false);
            M.Apply.GrowEntity();
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, 1.d4() + 7)
             .SetObjects(false);
            C.Apply.GrowEntity();
          }
        );
      });

      telekinetic_shove = AddSpell(Schools.transmutation, "telekinetic shove", 2, new Precept(Purpose.Toss), Glyphs.telekinetic_shove_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.Zero)
             .SetObjects();
            U.Apply.MobiliseBlock();
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4())
             .SetObjects();
            P.Apply.MobiliseBlock();
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 2)
             .SetObjects();
            S.Apply.MobiliseBlock();
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 4)
             .SetObjects();
            E.Apply.MobiliseBlock();
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 6)
             .SetObjects();
            M.Apply.MobiliseBlock();
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 8)
             .SetObjects();
            C.Apply.MobiliseBlock();
          }
        );
      });

      conjure_barricade = AddSpell(Schools.transmutation, "conjure barricade", 3, new Precept(Purpose.Block), Glyphs.conjure_barricade_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.tunnel, Dice.Zero)
             .SetObjects();
            U.Apply.CreateBlock(Dice.One, Block: null);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.tunnel, 1.d4() + 1)
             .SetObjects();
            P.Apply.CreateBlock(Dice.One, Block: null);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.tunnel, 1.d4() + 2)
             .SetObjects();
            S.Apply.CreateBlock(1.d2(), Block: null);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.tunnel, 1.d4() + 4)
             .SetObjects();
            E.Apply.CreateBlock(1.d2(), Block: null);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.tunnel, 1.d4() + 6)
             .SetObjects();
            M.Apply.CreateBlock(1.d3(), Block: null);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.tunnel, 1.d4() + 8)
             .SetObjects();
            C.Apply.CreateBlock(1.d3(), Block: null);
          }
        );
      });

      gravity_well = AddSpell(Schools.transmutation, "gravity well", 5, new Precept(Purpose.Teleport), Glyphs.gravity_well_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.Zero);
            U.Apply.TransitionDescend(SpatialProperty: null, Dice.One, Fixed: false);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4());
            P.Apply.TransitionDescend(SpatialProperty: null, Dice.One, Fixed: false);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 2);
            S.Apply.TransitionDescend(SpatialProperty: null, 1.d2(), Fixed: false);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 4);
            E.Apply.TransitionDescend(SpatialProperty: null, 1.d2(), Fixed: false);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 6);
            M.Apply.TransitionDescend(SpatialProperty: null, 1.d3(), Fixed: false);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 8);
            C.Apply.TransitionDescend(SpatialProperty: null, 1.d3(), Fixed: false);
          }
        );
      });

      artificers_exchange = AddSpell(Schools.transmutation, "artificer's exchange", 4, new Precept(Purpose.Unspecified), Glyphs.artificers_exchange_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.Zero)
             .SetObjects();
            U.Apply.ConvertItem(Codex.Stocks.weapon, WholeStack: false, Items.long_sword);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, Dice.One)
             .SetObjects();
            P.Apply.ConvertItem(Codex.Stocks.weapon, WholeStack: false, Items.long_sword);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 1)
             .SetObjects();
            S.Apply.ConvertItem(Codex.Stocks.armour, WholeStack: false, Items.leather_armour);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 2)
             .SetObjects();
            E.Apply.ConvertItem(Codex.Stocks.wand, WholeStack: false, Items.wand_of_digging);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 3)
             .SetObjects();
            M.Apply.ConvertItem(Codex.Stocks.ring, WholeStack: false, Items.ring_of_adornment);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 4)
             .SetObjects();
            C.Apply.ConvertItem(Codex.Stocks.amulet, WholeStack: false, Items.amulet_of_nada);
          }
        );
      });

      barring_ward = AddSpell(Schools.transmutation, "barring ward", 2, new Precept(Purpose.Block), Glyphs.barring_ward_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.tunnel, Dice.Zero)
             .SetObjects();
            U.Apply.ConvertBarrier(FromBarrier: null, ToBarrier: Codex.Barriers.iron_bars, Locality.Square);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.tunnel, 1.d4() + 1)
             .SetObjects();
            P.Apply.ConvertBarrier(FromBarrier: null, ToBarrier: Codex.Barriers.iron_bars, Locality.Square);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.tunnel, 1.d4() + 2)
             .SetObjects();
            S.Apply.ConvertBarrier(FromBarrier: null, ToBarrier: Codex.Barriers.iron_bars, Locality.Zone);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.tunnel, 1.d4() + 3)
             .SetObjects();
            E.Apply.ConvertBarrier(FromBarrier: null, ToBarrier: Codex.Barriers.iron_bars, Locality.Zone);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.tunnel, 1.d4() + 4)
             .SetObjects();
            M.Apply.ConvertBarrier(FromBarrier: null, ToBarrier: Codex.Barriers.iron_bars, Locality.Area);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.tunnel, 1.d4() + 5)
             .SetObjects();
            C.Apply.ConvertBarrier(FromBarrier: null, ToBarrier: Codex.Barriers.iron_bars, Locality.Area);
          }
        );
      });

      transmuters_mastery = AddSpell(Schools.transmutation, "transmuter's mastery", 7, new Precept(Purpose.Unspecified), Glyphs.transmuters_mastery_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.magic, Dice.Zero)
             .SetObjects();
            U.Apply.ConvertGround(FromGround: null, ToGround: Codex.Grounds.stone_path, Locality.Zone);
            U.Apply.ConvertBarrier(FromBarrier: null, ToBarrier: Codex.Barriers.stone_wall, Locality.Zone);
            U.Apply.TransmuteItem(Materials.gold);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.magic, Dice.One)
             .SetObjects();
            P.Apply.ConvertGround(FromGround: null, ToGround: Codex.Grounds.stone_path, Locality.Zone);
            P.Apply.ConvertBarrier(FromBarrier: null, ToBarrier: Codex.Barriers.stone_wall, Locality.Zone);
            P.Apply.TransmuteItem(Materials.gold);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.magic, 1.d4())
             .SetObjects();
            S.Apply.ConvertGround(FromGround: null, ToGround: Codex.Grounds.stone_path, Locality.Area);
            S.Apply.ConvertBarrier(FromBarrier: null, ToBarrier: Codex.Barriers.stone_wall, Locality.Area);
            S.Apply.TransmuteItem(Materials.gold);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.magic, 1.d4() + 1)
             .SetObjects();
            E.Apply.ConvertGround(FromGround: null, ToGround: Codex.Grounds.stone_path, Locality.Area);
            E.Apply.ConvertBarrier(FromBarrier: null, ToBarrier: Codex.Barriers.stone_wall, Locality.Area);
            E.Apply.TransmuteItem(Materials.gold);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.magic, 1.d4() + 2)
             .SetObjects();
            M.Apply.ConvertGround(FromGround: null, ToGround: Codex.Grounds.stone_path, Locality.Map);
            M.Apply.ConvertBarrier(FromBarrier: null, ToBarrier: Codex.Barriers.stone_wall, Locality.Map);
            M.Apply.TransmuteItem(Materials.gold);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.magic, 1.d4() + 4)
             .SetObjects();
            C.Apply.ConvertGround(FromGround: null, ToGround: Codex.Grounds.stone_path, Locality.Map);
            C.Apply.ConvertBarrier(FromBarrier: null, ToBarrier: Codex.Barriers.stone_wall, Locality.Map);
            C.Apply.TransmuteItem(Materials.gold);
          }
        );
      });

      harvest_of_souls = AddSpell(Schools.necromancy, "harvest of souls", 7, new Precept(Purpose.Blast, Elements.necrotic), Glyphs.harvest_of_souls_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.death, Dice.Zero)
             .SetTerminates();
            U.Apply.HarmEntity(Elements.necrotic, 8.d6());
            U.Apply.WhenChance(Chance.OneIn20, T => T.MurderEntity(MurderType.Hostile, Strikes.death, Kinds.Living.ToArray()));
          },
          P =>
          {
            P.SetCast().Strike(Strikes.death, Dice.Zero)
             .SetTerminates();
            P.Apply.HarmEntity(Elements.necrotic, 9.d6());
            P.Apply.WhenChance(Chance.OneIn16, T => T.MurderEntity(MurderType.Hostile, Strikes.death, Kinds.Living.ToArray()));
          },
          S =>
          {
            S.SetCast().Strike(Strikes.death, Dice.Zero)
             .SetTerminates();
            S.Apply.HarmEntity(Elements.necrotic, 10.d6());
            S.Apply.WhenChance(Chance.OneIn12, T => T.MurderEntity(MurderType.Hostile, Strikes.death, Kinds.Living.ToArray()));
          },
          E =>
          {
            E.SetCast().Strike(Strikes.death, Dice.Zero)
             .SetTerminates();
            E.Apply.HarmEntity(Elements.necrotic, 11.d6());
            E.Apply.WhenChance(Chance.OneIn8, T => T.MurderEntity(MurderType.Hostile, Strikes.death, Kinds.Living.ToArray()));
          },
          M =>
          {
            M.SetCast().Strike(Strikes.death, Dice.Zero)
             .SetTerminates();
            M.Apply.HarmEntity(Elements.necrotic, 12.d6());
            M.Apply.WhenChance(Chance.OneIn4, T => T.MurderEntity(MurderType.Hostile, Strikes.death, Kinds.Living.ToArray()));
          },
          C =>
          {
            C.SetCast().Strike(Strikes.death, Dice.Zero)
             .SetTerminates();
            C.Apply.HarmEntity(Elements.necrotic, 13.d6());
            C.Apply.WhenChance(Chance.OneIn2, T => T.MurderEntity(MurderType.Hostile, Strikes.death, Kinds.Living.ToArray()));
          }
        );
      });
      // <<< GENERATED SPELLS <<<
      Register.Alias(restoration, "restore ability");
      Register.Alias(deflection, "protection");
    }
#endif

    // >>> GENERATED SPELL-FIELDS >>>
    public readonly Spell stone_to_flesh;
    public readonly Spell clear_sight;
    public readonly Spell danger_sense;
    public readonly Spell mirror_ward;
    public readonly Spell planar_anchor;
    public readonly Spell hexbind;
    public readonly Spell blade_blessing;
    public readonly Spell atonement;
    public readonly Spell rally_cry;
    public readonly Spell chromatic_orb;
    public readonly Spell prismatic_spray;
    public readonly Spell hold_monster;
    public readonly Spell concussive_blast;
    public readonly Spell death_ward;
    public readonly Spell stoneskin;
    public readonly Spell displacement;
    public readonly Spell discord;
    public readonly Spell warning_ward;
    public readonly Spell owl_eyes;
    public readonly Spell augury;
    public readonly Spell keen_search;
    public readonly Spell find_traps;
    public readonly Spell detect_undead;
    public readonly Spell sense_curse;
    public readonly Spell revelation;
    public readonly Spell find_doors;
    public readonly Spell detect_metal;
    public readonly Spell mind_ken;
    public readonly Spell foreknowledge;
    public readonly Spell true_appraisal;
    public readonly Spell oracles_eye;
    public readonly Spell premonition;
    public readonly Spell world_vision;
    public readonly Spell hex;
    public readonly Spell daze;
    public readonly Spell swoon;
    public readonly Spell calm;
    public readonly Spell hideous_laughter;
    public readonly Spell psychic_shove;
    public readonly Spell battle_fury;
    public readonly Spell tongue_tied;
    public readonly Spell grasping_mind;
    public readonly Spell mind_spike;
    public readonly Spell iron_will;
    public readonly Spell hold_person;
    public readonly Spell mind_link;
    public readonly Spell clouded_mind;
    public readonly Spell song_of_discord;
    public readonly Spell creeping_palsy;
    public readonly Spell dominate_mind;
    public readonly Spell mindrend;
    public readonly Spell bless;
    public readonly Spell sense_the_restless;
    public readonly Spell command;
    public readonly Spell bane;
    public readonly Spell sanctuary;
    public readonly Spell commune;
    public readonly Spell silence;
    public readonly Spell freedom_of_movement;
    public readonly Spell consecration;
    public readonly Spell ward_of_return;
    public readonly Spell searing_light;
    public readonly Spell prayer;
    public readonly Spell spirit_shield;
    public readonly Spell holy_word;
    public readonly Spell divine_favor;
    public readonly Spell divine_intervention;
    public readonly Spell caltrop_swarm;
    public readonly Spell unseen_hand;
    public readonly Spell web_snare;
    public readonly Spell entangling_vines;
    public readonly Spell conjure_homunculus;
    public readonly Spell conjured_pit;
    public readonly Spell conjure_mount;
    public readonly Spell planar_swap;
    public readonly Spell conjure_boulder;
    public readonly Spell call_of_the_pack;
    public readonly Spell wall_of_thorns;
    public readonly Spell conjure_guardian;
    public readonly Spell repelling_ward;
    public readonly Spell elemental_servant;
    public readonly Spell binding_sphere;
    public readonly Spell earthen_colossus;
    public readonly Spell simulacrum;
    public readonly Spell gate;
    public readonly Spell sentinel_ward;
    public readonly Spell hazard_sense;
    public readonly Spell shimmer_shield;
    public readonly Spell slipping_free;
    public readonly Spell freedom;
    public readonly Spell veil_of_shadows;
    public readonly Spell inner_calm;
    public readonly Spell steadfast_ward;
    public readonly Spell unbinding;
    public readonly Spell blurred_form;
    public readonly Spell disarming_ward;
    public readonly Spell repulsion;
    public readonly Spell guided_path;
    public readonly Spell changeless_ward;
    public readonly Spell neutralize_poison;
    public readonly Spell elemental_warding;
    public readonly Spell dispel_magic;
    public readonly Spell undying_ward;
    public readonly Spell chill_grasp;
    public readonly Spell false_life;
    public readonly Spell deathwatch;
    public readonly Spell spectral_shroud;
    public readonly Spell ray_of_enfeeblement;
    public readonly Spell grave_rot;
    public readonly Spell spirit_leech;
    public readonly Spell contagion;
    public readonly Spell banshee_wail;
    public readonly Spell whispers_of_madness;
    public readonly Spell wither;
    public readonly Spell corpse_explosion;
    public readonly Spell legion_of_bone;
    public readonly Spell reap;
    public readonly Spell vampiric_feast;
    public readonly Spell deaths_bargain;
    public readonly Spell black_plague;
    public readonly Spell spark_bolt;
    public readonly Spell ember_lance;
    public readonly Spell frost_breath;
    public readonly Spell arcing_bolt;
    public readonly Spell timed_combustion;
    public readonly Spell scalding_cloud;
    public readonly Spell thunderclap;
    public readonly Spell corroding_ray;
    public readonly Spell sunburst_smite;
    public readonly Spell black_ice_patch;
    public readonly Spell wildfire_brand;
    public readonly Spell crimson_ichor_lance;
    public readonly Spell slick_detonation;
    public readonly Spell arc_conduit;
    public readonly Spell acid_splash;
    public readonly Spell entropic_siphon;
    public readonly Spell glacial_spike;
    public readonly Spell withering_ray;
    public readonly Spell juggernaut_ray;
    public readonly Spell starfall;
    public readonly Spell chain_lightning;
    public readonly Spell meteoric_cataclysm;
    public readonly Spell wall_breach;
    public readonly Spell causeway;
    public readonly Spell ironbind_door;
    public readonly Spell porters_reach;
    public readonly Spell sunder_gear;
    public readonly Spell gilding_touch;
    public readonly Spell claybind_touch;
    public readonly Spell arcane_whetstone;
    public readonly Spell arcane_blight;
    public readonly Spell petrifying_touch;
    public readonly Spell shrink;
    public readonly Spell enlarge;
    public readonly Spell telekinetic_shove;
    public readonly Spell conjure_barricade;
    public readonly Spell gravity_well;
    public readonly Spell artificers_exchange;
    public readonly Spell barring_ward;
    public readonly Spell transmuters_mastery;
    public readonly Spell harvest_of_souls;
    // <<< GENERATED SPELL-FIELDS <<<
    // abjuration = 8.
    public readonly Spell blinking;
    public readonly Spell haste;
    public readonly Spell invisibility;
    public readonly Spell jumping;
    public readonly Spell levitation;
    public readonly Spell light;
    public readonly Spell teleport_away;
    public readonly Spell deflection;
    public readonly Spell telekinesis;

    // conjuration = 7
    public readonly Spell create_familiar;
    public readonly Spell summoning;
    public readonly Spell flaming_sphere;
    public readonly Spell freezing_sphere;
    public readonly Spell shocking_sphere;
    public readonly Spell crushing_sphere;
    public readonly Spell soaking_sphere;

    // clerical = 8.
    public readonly Spell curing; // 2
    public readonly Spell healing; // 1
    public readonly Spell extra_healing; // 3
    public readonly Spell regenerate; // 4
    public readonly Spell restoration; // 4
    public readonly Spell full_healing; // 5
    public readonly Spell remove_curse;
    public readonly Spell turn_undead;
    // public readonly Spell mass_heal; // area of effect healing for allies (everyone nearby when unskilled).
    // public readonly Spell aid; // temporary above life maximum boost?
    // public readonly Spell sustenance; // nutrition up to zero only.

    // divination = 7.
    public readonly Spell clairvoyance;
    public readonly Spell detect_food;
    public readonly Spell detect_monsters;
    public readonly Spell detect_treasure;
    public readonly Spell detect_unseen;
    public readonly Spell identify;
    public readonly Spell magic_mapping;

    // evocation = 8.
    public readonly Spell acid_stream;
    public readonly Spell cone_of_cold;
    public readonly Spell fireball;
    public readonly Spell ice_storm;
    public readonly Spell force_bolt;
    public readonly Spell lightning_bolt;
    public readonly Spell magic_missile;
    public readonly Spell poison_blast;

    // enchantment = 7.
    public readonly Spell animate_object;
    public readonly Spell charm;
    public readonly Spell confusion;
    public readonly Spell fear;
    public readonly Spell phasing;
    public readonly Spell sleep;
    public readonly Spell slow;

    // necromancy = 7.
    public readonly Spell animate_dead;
    public readonly Spell bind_undead;
    public readonly Spell darkness;
    public readonly Spell drain_life;
    public readonly Spell finger_of_death;
    public readonly Spell living_wall;
    public readonly Spell raise_dead;

    // transmutation = 7.
    public readonly Spell cancellation;
    public readonly Spell dig;
    public readonly Spell disintegrate;
    public readonly Spell knock;
    public readonly Spell polymorph;
    public readonly Spell toxic_spray;
    public readonly Spell walling;
    public readonly Spell wizard_lock;
  }
}