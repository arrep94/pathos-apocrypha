using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexSpecials : CodexPage<ManifestSpecials, SpecialEditor, Special>
  {
    private CodexSpecials() { }
#if MASTER_CODEX
    internal CodexSpecials(Codex Codex)
      : base(Codex.Manifest.Specials)
    {
      var Glyphs = Codex.Glyphs;
      var Diets = Codex.Diets;
      var Items = Codex.Items;
      var Entities = Codex.Entities;
      var Materials = Codex.Materials;
      var Properties = Codex.Properties;
      var Elements = Codex.Elements;
      var Skills = Codex.Skills;
      var Spells = Codex.Spells;
      var Qualifications = Codex.Qualifications;
      var Sanctities = Codex.Sanctities;
      var Kinds = Codex.Kinds;
      var Attributes = Codex.Attributes;
      var Punishments = Codex.Punishments;
      var Afflictions = Codex.Afflictions;
      var Anatomies = Codex.Anatomies;
      var Sonics = Codex.Sonics;
      var Motions = Codex.Motions;
      var Stocks = Codex.Stocks;
      var Genders = Codex.Genders;
      var Races = Codex.Races;

      // NOTE: Special.Name should be a noun, not an adjective.

      Special AddSpecial(string Name, Action<SpecialEditor> Action)
      {
        return Register.Add(S =>
        {
          S.Name = Name;

          CodexRecruiter.Enrol(() => Action(S));
        });
      }
      Special AddLycanthrope(Entity Animal, Entity Beast, Entity Humanoid)
      {
        return AddSpecial("were" + Animal.Name, S =>
        {
          S.Description = $"Forever cursed with lycanthropy, these beings can transform themselves into monstrous {Animal.Name} forms.";
          S.Glyph = Humanoid.Glyph;
          S.Chemistry.SetVulnerability(Materials.silver);
          S.Startup.SetResistance(Elements.drain);
          S.SetTransformations(Beast, Humanoid);
        });
      }
      Special AddElemental(string Name, Glyph Glyph, Element PositiveElement, Element NegativeElement, Spell SphereSpell)
      {
        return AddSpecial(Name, S =>
        {
          S.Description = $"Highly attuned servants of elemental {Name}, they can conjure exploding spheres to destroy their enemies.";
          S.Glyph = Glyph;

          S.Chemistry.SetWeakness(NegativeElement);
          S.Startup.SetResistance(PositiveElement);
          S.Startup.SetTradeoffSkill(Skills.conjuration, SkillCategory.Defensive);
          S.Startup.AddGrimoire(Dice.One, SphereSpell);
        });
      }

      this.colossus = AddSpecial("colossus", S =>
      {
        S.Description = "Being unusually tall and heavy, they move a bit slower but are more resilient.";
        S.Glyph = Glyphs.colossus;

        S.LifeAdvancement.Set(Dice.Zero + 2);
        S.SpeedRateDelta = -0.3F;
        S.WeightMultiplier = 1.50F;
        S.DefenceModifier = Modifier.Plus1;
      });

      this.drunkard = AddSpecial("drunkard", S =>
      {
        S.Description = "The habitually drunk make for brazen yet slightly unsteady adventurers; just don't ask them to recite the alphabet backwards.";
        S.Glyph = Glyphs.drunkard_special;

        S.Startup.SetTalent(Properties.inebriation);
        S.Startup.SetPunishment(Codex.Punishments.thirst);
      });

      this.glass = AddSpecial("glass", S =>
      {
        S.Description = "Sculptured from living glass that reflects energy and makes a perfect but delicate emulation of their natural counterpart.";
        S.Glyph = Glyphs.glass_special;

        S.Diet = Diets.geophagy;
        S.LifeAdvancement.Set(Dice.Zero - 2);
        S.ManaAdvancement.Set(Dice.Zero + 2);
        S.DefenceModifier = Modifier.Minus2;
        S.Startup.SetTalent(Properties.reflection);
        S.Chemistry.SetWeakness(Elements.force);
        S.SetMaskFigure().Set
        (
          Material: Materials.glass,
          Head: true,
          Horns: true,
          Mind: true,
          Voice: true,
          Eyes: true,
          Ears: true,
          Hands: true,
          Limbs: true,
          Feet: true,
          Thermal: false,
          Blood: false,
          Mounted: true,
          Amorphous: true
        );
      });

      this.midget = AddSpecial("midget", S =>
      {
        S.Description = "Being unusually short and slight, they are more nimble but not as effective in combat.";
        S.Glyph = Glyphs.midget;

        S.LifeAdvancement.Set(Dice.Zero - 1);
        S.SpeedRateDelta = +0.3F;
        S.WeightMultiplier = 0.75F;
        S.AttackModifier = Modifier.Minus1;
      });

      this.fugitive = AddSpecial("fugitive", S =>
      {
        S.Description = "Whether innocent or guilty of their accused crimes, these individuals are desperate to escape custody.";
        S.Glyph = Glyphs.fugitive;

        S.Startup.SetPunishment(Codex.Punishments.wanted);
      });

      this.noble = AddSpecial("noble", S =>
      {
        S.Description = "Aristocrats who enjoy all the privilege that comes with wealth and wonder why everyone hates them.";
        S.Glyph = Glyphs.noble;

        S.Startup.SetTalent(Properties.aggravation);
        S.Startup.Loot.AddKit(Chance.Always, 1.d1000() + 1000, Items.gold_coin);
      });

      this.protagonist = AddSpecial("protagonist", S =>
      {
        S.Description = "Main character syndrome has resulted in a reliance on plot armour but they are destined for a major reality check.";
        S.Glyph = Glyphs.protagonist;
      
        S.DefenceModifier = Modifier.Minus1;
        S.Startup.SetAcquisition(Properties.lifesaving);
      });

      this.psychic = AddSpecial("psychic", S =>
      {
        S.Description = "Gifted with extrasensory perception to identify entities otherwise hidden from the normal senses but at the cost of a gnawing hunger.";
        S.Glyph = Glyphs.psychic_special;

        S.Startup.SetTalent(Properties.telepathy, Properties.telekinesis, Properties.clairvoyance, Properties.hunger);
      });

      this.quantum = AddSpecial("quantum", S =>
      {
        S.Description = "Positionally uncertain, these individuals are accustomed to being anywhere and everywhere all at once.";
        S.Glyph = Glyphs.quantum_special;

        S.Startup.SetTalent(Properties.teleportation);
      });

      this.scholar = AddSpecial("scholar", S =>
      {
        S.Description = "Lifetime of study has focused on learning from books at the cost of other hobbies and fitness.";
        S.Glyph = Glyphs.scholar;

        S.LifeAdvancement.Set(Dice.Zero - 1); // -1 per level.
        S.Startup.SetTradeoffSkill(Skills.literacy, SkillCategory.Utility);
        S.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(3), Items.scroll_of_blank_paper);
        S.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(1), Items.book_of_blank_paper);
        S.Startup.Loot.AddKit(Chance.Always, Items.magic_marker);
      });

      this.skeleton = AddSpecial("skeleton", S =>
      {
        S.Description = "Somehow still alive, albeit without the flesh required to be truly living, this peculiar existence has some advantages.";
        S.Glyph = Glyphs.skeleton_special;

        S.Diet = Diets.inediate;
        S.DefenceBias.Bludgeon = Modifier.Minus2;
        S.DefenceBias.Pierce = Modifier.Plus2;
        S.SpeedRateDelta = -0.5F;
        S.WeightMultiplier = 0.30F;
        S.SetMaskFigure().Set
        (
          Material: Materials.bone,
          Head: true,
          Horns: true,
          Mind: true,
          Voice: true,
          Eyes: true,
          Ears: false,
          Hands: true,
          Limbs: true,
          Feet: true,
          Thermal: false,
          Blood: false,
          Mounted: true,
          Amorphous: true
        );
        S.LifeAdvancement.Set(Dice.Zero - 1); // -1 per level.
        S.ManaAdvancement.Set(Dice.Zero - 1); // -1 per level.
        S.Startup.SetTalent(Properties.vitality);
        S.Startup.SetResistance(Elements.poison);
        S.Startup.Loot.AddKit(Chance.Always, Items.brass_bugle); // doot doot.
      });

      this.vampire = AddSpecial("vampire", S =>
      {
        S.Description = "Forsaken creature that subsists by feeding on the vital essence of the living.";
        S.Glyph = Glyphs.vampire_special;

        S.Diet = Diets.hematophagy;
        S.Chemistry.SetVulnerability(Materials.silver);

        S.SetMaskFigure().Set
        (
          Material: null,
          Head: true,
          Horns: true,
          Mind: true,
          Voice: true,
          Eyes: true,
          Ears: true,
          Hands: true,
          Limbs: true,
          Feet: true,
          Thermal: false,
          Blood: true,
          Mounted: true,
          Amorphous: true
        );

        S.Startup.SetTalent(Properties.dark_vision, Properties.slow_digestion);
        S.Startup.SetResistance(Elements.sleep);

        S.SetTransformations(Entities.vampire_bat, Entities.fog_cloud);
      });

      this.frost = AddElemental("frost", Glyphs.frost_special, Elements.cold, Elements.fire, Spells.freezing_sphere);
      this.flame = AddElemental("flame", Glyphs.flame_special, Elements.fire, Elements.cold, Spells.flaming_sphere);
      this.shock = AddElemental("shock", Glyphs.shock_special, Elements.shock, Elements.drain, Spells.shocking_sphere);
      this.earth = AddElemental("earth", Glyphs.earth_special, Elements.petrify, Elements.disintegrate, Spells.crushing_sphere);
      this.water = AddElemental("water", Glyphs.water_special, Elements.acid, Elements.shock, Spells.soaking_sphere);

      // lycanthrope: wolf, jackal, rat, panther, snaker, spider, tiger, wolf.
      //this.werejackal = AddLycanthrope(Entities.jackal, Entities.jackalwere, Entities.werejackal);
      //this.wererat = AddLycanthrope(Entities.giant_rat, Entities.ratwere, Entities.wererat);
      //this.werepanther = AddLycanthrope(Entities.panther, Entities.pantherwere, Entities.werepanther);
      //this.weresnake = AddLycanthrope(Entities.snake, Entities.snakewere, Entities.weresnake);
      //this.werespider = AddLycanthrope(Entities.giant_spider, Entities.spiderwere, Entities.werespider);
      //this.weretiger = AddLycanthrope(Entities.tiger, Entities.tigerwere, Entities.weretiger);
      this.werewolf = AddLycanthrope(Entities.wolf, Entities.wolfwere, Entities.werewolf);

      // https://docs.google.com/document/d/1ZhMiTDQoG988_1QpnmQ4HicclRRYtXqny9YTREPngCQ/edit
      // purist - class-only skill progression.
      // deaf/blind/mute (massive challenge mode, but would it be fun for _anyone_?)
      // imbued (+mana, -life?)
      // mutant: pig, cat, frog, turtle, rat, bird.
      // shapeshifter (doppelganger power at will)
      // astral (too close to Echo?)
      // zealot: +karma, beatitude, -what?
      // >>> GENERATED SPECIALS >>>
      spawn = AddSpecial("spawn", S =>
      {
        S.Description = "Newly turned by an elder vampire's bite, they crave blood but have not yet mastered the strengths of the curse.";
        S.Glyph = Glyphs.temptress;

        S.Diet = Diets.hematophagy;
        S.Chemistry.SetVulnerability(Materials.silver);
        S.Chemistry.SetWeakness(Elements.fire);
        S.LifeAdvancement.Set(Dice.Zero - 1); // -1 per level: the curse has not fully taken hold.
        S.Startup.SetTalent(Properties.dark_vision);
        S.Startup.SetPunishment(Codex.Punishments.thirst);
      });

      warlock = AddSpecial("warlock", S =>
      {
        S.Description = "Bound by an infernal pact to a patron whose power flows through them, whether they will it or not.";
        S.Glyph = Glyphs.Soul_Summoner;

        S.ManaAdvancement.Set(Dice.Zero + 1); // +1 per level: their patron feeds them power.
        S.Startup.SetTradeoffSkill(Skills.conjuration, SkillCategory.Defensive);
        S.Startup.AddGrimoire(Dice.One, Spells.summoning);
        S.Startup.SetPunishment(Codex.Punishments.ball__chain);
      });

      veteran = AddSpecial("veteran", S =>
      {
        S.Description = "Hardened by a lifetime of campaigns, they strike with brutal efficiency but carry old wounds that never fully healed.";
        S.Glyph = Glyphs.army_sergeant;

        S.Startup.SetSkill(Qualifications.expert, Skills.heavy_blade, Skills.heavy_armour);
        S.AttackModifier = Modifier.Plus1;
        S.SpeedRateDelta = -0.2F;
        S.DefenceModifier = Modifier.Minus1;
      });

      specimen = AddSpecial("specimen", S =>
      {
        S.Description = "Escaped from a laboratory before the experiment concluded, their body still bears the unstable mark of what was done to them.";
        S.Glyph = Glyphs.Krull;

        S.ManaAdvancement.Set(Dice.Zero + 1); // +1 per level: residual arcane augmentation.
        S.LifeAdvancement.Set(Dice.Zero - 1); // -1 per level: the procedures took their toll.
        S.Startup.SetTalent(Properties.mana_regeneration);
        S.Startup.SetAffliction(Codex.Afflictions.mutation);
      });

      plagued = AddSpecial("plagued", S =>
      {
        S.Description = "One of the few to survive a great plague, their blood now resists poison but their body was left frail and their nearby creatures keep their distance.";
        S.Glyph = Glyphs.hag;

        S.LifeAdvancement.Set(Dice.Zero - 1); // -1 per level: never fully recovered.
        S.Startup.SetResistance(Elements.poison);
        S.Chemistry.SetWeakness(Elements.necrotic);
        S.Startup.SetPunishment(Codex.Punishments.shunning);
      });

      ascetic = AddSpecial("ascetic", S =>
      {
        S.Description = "Trained since childhood in a monastic order, their body is a honed weapon but their vows forbid armour and forsake worldly learning.";
        S.Glyph = Glyphs.shaolin_monk;

        S.Startup.SetSkill(Qualifications.expert, Skills.unarmed_combat);
        S.Startup.SetTalent(Properties.free_action);
        S.DefenceModifier = Modifier.Minus1;
        S.Startup.SetPunishment(Codex.Punishments.illiteracy);
      });

      feral = AddSpecial("feral", S =>
      {
        S.Description = "Raised outside civilisation among beasts, they are quick and keen-eyed but never learned the discipline of formal combat.";
        S.Glyph = Glyphs.male_savage;

        S.Startup.SetTalent(Properties.dark_vision);
        S.SpeedRateDelta = +0.15F;
        S.DefenceModifier = Modifier.Minus1;
        S.Startup.SetPunishment(Codex.Punishments.ignoramus);
      });

      prophet = AddSpecial("prophet", S =>
      {
        S.Description = "Cursed with visions of what is to come, they see danger before it arrives, but the visions are slowly undoing their mind.";
        S.Glyph = Glyphs.elder_wizard;

        S.Startup.SetTalent(Properties.clairvoyance, Properties.warning);
        S.Startup.SetPunishment(Codex.Punishments.psychosis);
      });

      construct = AddSpecial("construct", S =>
      {
        S.Description = "Touched by the animating art of golem-crafters, their flesh has hardened toward stone, granting resilience at the cost of grace.";
        S.Glyph = Glyphs.General_Breetai;

        S.Diet = Diets.lithivore;
        S.SpeedRateDelta = -0.3F;
        S.WeightMultiplier = 1.6F;
        S.DefenceModifier = Modifier.Plus1;
        S.Chemistry.SetWeakness(Elements.disintegrate);
        S.SetMaskFigure().Set
        (
          Material: Materials.stone,
          Head: true,
          Horns: false,
          Mind: true,
          Voice: true,
          Eyes: true,
          Ears: true,
          Hands: true,
          Limbs: true,
          Feet: true,
          Thermal: false,
          Blood: false,
          Mounted: true,
          Amorphous: false
        );
      });

      fey = AddSpecial("fey", S =>
      {
        S.Description = "Changeling blood runs through their veins, granting them an otherworldly grace, but cold iron burns them like acid.";
        S.Glyph = Glyphs.Nymph_Princess;

        S.WeightMultiplier = 0.85F;
        S.SpeedRateDelta = +0.2F;
        S.DefenceModifier = Modifier.Minus1;
        S.Startup.SetTalent(Properties.stealth, Properties.jumping);
        S.Chemistry.SetVulnerability(Materials.iron);
      });

      drowned = AddSpecial("drowned", S =>
      {
        S.Description = "Pulled under by the sea and cursed to return, their lungs have made an uneasy peace with water while their waterlogged flesh dries out on land.";
        S.Glyph = Glyphs.Merdude;

        S.Startup.SetSkill(Qualifications.expert, Skills.swimming);
        S.Startup.SetResistance(Elements.water);
        S.Chemistry.SetWeakness(Elements.fire);
        S.SpeedRateDelta = -0.15F;
      });

      duelist = AddSpecial("duelist", S =>
      {
        S.Description = "Trained in the one-on-one blade duel, every strike is precise, but a lifetime spent perfecting offence left little time to master defence.";
        S.Glyph = Glyphs.bushi;

        S.Startup.SetSkill(Qualifications.expert, Skills.light_blade);
        S.AttackModifier = Modifier.Plus1;
        S.DefenceModifier = Modifier.Minus1;
        S.LifeAdvancement.Set(Dice.Zero - 1); // -1 per level: a lean frame built for speed, not endurance.
      });

      tamer = AddSpecial("tamer", S =>
      {
        S.Description = "More at home among beasts than people, they ride and bond with animals expertly but rely on their companions more than their own blade.";
        S.Glyph = Glyphs.huntress;

        S.Startup.SetSkill(Qualifications.expert, Skills.riding);
        S.AttackModifier = Modifier.Minus1;
        S.Startup.Loot.AddKit(Chance.Always, Items.saddle);
        S.Startup.Loot.AddKit(Chance.Always, Items.scroll_of_taming);
      });

      graverobber = AddSpecial("graverobber", S =>
      {
        S.Description = "Made a living prying open tombs that were meant to stay shut, and picked up a rare set of skills along the way; the dead have not forgiven them.";
        S.Glyph = Glyphs.thief;

        S.Startup.SetSkill(Qualifications.expert, Skills.locks);
        S.Startup.Loot.AddKit(Chance.Always, Items.lock_pick);
        S.Startup.SetPunishment(Codex.Punishments.wanted);
      });
      // <<< GENERATED SPECIALS <<<
    }
