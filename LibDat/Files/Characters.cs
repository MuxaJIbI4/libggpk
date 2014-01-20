using System;
using System.IO;

namespace LibDat.Files
{
	public class Characters : BaseDat
	{
		[StringIndex]
		public int OtFile { get; set; }
		[StringIndex]
		public int Name { get; set; }
		[StringIndex]
		public int AnimatedObject { get; set; }
		[StringIndex]
		public int Actor { get; set; }
		public int BaseMaxLife { get; set; }
		public int BaseMaxMana { get; set; }
		public int WeaponSpeed { get; set; }
		public int MinDamage { get; set; }
		public int MaxDamage { get; set; }
		public int MaxAttackDistance { get; set; } // possibly wrong
		[StringIndex]
		public int Icon { get; set; }
		public int Unknown6 { get; set; }
		public int BaseStrength { get; set; }
		public int BaseDexterity { get; set; }
		public int BaseIntelligence { get; set; }
		public int Data0Length { get; set; }
		[DataIndex]
		public int Data0 { get; set; }
		[UserStringIndex]
		public int Description { get; set; }
		public int Unknown11 { get; set; }
		public int Unknown12 { get; set; }
		public int Unknown13 { get; set; }
		public int Unknown14 { get; set; }
		public int Unknown15 { get; set; }
		public int Unknown16 { get; set; }
        public int Unknown17 { get; set; }
        [StringIndex]
        public int Unknown18 { get; set; }

		public Characters(BinaryReader inStream)
		{
			OtFile = inStream.ReadInt32();
			Name = inStream.ReadInt32();
			AnimatedObject = inStream.ReadInt32();
			Actor = inStream.ReadInt32();
			BaseMaxLife = inStream.ReadInt32();
			BaseMaxMana = inStream.ReadInt32();
			WeaponSpeed = inStream.ReadInt32();
			MinDamage = inStream.ReadInt32();
			MaxDamage = inStream.ReadInt32();
			MaxAttackDistance = inStream.ReadInt32();
			Icon = inStream.ReadInt32();
			Unknown6 = inStream.ReadInt32();
			BaseStrength = inStream.ReadInt32();
			BaseDexterity = inStream.ReadInt32();
			BaseIntelligence = inStream.ReadInt32();
            Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Description = inStream.ReadInt32();
			Unknown11 = inStream.ReadInt32();
			Unknown12 = inStream.ReadInt32();
			Unknown13 = inStream.ReadInt32();
			Unknown14 = inStream.ReadInt32();
			Unknown15 = inStream.ReadInt32();
			Unknown16 = inStream.ReadInt32();
            Unknown17 = inStream.ReadInt32();
            Unknown18 = inStream.ReadInt32();
		}

		public override void Save(System.IO.BinaryWriter outStream)
		{
			outStream.Write(OtFile);
			outStream.Write(Name);
			outStream.Write(AnimatedObject);
			outStream.Write(Actor);
			outStream.Write(BaseMaxLife);
			outStream.Write(BaseMaxMana);
			outStream.Write(WeaponSpeed);
			outStream.Write(MinDamage);
			outStream.Write(MaxDamage);
			outStream.Write(MaxAttackDistance);
			outStream.Write(Icon);
			outStream.Write(Unknown6);
			outStream.Write(BaseStrength);
			outStream.Write(BaseDexterity);
			outStream.Write(BaseIntelligence);
            outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Description);
			outStream.Write(Unknown11);
			outStream.Write(Unknown12);
			outStream.Write(Unknown13);
			outStream.Write(Unknown14);
			outStream.Write(Unknown15);
			outStream.Write(Unknown16);
            outStream.Write(Unknown17);
            outStream.Write(Unknown18);
		}

		public override int GetSize()
		{
			return 0x68;
		}
	}
}
