using System;
using System.Drawing;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Particle
    {
        private const double Infinity = double.PositiveInfinity;

        /// <summary>
        /// Position
        /// </summary>
        private double _rx, _ry;

        /// <summary>
        /// Velocity
        /// </summary>
        private double _vx, _vy;

        /// <summary>
        /// Radius
        /// </summary>
        private readonly double _radius;

        /// <summary>
        /// Mass
        /// </summary>
        private readonly double _mass;

        /// <summary>
        /// Color
        /// </summary>
        private readonly Color _color;

        /// <summary>
        /// Number of collisions so far
        /// </summary>
        private int _count;

        /// <summary>
        /// Create a new particle with given parameters
        /// </summary>
        /// <param name="rx"></param>
        /// <param name="ry"></param>
        /// <param name="vx"></param>
        /// <param name="vy"></param>
        /// <param name="radius"></param>
        /// <param name="mass"></param>
        /// <param name="color"></param>
        public Particle(double rx, double ry, double vx, double vy, double radius, double mass, Color color)
        {
            _vx = vx;
            _vy = vy;
            _rx = rx;
            _ry = ry;
            _radius = radius;
            _mass = mass;
            _color = color;
        }

        /// <summary>
        /// Create a random particle in the unit box (overlaps not checked)
        /// </summary>
        public Particle()
        {
            Random random = new Random();
            _rx = random.NextDouble();
            _ry = random.NextDouble();
            _vx = 0.01 * (random.NextDouble() - 0.5);
            _vy = 0.01 * (random.NextDouble() - 0.5);
            _radius = 0.01;
            _mass = 0.5;
            _color = Color.Black;
        }

        /// <summary>
        /// Updates position
        /// </summary>
        /// <param name="dt"></param>
        public void Move(double dt)
        {
            _rx += _vx * dt;
            _ry += _vy * dt;
        }

        /// <summary>
        /// Draw the particle
        /// </summary>
        public void Draw()
        {
            StdDraw.SetPenColor(_color);
            StdDraw.FilledCircle((float)_rx, (float)_ry, (float)_radius);
        }

        /// <summary>
        /// Return the number of collisions involving this particle
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return _count;
        }

        /// <summary>
        /// How long into future until collision between this particle a and b?
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public double TimeToHit(Particle b)
        {
            Particle a = this;
            if (a == b)
            {
                return Infinity;
            }
            double dx = b._rx - a._rx;
            double dy = b._ry - a._ry;
            double dvx = b._vx - a._vx;
            double dvy = b._vy - a._vy;
            double dvdr = dx * dvx + dy * dvy;
            if (dvdr > 0)
            {
                return Infinity;
            }
            double dvdv = dvx * dvx + dvy * dvy;
            double drdr = dx * dx + dy * dy;
            double sigma = a._radius + b._radius;
            double d = (dvdr * dvdr) - dvdv * (drdr - sigma * sigma);
            // if (drdr < sigma*sigma) StdOut.println("overlapping particles");
            if (d < 0)
            {
                return Infinity;
            }
            return -(dvdr + Math.Sqrt(d)) / dvdv;
        }

        /// <summary>
        /// How long into future until this particle collides with a vertical wall?
        /// </summary>
        /// <returns></returns>
        public double TimeToHitVerticalWall()
        {
            if (_vx > 0)
            {
                return (1.0 - _rx - _radius) / _vx;
            }
            if (_vx < 0)
            {
                return (_radius - _rx) / _vx;
            }
            return Infinity;
        }

        /// <summary>
        /// How long into future until this particle collides with a horizontal wall?
        /// </summary>
        /// <returns></returns>
        public double TimeToHitHorizontalWall()
        {
            if (_vy > 0)
            {
                return (1.0 - _ry - _radius) / _vy;
            }
            if (_vy < 0)
            {
                return (_radius - _ry) / _vy;
            }
            return Infinity;
        }

        /// <summary>
        /// Update velocities upon collision between this particle and that particle
        /// </summary>
        /// <param name="that"></param>
        public void BounceOff(Particle that)
        {
            double dx = that._rx - _rx;
            double dy = that._ry - _ry;
            double dvx = that._vx - _vx;
            double dvy = that._vy - _vy;
            double dvdr = dx * dvx + dy * dvy; // dv dot dr
            double dist = _radius + that._radius; // distance between particle centers at collison

            // normal force F, and in x and y directions
            double f = 2 * _mass * that._mass * dvdr / ((_mass + that._mass) * dist);
            double fx = f * dx / dist;
            double fy = f * dy / dist;

            // update velocities according to normal force
            _vx += fx / _mass;
            _vy += fy / _mass;
            that._vx -= fx / that._mass;
            that._vy -= fy / that._mass;

            // update collision counts
            _count++;
            that._count++;
        }

        /// <summary>
        /// Update velocity of this particle upon collision with a vertical wall
        /// </summary>
        public void BounceOffVerticalWall()
        {
            _vx = -_vx;
            _count++;
        }

        /// <summary>
        /// Update velocity of this particle upon collision with a horizontal wall
        /// </summary>
        public void BounceOffHorizontalWall()
        {
            _vy = -_vy;
            _count++;
        }

        /// <summary>
        /// Return kinetic energy associated with this particle
        /// </summary>
        /// <returns></returns>
        public double KineticEnergy()
        {
            return 0.5 * _mass * (_vx * _vx + _vy * _vy);
        }
    }
}