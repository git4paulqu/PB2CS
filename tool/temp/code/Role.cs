//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//-----------------------
using ProtoBuf;
using System.Collections.Generic;

namespace PB
{
	[ProtoContract]
	public class Role
	{
		public Role()
		{
			equips = new System.Collections.Generic.List<uint>();
			itemTest = new System.Collections.Generic.List<AwardDescInBox>();
		}

		[ProtoMember(1)]
		public uint uid;
		[ProtoMember(2)]
		public uint id;
		[ProtoMember(3)]
		public uint pos;
		[ProtoMember(4)]
		public uint group;
		[ProtoMember(5)]
		public System.Collections.Generic.List<uint> equips;
		[ProtoMember(6)]
		public EnumTest test;
		[ProtoMember(7)]
		public System.Collections.Generic.List<AwardDescInBox> itemTest;
		[ProtoMember(8)]
		public uint testrequired = 100;
		[ProtoMember(9)]
		public bool testrequired1 = false;
	}
}
