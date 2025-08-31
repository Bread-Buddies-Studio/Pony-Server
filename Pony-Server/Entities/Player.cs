// Directory //
namespace ServerSpace.Entities;
// Imports //
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;
// Class //
public class Player : Entity
{
    // Constructor //
    [SetsRequiredMembers]
    public Player(RectangleF Hitbox) : base(Hitbox) {}

    [SetsRequiredMembers]
    public Player() : this(new RectangleF()) {}

    [SetsRequiredMembers]
    public Player(Vector2 Position, SizeF Size) : this(new RectangleF()) {}
    // Base Methods //
}