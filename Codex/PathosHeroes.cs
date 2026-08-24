using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexHeroes : CodexPage<ManifestHeroes, HeroEditor, Hero>
  {
    private CodexHeroes() { }
#if MASTER_CODEX
    internal CodexHeroes(Codex Codex)
      : base(Codex.Manifest.Heroes)
    {
      var Genders = Codex.Genders;
      var Classes = Codex.Classes;
      var Entities = Codex.Entities;
      var Specials = Codex.Specials;
      var Glyphs = Codex.Glyphs;

      Hero AddHero(string Name, Gender Gender, Entity Entity, Class Class, Action<HeroEditor> EditorAction)
      {
        return Register.Add(H =>
        {
          H.Name = Name;
          H.Gender = Gender;
          H.Entity = Entity;
          H.Class = Class;
          H.Special = null;
          H.CustomGlyph = null;

          EditorAction(H);
        });
      }

      OrcBarbarian = AddHero("Grytt", Genders.male, Entities.orc, Classes.barbarian, H =>
      {
        H.Pet = H.NewPet("Razz", Genders.male, Entities.little_dog);
      });

      HumanCaveman = AddHero("Bok", Genders.male, Entities.human, Classes.caveman, H =>
      {
        H.Pet = H.NewPet("Dug", Genders.male, Entities.little_dog);
      });

      DwarfExplorer = AddHero("Quandon", Genders.female, Entities.dwarf, Classes.explorer, H =>
      {
        H.Pet = H.NewPet("Pinkerton", Genders.female, Entities.kitten);
      });

      GnomeHealer = AddHero("Malasuth", Genders.female, Entities.gnome, Classes.healer, H =>
      {
        H.Pet = H.NewPet("Capo", Genders.male, Entities.kitten);
      });

      HumanKnight = AddHero("Valorn", Genders.male, Entities.human, Classes.knight, H =>
      {
        H.Pet = H.NewPet("Perceval", Genders.male, Entities.pony);
      });

      OrcMonk = AddHero("Eybrinde", Genders.male, Entities.orc, Classes.monk, H =>
      {
        H.Pet = H.NewPet("Talon", Genders.male, Entities.kitten);
      });

      DwarfPriest = AddHero("Chemron", Genders.male, Entities.dwarf, Classes.priest, H =>
      {
        H.Pet = H.NewPet("Gasberg", Genders.female, Entities.kitten);
      });

      HumanRanger = AddHero("Amaya", Genders.female, Entities.human, Classes.ranger, H =>
      {
        H.Pet = H.NewPet("Cheyne", Genders.male, Entities.little_dog);
      });

      ElfRogue = AddHero("Xantari", Genders.female, Entities.elf, Classes.rogue, H =>
      {
        H.Pet = H.NewPet("Adebesi", Genders.female, Entities.little_dog);
      });

      HumanSamurai = AddHero("Rokiju", Genders.male, Entities.human, Classes.samurai, H =>
      {
        H.Pet = H.NewPet("Kira", Genders.female, Entities.little_dog);
      });

      GnomeTourist = AddHero("Pnerfa", Genders.female, Entities.gnome, Classes.tourist, H =>
      {
        H.Pet = H.NewPet("Ninny", Genders.male, Entities.little_dog);
      });

      HumanValkyrie = AddHero("Lagratha", Genders.female, Entities.human, Classes.valkyrie, H =>
      {
        H.Pet = H.NewPet("Floki", Genders.male, Entities.fledgling_raven);
      });

      ElfWizard = AddHero("Shinaas", Genders.male, Entities.elf, Classes.wizard, H =>
      {
        H.Pet = H.NewPet("Poe", Genders.female, Entities.fledgling_raven);
      });
      // >>> GENERATED HEROES >>>
      GiantGladiator = AddHero("Vashti", Genders.female, Entities.giant, Classes.gladiator, H =>
      {
        H.Pet = H.NewPet("Grum", Genders.male, Entities.little_dog);
      });

      SatyrBard = AddHero("Panderos", Genders.male, Entities.satyr, Classes.bard, H =>
      {
        H.Pet = H.NewPet("Lyric", Genders.female, Entities.fledgling_raven);
      });

      DemonConvict = AddHero("Malgrim", Genders.male, Entities.demon, Classes.convict, H =>
      {
        H.Pet = H.NewPet("Scraps", Genders.male, Entities.kitten);
      });

      DwarfMiner = AddHero("Borgrim", Genders.male, Entities.dwarf, Classes.miner, H =>
      {
        H.Pet = H.NewPet("Nugget", Genders.female, Entities.kitten);
      });

      KoboldGunslinger = AddHero("Skrix", Genders.male, Entities.kobold, Classes.gunslinger, H =>
      {
        H.Pet = H.NewPet("Buckshot", Genders.male, Entities.little_dog);
      });

      GiantHunter = AddHero("Thokk", Genders.male, Entities.giant, Classes.hunter, H =>
      {
        H.Pet = H.NewPet("Briar", Genders.female, Entities.little_dog);
      });

      LizardmanJester = AddHero("Sissik", Genders.female, Entities.lizardman, Classes.jester, H =>
      {
        H.Pet = H.NewPet("Giggles", Genders.male, Entities.kitten);
      });

      FairyMystic = AddHero("Wisp", Genders.female, Entities.fairy, Classes.mystic, H =>
      {
        H.Pet = H.NewPet("Pip", Genders.male, Entities.fledgling_raven);
      });

      AngelNinja = AddHero("Serathiel", Genders.female, Entities.angel, Classes.ninja, H =>
      {
        H.Pet = H.NewPet("Shadow", Genders.female, Entities.kitten);
      });

      OrcPaladin = AddHero("Grakna", Genders.female, Entities.orc, Classes.paladin, H =>
      {
        H.Pet = H.NewPet("Valor", Genders.male, Entities.pony);
      });

      RobotPirate = AddHero("Rustbeard", Genders.male, Entities.robot, Classes.pirate, H =>
      {
        H.Pet = H.NewPet("Gearwing", Genders.female, Entities.fledgling_raven);
      });

      MinotaurTemplar = AddHero("Korrath", Genders.male, Entities.minotaur, Classes.templar, H =>
      {
        H.Pet = H.NewPet("Sentinel", Genders.male, Entities.little_dog);
      });

      TrollReaver = AddHero("Ghrenna", Genders.female, Entities.troll, Classes.reaver, H =>
      {
        H.Pet = H.NewPet("Fangs", Genders.male, Entities.kitten);
      });

      LizardmanShaman = AddHero("Zathrik", Genders.male, Entities.lizardman, Classes.shaman, H =>
      {
        H.Pet = H.NewPet("Bones", Genders.female, Entities.kitten);
      });

      DraconDruid = AddHero("Vaelithra", Genders.female, Entities.dracon, Classes.druid, H =>
      {
        H.Pet = H.NewPet("Fern", Genders.male, Entities.little_dog);
      });

      GnomeTinker = AddHero("Tillie", Genders.female, Entities.gnome, Classes.tinker, H =>
      {
        H.Pet = H.NewPet("Sprocket", Genders.male, Entities.kitten);
      });

      EchoNecromancer = AddHero("Nihlus", Genders.male, Entities.echo, Classes.necromancer, H =>
      {
        H.Pet = H.NewPet("Wraith", Genders.female, Entities.kitten);
      });
      // <<< GENERATED HEROES <<<
    }
#endif

    // >>> GENERATED HEROES-FIELDS >>>
    public readonly Hero GiantGladiator;
    public readonly Hero SatyrBard;
    public readonly Hero DemonConvict;
    public readonly Hero DwarfMiner;
    public readonly Hero KoboldGunslinger;
    public readonly Hero GiantHunter;
    public readonly Hero LizardmanJester;
    public readonly Hero FairyMystic;
    public readonly Hero AngelNinja;
    public readonly Hero OrcPaladin;
    public readonly Hero RobotPirate;
    public readonly Hero MinotaurTemplar;
    public readonly Hero TrollReaver;
    public readonly Hero LizardmanShaman;
    public readonly Hero DraconDruid;
    public readonly Hero GnomeTinker;
    public readonly Hero EchoNecromancer;
    // <<< GENERATED HEROES-FIELDS <<<
    public readonly Hero OrcBarbarian;
    public readonly Hero OrcMonk;
    public readonly Hero DwarfExplorer;
    public readonly Hero DwarfPriest;
    public readonly Hero GnomeHealer;
    public readonly Hero GnomeTourist;
    public readonly Hero HumanCaveman;
    public readonly Hero HumanRanger;
    public readonly Hero HumanKnight;
    public readonly Hero HumanSamurai;
    public readonly Hero HumanValkyrie;
    public readonly Hero ElfRogue;
    public readonly Hero ElfWizard;
  }
}