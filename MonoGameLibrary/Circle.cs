using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary;

public readonly struct Circle : IEquatable<Circle>
{
  private static readonly Circle s_empty = new Circle();

  public readonly int X;
  public readonly int Y;
  public readonly int Radius;

  // gets the location of the center of this circle
  public readonly Point Location => new Point(X, Y);

  // tracking empty circles
  public static Circle Empty => s_empty;

  public readonly bool IsEmpty => X == 0 && Y == 0 && Radius == 0;

  // bondaries
  public readonly int Top => Y - Radius;
  public readonly int Bottom => Y + Radius;
  public readonly int Left => X - Radius;
  public readonly int Right => X + Radius;

  // constructor
  public Circle(int x, int y, int radius)
  {
    X = x;
    Y = y;
    Radius = radius;
  }

  public Circle(Point location, int radius)
  {
    X = location.X;
    Y = location.Y;
    Radius = radius;
  }

  // method
  public bool Intersects(Circle other)
  {
    int radiiSquared = (this.Radius + other.Radius) * (this.Radius + other.Radius);
    float distanceSquared = Vector2.DistanceSquared(this.Location.ToVector2(), other.Location.ToVector2());
    return distanceSquared < radiiSquared;
  }

  // implementing interface thing
  public override readonly bool Equals(object obj) => obj is Circle other && Equals(other);

  public readonly bool Equals(Circle other) => this.X == other.X && this.Y == other.Y && this.Radius == other.Radius;
  

  // get hashcode
  public override readonly int GetHashCode() => HashCode.Combine(X, Y, Radius);

  // following operator
  public static bool operator ==(Circle lhs, Circle rhs) => lhs.Equals(rhs);

  public static bool operator !=(Circle lhs, Circle rhs) => !lhs.Equals(rhs);
}
