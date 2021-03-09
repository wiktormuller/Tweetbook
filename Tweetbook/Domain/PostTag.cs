using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tweetbook.Domain
{
    public class PostTag
    {
        public Post Post { get; set; }
        public Guid PostId { get; set; }
        public string TagName { get; set; }
        [ForeignKey(nameof(TagName))]
        public Tag Tag { get; set; }
    }
}