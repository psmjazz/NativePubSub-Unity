using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

namespace PJ.Native.Messenger
{
    public partial struct Tag 
    {

        private string name;
        private ulong id;
        public string Name => name;

        private Tag(string name)
        {
            this.name = name;
            this.id = TagGenerator.Generate();
        }

        private Tag(ulong id)
        {
            this.name = id.ToString();
            this.id = id;
        }

        private Tag(string name, ulong id)
        {
            this.name = name;
            this.id = id;
        }

        public static Tag Create(string name)
        {
            if (namedTagDic.ContainsKey(name))
            {
                return namedTagDic[name];
            }
            else
            {
                var tag = new Tag(name);
                namedTagDic.Add(name, tag);
                tagDic.Add(tag.id, tag);
                return tag;
            }
        }

        public static Tag Join(Tag tag1, Tag tag2)
        {
            ulong joined = tag1.id | tag2.id;
            if(tagDic.ContainsKey(joined))
                return tagDic[joined];
            else
            {
                var tag = new Tag(joined);
                tagDic.Add(joined, tag);
                return tag;
            }
        }

        public static Tag Join(params Tag[] tags)
        {
            ulong joined = 0b0;
            foreach (var tag in tags)
            {
                joined |= tag.id;
            }
            if(tagDic.ContainsKey(joined))
                return tagDic[joined];
            else
            {
                var tag = new Tag(joined);
                tagDic.Add(joined, tag);
                return tag;
            }
        }
        
        public bool Contains(Tag tag)
        {
            return (id & tag.id) == tag.id;
        }

        public bool Except(Tag tag)
        {
            return (id & tag.id) == 0;
        }
    }

    public partial struct Tag
    {
        private static class TagGenerator
        {
            private static ulong id = 0b1;
            public static ulong Generate()
            {
                var result = id;
                id<<=1;
                return result;
            }
        }

        private static Dictionary<ulong, Tag> tagDic = new Dictionary<ulong, Tag>();
        private static Dictionary<string, Tag> namedTagDic = new Dictionary<string, Tag>();
    }

    public partial struct Tag : IEquatable<Tag>
    {
        public bool Equals(Tag other)
        {
            return id == other.id;
        }
    }

    public partial struct Tag
    {
        public static Tag None = new Tag("None", 0b0);
        public static Tag Native = new Tag("Native");
        public static Tag Game = new Tag("Game");
        
    }
}
