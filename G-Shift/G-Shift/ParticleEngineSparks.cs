
// Sparks Particle-Engine
// 
// - decent for first attempt
// - trouble with blending colors, or varying them at all really..
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace G_Shift
{
    //class ParticleEngineSparks
    //{
    //}

    public class ParticleEngineSparks
    {
        //Variables
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        private List<Texture2D> textures;

        private int totalParts = 5;

        // Constructor
        public ParticleEngineSparks(List<Texture2D> textures, Vector2 location)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();
        }

        // Play with this code!
        private Particle GenerateNewParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;


            /*Vector2 velocity = new Vector2(
                    1f * (float)(random.NextDouble() * 2 - 1),
                    1f * (float)(random.NextDouble() * 2 - 1));
            */
            Vector2 velocity = new Vector2(
                    3f * (float)(random.NextDouble() * 2 - 1),
                    3f * (float)(random.NextDouble()));
            //-2);
            //1f * (float)(random.NextDouble()));
            //3f * (float)(random.NextDouble() * 2 - 1));

            float angle = 0;
            //float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            float angularVelocity = 0.1f * (float)(0);


            //Color color = new Color(
            //        (float)random.NextDouble(),
            //        (float)random.NextDouble(),
            //        (float)random.NextDouble());

            //Color color = new Color(
            //        (float)random.Next(125,255),
            //        (float)random.Next(0,50),
            //        (float)random.Next(0,50));

            //Color color = new Color(
            //        (float)random.Next(255, 255),
            //        (float)random.Next(0, 2),
            //        (float)random.Next(0, 0));

            Color color = Color.White;
            //Color color = Color.Red;
            //Color color = Color.WhiteSmoke;

            //float size = (float)random.NextDouble();
            //float size = 1.0f;
            //float size = 0.25f;
            float size = 0.75f;

            int ttl = 20 + random.Next(40);
            //int ttl = 50 + random.Next(40);
            //int ttl = 30;

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        public void Update()
        {
            /*
            //int total = 10;
            int total = 5;

            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle());
            }
            */

            if (particles.Count <= 3)
                particles.Add(GenerateNewParticle());

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
            //spriteBatch.End();
        }
    }
}
