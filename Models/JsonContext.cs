using System;
using System.Text.Json.Serialization;

namespace iTimeSlot.Models;

[JsonSerializable(typeof(Stats))]
[JsonSerializable(typeof(Settings))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(TimeSpan))]
[JsonSerializable(typeof(TimeSlot))]
internal partial class JsonContext : JsonSerializerContext {}