#endif

    // >>> GENERATED SPECIALS-FIELDS >>>
    public readonly Special spawn;
    public readonly Special warlock;
    public readonly Special veteran;
    public readonly Special specimen;
    public readonly Special plagued;
    public readonly Special ascetic;
    public readonly Special feral;
    public readonly Special prophet;
    public readonly Special construct;
    public readonly Special fey;
    public readonly Special drowned;
    public readonly Special duelist;
    public readonly Special tamer;
    public readonly Special graverobber;
    // <<< GENERATED SPECIALS-FIELDS <<<
    public readonly Special colossus;
    public readonly Special drunkard;
    public readonly Special fugitive;
    public readonly Special glass;
    public readonly Special midget;
    public readonly Special noble;
    public readonly Special protagonist;
    public readonly Special psychic;
    public readonly Special quantum;
    public readonly Special scholar;
    public readonly Special skeleton;
    public readonly Special vampire;
    public readonly Special frost;
    public readonly Special flame;
    public readonly Special shock;
    public readonly Special earth;
    public readonly Special water;
    //public readonly Special werejackal;
    //public readonly Special werepanther;
    //public readonly Special wererat;
    //public readonly Special weresnake;
    //public readonly Special werespider;
    //public readonly Special weretiger;
    public readonly Special werewolf;
    //public readonly Special zealot;
  }
}
