// Directory //
namespace Rendering.Entities;
// Imports //
using System.Collections;
using System.Numerics;
using System.Reflection.Metadata;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using LiteNetLib.Utils;
using Rendering.Visible;
using Core.Extensions;
using Avalonia;
// Enums //
public enum EntityStates
{
    Walking,
    Idle,
    Chatting,
}
// Classes //
public abstract class Entity(Rect Hitbox) : Renderable(Hitbox), INetSerializable
{
    // Static Data //
    public static readonly HashSet<Entity> Entities = [];
    // Instance Data //
    public string Name = string.Empty;
    public EntityStates State = EntityStates.Idle;
    //public readonly List<OverheadText> OverheadTexts = [];
    public bool Direction { get; set; }

    #region Network Methods
    public virtual void Serialize(NetDataWriter writer)
    {
        writer.Put(Name);
        writer.Put((byte)State);
        writer.Put(Direction);
        writer.PutArray(Position.ToDoubleArray());
        writer.PutArray(Size.ToDoubleArray());
    }
    public virtual void Deserialize(NetDataReader reader)
    {
        Name = reader.GetString();
        State = (EntityStates)reader.GetByte();
        Direction = reader.GetBool();
        Position = PointExtensions.FromDoubleArray(reader.GetDoubleArray());
        Size = SizeExtensions.FromDoubleArray(reader.GetDoubleArray());
    }
    #endregion
    #region Event Methods
    public abstract void OnUpdate(long deltaTime);
    #endregion
}