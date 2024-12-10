// Directory //
namespace Entities;
// Imports //
using System.Drawing;
using System.Numerics;
// Class //
public class Player(RectangleF Hitbox) : Entity(Hitbox)
{
    // Constructor //
    public Player(Vector2 Position, SizeF Size) : this(new RectangleF())
    {
        this.Position = Position;
        this.Size = Size;
    }
    // Base Methods //
}