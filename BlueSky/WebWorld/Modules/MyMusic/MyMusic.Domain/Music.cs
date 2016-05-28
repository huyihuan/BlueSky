using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlueSky.Interfaces;
using BlueSky.Attribute;

namespace WebWorld.Modules.MyMusic.Domain
{
    public class Music : IEntity
    {
        [EntityFieldAttribute(IsPrimaryKey = true)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string MusicName { get; set; }
        public string MusicURL { get; set; }
        public string MusicType { get; set; }
    }
}
