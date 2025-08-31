// Directory //
namespace Rendering.Entities;
// Imports //
using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Avalonia;
using Avalonia.Media;

// Class //
public class Player(Rect Hitbox) : Entity(Hitbox)
{
    #region Constructors
    [SetsRequiredMembers]
    public Player() : this(new Rect()) { }

    [SetsRequiredMembers]
    public Player(Point Position, Size Size) : this(new Rect(Position, Size)) { }
    #endregion
    #region Base Methods
    public override void OnUpdate(long deltaTime)
    {

    }
    // Base Methods //
    #endregion
}