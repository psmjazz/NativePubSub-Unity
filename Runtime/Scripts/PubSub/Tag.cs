using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

namespace PJ.Native.PubSub
{
    public partial struct Tag 
    {

        private string name;
        private ulong id;
        private List<string> names;
        public string Name => name;

        private Tag(string name)
        {
            this.name = name;
            this.names = null;
            this.id = TagGenerator.Generate();
        }

        private Tag(ulong id)
        {
            this.name = id.ToString();
            this.names = null;
            this.id = id;
        }

        private Tag(string name, ulong id)
        {
            this.name = name;
            this.names = null;
            this.id = id;
        }

        public static Tag Named(string name)
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
        
        private static Tag NamedWithID(string name, ulong id)
        {
            if(tagDic.ContainsKey(id))
            {
                Tag tag = tagDic[id];
                tag.name = name;
                return tag;
            }
            else
            {
                var tag = new Tag(name, id);
                namedTagDic.Add(name, tag);
                tagDic.Add(id, tag);
                return tag;
            }
        }

        public static Tag Named(IEnumerable<string> names)
        {
            ulong joined = 0b0;
            foreach(var name in names)
            {
                Tag named = Named(name);
                joined |= named.id;
            }
            if(tagDic.ContainsKey(joined))
                return tagDic[joined];
            else
            {
                Tag tag = new Tag(joined);
                tagDic.Add(joined, tag);
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

        public Tag Unjoin(Tag tag2)
        {
            ulong unjoined = this.id & ~tag2.id;
            if(tagDic.ContainsKey(unjoined))
            {
                return tagDic[unjoined];
            }
            else
            {
                var tag = new Tag(unjoined);
                tagDic.Add(unjoined, tag);
                return tag;
            }
        }

        private bool Contains(ulong id)
        {
            return (this.id & id) == id;
        }

        public Tag Join(Tag tag)
        {
            return Tag.Join(this, tag);
        }
        
        public bool Contains(Tag tag)
        {
            return Contains(tag.id);
            // return (id & tag.id) == tag.id;
        }

        public bool Except(Tag tag)
        {
            return (id & tag.id) == 0;
        }

        public List<string> Names
        {
            get
            {
                if(names != null)
                    return names;

                names = new List<string>();
                for(ulong findID = 0b1; findID != 0b0; findID <<= 1)
                {
                    if(this.Contains(findID) && tagDic.TryGetValue(findID, out Tag findTag))
                    {
                        names.Add(name);
                    }
                }
                return names;
            }
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
        public static Tag None = Tag.NamedWithID("None", 0b0);
        public static Tag Native = Tag.Named("Native");
        public static Tag Game = Tag.Named("Game");
        
    }
}
