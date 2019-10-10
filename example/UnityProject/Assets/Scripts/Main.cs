//=====================================================
// - FileName:      Main.cs
// - Created:       #AuthorName#
// - UserName:      #CreateTime#
// - Email:         #AuthorEmail#
// - Description:   
// - Copyright © 2018 Qu Tong. All rights reserved.
//======================================================
using UnityEngine;
using PB;
using System.IO;

public class Main : MonoBehaviour {

	void Start () {

        PB.Role role = new PB.Role();
        role.id = 999;
        role.test = EnumTest.Role;

        byte[] data = null;
        using (MemoryStream ms = new MemoryStream())
        {
            ProtoBuf.Serializer.Serialize<Role>(ms, role);
            data = ms.ToArray();
        }

        Role role2 = null;
        using (MemoryStream ms = new MemoryStream(data))
        {
            role2 = ProtoBuf.Serializer.Deserialize<Role>(ms);
        }
        Debug.LogErrorFormat("id:{0}, enum:{1}", role2.id, role2.test);
    }
}
