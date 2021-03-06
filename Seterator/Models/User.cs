﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Seterator.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("GUID")]
        public Guid Guid { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("password_hash", TypeName = "BINARY(24)")]
        [JsonConverter(typeof(ReadOnlyCollectionConverter<byte>))]
        public IReadOnlyCollection<byte> PasswordHash { get; set; }

        public virtual List<Participant> Participants { get; set; }

        public virtual List<Role> Roles { get; set; }

        public User()
        {
            Guid = Guid.NewGuid();
        }
        private class ReadOnlyCollectionConverter<TItem> : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(IReadOnlyCollection<TItem>);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return serializer.Deserialize<TItem[]>(reader) as IReadOnlyCollection<TItem>;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, (value as IReadOnlyCollection<TItem>).ToArray());
            }
        }
    }
    
    public enum UserKind
    {
        Common,
        Jury,
        Moderator
    }
}
