﻿// Classes and structures being serialized

// Generated by ProtocolBuffer
// - a pure c# code generation implementation of protocol buffers
// Report bugs to: https://silentorbit.com/protobuf/

// DO NOT EDIT
// This file will be overwritten when CodeGenerator is run.
// To make custom modifications, edit the .proto file and add //:external before the message line
// then write the code and the changes in a separate file.
using System;
using System.Collections.Generic;

namespace APIMessage
{
    public partial class UserInfo
    {
        public string Email { get; set; }

        public string Pass { get; set; }

        public int Cash { get; set; }

    }

    public partial class UserCharacter
    {
        public string Name { get; set; }

        public string UserEmail { get; set; }

        public APIMessage.CharacterClassType CharacterClassType { get; set; }

        public int Level { get; set; }

        public int Exp { get; set; }

        public int Gold { get; set; }

        public int SkillPt { get; set; }

        public int StatsPt { get; set; }

        public int StrPt { get; set; }

        public int AgiPt { get; set; }

        public int IntPt { get; set; }

        public int StaPt { get; set; }

    }

    public partial class MasterCharacterClass
    {
        public string Id { get; set; }

        public APIMessage.CharacterClassType CharacterClassType { get; set; }

        public int HpBase { get; set; }

        public int SpBase { get; set; }

        public int AtkBase { get; set; }

        public int DefBase { get; set; }

        public float AtkSpdBase { get; set; }

        public float MoveSpdBase { get; set; }

        public int StrBase { get; set; }

        public int IntBase { get; set; }

        public int StaBase { get; set; }

        public int AgiBase { get; set; }

        public int DodgeBase { get; set; }

    }

    public partial class Skill
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string IconId { get; set; }

        public int CastTime { get; set; }

        public int CoolDown { get; set; }

        public int Cost { get; set; }

        public int MinDamage { get; set; }

        public int MaxDamage { get; set; }

        public int SkillFactor { get; set; }

        public APIMessage.DamageType DamageType { get; set; }

        public string Description { get; set; }

        public string SkillType { get; set; }

    }

    public partial class MasterItem
    {
        public string Id { get; set; }

        public string IconId { get; set; }

        public string ModelId { get; set; }

        public APIMessage.ItemType ItemType { get; set; }

    }

    public partial class Resource
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public APIMessage.ResourceType ResourceType { get; set; }

    }

    public partial class Inventory
    {
        public string Id { get; set; }

    }

    public partial class SlotInventory
    {
        public string Id { get; set; }

        public string InventoryId { get; set; }

        public string ItemId { get; set; }

        public int Amount { get; set; }

    }

    public enum CharacterClassType
    {
        WARRIOR = 0,
        MAGE = 1,
        HUNTER = 2,
    }


    public enum ResourceType
    {
        SOUNDS = 0,
        IMAGE = 1,
        MODEL3D = 2,
    }


    public enum ItemType
    {
        GEM = 0,
        SWORD = 1,
        ARMOUR = 2,
        SHOE = 3,
        RING = 4,
        GLOVE = 5,
        HELMET = 6,
        RECOVERYHP = 7,
        RECOVERYMANA = 8,
    }


    public enum DamageType
    {
        PHYSICS = 0,
        MAGIC = 1,
    }


    public enum SkillType
    {
        ACTIVE = 0,
        PASSIVE = 1,
    }


}
