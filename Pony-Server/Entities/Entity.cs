// Directory //
namespace ServerSpace.Entities;
// Imports //
using ServerSpace.Services;
using System.Collections;
using System.Drawing;
using System.Numerics;
using System.Reflection.Metadata;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

// Enums //
public enum EntityStates
{
    Walking,
    Idle,
    Chatting,
}
// Classes //
public abstract class Entity
{
    // Static Data //
    public static readonly List<Entity> Entities = new();
    // Instance Data //
    public required float Speed = 5;
    public required RectangleF Hitbox;
    public required Vector2 Position 
    {
        get => (Vector2)Hitbox.Location;
        set => Hitbox.Location = (PointF)value;
    }
    public required SizeF Size
    {
        get => Hitbox.Size;
        set => Hitbox.Size = value;
    }
    public required bool Direction = true;
    public required Color DialogueColor = Color.White;
    private bool animRunning = false;
    // Constructors // 
    [SetsRequiredMembers]
    public Entity(RectangleF Hitbox) : base()
    {
        // Settings //
        this.Hitbox = Hitbox;
        // Add Reference //
        Entities.Add(this);
    }

    [SetsRequiredMembers]
    public Entity() : this(new RectangleF()) {}

    [SetsRequiredMembers]
    public Entity(Vector2 Position, SizeF Size) : this()
    {
        this.Position = Position;
        this.Size = Size;
    }
    // Movement Methods //
    public Vector2 MoveTo(Vector2 Position)
    {
        // Settings //
        this.Position = Position;
        // Return New Position //
        return Position;
    }
    public Vector2 MoveTo(float x, float y) => MoveTo(new Vector2(x, y));

    public Vector2 Translate(Vector2 Offset)
    {
        // Settings //
        this.Position += Offset;
        // Return New Position //
        return Position;
    }
    public Vector2 Translate(float x, float y) => Translate(new Vector2(x, y));
}