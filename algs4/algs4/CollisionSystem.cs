using System;
using System.Drawing;
using algs4.stdlib;

namespace algs4.algs4
{
    public class CollisionSystem
    {
        /// <summary>
        /// The priority queue
        /// </summary>
        private MinPQ<Event> _pq;

        /// <summary>
        /// Simulation clock time
        /// </summary>
        private double _t;

        /// <summary>
        /// Number of redraw events per clock tick
        /// </summary>
        private const double Hz = 0.5;

        /// <summary>
        /// The array of particles
        /// </summary>
        private readonly Particle[] _particles;

        /// <summary>
        /// Create a new collision system with the given set of particles
        /// </summary>
        /// <param name="particles"></param>
        public CollisionSystem(Particle[] particles)
        {
            _particles = (Particle[])particles.Clone(); // defensive copy
        }

        /// <summary>
        /// Updates priority queue with all new events for particle a
        /// </summary>
        /// <param name="a"></param>
        /// <param name="limit"></param>
        private void Predict(Particle a, double limit)
        {
            if (a == null)
            {
                return;
            }

            // particle-particle collisions
            for (int i = 0; i < _particles.Length; i++)
            {
                double dt = a.TimeToHit(_particles[i]);
                if (_t + dt <= limit)
                {
                    _pq.Insert(new Event(_t + dt, a, _particles[i]));
                }
            }

            // particle-wall collisions
            double dtX = a.TimeToHitVerticalWall();
            double dtY = a.TimeToHitHorizontalWall();
            if (_t + dtX <= limit)
            {
                _pq.Insert(new Event(_t + dtX, a, null));
            }
            if (_t + dtY <= limit)
            {
                _pq.Insert(new Event(_t + dtY, null, a));
            }
        }

        /// <summary>
        /// Redraw all particles
        /// </summary>
        /// <param name="limit"></param>
        private void Redraw(double limit)
        {
            StdDraw.Clear();
            for (int i = 0; i < _particles.Length; i++)
            {
                _particles[i].Draw();
            }
            StdDraw.Show(20);
            if (_t < limit)
            {
                _pq.Insert(new Event(_t + 1.0 / Hz, null, null));
            }
        }

        /// <summary>
        /// Event based simulation for limit seconds
        /// </summary>
        /// <param name="limit"></param>
        public void Simulate(double limit)
        {
            // initialize PQ with collision events and redraw event
            _pq = new MinPQ<Event>();
            for (int i = 0; i < _particles.Length; i++)
            {
                Predict(_particles[i], limit);
            }
            _pq.Insert(new Event(0, null, null)); // redraw event

            // the main event-driven simulation loop
            while (!_pq.IsEmpty())
            {
                // get impending event, discard if invalidated
                Event e = _pq.DelMin();
                if (!e.IsValid())
                {
                    continue;
                }
                Particle a = e.A;
                Particle b = e.B;

                // physical collision, so update positions, and then simulation clock
                for (int i = 0; i < _particles.Length; i++)
                {
                    _particles[i].Move(e.Time - _t);
                }
                _t = e.Time;

                // process event
                if (a != null && b != null)
                {
                    a.BounceOff(b); // particle-particle collision
                }
                else if (a != null && b == null)
                {
                    a.BounceOffVerticalWall(); // particle-wall collision
                }
                else if (a == null && b != null)
                {
                    b.BounceOffHorizontalWall(); // particle-wall collision
                }
                else if (a == null && b == null)
                {
                    Redraw(limit); // redraw event
                }

                // update the priority queue with new collisions involving a or b
                Predict(a, limit);
                Predict(b, limit);
            }
        }

        /// <summary>
        /// An event during a particle collision simulation. Each event contains
        /// the time at which it will occur (assuming no supervening actions)
        /// and the particles a and b involved.
        /// 
        ///   -  a and b both null:      redraw event
        ///   -  a null, b not null:     collision with vertical wall
        ///   -  a not null, b null:     collision with horizontal wall
        ///   -  a and b both not null:  binary collision between a and b
        /// </summary>
        private class Event : IComparable<Event>
        {
            /// <summary>
            /// Time that event is scheduled to occur
            /// </summary>
            public double Time { get; private set; }

            /// <summary>
            /// Particles involved in event, possibly null
            /// </summary>
            public Particle A { get; private set; }

            /// <summary>
            /// Particles involved in event, possibly null
            /// </summary>
            public Particle B { get; private set; }

            /// <summary>
            /// Collision counts at event creation
            /// </summary>
            private readonly int _countA;

            /// <summary>
            /// Collision counts at event creation
            /// </summary>
            private readonly int _countB;

            /// <summary>
            /// Create a new event to occur at time t involving a and b
            /// </summary>
            /// <param name="t"></param>
            /// <param name="a"></param>
            /// <param name="b"></param>
            public Event(double t, Particle a, Particle b)
            {
                Time = t;
                A = a;
                B = b;
                if (a != null)
                {
                    _countA = a.Count();
                }
                else
                {
                    _countA = -1;
                }
                if (b != null)
                {
                    _countB = b.Count();
                }
                else
                {
                    _countB = -1;
                }
            }

            /// <summary>
            /// Compare times when two events will occur
            /// </summary>
            /// <param name="that"></param>
            /// <returns></returns>
            public int CompareTo(Event that)
            {
                if (Time < that.Time)
                {
                    return -1;
                }
                if (Time > that.Time)
                {
                    return +1;
                }
                return 0;
            }

            /// <summary>
            /// Has any collision occurred between when event was created and now?
            /// </summary>
            /// <returns></returns>
            public bool IsValid()
            {
                if (A != null && A.Count() != _countA)
                {
                    return false;
                }
                return !(B != null && B.Count() != _countB);
            }
        }

        /// <summary>
        /// Sample client
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            StdDraw.SetCanvasSize(800, 800);

            // remove the border
            // StdDraw.setXscale(1.0/22.0, 21.0/22.0);
            // StdDraw.setYscale(1.0/22.0, 21.0/22.0);

            // turn on animation mode
            StdDraw.Show(0);

            // the array of particles
            Particle[] particles;

            // create N random particles
            if (args.Length == 1)
            {
                int n = int.Parse(args[0]);
                particles = new Particle[n];
                for (int i = 0; i < n; i++)
                {
                    particles[i] = new Particle();
                }
            }

            // or read from standard input
            else
            {
                int n = StdIn.ReadInt();
                particles = new Particle[n];
                for (int i = 0; i < n; i++)
                {
                    double rx = StdIn.ReadDouble();
                    double ry = StdIn.ReadDouble();
                    double vx = StdIn.ReadDouble();
                    double vy = StdIn.ReadDouble();
                    double radius = StdIn.ReadDouble();
                    double mass = StdIn.ReadDouble();
                    int r = StdIn.ReadInt();
                    int g = StdIn.ReadInt();
                    int b = StdIn.ReadInt();
                    Color color = Color.FromArgb(r, g, b);
                    particles[i] = new Particle(rx, ry, vx, vy, radius, mass, color);
                }
            }

            // create collision system and simulate
            CollisionSystem system = new CollisionSystem(particles);
            system.Simulate(10000);
        }
    }
}