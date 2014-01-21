using System;
using System.IO;

namespace LibDat.Files
{
	public class ShopItem : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[UserStringIndex]
		public int Name { get; set; }
		[UserStringIndex]
		public int Description { get; set; }
		public int Data0Length { get; set; }
		[UInt32Index]
		public int Data0 { get; set; }
		public bool Flag0 { get; set; }
		public int Data1Length { get; set; }
		[UInt32Index]
		public int Data1 { get; set; }
		public int Price { get; set; }
		public int Data2Length { get; set; }
		[UInt64Index]
		public int Data2 { get; set; }
		[StringIndex]
		public int ArtPath { get; set; }
		public Int64 Unknown11 { get; set; }
		[StringIndex]
		public int YoutubeVideo { get; set; }
		[StringIndex]
		public int LargeArtPath { get; set; }
		public int Unknown15 { get; set; }
		[StringIndex]
		public int DailyDealArtPath { get; set; }


		public ShopItem(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Name = inStream.ReadInt32();
			Description = inStream.ReadInt32();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Data1Length = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			Price = inStream.ReadInt32();
			Data2Length = inStream.ReadInt32();
			Data2 = inStream.ReadInt32();
			ArtPath = inStream.ReadInt32();
			Unknown11 = inStream.ReadInt64();
			YoutubeVideo = inStream.ReadInt32();
			LargeArtPath = inStream.ReadInt32();
			Unknown15 = inStream.ReadInt32();
			DailyDealArtPath = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Name);
			outStream.Write(Description);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Flag0);
			outStream.Write(Data1Length);
			outStream.Write(Data1);
			outStream.Write(Price);
			outStream.Write(Data2Length);
			outStream.Write(Data2);
			outStream.Write(ArtPath);
			outStream.Write(Unknown11);
			outStream.Write(YoutubeVideo);
			outStream.Write(LargeArtPath);
			outStream.Write(Unknown15);
			outStream.Write(DailyDealArtPath);
		}

		public override int GetSize()
		{
			return 0x45;
		}
	}
}