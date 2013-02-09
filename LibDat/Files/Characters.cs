using System;
using System.IO;

namespace LibDat.Files
{
	class Characters : BaseDat
	{
		[StringIndex]
		public int OtFile;
		[StringIndex]
		public int Name;
		[StringIndex]
		public int AnimatedObject;
		[StringIndex]
		public int Actor;
		public int BaseMaxLife;
		public int BaseMaxMana;
		public int WeaponSpeed;
		public int MinDamage;
		public int MaxDamage;
		public int MaxAttackDistance; // possibly wrong
		[StringIndex]
		public int Icon;
		public int Unknown6;
		public int BaseStrength;
		public int BaseDexterity;
		public int BaseIntelligence;
		public int Unknown10;
		[DataIndex]
		public int Data0;
		[StringIndex]
		public int Description;
		public int Unknown11;
		public int Unknown12;
		public int Unknown13;
		public int Unknown14;
		public int Unknown15;
		public int Unknown16;
		public int Unknown17;

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
			Unknown10 = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Description = inStream.ReadInt32();
			Unknown11 = inStream.ReadInt32();
			Unknown12 = inStream.ReadInt32();
			Unknown13 = inStream.ReadInt32();
			Unknown14 = inStream.ReadInt32();
			Unknown15 = inStream.ReadInt32();
			Unknown16 = inStream.ReadInt32();
			Unknown17 = inStream.ReadInt32();
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
			outStream.Write(Unknown10);
			outStream.Write(Data0);
			outStream.Write(Description);
			outStream.Write(Unknown11);
			outStream.Write(Unknown12);
			outStream.Write(Unknown13);
			outStream.Write(Unknown14);
			outStream.Write(Unknown15);
			outStream.Write(Unknown16);
			outStream.Write(Unknown17);

		}

		public override int GetSize()
		{
			throw new NotImplementedException();
		}
	}
}
